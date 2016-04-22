using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScrapySharp.Core;
using ScrapySharp.Html.Parsing;
using ScrapySharp.Network;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Html.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace CostcoWinForm
{
    public partial class Form1 : Form
    {
        List<String> categoryUrlArray = new List<string>();
        List<String> categoryArray = new List<string>();
        List<String> subCategoryUrlArray = new List<string>();
        List<String> productUrlArray = new List<string>();

        List<String> newProductArray = new List<string>();
        List<String> discontinueddProductArray = new List<string>();
        List<String> priceUpProductArray = new List<string>();
        List<String> priceDownProductArray = new List<string>();

        List<Product> priceChangedProductArray = new List<Product>();

        string emailMessage;

        string destinFileName;

        DateTime startDT;
        DateTime endDT;

        ScrapingBrowser Browser = new ScrapingBrowser();
        WebPage PageResult;

        string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            ProductInfoForm productInfoForm = new ProductInfoForm();
            productInfoForm.ShowDialog();

        }

        private void btnImportCategories_Click(object sender, EventArgs e)
        {
            GetCategories();
        }

        private void btnImportProducts_Click(object sender, EventArgs e)
        {
            GetCategoryArray();

            GetSubCategoryUrls();

            GetProductUrls();

            GetProductInfo();

            PopulateTables();

            CompareProducts();

            ArchieveProducts();

            SendEmail();
        }

        private void GetCategories()
        {
            Browser.AllowAutoRedirect = true; // Browser has many settings you can access in setup
            Browser.AllowMetaRedirect = true;
            //go to the home page
            PageResult = Browser.NavigateToPage(new Uri("http://www.costco.com/view-more.html"));

            List<String> Names = new List<string>();
            List<HtmlNode> columnNodes = PageResult.Html.CssSelect(".viewmore-column").ToList<HtmlNode>();

            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            sqlString = "TRUNCATE TABLE Categories";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            foreach (HtmlNode columnNode in columnNodes)
            {
                string departmentName = (columnNode.SelectSingleNode("span")).InnerText;
                departmentName = departmentName.Replace("'", "");

                List<HtmlNode> ulNodes = columnNode.SelectNodes("ul").ToList<HtmlNode>();

                List<HtmlNode> liNodes = ulNodes[0].SelectNodes("li").ToList<HtmlNode>();

                foreach (var node1 in liNodes)
                {
                    string categoryName = node1.InnerText;
                    categoryName = categoryName.Replace("'", "");

                    if (node1.ChildNodes[0].Attributes[0].Name == "href")
                    {
                        sqlString = "INSERT INTO Categories (DepartmentName, CategoryName, CategoryUrl) VALUES ('" +
                                    departmentName + "', '" +
                                    categoryName + "', '" +
                                    node1.ChildNodes[0].Attributes[0].Value + "')";
                        cmd.CommandText = sqlString;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            //MessageBox.Show("Get Category Done");
        }

        private void GetCategoryArray()
        {
            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            sqlString = "SELECT CategoryUrl FROM Categories WHERE bInclude = 1";
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    categoryUrlArray.Add(reader.GetString(0));
                }
            }
            reader.Close();
            cn.Close();
        }

        private void GetProductInfo()
        {
            startDT = DateTime.Now;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            string sqlString = "TRUNCATE TABLE Raw_ProductInfo";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            //productUrlArray.Add("http://www.costco.com/ABC-and-123-Foam-Floor-Mat-Set%2c-36-Tiles-Set.product.11754291.html");



            int i = 1;

            foreach (string productUrl in productUrlArray)
            {
                try
                {

                    i++;

                    string UrlNum = productUrl.Substring(0, productUrl.LastIndexOf('.'));
                    UrlNum = UrlNum.Substring(UrlNum.LastIndexOf('.') + 1);


                    //webBrowser1.ScriptErrorsSuppressed = true;
                    //webBrowser1.Navigate(productUrl);

                    //waitTillLoad(this.webBrowser1);

                    //return;

                    //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    //var documentAsIHtmlDocument3 = (mshtml.IHTMLDocument3)webBrowser1.Document.DomDocument;
                    //StringReader sr = new StringReader(documentAsIHtmlDocument3.documentElement.outerHTML);
                    //doc.Load(sr);

                    //HtmlNode html = doc.DocumentNode;

                    PageResult = Browser.NavigateToPage(new Uri(productUrl));

                    HtmlNode html = PageResult.Html;

                    if (html.InnerText.Contains("Product Not Found"))
                        continue;

                    string stSubCategories = "";

                    HtmlNode category = html.SelectSingleNode("//ul[@itemprop='breadcrumb']");

                    List<HtmlNode> subCategories = category.SelectNodes("li").ToList();

                    foreach (HtmlNode subCategory in subCategories)
                    {
                        string temp = subCategory.InnerText.Replace("\n", "");
                        temp = temp.Replace("\t", "");
                        temp = temp.Replace("'", "");
                        stSubCategories += temp + "|";
                    }
                    stSubCategories = stSubCategories.Substring(0, stSubCategories.Length - 1);

                    HtmlNode productInfo = html.CssSelect(".product-info").ToList<HtmlNode>().First();

                    List<HtmlNode> topReviewPanelNode = productInfo.CssSelect(".top_review_panel").ToList<HtmlNode>();

                    string discount = "";

                    HtmlNode discountNote = topReviewPanelNode[0].SelectSingleNode("//p[@class='merchandisingText']");

                    if (discountNote != null)
                        discount = discountNote.InnerText.Replace("?", "");

                    string productName = ((topReviewPanelNode[0]).SelectNodes("h1"))[0].InnerText;
                    productName = productName.Replace("???", "");
                    productName = productName.Replace("??", "");

                    List<HtmlNode> col1Node = productInfo.CssSelect(".col1").ToList<HtmlNode>();
                    string itemNumber = (col1Node[0].SelectNodes("p")[0]).InnerText;
                    if (itemNumber.ToUpper().Contains("ITEM") && itemNumber.Length > 6)
                        itemNumber = itemNumber.Substring(6);
                    else
                        itemNumber = "";

                    discountNote = col1Node[0].CssSelect(".merchandisingText").FirstOrDefault();

                    if (discountNote != null)
                        discount = discount.Length == 0 ? discountNote.InnerText.Replace("?", "") : discount + "; " + discountNote.InnerText.Replace("?", "");

                    string price;
                    List<HtmlNode> yourPriceNode = col1Node.CssSelect(".your-price").ToList<HtmlNode>();
                    if (yourPriceNode.Count > 0)
                    {
                        List<HtmlNode> priceNode = yourPriceNode[0].CssSelect(".currency").ToList<HtmlNode>();
                        price = priceNode[0].InnerText;
                        price = price.Replace("$", "");
                        price = price.Replace(",", "");

                        if (price == "- -")
                            price = "-2";
                    }
                    else
                    {
                        price = "-1";
                    }

                    var productOptionsNode = col1Node.CssSelect(".product-option").FirstOrDefault();

                    string shipping = "0";

                    var productSHNode = col1Node[0].SelectSingleNode("//li[@class='product']");

                    if (productSHNode != null)
                    {
                        if (productSHNode.InnerText.ToUpper().Contains("INCLUDED"))
                        {
                            shipping = "0";
                        }
                        else
                        {
                            string shString = productSHNode.InnerText;
                            int nDollar = shString.IndexOf("$");
                            if (nDollar > 0)
                            {
                                shString = shString.Substring(nDollar + 1);
                                int nStar = shString.IndexOf("*");
                                if (nStar == -1)
                                    nStar = shString.IndexOf(" ");
                                shString = shString.Substring(0, nStar);
                                shString = shString.Replace(" ", "");
                                shipping = shString;
                            }
                            else
                            {
                                int nShipping = shString.IndexOf("Shipping");
                                int nQuantity = shString.ToUpper().IndexOf("QUANTITY");
                                shString = shString.Substring(nShipping, nQuantity);
                                Char[] strarr = shString.ToCharArray().Where(c => Char.IsDigit(c) || c.Equals('.')).ToArray();
                                decimal number = Convert.ToDecimal(new string(strarr));
                                shipping = number.ToString();
                            }
                        }
                    }


                    //List<HtmlNode> productsNode = col1Node.CssSelect(".products").ToList<HtmlNode>();
                    //if (productsNode.Count > 0)
                    //    shipping = productsNode[0].SelectSingleNode("li").SelectSingleNode("p") == null ? "-1" : productsNode[0].SelectSingleNode("li").SelectSingleNode("p").InnerText;
                    //else
                    //    shipping = "-1";

                    string description = "";

                    IWebDriver driver = new FirefoxDriver();

                    driver.Navigate().GoToUrl(productUrl);

                    IWebElement element = driver.FindElement(By.Id("product-tab1"));
                    //string elementHtml = element.GetAttribute("outerHTML");

                    //driver.Dispose();

                    //var pageTypeElements = element.FindElements(By.Id("wc-power-page"));

                    if (element.FindElements(By.Id("wc-power-page")).Count > 0)
                    {
                        description = ProcessPowerPage(element.FindElement(By.Id("wc-power-page")));
                    }
                    else if (element.FindElements(By.Id("sp_inline_product")).Count > 0)
                    {
                        description = ProcessInlineProduct(element.FindElement(By.Id("sp_inline_product")));
                    }

                    driver.Dispose();


                    //string description = GetProductionDescription(productUrl);
                    description = ProcessHtml(description);

                    description = description.Replace("???", "");
                    description = description.Replace("??", "");
                    description = description.Replace("'", "");
                    description = description.Replace("\"", "");

                    HtmlNode productDetailTabsNode = html.CssSelect(".product-detail-tabs").ToList<HtmlNode>().First();
                    if (description == "")
                    {
                        var productDetailTab1Node = productDetailTabsNode.SelectSingleNode("//div[@id='product-tab1']");

                        var productDescriptionNode = productDetailTab1Node.SelectSingleNode("//p[@itemprop='description']");

                        description = productDescriptionNode.InnerHtml;

                        description = ProcessHtml(description);

                        description = description.Replace("???", "");
                        description = description.Replace("??", "");
                        description = description.Replace("'", "");

                        if (description.Contains("<div style=text-align:center;>"))
                        {
                            string stStart = "<div style=text-align:center;>";
                            string stEnd = "</div>";

                            int iStart = description.IndexOf(stStart);

                            int iEnd = description.IndexOf(stEnd, iStart);
                            iEnd += stEnd.Length;

                            string stReplace = description.Substring(iStart, iEnd - iStart);

                            description = description.Replace(stReplace, "");
                        }
                    }

                    Regex emailReplace = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
                    emailReplace.Replace(description, "");

                    var productDetailTab2Node = productDetailTabsNode.SelectSingleNode("//div[@id='product-tab2']");

                    string specification = "";
                    if (productDetailTab2Node != null)
                    {
                        specification = productDetailTab2Node.InnerHtml;
                        string temp = specification;
                        //string convertedSpecification = HtmlToText.ConvertHtml(specification);
                        //specification = convertedSpecification.Replace("'", "");
                        //specification = convertedSpecification.Replace("\r", "");
                        //specification = convertedSpecification.Replace("\t", "");
                        //specification = convertedSpecification.Replace("\n", "");
                        temp = ProcessHtml(temp);
                        specification = temp;
                    }

                    HtmlNode imageColumnNode = html.CssSelect(".image-column").ToList<HtmlNode>().First();

                    //HtmlNode carouselWrapNode = imageColumnNode.CssSelect(".product-image-carousel").ToList<HtmlNode>().First();
                    //var thumbHolderNode = carouselWrapNode.SelectNodes("//ul[@id='thumb_holder']");

                    //var thumbNodes = thumbHolderNode.SelectNodes("/li");

                    HtmlNode imageNode = imageColumnNode.SelectSingleNode("//img[@itemprop='image']");

                    string imageUrl = (imageNode.Attributes["src"]).Value;

                    sqlString = "INSERT INTO Raw_ProductInfo (Name, UrlNumber, ItemNumber, Category, Price, Shipping, Discount, Details, Specification, ImageLink, Url) VALUES ('" + productName + "','" + UrlNum + "','" + itemNumber + "','" + stSubCategories + "'," + price + "," + shipping + "," + "'" + discount + "','" + description + "','" + specification + "','" + imageUrl + "','" + productUrl + "')";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    continue;
                }
            }

            cn.Close();

            endDT = DateTime.Now;

            //MessageBox.Show("Start: " + startDT.ToLongTimeString() + "; End: " + endDT.ToLongTimeString());
        }

        private void GetSubCategoryUrls()
        {
            foreach (var categoryUrl in categoryUrlArray)
            {
                string url;
                if (categoryUrl.Contains("http"))
                    url = categoryUrl;
                else
                    url = "http://www.costco.com" + categoryUrl;

                // level 0
                WebPage PageResult = Browser.NavigateToPage(new Uri(url));
                var mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                if (mainContentWrapperNote == null)
                    continue;
                List<HtmlNode> categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                {
                    subCategoryUrlArray.Add(url);
                }
                else
                {
                    List<HtmlNode> departmentNodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                    foreach (HtmlNode departmentNode in departmentNodes)
                    {
                        if (departmentNode.InnerText.Contains("("))
                        {
                            HtmlNode node = departmentNode.Descendants("a").First();
                            string departmentUrl = node.Attributes["href"].Value;

                            // level 1
                            PageResult = Browser.NavigateToPage(new Uri(departmentUrl));
                            mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                            if (mainContentWrapperNote == null)
                                continue;
                            categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                            if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                            {
                                subCategoryUrlArray.Add(departmentUrl);
                            }
                            else
                            {
                                List<HtmlNode> department_1_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                                foreach (HtmlNode department_1_Node in department_1_Nodes)
                                {
                                    if (department_1_Node.InnerText.Contains("("))
                                    {
                                        HtmlNode node_1 = department_1_Node.Descendants("a").First();
                                        string departmentUrl_1 = node_1.Attributes["href"].Value;

                                        // level 2
                                        PageResult = Browser.NavigateToPage(new Uri(departmentUrl_1));
                                        mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                                        if (mainContentWrapperNote == null)
                                            continue;
                                        categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                                        if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                                        {
                                            subCategoryUrlArray.Add(departmentUrl_1);
                                        }
                                        else
                                        {
                                            List<HtmlNode> department_2_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                                            foreach (HtmlNode department_2_Node in department_2_Nodes)
                                            {
                                                HtmlNode node_2 = department_2_Node.Descendants("a").First();
                                                string department_2_Url = node_2.Attributes["href"].Value;
                                                subCategoryUrlArray.Add(department_2_Url);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HtmlNode node_1 = departmentNode.Descendants("a").First();
                                        string department_1_Url = node_1.Attributes["href"].Value;
                                        subCategoryUrlArray.Add(department_1_Url);
                                    }
                                }
                            }
                        }
                        else
                        {
                            HtmlNode node = departmentNode.Descendants("a").First();
                            string departmentUrl = node.Attributes["href"].Value;
                            subCategoryUrlArray.Add(departmentUrl);
                        }
                    }
                }
            }

            //MessageBox.Show("Get subCategoryUrlArray Done");
        }

        private void GetProductUrls()
        {
            foreach (string url in subCategoryUrlArray)
            {
                PageResult = Browser.NavigateToPage(new Uri(url));
                var mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                List<HtmlNode> categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                {
                    List<HtmlNode> gridNodes = PageResult.Html.CssSelect(".grid-4col").ToList<HtmlNode>();
                    if (gridNodes.Count() > 0)
                    {
                        List<HtmlNode> productNodes = gridNodes.CssSelect(".product-tile").ToList<HtmlNode>();

                        foreach (HtmlNode productNode in productNodes)
                        {
                            HtmlNode product = productNode.CssSelect(".product-tile-image-container ").First();
                            if (((product.SelectNodes("a")).First().Attributes).First().Name == "href")
                            {
                                productUrlArray.Add(((product.SelectNodes("a")).First().Attributes).First().Value);
                            }
                        }
                    }
                }
            }

            //MessageBox.Show("Get productUrlArray Done");
        }

        private void PopulateTables()
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            // copy to staging_productInfo
            cn.Open();
            string sqlString = "TRUNCATE TABLE Staging_ProductInfo";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            sqlString = @"insert into dbo.staging_productInfo (Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url)
                        select distinct Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url
                        from dbo.Raw_ProductInfo
                        where Price > 0
                        order by UrlNumber";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            // copy to staging_productInfo_filtered
            sqlString = "TRUNCATE TABLE Staging_ProductInfo_Filtered";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            sqlString = @"insert into dbo.Staging_ProductInfo_Filtered(Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url)
                        select distinct Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url 
                        from dbo.Raw_ProductInfo 
                        where Price > 0 and Price < 100 and Shipping = 0
                        order by UrlNumber";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();
        }

        private void AdjustEBayListings()
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            SqlDataReader rdr;

            cn.Open();

            // price up
            string sqlString = @"select e.*, p.Price as NewPrice from eBay_CurrentListings e, ProductInfo p where e.CostcoUrlNumber = p.UrlNumber and e.CostcoPrice <> p.Price";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            Product product;

            while (rdr.Read())
            {
                product = new Product();
                product.Name = rdr["Name"].ToString();
                product.UrlNumber = rdr["CostcoItemNumber"].ToString();
                product.eBayItemNumber = rdr["eBayItemNumber"].ToString();
                product.Price = Convert.ToDecimal(rdr["NewPrice"]);
            }

            rdr.Close();
        }

        private void CompareProducts()
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            SqlDataReader rdr;

            cn.Open();

            // price up
            string sqlString = @"select s.Name, s.Price as newPrice, p.Price as oldPrice, s.Url from [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
                                where s.UrlNumber = p.UrlNumber
                                and s.Price > p.Price";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                priceUpProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["newPrice"].ToString() + "|(" + rdr["oldPrice"].ToString() + ")");
            }

            rdr.Close();

            // price down
            sqlString = @"select s.Name, s.Price as newPrice, p.Price as oldPrice, s.Url from [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
                                where s.UrlNumber = p.UrlNumber
                                and s.Price < p.Price";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                priceDownProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["newPrice"].ToString() + "|(" + rdr["oldPrice"].ToString() + ")");
            }

            rdr.Close();

            // new products
            sqlString = @"select * from Staging_ProductInfo sp
                        where 
                        not exists
                        (select 1 from ProductInfo p  where sp.UrlNumber = p.UrlNumber)";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                newProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["Price"].ToString());
            }

            rdr.Close();

            // discontinued products
            sqlString = @"select * from ProductInfo p 
                        where 
                        not exists
                        (select 1 from Staging_ProductInfo sp where sp.UrlNumber = p.UrlNumber)";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                discontinueddProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["Price"].ToString());
            }

            rdr.Close();

            cn.Close();
        }

        private void ArchieveProducts()
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            // price up
            string sqlString = @"insert into [dbo].[Archieve] (Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url, ImportedDT)
                                select distinct Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url, GETDATE()
                                from  [dbo].[ProductInfo]";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            sqlString = "TRUNCATE TABLE ProductInfo";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            sqlString = @"insert into [dbo].[ProductInfo] (Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url)
                        select distinct Name, urlNumber, itemnumber, Category, price, shipping, discount, details, specification, imageLink, url
                        from  dbo.staging_productInfo";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            sqlString = "TRUNCATE TABLE Staging_ProductInfo";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        private void SendEmail()
        {
            emailMessage = "<h3>Price up products: (" + priceUpProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (priceUpProductArray.Count == 0)
                emailMessage += "<p>No price up product</p>" + "</br>";
            else
            {
                foreach (string priceUpProduct in priceUpProductArray)
                {
                    emailMessage += "<p>" + priceUpProduct + "</p></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>Price down products: (" + priceDownProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (priceDownProductArray.Count == 0)
                emailMessage += "<p>No price down product" + "</p></br>";
            else
            {
                foreach (string priceDownProduct in priceDownProductArray)
                {
                    emailMessage += "<p>" + priceDownProduct + "</p></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>New products: (" + newProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (newProductArray.Count == 0)
                emailMessage = "<p>No new product</p>" + "</br>";
            else
            {
                foreach (string newProduct in newProductArray)
                {
                    emailMessage += "<p>" + newProduct + "</P></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>Discontinued products: (" + discontinueddProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (this.discontinueddProductArray.Count == 0)
                emailMessage = "<p>No new product</p>" + "</br>";
            else
            {
                foreach (string discontinueddProduct in discontinueddProductArray)
                {
                    emailMessage += "<p>" + discontinueddProduct + "</P></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<p>Start: " + startDT.ToLongTimeString() + "</p></br>";
            emailMessage += "<p>End: " + endDT.ToLongTimeString() + "</p></br>";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("zjding@gmail.com");
                mail.To.Add("zjding@gmail.com");
                mail.Subject = DateTime.Now.ToLongDateString();
                mail.Body = emailMessage;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("zjding@gmail.com", "yueding00");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            CompareProducts();

            SendEmail();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //GetCategoryArray();

            //GetSubCategoryUrls();

            //GetProductUrls();

            //GetProductInfo();

            //PopulateTables();

            //CompareProducts();

            //ArchieveProducts();

            //SendEmail();

            //this.Close();

            //webBrowser1.Navigate("http://www.ebay.com/sch/i.html?LH_Sold=1&LH_ItemCondition=11&_sop=12&rt=nc&LH_BIN=1&_nkw=Swingline+Commercial+Stapler+Black+SWI+44401S");

            //string a = webBrowser1.DocumentText;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void btnSubCategories_Click(object sender, EventArgs e)
        {
            categoryUrlArray.Add("http://www.costco.com/mens-clothing.html");

            foreach (var categoryUrl in categoryUrlArray)
            {
                string url;
                if (categoryUrl.Contains("http"))
                    url = categoryUrl;
                else
                    url = "http://www.costco.com" + categoryUrl;

                // level 0
                WebPage PageResult = Browser.NavigateToPage(new Uri(url));
                var mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                if (mainContentWrapperNote == null)
                    continue;
                List<HtmlNode> categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                {
                    subCategoryUrlArray.Add(url);
                }
                else
                {
                    List<HtmlNode> departmentNodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                    foreach (HtmlNode departmentNode in departmentNodes)
                    {
                        if (departmentNode.InnerText.Contains("("))
                        {
                            HtmlNode node = departmentNode.Descendants("a").First();
                            string departmentUrl = node.Attributes["href"].Value;

                            // level 1
                            PageResult = Browser.NavigateToPage(new Uri(departmentUrl));
                            mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                            if (mainContentWrapperNote == null)
                                continue;
                            categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                            if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                            {
                                subCategoryUrlArray.Add(departmentUrl);
                            }
                            else
                            {
                                List<HtmlNode> department_1_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                                foreach (HtmlNode department_1_Node in department_1_Nodes)
                                {
                                    if (department_1_Node.InnerText.Contains("("))
                                    {
                                        HtmlNode node_1 = department_1_Node.Descendants("a").First();
                                        string departmentUrl_1 = node_1.Attributes["href"].Value;

                                        // level 2
                                        PageResult = Browser.NavigateToPage(new Uri(departmentUrl_1));
                                        mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                                        if (mainContentWrapperNote == null)
                                            continue;
                                        categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                                        if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                                        {
                                            subCategoryUrlArray.Add(departmentUrl_1);
                                        }
                                        else
                                        {
                                            List<HtmlNode> department_2_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                                            foreach (HtmlNode department_2_Node in department_2_Nodes)
                                            {
                                                HtmlNode node_2 = department_2_Node.Descendants("a").First();
                                                string department_2_Url = node_2.Attributes["href"].Value;
                                                subCategoryUrlArray.Add(department_2_Url);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HtmlNode node_1 = departmentNode.Descendants("a").First();
                                        string department_1_Url = node_1.Attributes["href"].Value;
                                        subCategoryUrlArray.Add(department_1_Url);
                                    }
                                }
                            }
                        }
                        else
                        {
                            HtmlNode node = departmentNode.Descendants("a").First();
                            string departmentUrl = node.Attributes["href"].Value;
                            subCategoryUrlArray.Add(departmentUrl);
                        }
                    }
                }
            }
        }

        private void btnProductText_Click(object sender, EventArgs e)
        {
            productUrlArray.Add("http://www.costco.com/.product.100244718.html");

            //productUrlArray.Add("http://www.costco.com/.product.100244718.html");

            //productUrlArray.Add("http://www.costco.com/.product.100230476.html?cm_sp=RichRelevance-_-itempageVerticalRight-_-CategorySiloedViewCP&cm_vc=itempageVerticalRight|CategorySiloedViewCP");

            //productUrlArray.Add("http://www.costco.com/Kirkland-Signature%E2%84%A2-Hair-Regrowth-Treatment-Extra-Strength-for-Men-5%25-Minoxidil-Topical-Solution-6-pk.product.11501138.html");

            //productUrlArray.Add("http://www.costco.com/.product.100099102.html?cm_sp=RichRelevance-_-itempageVerticalRight-_-CategorySiloedViewCP&cm_vc=itempageVerticalRight|CategorySiloedViewCP");

            //productUrlArray.Add("http://www.costco.com/Titan-1.25-Waste-Disposer-Designer-Series.product.100277151.html");
            //productUrlArray.Add("http://www.costco.com/4-Tier-Toy-Organizer-with-Bins.product.100240406.html");
            //productUrlArray.Add("http://www.costco.com/.product.100244718.html");
            //productUrlArray.Add("http://www.costco.com/.product.100056803.html");
            //productUrlArray.Add("http://www.costco.com/.product.100169328.html");

            //productUrlArray.Add("http://www.costco.com/.product.100100971.html?cm_sp=RichRelevance-_-categorypageHorizontalTop-_-CategoryTopProducts&cm_vc=categorypageHorizontalTop|CategoryTopProducts");
            //productUrlArray.Add("http://www.costco.com/Ninja-Coffee-Bar%E2%84%A2-Coffee-Brewer.product.100244468.html");
            //productUrlArray.Add("http://www.costco.com/.product.100226551.html");
            //productUrlArray.Add("http://www.costco.com/Green-Mountain-Coffee%C2%AE-Breakfast-Blend-180-K-Cup%C2%AE-Pods.product.100242875.html");
            //productUrlArray.Add("http://www.costco.com/Starbucks%C2%AE-French-Roast-Whole-Bean-Coffee-240oz.product.100232750.html");
            //productUrlArray.Add("http://www.costco.com/Brother-QL-500-Label-Maker.product.11000878.html");

            GetProductInfo();
        }

        private void btnEbayCategory_Click(object sender, EventArgs e)
        {
            //GetEbayCategoryID();
        }

        private string GetEbayCategoryIDAndPrice(string productName)
        {
            string ebaySearchUrl = "http://www.ebay.com/sch/i.html?LH_Sold=1&LH_ItemCondition=11&_sop=12&rt=nc&LH_BIN=1&_nkw=";
            //string ebaySearchUrl = "http://www.ebay.com/sch/i.html?LH_Sold=1&LH_ItemCondition=11&_sop=12&rt=nc&LH_BIN=1&_nkw=Swingline+Commercial+Stapler+Black+SWI+44401s";
            productName = productName.Replace("  ", " ");
            productName = productName.Replace(" ", "+");
            productName = productName.Replace("%", "");
            ebaySearchUrl += productName;

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(ebaySearchUrl);

            waitTillLoad(this.webBrowser1);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            var documentAsIHtmlDocument3 = (mshtml.IHTMLDocument3)webBrowser1.Document.DomDocument;
            StringReader sr = new StringReader(documentAsIHtmlDocument3.documentElement.outerHTML);
            doc.Load(sr);

            var ulNote = doc.DocumentNode.SelectSingleNode("//ul[@id='ListViewInner']");

            if (ulNote == null)
            {
                return "99|0";
            }

            List<HtmlNode> liNotes = ulNote.SelectNodes("li").ToList();

            HtmlNode hrefNote = liNotes[0].SelectSingleNode("//h3[@class='lvtitle']");

            HtmlNode node = hrefNote.Descendants("a").First();
            string productUrl = node.Attributes["href"].Value;


            WebPage PageResult = Browser.NavigateToPage(new Uri(productUrl));

            HtmlNode priceNote = PageResult.Html.SelectSingleNode("//span[@itemprop='price']");
            if (priceNote == null)
            {
                priceNote = PageResult.Html.SelectSingleNode("//span[@id='mm-saleDscPrc']");
            }
            string price = priceNote.InnerText;
            price = price.Substring(4);

            HtmlNode brumbNote = PageResult.Html.SelectSingleNode("//table[@class='vi-bc-topM']");
            HtmlNode brumbTDNote = brumbNote.SelectSingleNode("//td[@id='vi-VR-brumb-lnkLst']");
            List<HtmlNode> brumbList = brumbTDNote.SelectNodes("//li[@itemprop='itemListElement']").ToList();

            List<string> categoryList = new List<string>();

            string categoryTemp;
            foreach (HtmlNode brumbItem in brumbList)
            {
                categoryTemp = brumbItem.InnerText;
                if (categoryTemp.Contains("Other"))
                    categoryTemp = "Other";
                categoryList.Add(categoryTemp);
            }

            //string subCategory = categoryList.ElementAt(categoryList.Count - 1);

            //if (subCategory.Contains("Other"))
            //{
            //    subCategory = "Other";
            //}

            string sqlString = "SELECT CategoryId FROM eBay_Categories WHERE " +
                                "F" + Convert.ToString(categoryList.Count) + "='" + categoryList.ElementAt(categoryList.Count - 2) + "' AND " +
                                "F" + Convert.ToString(categoryList.Count + 1) + "='" + categoryList.ElementAt(categoryList.Count - 1) + "'";

            string categoryID = "";

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                categoryID = reader.GetString(0);
            }
            reader.Close();

            if (categoryID == "")
            {
                sqlString = "SELECT CategoryId FROM eBay_Categories WHERE " +
                                "F" + Convert.ToString(categoryList.Count - 1) + "='" + categoryList.ElementAt(categoryList.Count - 3) + "' AND " +
                                "F" + Convert.ToString(categoryList.Count) + "='" + categoryList.ElementAt(categoryList.Count - 2) + "'";

                cmd.CommandText = sqlString;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    categoryID = reader.GetString(0);
                }
            }

            reader.Close();
            cn.Close();

            return categoryID + "|" + price;
        }

        private void waitTillLoad(WebBrowser webBrControl)
        {
            WebBrowserReadyState loadStatus;
            int waittime = 10000;
            int counter = 0;
            while (true)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if ((counter > waittime) ||
                    (loadStatus == WebBrowserReadyState.Uninitialized) ||
                    (loadStatus == WebBrowserReadyState.Loading) ||
                    (loadStatus == WebBrowserReadyState.Interactive))
                {
                    break;
                }
                counter++;
            }

            counter = 0;
            while (true)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if (loadStatus == WebBrowserReadyState.Complete && webBrControl.IsBusy != true)
                {
                    break;
                }
                counter++;
            }
        }

        private double GeneratePrice(double productPrice, double ebayPrice)
        {
            double cost = productPrice * 1.09 / 0.87;

            if (cost + 5 > ebayPrice)
                return cost + 5;
            else
                return ebayPrice;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            List<Product> products = new List<Product>();

            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = "  SELECT top 5 * FROM Raw_ProductInfo"; // WHERE shipping = 0.00 and Price < 100";

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Name = Convert.ToString(reader["Name"]);
                    product.UrlNumber = Convert.ToString(reader["UrlNumber"]);
                    product.ItemNumber = Convert.ToString(reader["ItemNumber"]);
                    product.Category = Convert.ToString(reader["Category"]);
                    product.Price = Convert.ToDecimal(reader["Price"]);
                    product.Shipping = Convert.ToDecimal(reader["Shipping"]);
                    product.Discount = Convert.ToString(reader["Discount"]);
                    product.Details = Convert.ToString(reader["Details"]);
                    product.Specification = Convert.ToString(reader["Specification"]);
                    product.ImageLink = Convert.ToString(reader["ImageLink"]);
                    product.Url = Convert.ToString(reader["Url"]);

                    products.Add(product);
                }
            }

            reader.Close();
            cn.Close();

            // add to Excel file
            string sourceFileName = @"c:\ebay\documents\" + "FileExchange.csv";
            destinFileName = @"c:\ebay\upload\" + "FileExchange-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
            File.Copy(sourceFileName, destinFileName);

            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Range oRange;

            //oXL.Visible = true;
            oXL.DisplayAlerts = false;

            string workbookPath = @"C:\ebay\FileExchangeTest13.csv";
            string newworkbookPath = @"c:\ebay\FileExchangeTest14.csv";

            //Microsoft.Office.Interop.Excel.Workbook oWB = oXL.Workbooks.Open(workbookPath,
            //        0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, ",",
            //        true, false, 0, true, false, false);

            Microsoft.Office.Interop.Excel.Workbook oWB = oXL.Workbooks.Open(
                                        destinFileName,               // Filename
                                        0,
                                        Type.Missing,
                                        Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV,   // Format
                                        Type.Missing,
                                        Type.Missing,
                                        Type.Missing,
                                        Type.Missing,
                                        ",",          // Delimiter
                                        Type.Missing,
                                        Type.Missing,
                                        Type.Missing,
                                        //Type.Missing,
                                        Type.Missing,
                                        Type.Missing);

            Microsoft.Office.Interop.Excel.Sheets oSheets = oWB.Worksheets;
            Microsoft.Office.Interop.Excel.Worksheet oSheet = oWB.ActiveSheet;

            int i = 2;
            foreach (Product product in products)
            {
                oSheet.Cells[i, 1] = "VerifyAdd";

                string temp = GetEbayCategoryIDAndPrice(product.Name);
                string eBayCategoryId = temp.Split('|')[0];
                string eBayPrice = temp.Split('|')[1];

                oSheet.Cells[i, 2] = eBayCategoryId.ToString();

                oSheet.Cells[i, 3] = product.Name;
                oSheet.Cells[i, 5] = product.Details + "<br>" + product.Specification;
                oSheet.Cells[i, 6] = "1000";
                oSheet.Cells[i, 7] = product.ImageLink;
                oSheet.Cells[i, 8] = "1";
                oSheet.Cells[i, 9] = "FixedPrice";
                oSheet.Cells[i, 10] = GeneratePrice(Convert.ToDouble(product.Price), Convert.ToDouble(eBayPrice)).ToString();
                oSheet.Cells[i, 12] = "30";
                oSheet.Cells[i, 13] = "1";
                oSheet.Cells[i, 14] = "AL USA";
                oSheet.Cells[i, 16] = "1";
                oSheet.Cells[i, 17] = "zjding@outlook.com";
                oSheet.Cells[i, 22] = "Flat";
                oSheet.Cells[i, 23] = "USPSPriority";
                oSheet.Cells[i, 24] = "0";
                oSheet.Cells[i, 25] = "1";
                oSheet.Cells[i, 31] = "1";
                oSheet.Cells[i, 33] = "ReturnsNotAccepted";

                i++;
            }

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            MessageBox.Show("Done");

            //releaseObject(xlWorkSheet);
            //releaseObject(xlWorkBook);
            //releaseObject(xlApp);

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //destinFileName = @"C:\ebay\Upload\FileExchange-2016-04-07-20-37-41.csv";
            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);
        }

        private string ProcessHtml(string inputSt)
        {
            //string pattern = "/[\n\r]+/g";
            //Regex rgx = new Regex(pattern);
            //string outputSt = rgx.Replace(inputSt, "''");

            //outputSt = outputSt.Replace("\"", "'");

            string outputSt = inputSt.Replace("\n", "");
            outputSt = outputSt.Replace("\t", "");
            outputSt = outputSt.Replace("\\\"", "'");

            return outputSt;
        }

        private void btnListedProducts_Click(object sender, EventArgs e)
        {
            ListedProducts listedProducts = new ListedProducts();
            listedProducts.ShowDialog();
        }

        private void btnCreatePDF_Click(object sender, EventArgs e)
        {
            string pdfTemplateFileName = @"c:\ebay\documents\" + "TaxExemption_Form.pdf";
            string newFileName = @"c:\ebay\TaxExemption\TaxExemption-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".pdf";
            PdfReader pdfReader = new PdfReader(pdfTemplateFileName);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFileName, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            pdfFormFields.SetField("txtLegalBusinessName", "eBay Business");
            pdfFormFields.SetField("txtBusinessAddress", "1642 Crossgate Dr., Vestavia");

            pdfStamper.FormFlattening = true;
            pdfStamper.Close();
            pdfReader.Close();


            List<string> fileNames = new List<string>();
            fileNames.Add(@"c:\ebay\TaxExemption\TaxExemptionTotal.pdf");
            fileNames.Add(newFileName);

            string tempFileName = @"c:\ebay\TaxExemption\TaxExemptionTemp.pdf";

            MergePDFs(fileNames, tempFileName);
        }

        public bool MergePDFs(IEnumerable<string> fileNames, string targetPdf)
        {
            bool merged = true;
            using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (string file in fileNames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            return merged;
        }

        private string GetProductionDescription(string url)
        {
            string result = "";

            IWebDriver driver = new FirefoxDriver();

            driver.Navigate().GoToUrl(url);

            IWebElement element = driver.FindElement(By.Id("product-tab1"));
            //string elementHtml = element.GetAttribute("outerHTML");

            //driver.Dispose();

            var pageTypeElements = element.FindElements(By.Id("wc-power-page"));

            if (pageTypeElements.Count > 0)
            {
                result = ProcessPowerPage(pageTypeElements[0]);
            }

            driver.Dispose();

            return result;

        }

        private string ProcessInlineProduct(IWebElement inputElement)
        {
            string html = inputElement.GetAttribute("outerHTML");

            return html;
        }

        private string ProcessPowerPage(IWebElement inputElement)
        {
            string outputHtml = "<div style = 'list-style-type: none; font-family: Verdana,sans-serif; font-size: 15px; color: grey'>";

            var lis = inputElement.FindElements(By.XPath("//li[contains(@class, 'wc-rich-feature-item')]"));

            foreach (IWebElement li in lis)
            {
                string lihtml = li.GetAttribute("outerHTML");

                var videos = li.FindElements(By.XPath(".//img[contains(@data-asset-type, 'video')]"));

                if (videos.Count > 0)
                {
                    string vhtml = videos[0].GetAttribute("outerHTML");
                    lihtml = lihtml.Replace(vhtml, "");
                }

                var iframes = li.FindElements(By.XPath(".//iframe"));

                if (iframes.Count > 0)
                {
                    string ihtml = iframes[0].GetAttribute("outerHTML");
                    lihtml = lihtml.Replace(ihtml, "");
                }

                var enlarges = li.FindElements(By.XPath(".//a[contains(@class, 'wc-zoom-image')]"));

                if (enlarges.Count > 0)
                {
                    string ehtml = enlarges[0].GetAttribute("outerHTML");
                    lihtml = lihtml.Replace(ehtml, "");
                }

                outputHtml += lihtml;
            }

            outputHtml += "</div>";

            return outputHtml;
        }

        private void btnFillWebForm_Click(object sender, EventArgs e)
        {
            IWebDriver driver = new FirefoxDriver();

            //string url = "http://www.costco.com/.product.100244524.html";
            string url = "http://www.costco.com/.product.100099102.html?cm_sp=RichRelevance-_-itempageVerticalRight-_-CategorySiloedViewCP&cm_vc=itempageVerticalRight|CategorySiloedViewCP";

            driver.Navigate().GoToUrl(url);

            IWebElement element = driver.FindElement(By.Id("product-tab1"));
            //string elementHtml = element.GetAttribute("outerHTML");

            //driver.Dispose();

            var pageTypeElements = element.FindElements(By.Id("wc-power-page"));

            if (pageTypeElements.Count > 0)
            {
                string output = ProcessPowerPage(pageTypeElements[0]);

                return;
            }

            pageTypeElements = element.FindElements(By.Id("sp_inline_product"));

            if (pageTypeElements.Count > 0)
            {
                var scripts = pageTypeElements[0].FindElements(By.TagName("script"));

                string sshtml = pageTypeElements[0].GetAttribute("outerHTML");

                foreach (IWebElement script in scripts)
                {
                    string shtml = script.GetAttribute("outerHTML");
                }

                var objects = pageTypeElements[0].FindElements(By.XPath(".//[local-name()='object']"));

                foreach (IWebElement obj in objects)
                {

                    string ohtml = obj.GetAttribute("outerHTML");
                }
            }

            driver.Dispose();

            //driver.Navigate().GoToUrl("https://www.costco.com/LogonForm");
            //driver.FindElement(By.Id("logonId")).SendKeys("zjding@gmail.com");
            //driver.FindElement(By.Id("logonPassword")).SendKeys("721123");
            //driver.FindElements(By.ClassName("submit"))[2].Click();

            //driver.Navigate().GoToUrl("http://www.costco.com/.product.100244524.html");
            //driver.FindElement(By.Id("addToCartBtn")).Click();

            //driver.Navigate().GoToUrl("https://www.costco.com/CheckoutCartView?langId=-1&storeId=10301&catalogId=10701&orderId=.");
            //driver.SwitchTo().Alert().Accept();
            //driver.FindElement(By.Id("shopCartCheckoutSubmitButton")).Click();





            //driver.FindElement(By.XPath("//button[@id='addToCartBtn']")).Click();
        }

        private void btnResearch_Click(object sender, EventArgs e)
        {
            List<string> sellers = new List<string>();

            //"http://feedback.ebay.com/ws/eBayISAPI.dll?ViewFeedback2&ftab=FeedbackAsSeller&userid=bigmayer5&iid=400936057158&de=off&items=25&interval=0&mPg=2112&keyword=400936057158&page="
            //string feedbackUrl1 = "http://feedback.ebay.com/ws/eBayISAPI.dll?ViewFeedback2&ftab=FeedbackAsSeller&userid=netspecials&iid=172000117265&de=off&items=200&interval=0&mPg=79&keyword=172000117265&page=";
            string feedbackUrl1 = "http://feedback.ebay.com/ws/eBayISAPI.dll?ViewFeedback2&ftab=FeedbackAsSeller&userid=infinity_giving_works&iid=111637054229&de=off&items=25&interval=0&mPg=598&keyword=111637054229&page=";

            int iUserStart = feedbackUrl1.IndexOf("userid=");
            int iUserEnd = feedbackUrl1.IndexOf("&", iUserStart);
            string UserId = feedbackUrl1.Substring(iUserStart + 7, iUserEnd - iUserStart - 7);

            for (int i = 1; i <= 5; i++)
            {
                sellers.Add(feedbackUrl1 + i.ToString());
            }

            IWebDriver driver = new FirefoxDriver();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString;

            sqlString = "TRUNCATE TABLE eBay_ProductsResearch";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();


            foreach (string url in sellers)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);

                    var trs = driver.FindElements(By.XPath("//tr[contains(@class, 'bot')]"));

                    foreach (IWebElement tr in trs)
                    {
                        var tds = tr.FindElements(By.XPath(".//td"));

                        string Name = tds[1].Text;

                        if (Name == "")
                            continue;

                        int iStart = Name.IndexOf("(");
                        int iEnd = Name.IndexOf(")");

                        Name = Name.Substring(0, iStart);
                        Name = Name.Replace("'", "*");

                        string Price = tds[2].Text;

                        Price = Price.Replace("US $", "");

                        IWebElement a = tds[3].FindElement(By.TagName("a"));

                        string Url = a.GetAttribute("href");

                        // Insert to DB
                        sqlString = "SELECT Name FROM eBay_ProductsResearch WHERE Name = '" + Name + "' AND eBayUserId = '" + UserId + "'" ;

                        cmd.CommandText = sqlString;
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Close();
                            continue;
                        }
                        reader.Close();

                        sqlString = "INSERT INTO eBay_ProductsResearch (Name, eBayUrl, eBayPrice, eBayUserId) VALUES ('" +
                                        Name + "', '" +
                                        Url + "', '" +
                                        Price + "', '" +
                                        UserId + "')";

                        cmd.CommandText = sqlString;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            List<string> eBayUrls = new List<string>();

            sqlString = "SELECT eBayUrl FROM eBay_ProductsResearch";

            cmd.CommandText = sqlString;
            SqlDataReader reader1 = cmd.ExecuteReader();
            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    string eBayUrl = reader1.GetString(0);

                    eBayUrls.Add(eBayUrl);
                }
            }

            reader1.Close();

            foreach (string eBayUrl in eBayUrls)
            {

                driver.Navigate().GoToUrl(eBayUrl);

                var spans = driver.FindElements(By.XPath("//span[contains(@class, 'vi-qtyS-hot-red  vi-bboxrev-dsplblk vi-qty-vert-algn vi-qty-pur-lnk')]"));

                if (spans.Count == 0)
                    spans = driver.FindElements(By.XPath("//span[contains(@class, 'vi-qtyS vi-bboxrev-dsplblk vi-qty-vert-algn vi-qty-pur-lnk')]"));

                if (spans.Count > 0)
                {
                    var a = spans[0].FindElement(By.TagName("a"));
                    if (a != null)
                    {
                        string num = a.Text.Replace("sold", "");
                        num = num.TrimEnd();
                        num = num.TrimStart();
                        num = num.Replace(",", "");

                        sqlString = "UPDATE eBay_ProductsResearch SET eBaySoldNumber = " + num + " WHERE eBayUrl = '" + eBayUrl + "'";
                        cmd.CommandText = sqlString;
                        cmd.ExecuteNonQuery();
                    }
                }

            }

            cn.Close();
            driver.Dispose();

        }

        private void btnEmailExtract_Click(object sender, EventArgs e)
        {
            string html = @"<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns:m='http://schemas.microsoft.com/office/2004/12/omml' xmlns='http://www.w3.org/TR/REC-html40'><head><meta name=Generator content='Microsoft Word 15 (filtered medium)'><!--[if !mso]><style>v:* {behavior:url(#default#VML);}                            o:* {behavior:url(#default#VML);}                            w:* {behavior:url(#default#VML);}                            .shape {behavior:url(#default#VML);}                            </style><![endif]--><style><!--                            /* Font Definitions */                            @font-face                            {font-family:'Cambria Math';                            panose-1:2 4 5 3 5 4 6 3 2 4;}                            @font-face                            {font-family:DengXian;                            panose-1:2 1 6 0 3 1 1 1 1 1;}                            @font-face                            {font-family:Calibri;                            panose-1:2 15 5 2 2 2 4 3 2 4;}                            @font-face                            {font-family:'@DengXian';                            panose-1:2 1 6 0 3 1 1 1 1 1;}                            /* Style Definitions */                            p.MsoNormal, li.MsoNormal, div.MsoNormal                            {margin:0in;                            margin-bottom:.0001pt;                            font-size:12.0pt;                            font-family:'Times New Roman',serif;}                            a:link, span.MsoHyperlink                            {mso-style-priority:99;                            color:blue;                            text-decoration:underline;}                            a:visited, span.MsoHyperlinkFollowed                            {mso-style-priority:99;                            color:purple;                            text-decoration:underline;}                            p.msonormal0, li.msonormal0, div.msonormal0                            {mso-style-name:msonormal;                            mso-margin-top-alt:auto;                            margin-right:0in;                            mso-margin-bottom-alt:auto;                            margin-left:0in;                            font-size:12.0pt;                            font-family:'Times New Roman',serif;}                            span.EmailStyle18                            {mso-style-type:personal;                            font-family:'Calibri',sans-serif;                            color:windowtext;}                            span.EmailStyle20                            {mso-style-type:personal-reply;                            font-family:'Calibri',sans-serif;                            color:windowtext;}                            .MsoChpDefault                            {mso-style-type:export-only;                            font-size:10.0pt;}                            @page WordSection1                            {size:8.5in 11.0in;                            margin:1.0in 1.0in 1.0in 1.0in;}                            div.WordSection1                            {page:WordSection1;}                            --></style><!--[if gte mso 9]><xml>                            <o:shapedefaults v:ext='edit' spidmax='1026' />                            </xml><![endif]--><!--[if gte mso 9]><xml>                            <o:shapelayout v:ext='edit'>                            <o:idmap v:ext='edit' data='1' />                            </o:shapelayout></xml><![endif]--></head><body lang=EN-US link=blue vlink=purple><div class=WordSection1><p class=MsoNormal style='margin-bottom:12.0pt'><o:p>&nbsp;</o:p></p><div><div><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%'><tr><td style='padding:0in 0in 0in 0in'></td></tr></table></div><p class=MsoNormal><o:p>&nbsp;</o:p></p><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=600 style='width:6.25in;color:#333333!important'><tr><td width='100%' valign=top style='width:100.0%;padding:0in 0in 0in 0in'><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=600 style='width:6.25in'><tr><td valign=top style='padding:0in 0in 0in 0in'><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><img id='_x0000_i1025' src='http://images.paypal.com/en_US/i/logo/paypal_logo.gif' alt='PayPal logo'><o:p></o:p></span></p></td><td style='padding:0in 0in 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>Jun 12, 2014 18:02:31 PDT<br>Transaction ID: <a href='https://www.paypal.com/us/cgi-bin/webscr?cmd=_view-a-trans&amp;id=6J8040028M3004619' target='_blank'>6J8040028M3004619</a><o:p></o:p></span></p></td></tr></table></div><div style='margin-top:22.5pt;color:#333!important'><p class=MsoNormal><b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>Hello Yue Zhang,</span></b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><br><br></span><b><span style='font-size:10.5pt;font-family:'Arial',sans-serif;color:#C88039'>You received a payment of $14.95 USD from nanetrawl (<a href='mailto:nanetcrawl@aol.com' target='_blank'>nanetcrawl@aol.com</a>)</span></b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p></o:p></span></p><table class=MsoNormalTable border=0 cellpadding=0><tr><td valign=top style='padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class=MsoNormal>Thanks for using PayPal. You can now ship any items.To see all the transaction details, log in to your PayPal account.<br><br>It may take a few moments for this transaction to appear in your account.<br><br><b><span style='color:#333333'>Seller Protection - </span></b><span style='color:#4C8F3A'>Eligible</span><o:p></o:p></p></td><td style='padding:3.75pt 3.75pt 3.75pt 3.75pt'></td></tr></table><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p>&nbsp;</o:p></span></p><div style='margin-top:3.75pt'><div class=MsoNormal align=center style='text-align:center'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><hr size=1 width='100%' align=center></span></div></div><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p>&nbsp;</o:p></span></p><table class=MsoNormalTable border=0 cellspacing=3 cellpadding=0 style='color:#333333!important'><tr><td style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='98%' style='width:98.0%'><tr><td width='50%' valign=top style='width:50.0%;padding:3.75pt 7.5pt 0in 0in'><p class=MsoNormal><b><span style='color:#333333'>Buyer</span></b><br>nanette crawley<br>nanetrawl<br><a href='mailto:nanetcrawl@aol.com' target='_blank'>nanetcrawl@aol.com</a><o:p></o:p></p></td><td valign=top style='padding:3.75pt 0in 0in 0in'><p class=MsoNormal><b><span style='color:#333333'>Note to seller</span></b><br>The buyer hasn't sent a note.<o:p></o:p></p></td></tr><tr><td width='40%' valign=top style='width:40.0%;padding:7.5pt 0in 0in 0in'><p class=MsoNormal><b><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>Shipping address - </span></b><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#4C8F3A'>confirmed</span><br>nanette a crawley<br>1040 buena Rd<br>Lake forest,&nbsp;IL&nbsp;60045<br>United States<o:p></o:p></p></td><td valign=top style='padding:7.5pt 0in 0in 0in'><p class=MsoNormal><b><span style='color:#333333'>Shipping details</span></b><br>You haven’t added any shipping details.<o:p></o:p></p></td></tr></table><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p>&nbsp;</o:p></span></p><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='color:#333!important'><tr><td width=348 style='width:261.0pt;border-top:solid #CCCCCC 1.0pt;border-left:none;border-bottom:solid #CCCCCC 1.0pt;border-right:none;padding:0in 0in 0in 0in 10px!important'><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>Description<o:p></o:p></span></p></td><td width=100 style='width:75.0pt;border-top:solid #CCCCCC 1.0pt;border-left:none;border-bottom:solid #CCCCCC 1.0pt;border-right:none;padding:0in 0in 0in 0in 10px!important'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>Unit price<o:p></o:p></span></p></td><td width=50 style='width:37.5pt;border-top:solid #CCCCCC 1.0pt;border-left:none;border-bottom:solid #CCCCCC 1.0pt;border-right:none;padding:0in 0in 0in 0in 10px!important'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>Qty<o:p></o:p></span></p></td><td width=80 style='width:60.0pt;border-top:solid #CCCCCC 1.0pt;border-left:none;border-bottom:solid #CCCCCC 1.0pt;border-right:none;padding:0in 0in 0in 0in 10px!important'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>Amount<o:p></o:p></span></p></td></tr><tr><td valign=top style='padding:7.5pt 7.5pt 7.5pt 7.5pt'><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><a href='http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item=200781909849' target='_blank'>~ICY PINK HEART~ Pendant Necklace with Swarovski Crystals</a><br>Item# 200781909849<o:p></o:p></span></p></td><td valign=top style='padding:7.5pt 7.5pt 7.5pt 7.5pt'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>$14.95 USD<o:p></o:p></span></p></td><td valign=top style='padding:7.5pt 7.5pt 7.5pt 7.5pt'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>1<o:p></o:p></span></p></td><td valign=top style='padding:7.5pt 7.5pt 7.5pt 7.5pt'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>$14.95 USD<o:p></o:p></span></p></td></tr></table></div><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p>&nbsp;</o:p></span></p><div align=center><table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width=595 style='width:446.25pt;border-top:solid #CCCCCC 1.0pt;border-left:none;border-bottom:solid #CCCCCC 1.0pt;border-right:none'><tr><td valign=top style='border:none;padding:0in 0in 0in 0in'><p class=MsoNormal><img border=0 id='_x0000_i1027' src='https://securepics.ebaystatic.com/aw/pics/logos/paypal/logo_ebay_62x26.gif'><o:p></o:p></p></td><td style='border:none;padding:0in 0in 0in 0in'><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 align=right style='margin-top:3.0pt;color:#333333!important'><tr><td width=390 style='width:292.5pt;padding:0in 7.5pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>Shipping and handling<o:p></o:p></span></p></td><td width=90 style='width:67.5pt;padding:0in 3.75pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>$0.00 USD<o:p></o:p></span></p></td></tr><tr><td width=390 style='width:292.5pt;padding:0in 7.5pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>Total</span></b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p></o:p></span></p></td><td width=90 style='width:67.5pt;padding:0in 3.75pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>$14.95 USD<o:p></o:p></span></p></td></tr><tr><td width=390 style='width:292.5pt;padding:15.0pt 7.5pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'>Payment</span></b><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p></o:p></span></p></td><td width=90 style='width:67.5pt;padding:15.0pt 3.75pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif;color:#333333'>$14.95 USD<o:p></o:p></span></p></td></tr><tr><td width=390 style='width:292.5pt;padding:0in 7.5pt 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><br>Payment sent to <a href='mailto:coffeebean217@gmail.com' target='_blank'>coffeebean217@gmail.com</a><o:p></o:p></span></p></td><td width=90 style='width:67.5pt;padding:15.0pt 3.75pt 0in 0in'></td></tr><tr style='height:7.5pt'><td colspan=2 style='padding:0in 0in 0in 0in;height:7.5pt'></td></tr></table></td></tr></table></div><p class=MsoNormal align=center style='text-align:center'><o:p></o:p></p></td></tr></table><p class=MsoNormal><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><img border=0 id='_x0000_i1028' src='http://images.paypal.com/en_US/i/icon/icon_help_16x16.gif'>Questions? Go to the Help Center at: <a href='https://www.paypal.com/help' target='_blank'>www.paypal.com/help</a>.<br><br></span><span style='font-size:8.5pt;font-family:'Arial',sans-serif;color:#333333'>Please do not reply to this email. This mailbox is not monitored and you will not receive a response. For assistance, log in to your PayPal account and click <strong><span style='font-family:'Arial',sans-serif'>Help</span></strong> in the top right corner of any PayPal page.</span><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><br><br></span><span style='font-size:8.5pt;font-family:'Arial',sans-serif;color:#333333'>You can receive plain text emails instead of HTML emails. To change your Notifications preferences, log in to your account, go to your Profile, and click <strong><span style='font-family:'Arial',sans-serif'>My settings</span></strong>.</span><span style='font-size:9.0pt;font-family:'Arial',sans-serif'><o:p></o:p></span></p></div><p class=MsoNormal><br><br><span style='font-size:8.5pt;font-family:'Arial',sans-serif;color:#333333'>PayPal Email ID PP753 - 534ee8f139d8d</span><img border=0 width=1 height=1 style='width:.0104in;height:.0104in' id='_x0000_i1029' src='https://paypal.112.2o7.net/b/ss/paypalglobal/1/H.22--NS/1403163154?c6=62T71479F9328045D&amp;v0=PP753_0_&amp;pe=lnk_o&amp;pev1=email&amp;pev2=D=v0&amp;events=scOpen'><o:p></o:p></p></td></tr></table></div></div></div><p class=MsoNormal><o:p>&nbsp;</o:p></p></div></body></html>";

            string body = SubstringInBetween(html, "<body", @"</body>", true, true);

            // TransactionID
            string stTime = SubstringEndBack(body, "PDT", ">", false, true);

            string stTransactionID = SubstringInBetween(body, "Transaction ID:", "</a>", true, true);

            stTransactionID = SubstringEndBack(stTransactionID, "</a>", ">", false, false);

            // Buyer Name
            string stBuyer = SubstringInBetween(body, "Buyer", @"</a>", true, true);

            string stFullName = SubstringInBetween(stBuyer, "<br>", "<br>", false, false);

            stBuyer = stBuyer.Replace("<br>" + stFullName + "<br>", "");

            string stUserID = SubstringInBetween(stBuyer, @"</b>", "<br>", false, false);

            string stUserEmail = SubstringEndBack(stBuyer, @"</a>", ">", false, false);

            // Shipping Address
            string stShippingAddress = SubstringInBetween(body, "Shipping address", "<o:p>", true, false);

            string stShippingName = SubstringInBetween(stShippingAddress, "<br>", "<br>", false, false);

            stShippingAddress = stShippingAddress.Replace("<br>" + stShippingName, "");

            string stShippingAddress1 = SubstringInBetween(stShippingAddress, "<br>", "<br>", false, false);

            stShippingAddress = stShippingAddress.Replace("<br>" + stShippingAddress1, "");

            string stShippingAddress2 = SubstringInBetween(stShippingAddress, "<br>", "<br>", false, false);

            string stShippingCity = stShippingAddress2.Substring(0, stShippingAddress2.IndexOf(","));

            string stShippingState = SubstringInBetween(stShippingAddress2, "&nbsp;", "&nbsp;", false, false);

            stShippingAddress2 = stShippingAddress2.Replace(stShippingCity, "");
            stShippingAddress2 = stShippingAddress2.Replace(stShippingState, "");
            stShippingAddress2 = stShippingAddress2.Replace("&nbsp;", "");
            stShippingAddress2 = stShippingAddress2.Replace(",", "");

            string stShippingZip = stShippingAddress2;

            // Buyer note
            string stBuyerNote = SubstringInBetween(body, "Note to seller", "<o:p>", false, true);
            stBuyerNote = SubstringInBetween(stBuyerNote, "<br>", "<o:p>", false, false);

            // Item 
            string stItemNum = SubstringInBetween(body, "Item#", "<o:p>", false, false);
            stItemNum = stItemNum.Trim();

            string stItemName = SubstringInBetween(body, "<a href='http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item=" + stItemNum + "' target='_blank'>", @"</a>", false, false);

            // Amount
            string stAmount = SubstringInBetween(body, "Item# " + stItemNum, @"</table>", false, false);

            stAmount = TrimTags(stAmount);

            string stUnitePrice = stAmount.Substring(0, stAmount.IndexOf("<"));

            stAmount = stAmount.Substring(stUnitePrice.Length);

            stAmount = TrimTags(stAmount);

            string stQuatity = stAmount.Substring(0, stAmount.IndexOf("<"));

            stAmount = stAmount.Substring(stQuatity.Length);

            stAmount = TrimTags(stAmount);

            string stTotal = stAmount.Substring(0, stAmount.IndexOf("<"));
        }

        private string SubstringInBetween(string input, string start, string end, bool bIncludeStart, bool bIncludeEnd)
        {
            int iStart = input.IndexOf(start);

            if (bIncludeStart)
                input = input.Substring(iStart);
            else
                input = input.Substring(iStart + start.Length);

            int iEnd = input.IndexOf(end);

            if (bIncludeEnd)
                input = input.Substring(0, iEnd + end.Length);
            else
                input = input.Substring(0, iEnd);

            return input;
        }

        private string SubstringEndBack(string input, string end, string start, bool bIncludeStart, bool bIncludeEnd)
        {
            int iEnd = input.IndexOf(end);

            if (bIncludeEnd)
                input = input.Substring(0, iEnd + end.Length);
            else
                input = input.Substring(0, iEnd);

            int iStart = input.LastIndexOf(">");

            if (bIncludeStart)
                input = input.Substring(iStart, input.Length - iStart);
            else
                input = input.Substring(iStart + start.Length, input.Length - iStart - start.Length);

            return input;
        }

        private string TrimTags(string input)
        {
            int iStart = input.IndexOf("<");
            string stTag;
            input = input.Substring(iStart);

            while (input.IndexOf("<") == 0)
            {
                stTag = SubstringInBetween(input, "<", ">", true, true);
                input = input.Substring(stTag.Length);
            }

            return input;
        }

        
    }
}
