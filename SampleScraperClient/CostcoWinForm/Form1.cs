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

        string emailMessage;

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

            this.lblTotal.Text = "/ " + productUrlArray.Count.ToString();

            int i = 1;

            foreach (string productUrl in productUrlArray)
            {
                try
                {
                    this.lblcurrent.Text = i.ToString();
                    i++;

                    string UrlNum = productUrl.Substring(0, productUrl.LastIndexOf('.'));
                    UrlNum = UrlNum.Substring(UrlNum.LastIndexOf('.') + 1);

                    PageResult = Browser.NavigateToPage(new Uri(productUrl));

                    if (PageResult.Html.InnerText.Contains("Product Not Found"))
                        continue;

                    string stSubCategories = "";

                    HtmlNode category = PageResult.Html.SelectSingleNode("//ul[@itemprop='breadcrumb']");

                    List<HtmlNode> subCategories = category.SelectNodes("li").ToList(); 

                    foreach(HtmlNode subCategory in subCategories)
                    {
                        string temp = subCategory.InnerText.Replace("\n", "");
                        temp = temp.Replace("\t", "");
                        stSubCategories += temp + "|";
                    }
                    stSubCategories = stSubCategories.Substring(0, stSubCategories.Length - 1);

                    HtmlNode productInfo = PageResult.Html.CssSelect(".product-info").ToList<HtmlNode>().First();

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
                            int nStar = shString.IndexOf("*");
                            shString = shString.Substring(nDollar + 1, nStar - nDollar - 1);
                            shString = shString.Replace(" ", "");
                            shipping = shString;
                        }
                    }


                    //List<HtmlNode> productsNode = col1Node.CssSelect(".products").ToList<HtmlNode>();
                    //if (productsNode.Count > 0)
                    //    shipping = productsNode[0].SelectSingleNode("li").SelectSingleNode("p") == null ? "-1" : productsNode[0].SelectSingleNode("li").SelectSingleNode("p").InnerText;
                    //else
                    //    shipping = "-1";

                    HtmlNode productDetailTabsNode = PageResult.Html.CssSelect(".product-detail-tabs").ToList<HtmlNode>().First();

                    var productDetailTab1Node = productDetailTabsNode.SelectSingleNode("//div[@id='product-tab1']");

                    var productDescriptionNode = productDetailTab1Node.SelectSingleNode("//p[@itemprop='description']");

                    string description = productDescriptionNode.InnerHtml;
                    description = description.Replace("???", "");
                    description = description.Replace("'", "\"");

                    var productDetailTab2Node = productDetailTabsNode.SelectSingleNode("//div[@id='product-tab2']");

                    string specification = "";
                    if (productDetailTab2Node != null)
                    {
                        specification = productDetailTab2Node.InnerHtml;
                        specification = specification.Replace("'", "\"");
                    }

                    HtmlNode imageColumnNode = PageResult.Html.CssSelect(".image-column").ToList<HtmlNode>().First();

                    //HtmlNode carouselWrapNode = imageColumnNode.CssSelect(".product-image-carousel").ToList<HtmlNode>().First();
                    //var thumbHolderNode = carouselWrapNode.SelectNodes("//ul[@id='thumb_holder']");

                    //var thumbNodes = thumbHolderNode.SelectNodes("/li");

                    HtmlNode imageNode = imageColumnNode.SelectSingleNode("//img[@itemprop='image']");

                    string imageUrl = (imageNode.Attributes["src"]).Value;

                    sqlString = "INSERT INTO Raw_ProductInfo (Name, UrlNumber, ItemNumber, Category, Price, Shipping, Discount, Details, Specification, ImageLink, Url) VALUES ('" + productName + "','" + UrlNum + "','" + itemNumber + "','" + stSubCategories + "',"  + price + "," + shipping + "," + "'" + discount + "','" + description + "','" + specification + "','" + imageUrl + "','" + productUrl + "')";
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

            GetProductInfo();
        }
    }
}
