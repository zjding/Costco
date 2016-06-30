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
using System.Collections;
using iTextSharp.text.html.simpleparser;
using System.Drawing.Imaging;

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

        public void ImportProducts()
        {
            GetDepartmentArray();

            GetSubCategoryUrls();

            GetProductUrls();

            GetProductInfo();

            PopulateTables();

            CompareProducts();

            ArchieveProducts();

            SendEmail();
        }

        private void btnImportProducts_Click(object sender, EventArgs e)
        {
            ImportProducts();
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

            sqlString = "TRUNCATE TABLE Costco_Departments";
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
                        sqlString = "INSERT INTO Costco_Departments (DepartmentName, CategoryName, CategoryUrl) VALUES ('" +
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

        private void GetDepartmentArray()
        {
            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            sqlString = "SELECT CategoryUrl FROM Costco_Departments WHERE bInclude = 1";
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

            sqlString = "TRUNCATE TABLE Costco_Categories";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            //productUrlArray.Add("http://www.costco.com/ABC-and-123-Foam-Floor-Mat-Set%2c-36-Tiles-Set.product.11754291.html");

            IWebDriver driver = new FirefoxDriver();

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

                    int iCategory = 1;
                    string columns = "";
                    string values = "";
                    foreach (HtmlNode subCategory in subCategories)
                    {
                        string temp = subCategory.InnerText.Replace("\n", "");
                        temp = temp.Replace("\t", "");
                        temp = temp.Replace("'", "");
                        stSubCategories += temp + "|";

                        columns += "Category" + iCategory.ToString() + ",";
                        values += "'" + temp + "',";

                        iCategory++;
                    }
                    stSubCategories = stSubCategories.Substring(0, stSubCategories.Length - 1);
                    columns = columns.Substring(0, columns.Length - 1);
                    values = values.Substring(0, values.Length - 1);

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

                    

                    driver.Navigate().GoToUrl(productUrl);

                    IWebElement element = driver.FindElement(By.Id("product-tab1"));
                    //string elementHtml = element.GetAttribute("outerHTML");

                    //driver.Dispose();

                    //var pageTypeElements = element.FindElements(By.Id("wc-power-page"));

                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    // Create an Encoder object based on the GUID
                    // for the Quality parameter category.
                    System.Drawing.Imaging.Encoder myEncoder =
                        System.Drawing.Imaging.Encoder.Quality;

                    // Create an EncoderParameters object.
                    // An EncoderParameters object has an array of EncoderParameter
                    // objects. In this case, there is only one
                    // EncoderParameter object in the array.
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 70L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    var screenshotDriver = driver as ITakesScreenshot;
                    Screenshot screenshot = screenshotDriver.GetScreenshot();
                    var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));

                    System.Drawing.Rectangle cropArea;

                    if (element.FindElements(By.Id("wc-power-page")).Count > 0)
                    {
                        
                        //bmpScreen.Save(@"C:\temp\" + UrlNum + ".jpg");

                        IWebElement e = element.FindElement(By.Id("wc-power-page"));
                        Size s = e.Size;
                        s.Height = s.Height - 100;
                        cropArea = new System.Drawing.Rectangle(e.Location, s);

                        

                        description = ProcessPowerPage(element.FindElement(By.Id("wc-power-page")));
                    }
                    else if (element.FindElements(By.Id("sp_inline_product")).Count > 0)
                    {

                        IWebElement e = element.FindElement(By.Id("sp_inline_product"));

                        Size s = e.Size;
                        s.Height = s.Height - 60;

                        Point p = e.Location;
                        p.Y = p.Y + 30;

                        cropArea = new System.Drawing.Rectangle(p, s);

                        

                        description = ProcessInlineProduct(element.FindElement(By.Id("sp_inline_product")));
                    }
                    else
                    {
                        IWebElement e = element.FindElement(By.Id("product-tab1"));

                        Size s = e.Size;
                        s.Height = s.Height - 60;

                        Point p = e.Location;
                        p.Y = p.Y + 30;

                        cropArea = new System.Drawing.Rectangle(p, s);
                    }

                    bmpScreen.Clone(cropArea, bmpScreen.PixelFormat).Save(@"C:\temp\" + UrlNum + ".jpg", jpgEncoder, myEncoderParameters);

                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                        client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/" + UrlNum + ".jpg", "STOR", @"C:\temp\" + UrlNum + ".jpg");
                    }

                    // images
                    IWebElement imageElement = driver.FindElement(By.Id("thumb_holder"));

                    
                    int numImages = 1;
                    if (imageElement.FindElements(By.TagName("li")) != null)
                        numImages = imageElement.FindElements(By.TagName("li")).ToList().Count;

                    

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

                    sqlString = "INSERT INTO Raw_ProductInfo (Name, UrlNumber, ItemNumber, Category, Price, Shipping, Discount, Details, Specification, ImageLink, Url, NumberOfImage) VALUES ('" + productName + "','" + UrlNum + "','" + itemNumber + "','" + stSubCategories + "'," + price + "," + shipping + "," + "'" + discount + "','" + description + "','" + specification + "','" + imageUrl + "','" + productUrl + "'," + numImages.ToString() + ")";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();



                    sqlString = "INSERT INTO Costco_Categories (" + columns + ") VALUES (" + values + ")";
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

            driver.Dispose();

            //MessageBox.Show("Start: " + startDT.ToLongTimeString() + "; End: " + endDT.ToLongTimeString());
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void CompressImage(System.Drawing.Image sourceImage, int imageQuality, string savePath)
        {
            try
            {
                //Create an ImageCodecInfo-object for the codec information
                ImageCodecInfo jpegCodec = null;

                //Set quality factor for compression
                EncoderParameter imageQualitysParameter = new EncoderParameter(
                            System.Drawing.Imaging.Encoder.Quality, imageQuality);

                //List all avaible codecs (system wide)
                ImageCodecInfo[] alleCodecs = ImageCodecInfo.GetImageEncoders();

                EncoderParameters codecParameter = new EncoderParameters(1);
                codecParameter.Param[0] = imageQualitysParameter;

                //Find and choose JPEG codec
                for (int i = 0; i < alleCodecs.Length; i++)
                {
                    if (alleCodecs[i].MimeType == "image/jpeg")
                    {
                        jpegCodec = alleCodecs[i];
                        break;
                    }
                }

                //Save compressed image
                sourceImage.Save(savePath, jpegCodec, codecParameter);
            }
            catch (Exception e)
            {
                throw e;
            }
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
            //productUrlArray.Add("http://www.costco.com/.product.100244718.html");
            //productUrlArray.Add("http://www.costco.com/.product.100283609.html");
            productUrlArray.Add("http://www.costco.com/.product.100146130.html");
            productUrlArray.Add("http://www.costco.com/iRobot%C2%AE-Roomba%C2%AE-655-Pet-Series-Vacuum-Cleaning-Robot.product.100127929.html");
            productUrlArray.Add("http://www.costco.com/.product.100161934.html?cm_sp=RichRelevance-_-itempageVerticalRight-_-CategorySiloedViewCP&cm_vc=itempageVerticalRight|CategorySiloedViewCP");
            productUrlArray.Add("http://www.costco.com/Crest-Pro-Health-Advanced-Toothpaste-4ct7oz.product.100230824.html");
            productUrlArray.Add("http://www.costco.com/Aquverse-Single-Serve-Coffee-Brewer.product.100229235.html");
            //productUrlArray.Add("http://www.costco.com/iRobot%C2%AE-Roomba%C2%AE-655-Pet-Series-Vacuum-Cleaning-Robot.product.100127929.html");

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

        public bool isAlertPresents(ref IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }// try
            catch (Exception e)
            {
                return false;
            }// catch
        }

        private void btnFillWebForm_Click(object sender, EventArgs e)
        {
            //IWebDriver driver = new FirefoxDriver();

            ////string url = "http://www.costco.com/.product.100244524.html";
            //string url = "http://www.costco.com/.product.100099102.html?cm_sp=RichRelevance-_-itempageVerticalRight-_-CategorySiloedViewCP&cm_vc=itempageVerticalRight|CategorySiloedViewCP";

            //driver.Navigate().GoToUrl(url);

            //IWebElement element = driver.FindElement(By.Id("product-tab1"));
            ////string elementHtml = element.GetAttribute("outerHTML");

            ////driver.Dispose();

            //var pageTypeElements = element.FindElements(By.Id("wc-power-page"));

            //if (pageTypeElements.Count > 0)
            //{
            //    string output = ProcessPowerPage(pageTypeElements[0]);

            //    return;
            //}

            //pageTypeElements = element.FindElements(By.Id("sp_inline_product"));

            //if (pageTypeElements.Count > 0)
            //{
            //    var scripts = pageTypeElements[0].FindElements(By.TagName("script"));

            //    string sshtml = pageTypeElements[0].GetAttribute("outerHTML");

            //    foreach (IWebElement script in scripts)
            //    {
            //        string shtml = script.GetAttribute("outerHTML");
            //    }

            //    var objects = pageTypeElements[0].FindElements(By.XPath(".//[local-name()='object']"));

            //    foreach (IWebElement obj in objects)
            //    {

            //        string ohtml = obj.GetAttribute("outerHTML");
            //    }
            //}

            //driver.Dispose();

            IWebDriver driver = new FirefoxDriver();

            driver.Navigate().GoToUrl("https://www.costco.com/LogonForm");
            driver.FindElement(By.Id("logonId")).SendKeys("zjding@gmail.com");
            driver.FindElement(By.Id("logonPassword")).SendKeys("721123");
            driver.FindElements(By.ClassName("submit"))[2].Click();

            driver.Navigate().GoToUrl("http://www.costco.com/");
            driver.FindElement(By.Id("mini-shopping-cart")).Click();

            while (driver.FindElements(By.LinkText("Remove from cart")).Count > 0)
            {
                driver.FindElements(By.LinkText("Remove from cart"))[0].Click();
                System.Threading.Thread.Sleep(3000);
            }
            
            driver.Navigate().GoToUrl("http://www.costco.com/.product.100244524.html");
            driver.FindElement(By.Id("minQtyText")).Clear();
            driver.FindElement(By.Id("minQtyText")).SendKeys("2");
            driver.FindElement(By.Id("addToCartBtn")).Click();

            if (isAlertPresents(ref driver))
                driver.SwitchTo().Alert().Accept();

            driver.FindElement(By.Id("mini-shopping-cart")).Click();

            if (isAlertPresents(ref driver))
                driver.SwitchTo().Alert().Accept();

            driver.FindElement(By.Id("shopCartCheckoutSubmitButton")).Click();

            if (isAlertPresents(ref driver))
                driver.SwitchTo().Alert().Accept();

            driver.FindElement(By.Id("addressFormInlineFirstName")).SendKeys("Jason");
            driver.FindElement(By.Id("addressFormInlineLastName")).SendKeys("Ding");
            driver.FindElement(By.Id("addressFormInlineAddressLine1")).SendKeys("1642 Crossgate Dr.");
            driver.FindElement(By.Id("addressFormInlineCity")).SendKeys("Vestavia");

            driver.FindElement(By.XPath("//select[@id='" + "addressFormInlineState" + "']/option[contains(.,'" + "Alabama" + "')]")).Click();
            driver.FindElement(By.Id("addressFormInlineZip")).SendKeys("35216");           
            driver.FindElement(By.Id("addressFormInlinePhoneNumber")).SendKeys("2056175063");
            driver.FindElement(By.Id("addressFormInlineAddressNickName")).SendKeys(DateTime.Now.ToString());
            
            if (driver.FindElement(By.Id("saveAddressCheckboxInline")).Selected)
            {
                driver.FindElement(By.Id("saveAddressCheckboxInline")).Click();
            }

            driver.FindElement(By.Id("addressFormInlineButton")).Click();

            System.Threading.Thread.Sleep(3000);

            if (driver.FindElements(By.XPath("//span[contains(text(), 'Continue')]")).Count > 0)
            {
                driver.FindElement(By.XPath("//span[contains(text(), 'Continue')]")).Click();
            }

            driver.FindElement(By.Id("deliverySubmitButton")).Click();

            driver.FindElement(By.Id("cc_cvc")).SendKeys("0905");

            driver.FindElement(By.Id("paymentSubButtonBot")).Click();

            //driver.FindElement(By.Id("orderButton")).Click();
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
            string html = @"<div dir='ltr'><div class='gmail_quote'><br><div><div>

<table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td></td></tr></tbody></table><table align='center' border='0' cellpadding='0' cellspacing='0' width='600'><tbody><tr valign='top'><td width='100%'><table align='center' border='0' cellpadding='0' cellspacing='0' style='font-family:arial,helvetica,sans-serif;font-size:12px;color:rgb(51,51,51)!important' width='600'><tbody><tr valign='top'><td><img src='http://images.paypal.com/en_US/i/logo/paypal_logo.gif' border='0' alt='PayPal logo'></td><td valign='middle' align='right'>Jun 12, 2016 18:02:31 PDT<br>Transaction ID: <a href='https://www.paypal.com/us/cgi-bin/webscr?cmd=_view-a-trans&amp;id=6J8040028M3004619' target='_blank'>6J8040028M3004619</a></td></tr></tbody></table><div style='margin-top:30px;font-family:arial,helvetica,sans-serif;font-size:12px;color:rgb(51,51,51)!important'><span style='font-weight:bold;font-family:arial,helvetica,sans-serif;color:rgb(51,51,51)!important'>Hello Yue Zhang,</span><br><br><span style='font-size:14px;color:rgb(200,128,57);font-weight:bold;text-decoration:none'>You received a payment of $26.98 USD from nanetrawl (<a href='mailto:nanetcrawl@aol.com' target='_blank'>nanetcrawl@aol.com</a>)</span><br><table cellpadding='5'><tbody><tr><td valign='top'>Thanks for using PayPal. You can now ship any items.To see all the transaction details, log in to your PayPal account.<br><br>It may take a few moments for this transaction to appear in your account.<br><br><span style='font-weight:bold;color:rgb(51,51,51)'>Seller Protection - </span><span style='color:rgb(76,143,58)'>Eligible</span></td><td></td></tr></tbody></table><br><div style='margin-top:5px'><hr size='1'></div><br><table border='0' cellpadding='3' cellspacing='3' style='font-family:arial,helvetica,sans-serif;font-size:12px;color:rgb(51,51,51)!important'><tbody><tr><td><table border='0' cellpadding='0' cellspacing='0' width='98%'><tbody><tr><td style='padding-top:5px;padding-right:10px' valign='top' width='50%' align='left'><span style='color:rgb(51,51,51);font-weight:bold'>Buyer</span><br>nanette crawley<br>nanetrawl<br><a href='mailto:nanetcrawl@aol.com' target='_blank'>nanetcrawl@aol.com</a></td><td style='padding-top:5px' valign='top'><span style='color:rgb(51,51,51);font-weight:bold'>Note to seller</span><br>The buyer hasn't sent a note.</td></tr><tr><td style='padding-top:10px' valign='top' width='40%' align='left'><span style='font-family:arial,helvetica,sans-serif;font-size:12px'><span style='color:rgb(51,51,51);font-weight:bold'>Shipping address - </span><span style='color:rgb(76,143,58)'>confirmed</span></span><br>nanette a crawley<br>1040 buena Rd<br>Lake forest,&nbsp;IL&nbsp;60045<br>United States<br></td><td style='padding-top:10px' valign='top'><span style='color:rgb(51,51,51);font-weight:bold'>Shipping details</span><br>You haven’t added any shipping details.</td></tr></tbody></table><br><table align='center' border='0' cellpadding='0' cellspacing='0' style='clear:both;font-size:12px;font-family:arial,helvetica,sans-serif;color:rgb(51,51,51)!important' width='598px'><tbody><tr><td style='border-top-width:1px;border-bottom-width:1px;border-style:solid none;border-top-color:rgb(204,204,204);border-bottom-color:rgb(204,204,204);color:rgb(51,51,51);padding:5px 10px!important' width='348' align='left'>Description</td><td style='border-top-width:1px;border-bottom-width:1px;border-style:solid none;border-top-color:rgb(204,204,204);border-bottom-color:rgb(204,204,204);color:rgb(51,51,51);padding:5px 10px!important' width='100' align='right'>Unit price</td><td style='border-top-width:1px;border-bottom-width:1px;border-style:solid none;border-top-color:rgb(204,204,204);border-bottom-color:rgb(204,204,204);color:rgb(51,51,51);padding:5px 10px!important' width='50' align='right'>Qty</td><td style='border-top-width:1px;border-bottom-width:1px;border-style:solid none;border-top-color:rgb(204,204,204);border-bottom-color:rgb(204,204,204);color:rgb(51,51,51);padding:5px 10px!important' width='80' align='right'>Amount</td></tr><tr><td valign='top' align='left' style='border-bottom-style:none;padding:10px'><a href='http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item=152147767708' target='_blank'>Every Man Jack 2-in-1 Shampoo &#43; Conditioner and Body Wash Combos</a><br>Item# 152147767708</td><td valign='top' align='right' style='border-bottom-style:none;padding:10px'>$26.98 USD</td><td valign='top' align='right' style='border-bottom-style:none;padding:10px'>1</td><td valign='top' align='right' style='border-bottom-style:none;padding:10px'>$26.98 USD</td></tr></tbody></table><table align='center' border='0' cellpadding='0' cellspacing='0' style='clear:both;border-top-width:1px;border-top-style:solid;border-top-color:rgb(204,204,204);border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:rgb(204,204,204);margin:0px 0px 10px' width='595'><tbody><tr><td valign='top'><img src='https://securepics.ebaystatic.com/aw/pics/logos/paypal/logo_ebay_62x26.gif' border='0' style='margin-top: 10px;' alt=''></td><td><table border='0' cellpadding='0' cellspacing='0' style='clear:both;margin-top:10px;font-family:arial,helvetica,sans-serif;font-size:12px;color:rgb(51,51,51)!important' align='right'><tbody><tr><td style='width:390px;text-align:right;padding:0px 10px 0px 0px'>Shipping and handling</td><td align='right' style='width:90px;color:rgb(51,51,51);text-align:right;padding:0px 5px 0px 0px'>$0.00 USD</td></tr><tr><td style='width:390px;text-align:right;padding:0px 10px 0px 0px'><span style='font-weight:bold;color:rgb(51,51,51)!important'>Total</span></td><td style='width:90px;color:rgb(51,51,51);text-align:right;padding:0px 5px 0px 0px'>$26.98 USD</td></tr><tr><td style='width:390px;text-align:right;padding:20px 10px 0px 0px'><span style='font-weight:bold;color:rgb(51,51,51)!important'>Payment</span></td><td style='width:90px;color:rgb(51,51,51);text-align:right;padding:20px 5px 0px 0px'>$26.98 USD</td></tr><tr><td style='width:390px;text-align:right;padding:0px 10px 0px 0px'><br>Payment sent to <a href='mailto:coffeebean217@gmail.com' target='_blank'>coffeebean217@gmail.com</a></td><td style='width:90px;color:rgb(51,51,51);text-align:right;padding:20px 5px 0px 0px'></td></tr><tr><td colspan='2' style='height:10px'></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table><img src='http://images.paypal.com/en_US/i/icon/icon_help_16x16.gif' border='0' style='margin-right: 5px; vertical-align: middle;' alt=''>Questions? Go to the Help Center at: <a href='https://www.paypal.com/help' target='_blank'>www.paypal.com/help</a>.<br><br><span style='font-size:11px;color:rgb(51,51,51)'>Please do not reply to this email. This mailbox is not monitored and you will not receive a response. For assistance, log in to your PayPal account and click <strong>Help</strong> in the top right corner of any PayPal page.</span><br><br><span style='font-size:11px;color:rgb(51,51,51)'>You can receive plain text emails instead of HTML emails. To change your Notifications preferences, log in to your account, go to your Profile, and click <strong>My settings</strong>.</span></div><br><br><span style='color:rgb(51,51,51);font-family:arial,helvetica,sans-serif;font-size:11px'><span>PayPal Email ID PP753 - 534ee8f139d8d</span></span><img height='1' width='1' src='https://paypal.112.2o7.net/b/ss/paypalglobal/1/H.22--NS/1403163154?c6=62T71479F9328045D&amp;v0=PP753_0_&amp;pe=lnk_o&amp;pev1=email&amp;pev2=D=v0&amp;events=scOpen' border='0' alt=''></td></tr></tbody></table></div></div></div><br></div>
";
            ProcessPaymentReceivedEmail(html);
        }

        private void ProcessPaymentReceivedEmail(string html)
        {
            string body = html;

            // TransactionID
            string stTime = SubstringEndBack(body, "PDT", ">", false, true);

            DateTime dtTime = Convert.ToDateTime(stTime.Replace("PDT", "-0700"));

            string stTransactionID = SubstringInBetween(body, "Transaction ID:", "</a>", true, true);

            stTransactionID = SubstringEndBack(stTransactionID, "</a>", ">", false, false);

            // Buyer Name
            string stBuyer = SubstringInBetween(body, "Buyer", @"</a>", true, true);

            string stFullName = SubstringInBetween(stBuyer, "<br>", "<br>", false, false);

            stBuyer = stBuyer.Replace("<br>" + stFullName + "<br>", "");

            string stUserID = SubstringInBetween(stBuyer, @"</span>", "<br>", false, false);

            string stUserEmail = SubstringEndBack(stBuyer, @"</a>", ">", false, false);

            // Shipping Address
            string stShippingAddress = SubstringInBetween(body, "Shipping address", "</td>", true, false);

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
            string stBuyerNote = SubstringInBetween(body, "Note to seller", "</td>", false, true);
            stBuyerNote = SubstringInBetween(stBuyerNote, "<br>", "</td>", false, false);
            stBuyerNote = stBuyerNote.Replace("The buyer hasn't sent a note.", "");

            // Item 
            string stItemNum = SubstringInBetween(body, "Item#", "</td>", false, false);
            stItemNum = stItemNum.Trim();

            string stItemName = SubstringInBetween(body, "<a href='http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item=" + stItemNum + "' target='_blank'>", @"</a>", false, false);

            // Amount
            string stAmount = SubstringInBetween(body, "Item# " + stItemNum, @"</table>", false, false);

            stAmount = TrimTags(stAmount);

            string stUnitePrice = stAmount.Substring(0, stAmount.IndexOf("<"));
            stUnitePrice = stUnitePrice.Replace("$", "");
            stUnitePrice = stUnitePrice.Replace("USD", "");
            stUnitePrice = stUnitePrice.Trim();

            stAmount = stAmount.Substring(stUnitePrice.Length);

            stAmount = TrimTags(stAmount);

            string stQuatity = stAmount.Substring(0, stAmount.IndexOf("<"));

            stAmount = stAmount.Substring(stQuatity.Length);

            stAmount = TrimTags(stAmount);

            string stTotal = stAmount.Substring(0, stAmount.IndexOf("<"));
            stTotal = stTotal.Replace("$", "");
            stTotal = stTotal.Replace("USD", "");
            stTotal = stTotal.Trim();

            // Generate PDF for email
            File.WriteAllText(@"C:\temp\temp.html", body);

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("print.always_print_silent", true);

            IWebDriver driver = new FirefoxDriver(profile);

            driver.Navigate().GoToUrl(@"file:///C:/temp/temp.html");

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            js.ExecuteScript("window.print();");

            driver.Dispose();

            System.Threading.Thread.Sleep(3000);

            // Process files
            string[] files = Directory.GetFiles(@"C:\temp\tempPDF\");

            string sourceFileFullName = files[0];

            string sourceFileName = sourceFileFullName.Replace(@"C:\temp\tempPDF\", "");

            string destinationFileName = dtTime.ToString("yyyyMMddHHmmss") + "_" + stTransactionID + ".pdf";

            File.Move(sourceFileFullName, @"C:\temp\PaypalPaidEmails\" + destinationFileName);

            // db stuff
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"UPDATE eBay_SoldTransactions SET PaypalTransactionID = @_paypalTransactionID, 
                                PaypalPaidDateTime = @_paypalPaidDateTime, PaypalPaidEmailPdf = @_paypalPaidEmailPdf,
                                BuyerAddress1 = @_buyerAddress1, 
                                BuyerAddress2 = @_buyAddress2, BuyerState = @_buyerState, BuyerNote = @_buyerNote 
                                WHERE eBayItemNumber = @_eBayItemNumber AND BuyerID = @_buyerID";

            cmd.CommandText = sqlString;
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalPaidDateTime", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
            cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);




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

            int iStart = input.LastIndexOf(start);

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

        private void ProcessItemSoldEmail(string subject, string body)
        {
            string stItemNum = SubstringInBetween(subject, "(", ")", false, false);

            subject = subject.Replace("(" + stItemNum + ")", "");
            subject = subject.Replace("Your eBay item sold!", "");

            string stItemName = subject.Trim();

            string stUrl = SubstringEndBack(body, ">" + stItemName, "<a href=", false, false);

            stUrl = SubstringInBetween(stUrl, "'", "'", false, false);

            string stEndTime = SubstringInBetween(body, "End time:", "PDT", false, true);

            stEndTime = SubstringEndBack(stEndTime, "PDT", ">", false, true);

            string correctedTZ = stEndTime.Replace("PDT", "-0700");
            DateTime dt = Convert.ToDateTime(correctedTZ);

            string stPrice = SubstringInBetween(body, "Sale price:", "Quantity:", false, false);

            stPrice = SubstringInBetween(stPrice, "$", "<", false, false);

            string stQuantity = SubstringInBetween(body, "Quantity:", "Quantity sold:", false, false);

            stQuantity = TrimTags(stQuantity);

            stQuantity = stQuantity.Substring(0, stQuantity.IndexOf("<"));

            string stQuantitySold = SubstringInBetween(body, "Quantity sold:", "Quantity remaining:", false, false);

            stQuantitySold = TrimTags(stQuantitySold);

            stQuantitySold = stQuantitySold.Substring(0, stQuantitySold.IndexOf("<"));

            string stQuantityRemaining = SubstringInBetween(body, "Quantity remaining:", "Buyer:", false, false);

            stQuantityRemaining = TrimTags(stQuantityRemaining);

            stQuantityRemaining = stQuantityRemaining.Substring(0, stQuantityRemaining.IndexOf("<"));

            string stBuyerName = SubstringInBetween(body, "Buyer:", "<div>", false, false);

            stBuyerName = TrimTags(stBuyerName);

            stBuyerName = stBuyerName.Substring(0, stBuyerName.IndexOf("<"));

            string stBuyerId = SubstringInBetween(body, stBuyerName, "(<a href='mailto", false, true);

            stBuyerId = TrimTags(stBuyerId);

            stBuyerId = stBuyerId.Substring(0, stBuyerId.IndexOf("(<a href='mailto"));

            stBuyerId = stBuyerId.Trim();

            string stBuyerEmail = SubstringInBetween(body, "(<a href='mailto:", "'", false, false);

            // Generate PDF for email
            File.WriteAllText(@"C:\temp\temp.html", body);

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("print.always_print_silent", true);

            IWebDriver driver = new FirefoxDriver(profile);

            driver.Navigate().GoToUrl(@"file:///C:/temp/temp.html");

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            js.ExecuteScript("window.print();");

            driver.Dispose();

            System.Threading.Thread.Sleep(3000);

            // Process files
            string[] files = Directory.GetFiles(@"C:\temp\tempPDF\");

            string sourceFileFullName = files[0];

            string sourceFileName = sourceFileFullName.Replace(@"C:\temp\tempPDF\", "");

            string destinationFileName = dt.ToString("yyyyMMddHHmmss") + "_" + stItemNum + ".pdf";

            File.Move(sourceFileFullName, @"C:\temp\eBaySoldEmails\" + destinationFileName);

            // db stuff
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = @"SELECT * FROM eBay_CurrentListings WHERE eBayItemNumber = " + stItemNum;

            eBayListingProduct eBayProduct = new eBayListingProduct();

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();

                eBayProduct.Name = Convert.ToString(reader["Name"]);
                eBayProduct.eBayListingName = Convert.ToString(reader["eBayListingName"]);
                eBayProduct.eBayCategoryID = Convert.ToString(reader["eBayCategoryID"]);
                eBayProduct.eBayItemNumber = Convert.ToString(reader["eBayItemNumber"]);
                eBayProduct.eBayListingPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
                eBayProduct.eBayDescription = Convert.ToString(reader["eBayDescription"]);
                eBayProduct.eBayListingDT = Convert.ToDateTime(reader["eBayListingDT"]);
                eBayProduct.eBayUrl = Convert.ToString(reader["eBayUrl"]);
                eBayProduct.CostcoUrlNumber = Convert.ToString(reader["CostcoUrlNumber"]);
                eBayProduct.CostcoItemNumber = Convert.ToString(reader["CostcoItemNumber"]);
                eBayProduct.CostcoUrl = Convert.ToString(reader["CostcoUrl"]);
                eBayProduct.CostcoPrice = Convert.ToDecimal(reader["CostcoPrice"]);
                eBayProduct.ImageLink = Convert.ToString(reader["ImageLink"]);
            }
            reader.Close();

            // check exist

            bool bExist = false;

            sqlString = "SELECT * FROM eBay_SoldTransactions WHERE eBayItemNumber = " + stItemNum;
            cmd.CommandText = sqlString;
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
                bExist = true;
            reader.Close();

            if (!bExist)
            {
                sqlString = @"INSERT INTO eBay_SoldTransactions 
                              (eBayItemNumber, eBaySoldDateTime, eBayItemName, eBayUrl, eBayPrice, eBayListingQuality, eBaySoldQuality, eBayRemainingQuality, eBaySoldEmailPdf,
                               BuyerName, BuyerID, BuyerEmail, CostcoUrlNumber, CostcoUrl, CostcoPrice)
                              VALUES (@_eBayItemNumber, @_eBaySoldDateTime, @_eBayItemName, @_eBayUrl, @_eBayPrice, @_eBayListingQuality, @_eBaySoldQuality, @_eBayRemainingQuality, @_eBaySoldEmailPdf,
                               @_BuyerName, @_BuyerID, @_BuyerEmail, @_CostcoUrlNumber, @_CostcoUrl, @_CostcoPrice)";

                cmd.CommandText = sqlString;
                cmd.Parameters.AddWithValue("@_eBayItemNumber", stItemNum);
                cmd.Parameters.AddWithValue("@_eBaySoldDateTime", dt);
                cmd.Parameters.AddWithValue("@_eBayItemName", stItemName);
                cmd.Parameters.AddWithValue("@_eBayUrl", stUrl);
                cmd.Parameters.AddWithValue("@_eBayPrice", Convert.ToDecimal(stPrice));
                cmd.Parameters.AddWithValue("@_eBayListingQuality", Convert.ToInt16(stQuantity));
                cmd.Parameters.AddWithValue("@_eBaySoldQuality", Convert.ToInt16(stQuantitySold));
                cmd.Parameters.AddWithValue("@_eBayRemainingQuality", Convert.ToInt16(stQuantityRemaining));
                cmd.Parameters.AddWithValue("@_eBaySoldEmailPdf", destinationFileName);
                cmd.Parameters.AddWithValue("@_BuyerName", stBuyerName);
                cmd.Parameters.AddWithValue("@_BuyerID", stBuyerId);
                cmd.Parameters.AddWithValue("@_BuyerEmail", stBuyerEmail);
                cmd.Parameters.AddWithValue("@_CostcoUrlNumber", eBayProduct.CostcoUrlNumber);
                cmd.Parameters.AddWithValue("@_CostcoUrl", eBayProduct.CostcoUrl);
                cmd.Parameters.AddWithValue("@_CostcoPrice", eBayProduct.CostcoPrice);

                cmd.ExecuteNonQuery();
            }
            else
            {

            }

            cn.Close();
        }

        private void ProcessCostcoOrderEmail(string body)
        {
            body = body.Replace("\r", "");
            body = body.Replace("\t", "");
            body = body.Replace("\n", "");
            string stOrderNumber = SubstringInBetween(body, "Order Number:</td>", "</td>", false, true);
            stOrderNumber = SubstringEndBack(stOrderNumber, "</td>", ">", false, false);
            stOrderNumber = stOrderNumber.Trim();

            string stDatePlaced = SubstringInBetween(body, "Date Placed:</td>", "</td>", false, true);
            stDatePlaced = SubstringEndBack(stDatePlaced, "</td>", ">", false, false);
            stDatePlaced = stDatePlaced.Trim();

            string stWorking = SubstringInBetween(body, "Item Total", "Shipping &amp; Terms", false, false);
            stWorking = TrimTags(stWorking);

            string stQuatity = stWorking.Substring(0, stWorking.IndexOf("<"));
            stQuatity = stQuatity.Trim();

            stWorking = stWorking.Substring(stQuatity.Length);
            stWorking = TrimTags(stWorking);
            string stProductName = stWorking.Substring(0, stWorking.IndexOf("<"));
            stProductName = stProductName.Trim();

            string stItemNum = stProductName.Substring(stProductName.IndexOf("Item#"));

            stItemNum = stItemNum.Replace("Item#", "");
            stItemNum = stItemNum.Trim();

            string stShipping = SubstringInBetween(body, "Shipping Address", "Note:", false, false);

            string stBuyerName = TrimTags(stShipping);

            // Generate PDF for email
            string destinationFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + stOrderNumber;

            File.WriteAllText(@"C:\temp\" + @"\" + destinationFileName + ".html", body);

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("print.always_print_silent", true);

            IWebDriver driver = new FirefoxDriver(profile);

            driver.Navigate().GoToUrl(@"file:///" + @"C:\temp\" + @"\" + destinationFileName + ".html");

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            js.ExecuteScript("window.print();");

            driver.Dispose();

            System.Threading.Thread.Sleep(3000);

            // Process files
            string sourceFileName = @"C:\temp\tempPDF\file__C__temp_" + destinationFileName + @"\" + "file_C_temp_" + destinationFileName + ".pdf";

            File.Move(sourceFileName, @"C:\temp\CostcoOrderEmails\" + destinationFileName + ".pdf");

            File.Delete(@"C:\temp\" + destinationFileName + ".html");
            Directory.Delete(@"C:\temp\tempPDF\file__C__temp_" + destinationFileName);

            // db stuff
            string sqlString;
            bool bExist = false;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            if (stItemNum != "")
            {
                sqlString = @"SELECT * FROM eBay_SoldTransactions WHERE CostcoItemNumber = @_costcoItemNumber 
                                AND BuyerName = @_buyerName AND  CostcoOrderNumber IS NULL";

                cmd.CommandText = sqlString;
                cmd.Parameters.AddWithValue("@_costcoItemNumber", stItemNum);
                cmd.Parameters.AddWithValue("@_buyerName", stBuyerName);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bExist = true;
                }
                reader.Close();

                if (bExist)
                {
                    sqlString = @"UPDATE eBay_SoldTransactions SET CostcoOrderNumber = @_costcoOrderNumber,
                                CostcoOrderEmailPdf = @_costcoOrderEmailPdf
                                WHERE WHERE CostcoItemNumber = @_costcoItemNumber 
                                AND BuyerName = @_buyerName AND  CostcoOrderNumber IS NULL";

                    cmd.CommandText = sqlString;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_costcoOrderNumber", stOrderNumber);
                    cmd.Parameters.AddWithValue("@_costcoOrderEmailPdf", destinationFileName);
                    cmd.Parameters.AddWithValue("@_costcoItemNumber", stItemNum);
                    cmd.Parameters.AddWithValue("@_buyerName", stBuyerName);

                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                sqlString = @"SELECT * FROM eBay_SoldTransactions WHERE CostcoItemName = @_costcoItemName
                                AND CostcoOrderNumber IS NULL";

                cmd.CommandText = sqlString;
                cmd.Parameters.AddWithValue("@_costcoItemName", stProductName);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bExist = true;
                }
                reader.Close();

                if (bExist)
                {
                    sqlString = @"UPDATE eBay_SoldTransactions SET CostcoOrderNumber = @_costcoOrderNumber,
                                CostcoOrderEmailPdf = @_costcoOrderEmailPdf
                                WHERE WHERE CostcoItemName = @_costcoItemName 
                                AND BuyerName = @_buyerName AND  CostcoOrderNumber IS NULL";

                    cmd.CommandText = sqlString;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_costcoOrderNumber", stOrderNumber);
                    cmd.Parameters.AddWithValue("@_costcoOrderEmailPdf", destinationFileName);
                    cmd.Parameters.AddWithValue("@_costcoItemName", stProductName);
                    cmd.Parameters.AddWithValue("@_buyerName", stBuyerName);

                    cmd.ExecuteNonQuery();
                }
            }

            cn.Close();
            

        }

        private void btnSoldEmailExtract_Click(object sender, EventArgs e)
        {
            string subject = "Your eBay item sold! New Estée Lauder Pure Color Long Lasting lipstick ~Rubellite SHIMMER~ (161193141452)";
            string body = @"<div dir='ltr'><div class='gmail_quote'><br><div><div><div><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='100%' style='word-wrap:break-word'><table cellpadding='2' cellspacing='3' border='0' width='100%'><tbody><tr><td width='1%' nowrap><img src='http://q.ebaystatic.com/aw/pics/logos/ebay_95x39.gif' height='39' width='95' alt='eBay'></td><td align='left' valign='bottom'><span style='font-weight:bold;font-size:xx-small;font-family:verdana,sans-serif;color:#666'><b>eBay sent this message to Yue Zhang (coffeebean217).</b><br></span><span style='font-size:xx-small;font-family:verdana,sans-serif;color:#666'>Your registered name is included to show this message originated from eBay. <a href='http://pages.ebay.com/help/confidence/name-userid-emails.html' target='_blank'>Learn more</a>.</span></td></tr></tbody></table></td></tr></tbody></table></div></div><div><div><table style='background-color:#ffe680' border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='8' valign='top'><img src='http://q.ebaystatic.com/aw/pics/globalAssets/ltCurve.gif' height='8' width='8'></td><td valign='bottom' width='100%'><span style='font-weight:bold;font-size:14pt;font-family:arial,sans-serif;color:#000;margin:2px 0 2px 0'>Congratulations, your item sold-get ready to ship!</span></td><td width='8' valign='top' align='right'><img src='http://p.ebaystatic.com/aw/pics/globalAssets/rtCurve.gif' height='8' width='8'></td></tr><tr><td style='background-color:#fc0' colspan='3' height='4'></td></tr></tbody></table></div></div><div><div><table border='0' cellpadding='2' cellspacing='3' width='100%'><tbody><tr><td><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'>Hi coffeebean217,<table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='10' alt=' '></td></tr></tbody></table>You did it! Your item sold. Please ship this item to the buyer after your buyer pays. As soon as your buyer pays, print your <a href='http://rover.ebay.com/rover/0/e12011.m354.l1337/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostage%26transactionid%3D1105542200006%26itemid%3D161193141452%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1337' target='_blank'>eBay shipping label</a>.<table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='10' alt=' '></td></tr></tbody></table><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'><ul style='list-style-type:decimal'><li style='padding:5px'>Get discounted rates for shipping.</li><li style='padding:5px'>eBay label printing service is FREE!</li><li style='padding:5px'>Tracking information is uploaded automatically to My eBay and an email is sent to your buyer that their shipment is on the way.</li></ul><br><div style='padding-bottom:20px'>If you don&#39;t use eBay label printing, <a href='http://rover.ebay.com/rover/0/e12011.m354.l1663/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FAddTrackingNumber2%26flow%3Dmyebay%26LineID%3D161193141452_1105542200006%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1663' target='_blank'>upload your tracking information manually</a>.</div><br><div style='padding-bottom:20px'> You should always <a href='http://rover.ebay.com/rover/0/e12011.m354.l1332/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Ffeedback.ebay.com%2Fws%2FeBayISAPI.dll%3FLeaveFeedback2%26show_as%3Dsold%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1332' target='_blank'>leave feedback</a> for your buyer to encourage them to buy from you again.</div></font></font><div><table width='100%' cellpadding='0' cellspacing='3' border='0'><tbody><tr><td valign='top' align='center' width='100' nowrap><a href='http://rover.ebay.com/rover/0/e12011.m43.l1123/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D161193141452%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1123' target='_blank'><img src='http://thumbs.ebaystatic.com/pict/161193141452.jpg' alt='New Estée Lauder Pure Color Long Lasting lipstick ~Rubellite SHIMMER~' border='0'></a></td><td colspan='2' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' colspan='2'><a href='http://rover.ebay.com/rover/0/e12011.m43.l1123/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D161193141452%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1123' target='_blank'>New Estée Lauder Pure Color Long Lasting lipstick ~Rubellite SHIMMER~</a></td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>End time:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>Jun-04-14 08:29:55 PDT</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'><font style='font-weight:bold;font-size:10pt;font-family:arial,sans-serif;color:#000'>Sale price:</font></td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'><font style='font-weight:bold;font-size:10pt;font-family:arial,sans-serif;color:#000'>$9.50</font></td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Quantity:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>1</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Quantity sold:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>1</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Quantity remaining:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>0</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Buyer:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>STAVROULLA XENI</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'></td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'><div><table border='0' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'>stavri.lav (<a href='mailto:sxeni@treasury.gov.cy' target='_blank'>sxeni@treasury.gov.cy</a>) [<a href='http://contact.ebay.com/ws/eBayISAPI.dll?ReturnUserEmail&amp;requested=stavri.lav&amp;redirect=0&amp;iid=161193141452' target='_blank'>contact buyer</a>]</font></td></tr></tbody></table></div></td></tr><tr><td colspan='2'><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'><a href='http://rover.ebay.com/rover/0/e12011.m43.l1151/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws%2FeBayISAPI.dll%3FSellHub3%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1151' target='_blank'>Sell another Item</a>   |    <a href='http://rover.ebay.com/rover/0/e12011.m43.l1156/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FUnifiedCheckoutSellerUpdateDetails%26itemId%3D161193141452%26transId%3D1105542200006%26buyerid%3D0%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1156' target='_blank'>Send invoice to buyer</a></font></td></tr></tbody></table></td></tr></tbody></table></div></td><td valign='top' width='185'><div><span style='font-weight:bold;font-size:10pt;font-family:arial,sans-serif;color:#000'><strong>As soon as your buyer pays</strong></span><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='4' alt=' '></td></tr></tbody></table><a href='http://rover.ebay.com/rover/0/e12011.m44.l1337/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=https%3A%2F%2Fpostage.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostageLabelRedirect%26itemid%3D161193141452%26transactionid%3D1105542200006%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1337' title='http://rover.ebay.com/rover/0/e12011.m44.l1337/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=https%3A%2F%2Fpostage.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostageLabelRedirect%26itemid%3D161193141452%26transactionid%3D1105542200006%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1337' target='_blank'><img src='http://p.ebaystatic.com/aw/pics/buttons/btnPrintShippingLabel.gif' border='0' height='32' width='120'></a><br></div></td></tr></tbody></table><br></div></div><div></div><div></div><div><div><hr style='min-height:1px'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='100%'><font style='font-size:8pt;font-family:arial,sans-serif;color:#000000'>Email reference id: [#80af0a23f2c047c5b6fac4936c26c954#]</font></td></tr></tbody></table><br></div><hr style='min-height:1px'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='100%'><font style='font-size:xx-small;font-family:verdana;color:#666'><a href='http://pages.ebay.com/education/spooftutorial/index.html' target='_blank'>Learn More</a> to protect yourself from spoof (fake) emails.<br><br>eBay sent this email to you at <a href='mailto:coffeebean217@gmail.com' target='_blank'>coffeebean217@gmail.com</a> about your account registered on <a href='http://www.ebay.com' target='_blank'>www.ebay.com</a>.<br><br>eBay will periodically send you required emails about the site and your transactions. Visit our <a href='http://pages.ebay.com/help/policies/privacy-policy.html' target='_blank'>Privacy Policy</a> and <a href='http://pages.ebay.com/help/policies/user-agreement.html' target='_blank'>User Agreement</a> if you have any questions.<br><br>Copyright © 2014 eBay Inc. All Rights Reserved. Designated trademarks and brands are the property of their respective owners. eBay and the eBay logo are trademarks of eBay Inc. eBay Inc. is located at 2145 Hamilton Avenue, San Jose, CA 95125.  </font></td></tr></tbody></table><img src='http://rover.ebay.com/roveropen/0/e12011/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;' height='1' width='1'></div></div></div><br></div>";

            ProcessItemSoldEmail(subject, body);
        }

        private void btnHtmlToPDF_Click(object sender, EventArgs e)
        {
            

            string[] files = Directory.GetFiles(@"C:\temp\");

            foreach (string file in files)
            {
                //FirefoxProfile profile = new FirefoxProfile();
                ////profile.SetPreference("print.always_print_silent", true);

                //IWebDriver driver = new FirefoxDriver(profile);
                //driver.Navigate().GoToUrl(@"file:///" + file);
                //IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                //js.ExecuteScript("window.print();");
                //driver.Dispose();

                //System.Threading.Thread.Sleep(3000);
                //string command = "c:\\temp\\wkhtmltopdf.exe c:\\temp\\1.html c:\\temp\\1.pdf ";
                string f = (string)file;

                string command = "c:\\temp\\wkhtmltopdf.exe " + f + " " + f.Replace("html", "pdf");

                //System.Diagnostics.Process.Start("CMD.exe", "/c" + command);

                var startInfo = new ProcessStartInfo("CMD.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.Arguments = "/c" + command;
                System.Diagnostics.Process.Start(startInfo);
            }



        }

        private void btnCostcoOrderEmail_Click(object sender, EventArgs e)
        {
            string body = @"<html><head></head><body><div dir='ltr'><div class='gmail_quote'><br>
<table border='0' cellpadding='0' cellspacing='0' width='650'>
<tbody><tr>
  <td>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><table border='0' cellspacing='0' cellpadding='0' width='650'>
            <tbody><tr>
              <td><a href='http://www.costco.com/?EMID=Transactional_OrderReceived_TopNav_Logo' target='_blank'><img src='http://www.costco.com/wcsstore/CostcoUSBCCatalogAssetStore/email/costco-em-toplogo.gif' border='0'></a> </td>
            </tr>
            <tr height='10'>
              <td height='10'></td>
            </tr>
          </tbody></table></td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><table border='0' width='100%'>
            <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
              <td><table border='0' width='300px' height='50px'>
                  <tbody><tr>
                    <td style='font-size:19pt'><b>Order Received</b></td>
                  </tr>
                </tbody></table></td>
              <td style='width:150px'></td>
              <td><table border='0' cellspacing='0' cellpadding='0' width='100%'>
                  <tbody><tr>
                    <td style='padding:0px 0px 3px 0px;text-align:left;font-size:14px;color:#1c6699;font-weight:bold;vertical-align:bottom'>STEPS FOR YOUR ORDER:</td>
                  </tr>
                  <tr>





                    <td style='padding:2px 0px 2px 0px;border-top:solid 1px #969696;border-bottom:solid 1px #969696'><b>1. Order Received</b>    <span style='color:#969696'>2. Sent to Fulfillment</span>    <span style='color:#969696'>3. Shipped</span></td>
                  </tr>
                </tbody></table></td>
            </tr>
          </tbody></table></td>
      </tr>
    </tbody></table>
    
    

<table border='0'>
  <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
    <td>
      <table border='0' cellpadding='2' cellspacing='2' height='90' width='250'>
        <tbody><tr>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#cee7ff;color:Black;font-weight:bold;font-size:12px;width:100px'>
          Order Number:</td>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:normal;font-size:12px'>
          597052288</td>
        </tr>
<tr>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#cee7ff;color:Black;font-weight:bold;font-size:12px;width:100px'>
          Membership Number:</td>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:normal;font-size:12px'>
          111775568587</td>
        </tr>
        <tr>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#cee7ff;color:Black;font-weight:bold;font-size:12px;width:100px'>
          Date Placed:</td>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:normal;font-size:12px'>
          01/08/2016</td>
        </tr>
      </tbody></table>
    </td>

    <td>
      <table border='0' cellpadding='2' cellspacing='2' height='90' width='250'>
        <tbody><tr>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#cee7ff;color:Black;font-weight:bold;font-size:12px'>
          Shipping Address</td>
        </tr>
        <tr>
          <td style='background-color:#f0f0f0'>
            <table border='0' cellspacing='0' cellpadding='0' width='100%' height='100%'>
              <tbody><tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>
                Zhijun Ding
                </td>
              </tr>
              <tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>1642 Crossgate Dr</td>
              </tr>
              
              <tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>Vestavia, AL   35216-3181</td>
              </tr>
            </tbody></table>
          </td>
        </tr>
      </tbody></table>
    </td>

    <td>
      <table border='0' cellpadding='2' cellspacing='2' height='90' width='250'>
        <tbody><tr>
          <td style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#cee7ff;color:Black;font-weight:bold;font-size:12px'>
          Note:</td>
        </tr>
        <tr>
          <td style='background-color:#f1f1f1'>
          This email was automatically generated from a mailbox that is not monitored. If you have any questions, please visit <a href='https://customerservice.costco.com' target='_blank'>customer service</a>.</td>
        </tr>
      </tbody></table>
    </td>
  </tr>
</tbody></table>
<table border='0'>
  <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
    <td>
      <table border='0' cellpadding='0' cellspacing='0'>
        <tbody><tr>
          <td style='height:20px'></td>
        </tr>
      </tbody></table>
    </td>
  </tr>
</tbody></table>


    
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td>Dear Zhijun Ding,</td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td>Thank you for ordering from <b><a href='http://www.costco.com/?EMID=Transactional_OrderReceived_Main_Intro' target='_blank'>Costco.com</a></b>. We have received the order and, after processing it, will send it to the fulfillment facility. Please keep this email for your records.</td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><table border='0' cellpadding='0' cellspacing='0'>
            <tbody><tr>
              <td style='height:10px'></td>
            </tr>
          </tbody></table></td>
      </tr>
    </tbody></table>
    
    

<table cellspacing='0' cellpadding='0' border='0'>
<tbody><tr valign='top'><td width='760'>
<table cellspacing='0' cellpadding='0' border='0'>
<tbody>
<tr valign='top'>
<td bgcolor='#6495ed' width='760' valign='middle' colspan='6'>
<b><font size='2' face='Arial' color='#ffffff'>Your Order</font></b>
</td>
</tr>

<tr valign='top'>
<td bgcolor='#cee7ff' width='31' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Qty</font></b></div>
</td>
<td bgcolor='#cee7ff' width='307' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Description</font></b></div>
</td>
<td bgcolor='#cee7ff' width='125' valign='middle'><div align='center'>
<b><font size='2' face='Arial'>Shipping Method</font></b></div>
</td>

<td bgcolor='#cee7ff' width='104' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Price</font></b></div>
</td>
<td bgcolor='#cee7ff' width='125' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Item Total</font></b></div>
</td>
<td width='68' valign='middle'>
<img height='1' border='0' width='1' alt=''>
</td>
</tr>

<tr valign='top'>
<td bgcolor='#f0f0f0' width='31' valign='middle'>
<div align='center'>
<font size='2' face='Arial'>

1

</font>
</div>
</td>
<td bgcolor='#f0f0f0' width='307' valign='middle'>
<div align='center'>
<font size='2' face='Arial'>

Pacific Coast® Platinum European Comforter with Pyrénées Down - Year Round Warmth, F/Q
- Item# 868374

</font>
</div>
</td>

<td bgcolor='#f0f0f0' width='125' valign='middle'>
<div align='center'>


<font size='2' face='Arial'>Standard 3 to 5 Business Days</font>

</div>
</td>

<td bgcolor='#f0f0f0' width='104' valign='middle'><div align='center'><font size='2' face='Arial'>$109.99</font></div></td>
<td bgcolor='#f0f0f0' width='125' valign='middle'><div align='center'><font size='2' face='Arial'>$109.99</font></div></td>
<td width='68' valign='middle'><img height='1' border='0' width='1' alt=''></td>
</tr>
<tr valign='top'>
<td bgcolor='#f0f0f0' width='760' valign='middle' colspan='6'>

</td>
</tr>


<tr valign='top'>
<td width='760' colspan='6' bgcolor='#ffffcc'><font face='Arial'> 
 </font>
</td>
</tr>


<tr valign='top'>
<td bgcolor='#ffffcc' width='760' valign='middle' colspan='6'>
<table cellspacing='0' cellpadding='0' border='0'>
<tbody>
<tr valign='top'>
<td bgcolor='#ffffcc' width='760'><b><font size='2' face='Arial'>Shipping &amp; Terms</font></b></td>
</tr>

<tr valign='top'>
<td width='760' valign='middle'><img height='1' border='0' width='1' alt=''></td>
</tr>
<tr valign='top'>
<td bgcolor='#ffffcc' width='760'><font face='Arial'>

Standard shipping via UPS Ground is included in the quoted price. <strong>The estimated delivery time will be approximately 3 - 5 business days from the time of order.</strong> 
 </font></td>
</tr>
</tbody>
</table>
</td>
</tr>

<tr valign='top'><td width='31' valign='middle'><img height='1' border='0' width='1' alt='' src='http://f1617.mail.yahoo.com/ya/download?mid=1%5f1273896%5fALzSi2IAAPL%2bTYJiwwNQEEAuEwo&amp;pid=2&amp;fid=Inbox&amp;inline=1'></td><td width='307' valign='middle'><img height='1' border='0' width='1' alt='' src='http://f1617.mail.yahoo.com/ya/download?mid=1%5f1273896%5fALzSi2IAAPL%2bTYJiwwNQEEAuEwo&amp;pid=2&amp;fid=Inbox&amp;inline=1'></td><td width='125' valign='middle'><img height='1' border='0' width='1' alt='' src='http://f1617.mail.yahoo.com/ya/download?mid=1%5f1273896%5fALzSi2IAAPL%2bTYJiwwNQEEAuEwo&amp;pid=2&amp;fid=Inbox&amp;inline=1'></td><td width='104' valign='middle'><img height='1' border='0' width='1' alt='' src='http://f1617.mail.yahoo.com/ya/download?mid=1%5f1273896%5fALzSi2IAAPL%2bTYJiwwNQEEAuEwo&amp;pid=2&amp;fid=Inbox&amp;inline=1'></td><td width='125' valign='middle'><img height='1' border='0' width='1' alt='' src='http://f1617.mail.yahoo.com/ya/download?mid=1%5f1273896%5fALzSi2IAAPL%2bTYJiwwNQEEAuEwo&amp;pid=2&amp;fid=Inbox&amp;inline=1'></td><td width='68' valign='middle'><img height='1' border='0' width='1' alt='' src='http://f1617.mail.yahoo.com/ya/download?mid=1%5f1273896%5fALzSi2IAAPL%2bTYJiwwNQEEAuEwo&amp;pid=2&amp;fid=Inbox&amp;inline=1'></td></tr>
</tbody></table>
</td></tr>
</tbody></table>


    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><table style='border:solid 1px #696969' cellpadding='5' cellspacing='2'>
            <tbody><tr>
              <td><b>Are you missing out on our LIMITED TIME OFFERS?</b> <br>
                Sign up to receive <a href='http://costco.com' target='_blank'>costco.com</a> e-mails on the <a href='https://www.costco.com/LogonForm?langId=-1&amp;storeId=10301&amp;catalogId=10701&amp;URL=LogonForm&amp;cm_re=Common-_-Top_Nav-_-My_Account' target='_blank'>My Account</a> page.<br>
                <br>
                <strong>Costco Office Online</strong> - Save on your office supplies! <a href='http://www.costco.com/office-products.html?EMID=Transactional_OrderReceived_OfficeProducts' target='_blank'>Click here</a> to start savings TODAY!<br>
                <br>
                
                Click to see <a href='http://www.costco.com/CatalogSearch?langId=-1&amp;storeId=10301&amp;catalogId=10701&amp;keyword=WhatsNewAZ&amp;sortBy=EnglishAverageRating%7C1&amp;EMID=Transactional_OrderReceived_WhatsNew' target='_blank'><strong>What&#39;s New</strong></a> at Costco.com. <br></td>
            </tr>
          </tbody></table></td>
        <td>
          

<table>
  <tbody>
    <tr>
      <td bgcolor='#cee7ff'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>Subtotal:</b>
          </font>
        </div>
      </td>
      <td bgcolor='#ffffcc'>
        <div align='right'>
          <font size='2' face='Arial'>$109.99</font>
        </div>
      </td>
    </tr>
    
    <tr>
      <td bgcolor='#cee7ff'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>Shipping &amp; Handling:</b>
          </font>
        </div>
      </td>
      <td bgcolor='#ffffcc'>
        <div align='right'>
          <font size='2' face='Arial'>$0.00</font>
        </div>
      </td>
    </tr>
    
    <tr>
      <td bgcolor='#cee7ff'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>Non-Member Surcharge:</b>
          </font>
        </div>
      </td>
      <td bgcolor='#ffffcc'>
        <div align='right'>
          <font size='2' face='Arial'>$0.00</font>
        </div>
      </td>
    </tr>
    
<tr>
      <td bgcolor='#cee7ff'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>Tax:</b>
          </font>
        </div>
      </td>
      <td bgcolor='#ffffcc'>
        <div align='right'>
          <font size='2' face='Arial'>$6.60</font>
        </div>
      </td>
    </tr>

    <tr>
      <td bgcolor='#cee7ff'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>Order Total:</b>
          </font>
        </div>
      </td>
      <td bgcolor='#ffffcc'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>$116.59</b>
          </font>
        </div>
      </td>
    </tr>
  </tbody>
</table> </td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><br>
          <font face='Arial' style='font-size:9pt'><b>Shop Confidently</b><br>
          <br>
          <b>Membership:</b> We will refund your membership fee in full at any time if you are dissatisfied.<br>
          <br>
          <b>Merchandise:</b> We guarantee your satisfaction on every product we sell with a full refund. The return policy for televisions, computers, major appliances, tablets, cameras, camcorders, MP3 players, and cellular phones is 90 days from date of purchase.  Manufacturer&#39;s warranty service is available on all electronics products. See manufacturer&#39;s warranty for specific coverage terms. For TV, Computers (excluding tablets) and Major Appliances, Costco extends the manufacturer&#39;s warranty to two years from date of purchase. Please call Costco Concierge @ <a href='tel:1-866-861-0450' value='+18668610450' target='_blank'>1-866-861-0450</a> for warranty assistance, set-up help or technical support.<br>
          <br>
          <b>How to Return:</b> Simply return your purchase at any one of our Costco warehouses worldwide for a refund (including shipping and handling). If you are unable to return your order at one of our warehouses, please contact <a href='https://customerservice.costco.com' target='_blank'>customer service</a> or call our customer service center at <a href='tel:1-800-955-2292' value='+18009552292' target='_blank'>1-800-955-2292</a> for assistance. To expedite the processing of your return, please reference your order number.</font><br>
          <br>
          <p><font face='Arial' style='font-size:9pt'><strong>Costco.com</strong> <a href='https://customerservice.costco.com' target='_blank'>customer service</a> or call <strong><a href='tel:1-800-955-2292' value='+18009552292' target='_blank'>1-800-955-2292</a></strong></font></p>
                    <p><font face='Arial' style='font-size:9pt'>To expedite the processing of your return, please reference your order number.</font></p>
          <p><font face='Arial' style='font-size:9pt'>If you request an item be picked up for return, the item must be packaged and   available for pick up in the same manner as it was delivered. <br>
            If your order was delivered “curb side,” it will need to be available for curb side pick up. <br>
            If the item arrived to you in a box, it will need to be in a box at the time   of pick up. </font> </p>
          
          <font face='Arial' style='font-size:9pt'> Please <a href='http://www.costco.com/returns-replacements-refunds.html' target='_blank'>click here</a> for additional returns information.</font>
        <p> </p></td>
      </tr>
    </tbody></table></td>
</tr>
</tbody></table>

</div><br></div>
</body></html>";

            ProcessCostcoOrderEmail(body);

        }

        private void btnFtp_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/11.jpg", "STOR", @"C:\temp\1.jpg");
            }
        }

        private void btnListEmailExtract_Click(object sender, EventArgs e)
        {
            string body = @"<html><head></head><body><div id='Header'><div><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td width='100%' style='word-wrap:break-word'><table cellpadding='2' cellspacing='3' border='0' width='100%'><tr><td width='1%' nowrap='nowrap'><img src='http://q.ebaystatic.com/aw/pics/logos/ebay_95x39.gif' height='39' width='95' alt='eBay'></td><td align='left' valign='bottom'><span style='font-weight:bold; font-size:xx-small; font-family:verdana, sans-serif; color:#666'><b>eBay sent this message to Zhijun Ding (zjding2016).</b><br></span><span style='font-size:xx-small; font-family:verdana, sans-serif; color:#666'>Your registered name is included to show this message originated from eBay. <a href='http://pages.ebay.com/help/confidence/name-userid-emails.html'>Learn more</a>.</span></td></tr></table></td></tr></table></div></div><div id='Title'><div><table style='background-color:#ffe680' border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td width='8' valign='top'><img src='http://q.ebaystatic.com/aw/pics/globalAssets/ltCurve.gif' height='8' width='8' alt=''></td><td valign='bottom' width='100%'><span style='font-weight:bold; font-size:14pt; font-family:arial, sans-serif; color:#000; margin:2px 0 2px 0'>Your item has been listed. Sell another item now!</span></td><td width='8' valign='top' align='right'><img src='http://p.ebaystatic.com/aw/pics/globalAssets/rtCurve.gif' height='8' width='8' alt=''></td></tr><tr><td style='background-color:#fc0' colspan='3' height='4'></td></tr></table></div></div><div id='SingleItemCTA'><div><table border='0' cellpadding='2' cellspacing='3' width='100%'><tr><td><font style='font-size:10pt; font-family:arial, sans-serif; color:#000'>Hi zjding2016,<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='10' alt=' '></td></tr></table>Your item has been successfully listed on eBay. It may take some time for the item to appear on eBay search results. Here are the listing details:<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='10' alt=' '></td></tr></table><div></div></font><div><table width='100%' cellpadding='0' cellspacing='3' border='0'><tr><td valign='top' align='center' width='100' nowrap='nowrap'><a href='http://rover.ebay.com/rover/0/e12000.m43.l1123/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D152102133759%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1123'><img src='http://pics.ebaystatic.com/aw/pics/icon/iconPic_20x20.gif' alt='Swingline Optima Grip Full Strip Stapler, 25-Sheet Capacity, Graphite SWI 87810' border='0'></a></td><td colspan='2' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' colspan='2'><a href='http://rover.ebay.com/rover/0/e12000.m43.l1123/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D152102133759%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1123'>Swingline Optima Grip Full Strip Stapler, 25-Sheet Capacity, Graphite SWI 87810</a></td></tr><tr><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' width='15%' nowrap='nowrap' valign='top'>Item Id:</td><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' valign='top'>152102133759</td></tr><tr><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' width='15%' nowrap='nowrap' valign='top'>Price:</td><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' valign='top'>$25.40</td></tr><tr><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' width='15%' nowrap='nowrap' valign='top'>End time:</td><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' valign='top'>Jun-23-16 09:18:06 PDT</td></tr><tr><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' width='15%' nowrap='nowrap' valign='top'>Listing fees:</td><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' valign='top'>0</td></tr><tr><td colspan='2'><font style='font-size:10pt; font-family:arial, sans-serif; color:#000'><a href='http://rover.ebay.com/rover/0/e12000.m43.l1125/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws2%2FeBayISAPI.dll%3FUserItemVerification%26%26item%3D152102133759%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1125'>Revise item</a>   |    <a href='http://rover.ebay.com/rover/0/e12000.m43.l1121/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;loc=http%3A%2F%2Fmy.ebay.com%2Fws%2FeBayISAPI.dll%3FMyeBay%26%26CurrentPage%3DMyeBaySelling%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1121'>Go to My eBay</a></font></td></tr></table></td></tr></table></div><td valign='top' width='185'><div><span style='font-weight:bold; font-size:10pt; font-family:arial, sans-serif; color:#000'>Ready to List Your Next Item?</span><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='4' alt=' '></td></tr></table><a href='http://rover.ebay.com/rover/0/e12000.m44.l1127/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws%2FeBayISAPI.dll%3FSellHub3%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1127' title='http://rover.ebay.com/rover/0/e12000.m44.l1127/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws%2FeBayISAPI.dll%3FSellHub3%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1127'><img src='http://p.ebaystatic.com/aw/pics/buttons/btnSellMore.gif' border='0' height='32' width='120'></img></a><br><span style='font-style:italic; font-size:8pt; font-family:arial, sans-serif; color:#000'>Click to list another item</span></div></td></td></tr></table><br></div></div><div id='OneClickUnsubscribe'><div><style>.cub-cwrp {display:block; border:1px solid #dedfde; font-family:arial, sans-serif; font-size:10pt; margin-bottom:20px}h3.cub-chd {margin:0px; padding:5px; display:block; background:#e7e7e7; font-size:14px}.cub-ccnt {padding:0px 10px 10px 5px; display:block}ul.cub-ulst {margin:0px 0px 0px 10px; padding:0px 0px 0px 10px}ul.cub-ulst li, ul.cub-ulst li.cub-licn {list-style:square outside none; margin:0px; padding:10px 0px 0px 0px; line-height:16px}.cub-ltxt {color:#333; display:block}</style><div class='cub-cwrp'><h3 class='cub-chd'>Select your email preferences</h3><div class='cub-ccnt'><ul class='cub-ulst'><li><span class='cub-ltxt'><span>Want to reduce your inbox email volume? <a href='http://my.ebay.com/ws/eBayISAPI.dll?DigestEmail&amp;emailType=12000'>Receive this email as a daily digest</a>.</span><br><span>For other email digest options, go to <a href='http://my.ebay.com/ws/eBayISAPI.dll?MyEbayBeta&amp;CurrentPage=MyeBayNextNotificationPreferences'>Notification Preferences</a> in My eBay.</span><br></span></li><li><span class='cub-ltxt'><span>Don't want to receive this email? <a href='http://my.ebay.com/ws/eBayISAPI.dll?EmailUnsubscribe&amp;emailType=12000'>Unsubscribe from this email</a>.</span><br></span></li></ul></div></div></div></div><div id='Tips'></div><div id='RTMEducational'></div><div id='MST'><div><table style='border:1px solid #6b7b91' border='0' cellpadding='0' cellspacing='0' width='100%'><tr style='background-color:#c9d2dc' height='1'><td><img src='http://p.ebaystatic.com/aw/pics/securityCenter/imgShield_25x25.gif' height='25' width='25' alt='Marketplace Safety Tip' align='absmiddle'></td><td style='font-weight:bold; font-size:10pt; font-family:arial, sans-serif; color:#000' nowrap='nowrap' width='20%'>Marketplace Safety Tip</td><td><img src='http://p.ebaystatic.com/aw/pics/securityCenter/imgTabCorner_25x25.gif' height='25' width='25' alt='' align='absmiddle'></td><td background='http://q.ebaystatic.com/aw/pics/securityCenter/imgFlex_1x25.gif' height='1' width='80%'></td></tr><tr><td style='font-size:10pt; font-family:arial, sans-serif; color:#000' colspan='4'><ul style='margin-top: 5px; margin-bottom: 5px;'><li style='padding-bottom: 3px; line-height: 120%; padding-top: 3px; list-style-type: square;'>If you are contacted about buying a similar item outside of eBay, please do not respond. Outside-of-eBay transactions are against eBay policy, and they are not covered by eBay services such as feedback and eBay purchase protection programs.</li></ul></td></tr><tr><td style='background-color:#c9d2dc' colspan='4'><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='1' width='1'></td></tr></table><br></div></div><div id='Footer'><div><hr style='HEIGHT: 1px'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td width='100%'><font style='font-size:8pt; font-family:arial, sans-serif; color:#000000'>Email reference id: [#d7aa3b6062934f4d99682042ae7471a4#]</font></td></tr></table><br></div><hr style='HEIGHT: 1px'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td width='100%'><font style='font-size:xx-small; font-family:verdana; color:#666'><a href='http://pages.ebay.com/education/spooftutorial/index.html'>Learn More</a> to protect yourself from spoof (fake) emails.<br><br>eBay sent this email to you at zjding@outlook.com about your account registered on <a href='http://www.ebay.com'>www.ebay.com</a>.<br><br>eBay sends these emails based on the preferences you set for your account. To unsubscribe from this email, change your <a href='http://my.ebay.com/ws/eBayISAPI.dll?MyEbayBeta&amp;CurrentPage=MyeBayNextNotificationPreferences'>communication preferences</a>. Please note that it may take up to 10 days to process your request. Visit our <a href='http://pages.ebay.com/help/policies/privacy-policy.html'>Privacy Notice</a> and <a href='http://pages.ebay.com/help/policies/user-agreement.html'>User Agreement</a> if you have any questions.<br><br>Copyright © 2016 eBay Inc. All Rights Reserved. Designated trademarks and brands are the property of their respective owners. eBay and the eBay logo are trademarks of eBay Inc. eBay Inc. is located at 2145 Hamilton Avenue, San Jose, CA 95125.  </font></td></tr></table><img src='http://rover.ebay.com/roveropen/0/e12000/7?euid=d7aa3b6062934f4d99682042ae7471a4&amp;' height='1' width='1'></div></body></html>";

            string subject = @"Your eBay listing is confirmed: Swingline Optima Grip Full Strip Stapler, 25-Sheet Capacity, Graphite SWI 87810";

            ProcessListingConfirmEmail(body, subject);
        }

        private void ProcessListingConfirmEmail(string body, string subject)
        {
            body = body.Replace("\n", "");
            body = body.Replace("\t", "");
            //body = body.Replace("\\", "");
            body = body.Replace("\"", "'");

            string productName = subject.Substring(32, subject.Length - 32);

            string sqlString = "SELECT * FROM eBay_ToAdd WHERE name = '" + productName + "'";

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            Product product = new Product();

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();

                product.Name = Convert.ToString(reader["Name"]);
                product.UrlNumber = Convert.ToString(reader["UrlNumber"]);
                product.ItemNumber = Convert.ToString(reader["ItemNumber"]);
                product.Category = Convert.ToString(reader["Category"]);
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.Shipping = Convert.ToDecimal(reader["Shipping"]);
                product.Limit = Convert.ToString(reader["Limit"]);
                product.Details = Convert.ToString(reader["Details"]);
                product.Specification = Convert.ToString(reader["Specification"]);
                product.ImageLink = Convert.ToString(reader["ImageLink"]);
                product.NumberOfImage = Convert.ToInt16(reader["NumberOfImage"]);
                product.Url = Convert.ToString(reader["Url"]);
                product.eBayCategoryID = Convert.ToString(reader["eBayCategoryID"]);
                product.eBayReferencePrice = Convert.ToDecimal(reader["eBayReferencePrice"]);
                product.eBayListingPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
                product.DescriptionImageWidth = Convert.ToInt16(reader["DescriptionImageWidth"]);
                product.DescriptionImageHeight = Convert.ToInt16(reader["DescriptionImageHeight"]);
            }

            reader.Close();

            sqlString = @"DELETE FROM eBay_ToAdd WHERE name = '" + productName + "'";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            body = body.Replace("\r", "");
            body = body.Replace("\t", "");
            body = body.Replace("\n", "");

            string stItemId = SubstringInBetween(body, "Item Id:</td>", "</td>", false, true);
            stItemId = SubstringEndBack(stItemId, "</td>", ">", false, false);
            stItemId = stItemId.Trim();

            //string stListingUrl = SubstringEndBack(body, "Item Id:</td>", "<a href='", true, false);
            //stListingUrl = SubstringInBetween(stListingUrl, "<a href='", "target", false, false);
            //stListingUrl = stListingUrl.Trim();

            string stPrice = SubstringInBetween(body, "Price:</td>", "</td>", false, true);
            stPrice = SubstringEndBack(stPrice, "</td>", "$", false, false);
            stPrice = stPrice.Replace("$", "");
            stPrice = stPrice.Trim();

            string stEndTime = SubstringInBetween(body, "End time:</td>", "</td>", false, false);
            stEndTime = SubstringEndBack(stEndTime, "PDT", ">", false, true);
            stEndTime = stEndTime.Trim();
            string correctedTZ = stEndTime.Replace("PDT", "-0700");
            DateTime dtEndTime = Convert.ToDateTime(correctedTZ);

            sqlString = @"INSERT INTO eBay_CurrentListings
                            (Name, eBayListingName, eBayCategoryID, eBayItemNumber, eBayListingPrice, eBayDescription, 
                             eBayEndTime,  CostcoUrlNumber, CostcoItemNumber, CostcoUrl, CostcoPrice, ImageLink) 
                          VALUES (@_name, @_eBayListingName, @_eBayCategoryID, @_eBayItemNumber, @_eBayListingPrice, @_eBayDescription,
                                @_eBayEndTime, @_CostcoUrlNumber, @_CostcoItemNumber, @_CostcoUrl, @_CostcoPrice, @_ImageLink)";

            cmd.CommandText = sqlString;
            cmd.Parameters.AddWithValue("@_name", product.Name);
            cmd.Parameters.AddWithValue("@_eBayListingName", productName);
            cmd.Parameters.AddWithValue("@_eBayCategoryID", product.eBayCategoryID);
            cmd.Parameters.AddWithValue("@_eBayItemNumber", stItemId);
            cmd.Parameters.AddWithValue("@_eBayListingPrice", product.eBayListingPrice);
            cmd.Parameters.AddWithValue("@_eBayDescription", product.Details);
            cmd.Parameters.AddWithValue("@_eBayEndTime", dtEndTime);
            //cmd.Parameters.AddWithValue("@_eBayUrl", stListingUrl);
            cmd.Parameters.AddWithValue("@_CostcoUrlNumber", product.UrlNumber);
            cmd.Parameters.AddWithValue("@_CostcoItemNumber", product.ItemNumber);
            cmd.Parameters.AddWithValue("@_CostcoUrl", product.Url);
            cmd.Parameters.AddWithValue("@_CostcoPrice", product.Price);
            cmd.Parameters.AddWithValue("@_ImageLink", product.ImageLink);

            cmd.ExecuteNonQuery();

            cn.Close();
        }
    }

}
