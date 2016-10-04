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
using System.Web;

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

        string connectionString; // = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            ProductInfoForm productInfoForm = new ProductInfoForm();
            productInfoForm.ShowDialog();

        }

        public void SetConnectionString()
        {
            string azureConnectionString = "Server=tcp:zjding.database.windows.net,1433;Initial Catalog=Costco;Persist Security Info=False;User ID=zjding;Password=G4indigo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cn = new SqlConnection(azureConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            string sqlString = "SELECT ConnectionString FROM DatabaseToUse WHERE bUse = 1 and ApplicationName = 'Crawler'";
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    connectionString = (reader.GetString(0)).ToString();
                }
            }
            reader.Close();
            cn.Close();
        }

        private void btnImportCategories_Click(object sender, EventArgs e)
        {
            GetCategories();
        }

        public void ImportProducts()
        {
            //GetDepartmentArray();

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

            foreach (string url in productUrlArray)
            {
                try
                {
                    string productUrl = HttpUtility.HtmlDecode(url);


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
                    //productName = productName.Replace("'", "''");

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

                    sqlString = "INSERT INTO Raw_ProductInfo (Name, UrlNumber, ItemNumber, Category, Price, Shipping, Discount, Details, Specification, ImageLink, Url, NumberOfImage) VALUES ('" + productName.Replace("'", "''") + "','" + UrlNum + "','" + itemNumber + "','" + stSubCategories + "'," + price + "," + shipping + "," + "'" + discount + "','" + description + "','" + specification + "','" + imageUrl + "','" + url.Replace("'", "''") + "'," + numImages.ToString() + ")";
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
            categoryUrlArray.Clear();
            categoryUrlArray.Add(@"/mens-clothing.html");

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

            SetConnectionString();
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

            /*
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
            */

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("https://www.costco.com/LogonForm");
            IWebElement logonForm = driver.FindElement(By.Id("LogonForm"));
            logonForm.FindElement(By.Id("logonId")).SendKeys("zjding@gmail.com");
            logonForm.FindElement(By.Id("logonPassword")).SendKeys("721123");
            logonForm.FindElement(By.ClassName("submit")).Click();
            driver.Navigate().GoToUrl("http://www.costco.com/Adidas%C2%AE-Men%E2%80%99s-Adissage-SUPERCLOUD%E2%84%A2-Slide-Sandal-Black-%2526-Lime.product.100234752.html");
            var productOptions = driver.FindElements(By.ClassName("product-option"));

            List<string> selectList = new List<string>();

            foreach (var productOption in productOptions)
            {
                selectList.Add(productOption.FindElement(By.TagName("select")).GetAttribute("id").ToString());
            }

            string optionsString = string.Empty;


            if (selectList.Count == 2)
            {
                #region
                IWebElement selectElement0 = driver.FindElement(By.Id(selectList[0]));
                var options0 = selectElement0.FindElements(By.TagName("option"));
                foreach (IWebElement option0 in options0)
                {
                    if (option0.GetAttribute("value").ToString().ToUpper() != "UNSELECTED")
                    {
                        string option0String = option0.Text;
                        string swatch0 = option0.GetAttribute("swatch") == string.Empty ? string.Empty : "(" + option0.GetAttribute("swatch") + ")";

                        option0.Click();

                        IWebElement selectElement1 = driver.FindElement(By.Id(selectList[1]));
                        var options1 = selectElement1.FindElements(By.TagName("option"));

                        optionsString += option0String + swatch0 + ":";

                        foreach (IWebElement option1 in options1)
                        {
                            if (option1.GetAttribute("value").ToString().ToUpper() != "UNSELECTED")
                            {
                                if (option1.Text.Contains("-"))
                                {
                                    optionsString += option1.Text.Split('-')[0].Trim() + ";";
                                }
                            }
                        }

                        optionsString = optionsString.Substring(0, optionsString.Length - 1);
                        optionsString += "|";
                    }
                }

                optionsString = optionsString.Substring(0, optionsString.Length - 1);
                #endregion
            }
            else if (selectList.Count == 1)
            {
                IWebElement selectElement0 = driver.FindElement(By.Id(selectList[0]));
                var options0 = selectElement0.FindElements(By.TagName("option"));
                foreach (IWebElement option0 in options0)
                {
                    if (option0.GetAttribute("value").ToString().ToUpper() != "UNSELECTED")
                    {
                        if (option0.Text.Contains("-"))
                        {
                            optionsString += option0.Text.Split('-')[0].Trim() + ";";
                        }
                    }
                }
                optionsString = optionsString.Substring(0, optionsString.Length - 1);
            }
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

            for (int i = 1; i <= 20; i++)
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

                driver.Navigate().GoToUrl(url);

                var trs = driver.FindElements(By.XPath("//tr[contains(@class, 'bot')]"));

                foreach (IWebElement tr in trs)
                {
                    try
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
                        sqlString = "SELECT Name FROM eBay_ProductsResearch WHERE Name = '" + Name + "' AND eBayUserId = '" + UserId + "'";

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
                    catch (Exception ex)
                    {
                        continue;
                    }
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

            html = @"<html><head title='PayPal'>
<body><div class='ppmail'><style type='text/css'>
								body, td {font-size: 12px;font-family: arial,helvetica,sans-serif;}
                               .itemtbl {margin-top:20px;}
						      .itemtbl tr td {padding: 10px;color: #333;}
				    			tr.itemheading td{border:1px solid #ccc;border-right:none;border-left:none;padding:5px 10px 5px 10px !important;color: #333333 !important;}
								.padtoptwty {padding-top:20px !important;}
								tr.last td {border-bottom:none;}
								tr.itemrepeat td {border-bottom:1px dashed #ccc;}
								 .itemtbl {margin-top:20px;}
							    .itemtbl tr td {padding: 10px;color: #333;}	
							    .content {margin-top: 30px;color:#333 !important; }
								.mainheading{color:#333333;font-weight:bold;}
				      			.confirm {color: #4c8f3a;}
								.greeting {color:#333333 !important;font-weight:bold;}
								.status {font-size:14px;color:#C88039;font-weight:bold;text-decoration:none;}	
								.strong {font-weight: bold;}
								.valign {vertical-align: middle;}
								.purchasebox {width: 598px;}
								.purchasetitle {border-bottom:1px solid #ccc;padding:5px 0 5px 10px;}
								.purchaseboxmul {border:1px solid #ccc;margin:10px 0 10px 0;padding-bottom:10px;}
								.purchasetotal {border-top: 1px solid #ccc;border-bottom: 1px solid #ccc;}
								.purchasetotal {margin: 7px 0 20px 0;}
								.purchasetotal tr td{padding-bottom:20px;}
								.purchasetotalmul {border-top: 1px solid #ccc;width : 580px;}
								.purchasetotalmul img , .purchasetotal img {margin-top: 20px;}
								.purchasedetails tr td{color:#333;padding-top:15px;}
								.purchasedetailsmul tr td{color:#333;padding:20px 0 0 10px;}
							    .headinfo {color:#333333;}
							     hr {color: #dedede !important;height:1px;}
							     .totaltbl {margin-top: 20px; width: 100%;}
							     .totaltbl tr td {text-align:right;color: #333;padding:0 5px 0 0;width: 90px;}
							    .totaltblmlt {margin: 10px 0 0 10px;}
							    .totaltblmlt tr td {text-align:right;color: #333;width: 85px;}
							    .totaltblmlt tr td.subhd , .totaltbl tr td.subhd {width: 390px;padding-right: 10px;text-align:right;}
							     .clr {clear:both;}
							     .airtbl {margin-top : 30px;width:320px;}
							    .emphasis {color:#333333 !important;font-weight:bold;}
							     .padleftten {padding-left: 10px;}
							     .option {color: #aaaaaa;}
							     .fmf {color:red;}
							      .airtbl tr td{padding-right: 15px;color: #757575;padding-bottom: 5px;}
							      .tblfooter {color: #757575;padding-left:10px;}
							      .help{margin-right: 5px;vertical-align: middle;}
							      .footercontainer{width:400px;}
							      .exchange{width:100%;}
							      
/*For 26568 spec */
.headerDetails {color:#000000 !important;font-weight:bold;}
.description {color:#333333;}
.topBorder{border-top:1px solid #CCCCCC;}
.bottomBorder{border-bottom:1px solid #CCCCCC;}
.bottomMargin{margin:7px 0 20px;}
.redFmf{color:#ff0000;}
.padBottom {padding-bottom:20px !important;}
.contentNew {color:#666666;}
HR.dotted {width: 100%; margin-top: 0px; margin-bottom: 0px; border-left: #fff; border-right: #fff; border-top: #fff; border-bottom: 2px dotted #ccc;}
.useNextBox {
background:#FFFFFF url(http://www.paypal.com/en_US/i/scr/scr_table_bg_1x70.gif) repeat-x scroll left bottom;
border:1px solid #CCCCCC;
border-collapse:separate;
padding:10px 10 20px 10px;
width:530px;
margin-left:20px;
margin-top:20px;
color:#333333;
font-family:arial,helvetica,sans-serif;
font-size:11px;
}
.boxHead{width:530px;padding-bottom:10px !important;border-bottom:1px solid #CCCCCC;text-align:left;}
.boxHeadingTxt{font-size:12px;font-weight:bold;}
.padBoxTop {padding-top:10px !important;}
.SignupLink{color:#084482 !important;}
							     </style>

<table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td></tr></table><table align='center' border='0' cellpadding='0' cellspacing='0' width='600'><tr valign='top'><td width='100%'><table align='center' border='0' cellpadding='0' cellspacing='0' style='color:#333333 !important;font-family: arial,helvetica,sans-serif;font-size:12px;' width='600'><tr valign='top'><td><img src='http://images.paypal.com/en_US/i/logo/paypal_logo.gif' border='0' alt='PayPal logo'></td><td valign='middle' align='right'>Oct 2, 2016 07:52:25 CDT<br>Transaction ID: <a target='new' href='https://www.paypal.com/us/cgi-bin/webscr?cmd=_view-a-trans&amp;id=2CA1467185766913E'>2CA1467185766913E</a></td></tr></table><div style='margin-top: 30px;color:#333 !important;font-family: arial,helvetica,sans-serif;font-size:12px;'><span style='color:#333333 !important;font-weight:bold;font-family: arial,helvetica,sans-serif;'>Hello Zhijun Ding,</span><br><br><span style='font-size:14px;color:#C88039;font-weight:bold;text-decoration:none;'>You received a payment of $10.90 USD from coffeebean217 (yueding@gmail.com)</span><br><table cellpadding='5'><tr><td valign='top'>Thanks for using PayPal. You can now ship any items.To see all the transaction details, log in to your PayPal account.<br><br>It may take a few moments for this transaction to appear in your account.<br><br><span style='font-weight:bold;color:#333333;'>Seller Protection - </span><span style='color: #4c8f3a;'>Eligible</span></td><td></tr></table><br><div style='margin-top:5px;'><hr size='1'></div><br><table border='0' cellpadding='3' cellspacing='3' style='color:#333333 !important;font-family: arial,helvetica,sans-serif;font-size:12px;'><tr><td><table border='0' cellpadding='0' cellspacing='0' width='98%'><tr><td style='padding-top:5px;padding-right:10px' valign='top' width='50%' align='left'><span style='color:#333333;font-weight:bold;'>Buyer</span><br>Yue Zhang<br>coffeebean217<br>yueding@gmail.com</td><td style='padding-top:5px;' valign='top'><span style='color:#333333;font-weight:bold;'>Note to seller</span><br>The buyer hasn't sent a note.</td></tr><tr><td style='padding-top:10px;' valign='top' width='40%' align='left'><span style='font-family: arial,helvetica,sans-serif;font-size:12px;'><span style='color:#333333;font-weight:bold;'>Shipping address - </span><span style='color: #4c8f3a;'>confirmed</span></span><br>Yue Zhang<br>1642 Crossgate Dr<br>Vestavia,&nbsp;AL&nbsp;35216<br>United States<br></td><td style='padding-top:10px;' valign='top'><span style='color:#333333;font-weight:bold;'>Shipping details</span><br>You haven’t added any shipping details.</td></tr></table><br><table align='center' border='0' cellpadding='0' cellspacing='0' style='clear:both;color: #333 !important;font-size: 12px;font-family: arial,helvetica,sans-serif;' width='598px'><tr><td style='border:1px solid #ccc;border-right:none;border-left:none;padding:5px 10px 5px 10px !important;color: #333333;' width='348' align='left'>Description</td><td style='border:1px solid #ccc;border-right:none;border-left:none;padding:5px 10px 5px 10px !important;color: #333333;' width='100' align='right'>Unit price</td><td style='border:1px solid #ccc;border-right:none;border-left:none;padding:5px 10px 5px 10px !important;color: #333333;' width='50' align='right'>Qty</td><td style='border:1px solid #ccc;border-right:none;border-left:none;padding:5px 10px 5px 10px !important;color: #333333;' width='80' align='right'>Amount</td></tr><tr><td valign='top' align='left' style='border-bottom:none;padding: 10px;'><a target='new' href='http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item=152262952392&amp;var=451456781220'>Kirkland Signature Men's Crew Neck Tee 6-pack White [Medium]</a><br>Item# 152262952392</td><td valign='top' align='right' style='border-bottom:none;padding: 10px;'>$10.00 USD</td><td valign='top' align='right' style='border-bottom:none;padding: 10px;'>1</td><td valign='top' align='right' style='border-bottom:none;padding: 10px;'>$10.00 USD</td></tr></table><table align='center' border='0' cellpadding='0' cellspacing='0' style='clear:both;border-top: 1px solid #ccc;border-bottom: 1px solid #ccc;margin: 0 0 10px 0;' width='595'><tr><td valign='top'><img src='https://securepics.ebaystatic.com/aw/pics/logos/paypal/logo_ebay_62x26.gif' border='0' style='margin-top:10px;' alt=''></td><td><table border='0' cellpadding='0' cellspacing='0' style='clear:both;margin-top: 10px;color:#333333 !important;font-family: arial,helvetica,sans-serif;font-size:12px;' align='right'><tr><td style='width:390px;text-align:right; padding:0 10px 0 0;'>Shipping and handling</td><td align='right' style='width:90px; color:#333;text-align:right; padding:0 5px 0 0;'>$0.00 USD</td></tr><tr><td style='width:390px;text-align:right; padding:0 10px 0 0;'>Tax</td><td style='width:90px; color:#333;text-align:right; padding:0 5px 0 0;'>$0.90 USD</td></tr><tr><td style='width:390px;text-align:right; padding:0 10px 0 0;'><span style='color:#333333 !important;font-weight:bold;'>Total</span></td><td style='width:90px; color:#333;text-align:right; padding:0 5px 0 0;'>$10.90 USD</td></tr><tr><td style='width:390px;text-align:right; padding:20px 10px 0 0;'><span style='color:#333333 !important;font-weight:bold;'>Payment</span></td><td style='width:90px; color:#333;text-align:right; padding:20px 5px 0 0;'>$10.90 USD</td></tr><tr><td style='width:390px;text-align:right; padding:0 10px 0 0;'><br>Payment sent to zjding@outlook.com</td><td style='width:90px; color:#333;text-align:right; padding:20px 5px 0 0;'></tr><tr><td colspan='2' style='height:10px;'></tr></table></td></tr></table></td></tr></table><img src='http://images.paypal.com/en_US/i/icon/icon_help_16x16.gif' border='0' style='margin-right: 5px;vertical-align: middle;' alt=''>Questions? Go to the Help Center at: <a target='_blank' href='https://www.paypal.com/help'>www.paypal.com/help</a>.<br><br><span style='font-size:11px;color:#333;'>Please do not reply to this email. This mailbox is not monitored and you will not receive a response. For assistance, log in to your PayPal account and click <strong>Help</strong> in the top right corner of any PayPal page.</span><br><br><span style='font-size:11px;color:#333;'>You can receive plain text emails instead of HTML emails. To change your Notifications preferences, log in to your account, go to your Profile, and click <strong>My settings</strong>.</span></div><br><hr width='400'><span class='xptFooter'>Copyright © 1999-2016 PayPal. All rights reserved.<br></span><br><span style='color: #333333;font-family: arial,helvetica,sans-serif;font-size:11px;'><span class='xptFooter ppid'>PayPal Email ID PP753 - d3f6f005dc559</span></span><img height='1' width='1' src='https://paypal.112.2o7.net/b/ss/paypalglobal/1/H.22--NS/1186545974?c6=2HM10668EP149602B&amp;v0=PP753_0_&amp;pe=lnk_o&amp;pev1=email&amp;pev2=D=v0&amp;events=scOpen' border='0' alt=''><img height='1' width='1' src='https://t.paypal.com/ts?ppid=&amp;cust=2HM10668EP149602B&amp;cnac=US&amp;rsta=en_US&amp;unptid=1ae71380-889f-11e6-9f66-441ea14ea960&amp;unp_tpcid=email-auction-payment-notification&amp;e=op&amp;mchn=em&amp;s=ci&amp;mail=sys&amp;page=main:email::::::' border='0' alt=''></td></tr></table></div></body></html>";

            ProcessPaymentReceivedEmail(html);
        }

        private void ProcessPaymentReceivedEmail(string html)
        {
            {
                string body = html;

                Dictionary<string, string> timeZones = new Dictionary<string, string>() {
            {"ACDT", "+1030"},
            {"ACST", "+0930"},
            {"ADT", "-0300"},
            {"AEDT", "+1100"},
            {"AEST", "+1000"},
            {"AHDT", "-0900"},
            {"AHST", "-1000"},
            {"AST", "-0400"},
            {"AT", "-0200"},
            {"AWDT", "+0900"},
            {"AWST", "+0800"},
            {"BAT", "+0300"},
            {"BDST", "+0200"},
            {"BET", "-1100"},
            {"BST", "-0300"},
            {"BT", "+0300"},
            {"BZT2", "-0300"},
            {"CADT", "+1030"},
            {"CAST", "+0930"},
            {"CAT", "-1000"},
            {"CCT", "+0800"},
            {"CDT", "-0500"},
            {"CED", "+0200"},
            {"CET", "+0100"},
            {"CEST", "+0200"},
            {"CST", "-0600"},
            {"EAST", "+1000"},
            {"EDT", "-0400"},
            {"EED", "+0300"},
            {"EET", "+0200"},
            {"EEST", "+0300"},
            {"EST", "-0500"},
            {"FST", "+0200"},
            {"FWT", "+0100"},
            {"GMT", "GMT"},
            {"GST", "+1000"},
            {"HDT", "-0900"},
            {"HST", "-1000"},
            {"IDLE", "+1200"},
            {"IDLW", "-1200"},
            {"IST", "+0530"},
            {"IT", "+0330"},
            {"JST", "+0900"},
            {"JT", "+0700"},
            {"MDT", "-0600"},
            {"MED", "+0200"},
            {"MET", "+0100"},
            {"MEST", "+0200"},
            {"MEWT", "+0100"},
            {"MST", "-0700"},
            {"MT", "+0800"},
            {"NDT", "-0230"},
            {"NFT", "-0330"},
            {"NT", "-1100"},
            {"NST", "+0630"},
            {"NZ", "+1100"},
            {"NZST", "+1200"},
            {"NZDT", "+1300"},
            {"NZT", "+1200"},
            {"PDT", "-0700"},
            {"PST", "-0800"},
            {"ROK", "+0900"},
            {"SAD", "+1000"},
            {"SAST", "+0900"},
            {"SAT", "+0900"},
            {"SDT", "+1000"},
            {"SST", "+0200"},
            {"SWT", "+0100"},
            {"USZ3", "+0400"},
            {"USZ4", "+0500"},
            {"USZ5", "+0600"},
            {"USZ6", "+0700"},
            {"UT", "-0000"},
            {"UTC", "-0000"},
            {"UZ10", "+1100"},
            {"WAT", "-0100"},
            {"WET", "-0000"},
            {"WST", "+0800"},
            {"YDT", "-0800"},
            {"YST", "-0900"},
            {"ZP4", "+0400"},
            {"ZP5", "+0500"},
            {"ZP6", "+0600"}
        };

                // TransactionID
                string stTime = SubstringEndBack(body, "Transaction ID:", "<td ", true, false);
                stTime = TrimTags(stTime);
                stTime = stTime.Replace("<br>", "");


                string stTimeZone = stTime.Substring(stTime.LastIndexOf(' ') + 1, stTime.Length - stTime.LastIndexOf(' ') - 1);

                DateTime dtTime = Convert.ToDateTime(stTime.Replace(stTimeZone, timeZones[stTimeZone]));

                string stTransactionID = SubstringInBetween(body, "Transaction ID:", "</a>", true, true);

                stTransactionID = SubstringEndBack(stTransactionID, "</a>", ">", false, false);

                // Buyer Name
                string stBuyer = SubstringInBetween(body, "Buyer", @"</a>", false, true);

                string stFullName = SubstringInBetween(stBuyer, "<br>", "<br>", false, false);

                stBuyer = stBuyer.Replace("<br>" + stFullName + "<br>", "");

                string stUserID = SubstringInBetween(stBuyer, @"</span>", "<br>", false, false);

                stBuyer = stBuyer.Replace(stUserID, "");
                stBuyer = TrimTags(stBuyer);

                string stUserEmail = stBuyer.Substring(0, stBuyer.IndexOf('<'));
                

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

                string stItemName = SubstringEndBack(body , "Item# " + stItemNum, "<a target='new' href='http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item=" + stItemNum, true, false);
                stItemName = TrimTags(stItemName);
                stItemName = stItemName.Substring(0, stItemName.IndexOf('<'));

                // Amount
                string stAmount = SubstringInBetween(body, "Item# " + stItemNum, @"</table>", false, false);
                stAmount = TrimTags(stAmount);
                //stAmount = stAmount.Substring(0, stAmount.IndexOf('<'));

                string stUnitePrice = stAmount.Substring(0, stAmount.IndexOf("<"));
                stUnitePrice = stUnitePrice.Replace("$", "");
                stUnitePrice = stUnitePrice.Replace("USD", "");
                stUnitePrice = stUnitePrice.Trim();

                stAmount = stAmount.Substring(stUnitePrice.Length + 5);

                stAmount = TrimTags(stAmount);

                string stQuatity = stAmount.Substring(0, stAmount.IndexOf("<"));

                stAmount = stAmount.Substring(stQuatity.Length);

                stAmount = TrimTags(stAmount);

                string stTotal = stAmount.Substring(0, stAmount.IndexOf("<"));
                stTotal = stTotal.Replace("$", "");
                stTotal = stTotal.Replace("USD", "");
                stTotal = stTotal.Trim();

                // Tax 
                string stTax = SubstringInBetween(body, "Tax", "Total", false, false);
                stTax = TrimTags(stTax);
                stTax = stTax.Substring(0, stTax.IndexOf("<"));
                stTax = stTax.Replace("$", "");
                stTax = stTax.Replace("USD", "");
                stTax = stTax.Trim();

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

                File.Delete(@"C:\eBayApp\Files\Emails\PaypalPaidEmails\" + destinationFileName);

                File.Move(sourceFileFullName, @"C:\eBayApp\Files\Emails\PaypalPaidEmails\" + destinationFileName);

                sourceFileFullName = destinationFileName;
                destinationFileName = dtTime.ToString("yyyyMMdd") + "-" + stTotal + "-" + "Paypal" + stTransactionID + ".pdf";
                File.Delete(@"C:\Users\Jason Ding\Dropbox\Bookkeeping\Income\" + destinationFileName);
                File.Copy(@"C:\eBayApp\Files\Emails\PaypalPaidEmails\" + sourceFileFullName, @"C:\Users\Jason Ding\Dropbox\Bookkeeping\Income\" + destinationFileName);

                // db stuff
                SqlConnection cn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cn.Open();

                string sqlString = @"UPDATE eBay_SoldTransactions SET PaypalTransactionID = @_paypalTransactionID, 
                                PaypalPaidDateTime = @_paypalPaidDateTime, PaypalPaidEmailPdf = @_paypalPaidEmailPdf,
                                BuyerEmail = @_buyerEmail,
                                BuyerName = @_buyerName,
                                BuyerAddress1 = @_buyerAddress1, 
                                BuyerCity = @_buyerCity, 
                                BuyerState = @_buyerState, BuyerZip = @_buyerZip, BuyerNote = @_buyerNote,
                                eBaySoldQuality = @_eBaySoldQuality, eBaySaleTax = @_eBaySaleTax
                                WHERE eBayItemNumber = @_eBayItemNumber AND BuyerID = @_buyerID";

                cmd.CommandText = sqlString;
                cmd.Parameters.AddWithValue("@_paypalTransactionID", stTransactionID);
                cmd.Parameters.AddWithValue("@_paypalPaidDateTime", dtTime);
                cmd.Parameters.AddWithValue("@_paypalPaidEmailPdf", destinationFileName);
                cmd.Parameters.AddWithValue("@_buyerEmail", stUserEmail);
                cmd.Parameters.AddWithValue("@_buyerName", stFullName);
                cmd.Parameters.AddWithValue("@_buyerAddress1", stShippingAddress1);
                //cmd.Parameters.AddWithValue("@_buyAddress2", stShippingAddress2);
                cmd.Parameters.AddWithValue("@_buyerCity", stShippingCity);
                cmd.Parameters.AddWithValue("@_buyerState", stShippingState);
                cmd.Parameters.AddWithValue("@_buyerZip", stShippingZip);
                cmd.Parameters.AddWithValue("@_buyerNote", stBuyerNote);
                cmd.Parameters.AddWithValue("@_eBaySoldQuality", stQuatity);
                cmd.Parameters.AddWithValue("@_eBayItemNumber", stItemNum);
                cmd.Parameters.AddWithValue("@_eBaySaleTax", stTax);
                cmd.Parameters.AddWithValue("@_buyerID", stUserID);

                cmd.ExecuteNonQuery();

                sqlString = @"SELECT * FROM eBay_SoldTransactions WHERE eBayItemNumber = @_eBayItemNumber AND BuyerID = @_buyerID";

                cmd.CommandText = sqlString;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@_eBayItemNumber", stItemNum);
                cmd.Parameters.AddWithValue("@_buyerID", stUserID);

                eBaySoldProduct soldProduct = new eBaySoldProduct();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    soldProduct.PaypalTransactionID = Convert.ToString(reader["PaypalTransactionID"]);
                    soldProduct.PaypalPaidDateTime = Convert.ToDateTime(reader["PaypalPaidDateTime"]);
                    soldProduct.PaypalPaidEmailPdf = Convert.ToString(reader["PaypalPaidEmailPdf"]);
                    soldProduct.eBayItemNumber = Convert.ToString(reader["eBayItemNumber"]);
                    soldProduct.eBaySoldDateTime = Convert.ToDateTime(reader["eBaySoldDateTime"]);
                    soldProduct.eBayItemName = Convert.ToString(reader["eBayItemName"]);
                    //soldProduct.eBayListingQuality = Convert.ToInt16(reader["eBayListingQuality"]);
                    soldProduct.eBaySoldQuality = Convert.ToInt16(reader["eBaySoldQuality"]);
                    soldProduct.eBaySoldEmailPdf = Convert.ToString(reader["eBaySoldEmailPdf"]);
                    soldProduct.BuyerName = Convert.ToString(reader["BuyerName"]);
                    soldProduct.BuyerID = Convert.ToString(reader["BuyerID"]);
                    soldProduct.BuyerAddress1 = Convert.ToString(reader["BuyerAddress1"]);
                    soldProduct.BuyerAddress2 = Convert.ToString(reader["BuyerAddress2"]);
                    soldProduct.BuyerCity = Convert.ToString(reader["BuyerCity"]);
                    soldProduct.BuyerState = Convert.ToString(reader["BuyerState"]);
                    soldProduct.BuyerZip = Convert.ToString(reader["BuyerZip"]);
                    soldProduct.BuyerEmail = Convert.ToString(reader["BuyerEmail"]);
                    soldProduct.BuyerNote = Convert.ToString(reader["BuyerNote"]);
                    soldProduct.CostcoUrlNumber = Convert.ToString(reader["CostcoUrlNumber"]);
                    soldProduct.CostcoUrl = Convert.ToString(reader["CostcoUrl"]);
                    soldProduct.CostcoPrice = Convert.ToDecimal(reader["CostcoPrice"]);

                    //soldProduct.CostcoUrl = "http://www.costco.com/Vasanti-Gel-Matte-Lipstick-with-Lipline-Extreme-Lipliner.product.100243171.html";
                }
                reader.Close();

                cn.Close();
            }
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
                input = input.TrimStart();
            }

            return input;
        }

        private void ProcessItemSoldEmail(string subject, string body)
        {
            try
            {
                int iLastSpace = subject.LastIndexOf(' ');
                string stItemNum = subject.Substring(iLastSpace, subject.Length - iLastSpace);
                stItemNum = stItemNum.Trim();

                //string stItemNum = SubstringInBetween(subject, "(", ")", false, false);

                subject = subject.Replace(stItemNum, "");
                subject = subject.Replace("Your eBay item sold!", "");

                string stItemName = subject.Trim();

                stItemName = WebUtility.HtmlDecode(stItemName);

                body = WebUtility.HtmlDecode(body);

                body = SubstringInBetween(body, @"<body", @"</body>", true, true);

                string stUrl = SubstringEndBack(body, ">" + stItemName, "<a href =", false, false);
                stUrl = SubstringInBetween(stUrl, "'", "'", false, false);

                string stPaid = SubstringInBetween(body, "Paid:", @"<br>", false, false);
                stPaid = stPaid.Replace("$", "");
                stPaid = stPaid.Trim();

                string stSize = SubstringInBetween(body, @"Size:", @"</td>", false, false);
                stSize = stSize.Trim();

                string stDateSold = SubstringInBetween(body, @"Date Sold:", @"</td>", false, false);
                stDateSold = stDateSold.Trim();

                string stQuantitySold = SubstringInBetween(body, @"Quantity Sold:", @"</td>", false, false);
                stQuantitySold = stQuantitySold.Trim();

                string stBuyer = SubstringEndBack(body, "Contact Buyer", "Buyer", false, false);
                stBuyer = stBuyer.Replace(@"&nbsp", "");
                stBuyer = TrimTags(stBuyer);
                stBuyer = stBuyer.Substring(0, stBuyer.IndexOf("<"));

                //string stEndTime = SubstringInBetween(body, "End time:", "PDT", false, true);

                //stEndTime = SubstringEndBack(stEndTime, "PDT", ">", false, true);

                //string correctedTZ = stEndTime.Replace("PDT", "-0700");
                //DateTime dt = Convert.ToDateTime(correctedTZ);

                //string stPrice = SubstringInBetween(body, "Sale price:", "Quantity:", false, false);

                //stPrice = SubstringInBetween(stPrice, "$", "<", false, false);

                //string stQuantity = SubstringInBetween(body, "Quantity:", "Quantity sold:", false, false);

                //stQuantity = TrimTags(stQuantity);

                //stQuantity = stQuantity.Substring(0, stQuantity.IndexOf("<"));

                //string stQuantitySold = SubstringInBetween(body, "Quantity sold:", "Quantity remaining:", false, false);

                //stQuantitySold = TrimTags(stQuantitySold);

                //stQuantitySold = stQuantitySold.Substring(0, stQuantitySold.IndexOf("<"));

                //string stQuantityRemaining = SubstringInBetween(body, "Quantity remaining:", "Buyer:", false, false);

                //stQuantityRemaining = TrimTags(stQuantityRemaining);

                //stQuantityRemaining = stQuantityRemaining.Substring(0, stQuantityRemaining.IndexOf("<"));

                //string stBuyerName = SubstringInBetween(body, "Buyer:", "<div>", false, false);

                //stBuyerName = TrimTags(stBuyerName);

                //stBuyerName = stBuyerName.Substring(0, stBuyerName.IndexOf("<"));

                //string stBuyerId = SubstringInBetween(body, stBuyerName, "(<a href='mailto", false, true);

                //stBuyerId = TrimTags(stBuyerId);

                //stBuyerId = stBuyerId.Substring(0, stBuyerId.IndexOf("(<a href='mailto"));

                //stBuyerId = stBuyerId.Trim();

                //string stBuyerEmail = SubstringInBetween(body, "(<a href='mailto:", "'", false, false);

                // Generate PDF for email
                //string destinationFileName = dt.ToString("yyyyMMddHHmmss") + "_" + stItemNum + ".html";
                //File.WriteAllText(@"C:\temp\eBaySoldEmails\" + destinationFileName, body);

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

                string destinationFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + stItemNum + ".pdf";

                File.Delete(@"C:\temp\eBaySoldEmails\" + destinationFileName);

                File.Move(sourceFileFullName, @"C:\eBayApp\Files\Emails\eBaySoldEmails\" + destinationFileName);


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
                    // eBayProduct.eBayListingDT = Convert.ToDateTime(reader["eBayListingDT"]);
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
                              (eBayItemNumber, eBaySoldDateTime, eBayItemName, eBayUrl, eBaySoldVariation, eBaySoldPrice, eBaySoldQuality, eBaySoldEmailPdf,
                               BuyerID, CostcoUrlNumber, CostcoItemNumber, CostcoUrl, CostcoPrice)
                              VALUES (@_eBayItemNumber, @_eBaySoldDateTime, @_eBayItemName, @_eBayUrl, @_eBaySoldVariation, @_eBayPrice, @_eBaySoldQuality,  @_eBaySoldEmailPdf,
                               @_BuyerID, @_CostcoUrlNumber, @_CostcoItemNumber, @_CostcoUrl, @_CostcoPrice)";

                    cmd.CommandText = sqlString;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_eBayItemNumber", stItemNum);
                    cmd.Parameters.AddWithValue("@_eBaySoldDateTime", Convert.ToDateTime(stDateSold));
                    cmd.Parameters.AddWithValue("@_eBayItemName", stItemName);
                    cmd.Parameters.AddWithValue("@_eBayUrl", stUrl);
                    cmd.Parameters.AddWithValue("@_eBaySoldVariation", stSize);
                    cmd.Parameters.AddWithValue("@_eBayPrice", Convert.ToDecimal(eBayProduct.eBayListingPrice));
                   // cmd.Parameters.AddWithValue("@_eBayListingQuality", Convert.ToInt16(stQuantity));
                    cmd.Parameters.AddWithValue("@_eBaySoldQuality", Convert.ToInt16(stQuantitySold));
                   // cmd.Parameters.AddWithValue("@_eBayRemainingQuality", Convert.ToInt16(stQuantityRemaining));
                    cmd.Parameters.AddWithValue("@_eBaySoldEmailPdf", destinationFileName);
                    cmd.Parameters.AddWithValue("@_BuyerID", stBuyer);
                    //cmd.Parameters.AddWithValue("@_BuyerID", stBuyerId);
                    //cmd.Parameters.AddWithValue("@_BuyerEmail", stBuyerEmail);
                    cmd.Parameters.AddWithValue("@_CostcoUrlNumber", eBayProduct.CostcoUrlNumber);
                    cmd.Parameters.AddWithValue("@_CostcoItemNumber", eBayProduct.CostcoItemNumber);
                    cmd.Parameters.AddWithValue("@_CostcoUrl", eBayProduct.CostcoUrl);
                    cmd.Parameters.AddWithValue("@_CostcoPrice", eBayProduct.CostcoPrice);

                    cmd.ExecuteNonQuery();

                    //eBaySoldProduct soldProduct = new eBaySoldProduct();
                    //soldProduct.eBayItemName = stItemNum;
                    //soldProduct.eBaySoldDateTime = dt;
                    //soldProduct.eBayItemName = stItemName;
                    //soldProduct.eBayUrl = stUrl;
                    //soldProduct.eBaySoldPrice = Convert.ToDecimal(stPrice);
                    //soldProduct.eBayListingQuality = Convert.ToInt16(stQuantity);
                    //soldProduct.eBaySoldQuality = Convert.ToInt16(stQuantitySold);
                    //soldProduct.eBayRemainingQuality = Convert.ToInt16(stQuantityRemaining);
                    //soldProduct.eBaySoldEmailPdf = destinationFileName;
                    //soldProduct.BuyerName = stBuyerName;
                    //soldProduct.BuyerID = stBuyerId;
                    //soldProduct.BuyerEmail = stBuyerEmail;
                    //soldProduct.CostcoUrlNumber = eBayProduct.CostcoUrlNumber;
                    //soldProduct.CostcoUrl = eBayProduct.CostcoUrl;
                    //soldProduct.CostcoPrice = eBayProduct.CostcoPrice;
                }
                else
                {

                }

                cn.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
        }

        private void ProcessCostcoOrderEmail(string body)
        {
            try
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


                stWorking = TrimTags(stShipping);

                string stBuyerName = stWorking.Substring(0, stWorking.IndexOf("<"));

                stWorking = stWorking.Replace(stBuyerName, "");

                stWorking = TrimTags(stWorking);

                string stAddress1 = stWorking.Substring(0, stWorking.IndexOf("<"));

                stWorking = stWorking.Replace(stAddress1, "");

                stWorking = TrimTags(stWorking);

                string stAddress2 = stWorking.Substring(0, stWorking.IndexOf("<"));

                string stTax = SubstringInBetween(body, "Tax:", "</tr>", false, false);

                stTax = TrimTags(stTax);

                stTax = stTax.Substring(0, stTax.IndexOf("<"));

                stTax = stTax.Replace("$", "");

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

                string destinationFileName = Convert.ToDateTime(stDatePlaced).ToString("yyyyMMddHHmmss") + "_" + stOrderNumber + ".pdf";

                File.Delete(@"C:\temp\CostcoOrderEmails\" + destinationFileName);

                File.Move(sourceFileFullName, @"C:\temp\CostcoOrderEmails\" + destinationFileName);

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
                                CostcoOrderEmailPdf = @_costcoOrderEmailPdf, CostcoTax = @_costcoTax 
                                WHERE CostcoItemNumber = @_costcoItemNumber 
                                AND BuyerName = @_buyerName AND  CostcoOrderNumber IS NULL";

                        cmd.CommandText = sqlString;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@_costcoOrderNumber", stOrderNumber);
                        cmd.Parameters.AddWithValue("@_costcoOrderEmailPdf", destinationFileName);
                        cmd.Parameters.AddWithValue("@_costcoTax", stTax);
                        cmd.Parameters.AddWithValue("@_costcoItemNumber", stItemNum);
                        cmd.Parameters.AddWithValue("@_buyerName", stBuyerName);

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    sqlString = @"SELECT * FROM eBay_SoldTransactions WHERE CostcoItemName = @_costcoItemName
                                AND BuyerName = @_buyerName AND CostcoOrderNumber IS NULL";

                    cmd.CommandText = sqlString;
                    cmd.Parameters.AddWithValue("@_costcoItemName", stProductName);
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
                                CostcoOrderEmailPdf = @_costcoOrderEmailPdf, CostcoTax = @_costcoTax 
                                WHERE CostcoItemName = @_costcoItemName 
                                AND BuyerName = @_buyerName AND  CostcoOrderNumber IS NULL";

                        cmd.CommandText = sqlString;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@_costcoOrderNumber", stOrderNumber);
                        cmd.Parameters.AddWithValue("@_costcoOrderEmailPdf", destinationFileName);
                        cmd.Parameters.AddWithValue("@_costcoTax", stTax);
                        cmd.Parameters.AddWithValue("@_costcoItemName", stProductName);
                        cmd.Parameters.AddWithValue("@_buyerName", stBuyerName);

                        cmd.ExecuteNonQuery();
                    }
                }

                cn.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
        }

        private void btnSoldEmailExtract_Click(object sender, EventArgs e)
        {
            string subject = "Your eBay item sold! Kirkland Signature Men's Crew Neck Tee 6-pack White 152262952392";
            string body = @"<div dir='ltr'><div class='gmail_quote'><br><div><div><div><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='100%' style='word-wrap:break-word'><table cellpadding='2' cellspacing='3' border='0' width='100%'><tbody><tr><td width='1%' nowrap><img src='http://q.ebaystatic.com/aw/pics/logos/ebay_95x39.gif' height='39' width='95' alt='eBay'></td><td align='left' valign='bottom'><span style='font-weight:bold;font-size:xx-small;font-family:verdana,sans-serif;color:#666'><b>eBay sent this message to Yue Zhang (coffeebean217).</b><br></span><span style='font-size:xx-small;font-family:verdana,sans-serif;color:#666'>Your registered name is included to show this message originated from eBay. <a href='http://pages.ebay.com/help/confidence/name-userid-emails.html' target='_blank'>Learn more</a>.</span></td></tr></tbody></table></td></tr></tbody></table></div></div><div><div><table style='background-color:#ffe680' border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='8' valign='top'><img src='http://q.ebaystatic.com/aw/pics/globalAssets/ltCurve.gif' height='8' width='8'></td><td valign='bottom' width='100%'><span style='font-weight:bold;font-size:14pt;font-family:arial,sans-serif;color:#000;margin:2px 0 2px 0'>Congratulations, your item sold-get ready to ship!</span></td><td width='8' valign='top' align='right'><img src='http://p.ebaystatic.com/aw/pics/globalAssets/rtCurve.gif' height='8' width='8'></td></tr><tr><td style='background-color:#fc0' colspan='3' height='4'></td></tr></tbody></table></div></div><div><div><table border='0' cellpadding='2' cellspacing='3' width='100%'><tbody><tr><td><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'>Hi coffeebean217,<table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='10' alt=' '></td></tr></tbody></table>You did it! Your item sold. Please ship this item to the buyer after your buyer pays. As soon as your buyer pays, print your <a href='http://rover.ebay.com/rover/0/e12011.m354.l1337/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostage%26transactionid%3D1105542200006%26itemid%3D161193141452%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1337' target='_blank'>eBay shipping label</a>.<table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='10' alt=' '></td></tr></tbody></table><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'><ul style='list-style-type:decimal'><li style='padding:5px'>Get discounted rates for shipping.</li><li style='padding:5px'>eBay label printing service is FREE!</li><li style='padding:5px'>Tracking information is uploaded automatically to My eBay and an email is sent to your buyer that their shipment is on the way.</li></ul><br><div style='padding-bottom:20px'>If you don&#39;t use eBay label printing, <a href='http://rover.ebay.com/rover/0/e12011.m354.l1663/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FAddTrackingNumber2%26flow%3Dmyebay%26LineID%3D161193141452_1105542200006%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1663' target='_blank'>upload your tracking information manually</a>.</div><br><div style='padding-bottom:20px'> You should always <a href='http://rover.ebay.com/rover/0/e12011.m354.l1332/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Ffeedback.ebay.com%2Fws%2FeBayISAPI.dll%3FLeaveFeedback2%26show_as%3Dsold%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1332' target='_blank'>leave feedback</a> for your buyer to encourage them to buy from you again.</div></font></font><div><table width='100%' cellpadding='0' cellspacing='3' border='0'><tbody><tr><td valign='top' align='center' width='100' nowrap><a href='http://rover.ebay.com/rover/0/e12011.m43.l1123/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D161193141452%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1123' target='_blank'><img src='http://thumbs.ebaystatic.com/pict/161193141452.jpg' alt='New Estée Lauder Pure Color Long Lasting lipstick ~Rubellite SHIMMER~' border='0'></a></td><td colspan='2' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' colspan='2'><a href='http://rover.ebay.com/rover/0/e12011.m43.l1123/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D161193141452%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1123' target='_blank'>New Estée Lauder Pure Color Long Lasting lipstick ~Rubellite SHIMMER~</a></td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>End time:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>Jun-04-14 08:29:55 PDT</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'><font style='font-weight:bold;font-size:10pt;font-family:arial,sans-serif;color:#000'>Sale price:</font></td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'><font style='font-weight:bold;font-size:10pt;font-family:arial,sans-serif;color:#000'>$9.50</font></td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Quantity:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>1</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Quantity sold:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>1</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Quantity remaining:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>0</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'>Buyer:</td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'>STAVROULLA XENI</td></tr><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' width='15%' nowrap valign='top'></td><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'><div><table border='0' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-size:10pt;font-family:arial,sans-serif;color:#000' valign='top'><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'>stavri.lav (<a href='mailto:sxeni@treasury.gov.cy' target='_blank'>sxeni@treasury.gov.cy</a>) [<a href='http://contact.ebay.com/ws/eBayISAPI.dll?ReturnUserEmail&amp;requested=stavri.lav&amp;redirect=0&amp;iid=161193141452' target='_blank'>contact buyer</a>]</font></td></tr></tbody></table></div></td></tr><tr><td colspan='2'><font style='font-size:10pt;font-family:arial,sans-serif;color:#000'><a href='http://rover.ebay.com/rover/0/e12011.m43.l1151/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws%2FeBayISAPI.dll%3FSellHub3%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1151' target='_blank'>Sell another Item</a>   |    <a href='http://rover.ebay.com/rover/0/e12011.m43.l1156/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FUnifiedCheckoutSellerUpdateDetails%26itemId%3D161193141452%26transId%3D1105542200006%26buyerid%3D0%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1156' target='_blank'>Send invoice to buyer</a></font></td></tr></tbody></table></td></tr></tbody></table></div></td><td valign='top' width='185'><div><span style='font-weight:bold;font-size:10pt;font-family:arial,sans-serif;color:#000'><strong>As soon as your buyer pays</strong></span><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td><img src='http://q.ebaystatic.com/aw/pics/s.gif' height='4' alt=' '></td></tr></tbody></table><a href='http://rover.ebay.com/rover/0/e12011.m44.l1337/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=https%3A%2F%2Fpostage.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostageLabelRedirect%26itemid%3D161193141452%26transactionid%3D1105542200006%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1337' title='http://rover.ebay.com/rover/0/e12011.m44.l1337/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;loc=https%3A%2F%2Fpostage.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostageLabelRedirect%26itemid%3D161193141452%26transactionid%3D1105542200006%26ssPageName%3DADME%3AL%3AEOISSA%3AUS%3A1337' target='_blank'><img src='http://p.ebaystatic.com/aw/pics/buttons/btnPrintShippingLabel.gif' border='0' height='32' width='120'></a><br></div></td></tr></tbody></table><br></div></div><div></div><div></div><div><div><hr style='min-height:1px'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='100%'><font style='font-size:8pt;font-family:arial,sans-serif;color:#000000'>Email reference id: [#80af0a23f2c047c5b6fac4936c26c954#]</font></td></tr></tbody></table><br></div><hr style='min-height:1px'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr><td width='100%'><font style='font-size:xx-small;font-family:verdana;color:#666'><a href='http://pages.ebay.com/education/spooftutorial/index.html' target='_blank'>Learn More</a> to protect yourself from spoof (fake) emails.<br><br>eBay sent this email to you at <a href='mailto:coffeebean217@gmail.com' target='_blank'>coffeebean217@gmail.com</a> about your account registered on <a href='http://www.ebay.com' target='_blank'>www.ebay.com</a>.<br><br>eBay will periodically send you required emails about the site and your transactions. Visit our <a href='http://pages.ebay.com/help/policies/privacy-policy.html' target='_blank'>Privacy Policy</a> and <a href='http://pages.ebay.com/help/policies/user-agreement.html' target='_blank'>User Agreement</a> if you have any questions.<br><br>Copyright © 2014 eBay Inc. All Rights Reserved. Designated trademarks and brands are the property of their respective owners. eBay and the eBay logo are trademarks of eBay Inc. eBay Inc. is located at 2145 Hamilton Avenue, San Jose, CA 95125.  </font></td></tr></tbody></table><img src='http://rover.ebay.com/roveropen/0/e12011/7?euid=80af0a23f2c047c5b6fac4936c26c954&amp;' height='1' width='1'></div></div></div><br></div>";
            body = @"<!DOCTYPE html><!--a06598b3-bb0a-444e-e0f9-d3c8edd4635b_v400--><html><head>
<!-- </meta> --><style type='text / css'>



@media only screen and (max - width: 620px) {

                body[yahoo].device - width {

                width: 450px !important
                 
}

                body[yahoo].threeColumns {

                width: 140px !important
               
}

                body[yahoo].threeColumnsTd {

                padding: 10px 4px !important
               
}

                body[yahoo].fourColumns {

                width: 225px !important
               
}

                body[yahoo].fourColumnsLast {

                width: 225px !important
               
}

                body[yahoo].fourColumnsTd {

                padding: 10px 0px !important
               
}

                body[yahoo].fourColumnsPad {

                padding: 0 0 0 0 !important
               
}

                body[yahoo].secondary - product - image {

                width: 200px !important;

                height: 200px !important
                   
}

                body[yahoo].center {

                    text - align: center !important
               
}

                body[yahoo].twoColumnForty {

                width: 200px !important
               
    height: 200px !important
               
}

                body[yahoo].twoColumnForty img {

                width: 200px !important;

                height: 200px !important
               
}

                body[yahoo].twoColumnSixty {

                width: 228px !important
               
}

                body[yahoo].secondary - subhead - right {

                display: none !important
                   
}

                body[yahoo].secondary - subhead - left {

                width: 450px !important
                   
}

                body[yahoo].desktop - img {

                display: none !important;
                    max - height: 0px !important;
                overflow: hidden !important;

                }

                body[yahoo].mobile - img {

                display: block !important;
                    max - height: none !important;
                overflow: visible !important;

                }

            }

            @media only screen and (max - width: 479px) {

                body[yahoo].navigation {

                display: none !important
               
}

                body[yahoo].device - width {

                width: 300px !important;

                padding: 0
                 
}

                body[yahoo].narrow - list {
                width: 260px !important;
                padding: 0;
                }

                body[yahoo].threeColumns {

                width: 150px !important
               
}

                body[yahoo].fourColumns {

                width: 150px !important
               
}
                body[yahoo].fourColumnsLast {

                width: 150px !important
               
}
                body[yahoo].fourColumnsTd {

                padding: 10px 0px !important
               
}

                body[yahoo].fourColumnsPad {

                padding: 0 0 0 0 !important
               
}

                body[yahoo].secondary - product - image {

                width: 240px !important;

                height: 240px !important
                   
}

                body[yahoo].single - product - table {

                    float: none !important;
                    margin - bottom: 10px !important;
                    margin - right: auto !important;
                    margin - left: auto !important;

                }

                body[yahoo].single - product - pad {

                padding: 0 0 0 0 !important;

                }

                body[yahoo].single - product - image {
                align: center;
                width: 200px !important;
                height: 200px !important
                   
}

                body[yahoo].mobile - full - width {

                width: 300px !important
                   
}

                body[yahoo].twoColumnForty {
                align: center; !important
               
    width: 200px !important
               
}

                body[yahoo].twoColumnForty img {

                }

                body[yahoo].twoColumnSixty {
                    padding - left: 0px !important;
                width: 300px !important
               
}

                body[yahoo].adviceColumn {
                width: 210px !important
               
}

                body[yahoo].secondary - subhead - left {

                width: 300px !important
                   
}

                body[yahoo].ThreeColumnItemTable{

                padding: 0px 0px 0px 74px !important
               
}
                body[yahoo].FourColumnFloater
              {
                    float: right !important;
                }

                *[class=colSplit300] {
    width: 300px !important;
    display: block !important;
    width: 320px !important;
    height: auto !important;
    padding: 0 !important;
    float: left !important;
    text-align: center;
}

*[class=noMobile] {
 display: none !important;
}
*[class=turnOnMobile300] {
 display: block !important;
 width: 300px !important;
 height: auto !important;
 padding: 0;
 max-height: inherit !important;
 overflow: visible !important;
}
*[class=Resize320] {
 width: 320px !important;
 height: auto !important;
 padding: 0 !important;
}
*[class=Resize300] {
 width: 300px !important;
}
*[class=Resize298] {
 width: 298px !important;
}
*[class=Resize50] {
 width: 50px !important;
}
*[class=Resize40] {
 width: 40px !important;
}
*[class=font-size-to-24] {
 font-size: 24px !important;
 line-height: 28px !important;
}
*[class=font-size-to-13] {
 font-size: 13px !important;
 line-height:19px !important;
}

body[yahoo] .desktop-img {

      display: none !important;
      max-height: 0px !important;
      overflow: hidden !important;

}

body[yahoo] .mobile-img {

      display: block !important;
      max-height: none !important;
      overflow: visible !important;

}

body[yahoo] .no-border {
    
      border: none !important;
    
}

}


body[yahoo] .mobile-full-width {

	min-width: 103px;
    max-width: 300px;
	height: 38px;
}
body[yahoo] .mobile-full-width a
{

    display: block;
    padding: 10px 0;
}
body[yahoo] .mobile-full-width td
{

    padding: 0px !important
}
body[yahoo] .cta-link {
	padding: 10px 17px !important;
	display: block !important;
}
body[yahoo] .cta-cell {
	padding: 0px !important;

}

td.wrapText{
  white-space: pre-wrap; /* css-3 */
  white-space: -moz-pre-wrap; /* Mozilla, since 1999 */
  white-space: -pre-wrap; /* Opera 4-6 */
  white-space: -o-pre-wrap; /* Opera 7 */
  word-wrap: break-word; /* Internet Explorer 5.5+, 6, 7, 8 compability-mode */
  -ms-word-break: break-all; /* Internet Explorer 8 */
}

body { width: 100% !important; -webkit-text-size-adjust: 100% !important; -ms-text-size-adjust: 100% !important; -webkit-font-smoothing: antialiased !important; margin: 0 !important; padding: 0 0 100px !important; font-family: Helvetica, Arial, sans-serif !important; background-color:#f9f9f9}

.ReadMsgBody { width: 100% !important; background-color: #ffffff !important; }

.ExternalClass { width: 100% !important; }

.ExternalClass { line-height: 100% !important; }

img { display: block !important; outline: none !important; text-decoration: none !important; -ms-interpolation-mode: bicubic !important; }

td{word-wrap: break-word;}

.blueLinks a
{
    color: #0654ba !important;
    text-decoration: none !important;
}

.whiteLinks a
{
    color: #ffffff !important;
    text-decoration: none !important;
    font-weight: bold !important;
}

.spacedList li
{
    margin-left: 0;
    margin-top: 20px;
    margin-bottom: 20px;
}

.wrapper {
    width: 100%;
    table-layout: fixed;
    -webkit-text-size-adjust: 100%;
    -ms-text-size-adjust: 100%;
}
.webkit {
    max-width: 100%;
    margin: 0 auto;
}

</style>


<!--[if gte mso 9]>

	<style>td.product-details-block{word-break:break-all}.threeColumns{width:140px !important}.threeColumnsTd{padding:10px 20px !important}.fourColumns{width:158px !important}.fourColumnsPad{padding: 0 18px 0 0 !important}.fourColumnsTd{padding:10px 0px !important}.twoColumnSixty{width:360px !important}table{mso-table-lspace:0pt; mso-table-rspace:0pt;}</style>

	<![endif]-->
  </head>
  <body yahoo = 'fix' >





< table width='100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9'>
    <tr>
        <td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
            <table width = '600' class='device-width header-logo' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#f9f9f9' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
                <tr>
                    <td valign = 'top' style='border-collapse: collapse !important; border-spacing: 0 !important; padding: 0; border: none;'>
                        <p style = 'font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: normal; color: #888888; text-align: left; font-size: 10px; margin: 5px 0 10px 0; color: #333;' align='left'>
                        Payment received for your item.Now let's print a label and ship it out!

                      </p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table> <table id = 'area3Container' width= '100%' border= '0' cellpadding= '0' cellspacing= '0' align= 'center' style= 'border-collapse:collapse !important;border-spacing:0 !important;border:none;background-color:#f9f9f9;' >

    < tr >

        < td width= '100%' valign= 'top' style= 'border-collapse:collapse !important;border-spacing:0 !important;border:none;' >

            < table width= '100%' height= '7' border= '0' cellpadding= '0' cellspacing= '0' align= 'center' style= 'border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-image: url('http://p.ebaystatic.com/aw/navbar/preHeaderBottomShadow.png'); background-repeat: repeat-y no-repeat; margin: 0; padding: 0'>
				<!--[if gte mso 9]>
				<v:rect xmlns:v= 'urn:schemas-microsoft-com:vml' fill= 'true' stroke= 'false' style= 'mso-width-percent:1000;height:1px;' >

                    < v:fill type = 'tile' color= '#dddddd' />

                </ v:rect>
				<v:rect xmlns:v= 'urn:schemas-microsoft-com:vml' fill= 'true' stroke= 'false' style= 'mso-width-percent:1000;height:6px;' >

                    < v:fill type = 'tile' src= 'http://p.ebaystatic.com/aw/navbar/preHeaderBottomShadow.png' color= '#f9f9f9' />

                    < div style= 'width:0px; height:0px; overflow:hidden; display:none; visibility:hidden; mso-hide:all;' >

                        < ![endif]-- >

                        < tr >

                            < td width= '100%' height= '1' valign= 'top' align= 'center' style= 'border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color: #dddddd; font-size: 1px; line-height: 1px;' >

                                < !--[if gte mso 15] > &nbsp;<![endif]-->
							</td>
						</tr>
						<tr>
							<td width = '100%' height='6' valign='top' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color: none; font-size: 1px; line-height: 1px;'>&nbsp;</td>
						</tr>
						<!--[if gte mso 9]>
					</div>
				</v:rect>
				<![endif]-->
			</table>
		</td>
	</tr>
</table> <table id = 'area4Container' width='100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9'>
	<tr>
		<td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
			<table width = '600' class='device-width header-logo' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
				<tr>
					<td valign = 'top' style='border-collapse: collapse !important; border-spacing: 0 !important; padding: 15px 0 20px; border: none;'><a href = 'http://rover.ebay.com/rover/0/e12011.m1831.l3127/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fwww.ebay.com%2Fulk%2Fstart%2Fshop&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;'><img src = 'http://p.ebaystatic.com/aw/email/eBayLogo.png' width='133' border='0' alt='eBay' align='left' style='display: inline block; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; border: none;'></a><img src = 'http://rover.ebay.com/roveropen/0/e12011/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;sojTags=bu=bu' alt='' style='border:0; height:1;'></td>
				</tr>
			</table>
		</td>
	</tr>
</table> 
<table width = '100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9'>
<tr>
<td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
<table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#f9f9f9' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
<tr>
<td valign = 'top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
<h1 style = 'font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: 26px; color: #333333; text-align: left; font-size:24px; margin: 0 0 14px;' align='left'>
    Congratulations, your item sold and it's been paid for. 
</h1>
</td>
</tr>
</table>
</td>
</tr>
</table> 














    






<table width = '100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9'><tr><td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
<table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#f9f9f9' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
  <tr>
    <td valign = 'top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
    
<h3 style = 'font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: 16px; color: #666666; text-align: left; font-size: 14px; margin: 0 0 10px;' align='left'> 
        Hi
            Zhijun,
</h3>        
<h3 style = 'font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: 16px; color: #666666; text-align: left; font-size: 14px; margin: 0 0 10px;' align='left'>         
        It's time to pack up your item and ship it out. Make sure you send it within 1 day, the handling option you selected in your listing.
        <br>
       
    </h3>
    </td>
  </tr>
</table>
</td>
</tr>
</table> 




<table width = '100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9'>
    <tr>
        <td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
            <table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#f9f9f9' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>

                <tr>
                    <td valign = 'top' class='cta-block' style='border-collapse: collapse !important; border-spacing: 0 !important; padding: 15px 0 10px; border: none;'>
                
                        <table align = 'left' cellpadding='0' cellspacing='0' border='0' class='mobile-full-width' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
                            <tr>
                                <td valign = 'top' class='center cta-button primary-cta-button' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; font-size: 14px; line-height: normal; font-weight: bold; box-shadow: 2px 3px 0 #e5e5e5; filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#0079bc', endColorstr='#00519e',GradientType=0 ); background-image: linear-gradient(to bottom,  #0079bc 0%,#00519e 100%); background-color: #0079bc; padding: 10px 17px; border: 1px solid #00519e;' bgcolor='#0079bc'>
                                    <a style = 'text-decoration: none; color: #ffffff; font-size: 14px; line-height: normal; font-weight: bold; font-family: Helvetica, Arial, sans-serif; text-shadow: 1px 1px 0 #00519e;' href='http://rover.ebay.com/rover/0/e12011.m44.l1337/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostage%26transactionid%3D1463238214005%26itemid%3D152262952392&amp;sojTags=bu=bu'>
                                        <span style = 'padding: 0px 10px' > Print shipping label</span>
                                        </a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9'>
    	        <tr>
    	            <td valign = 'top' class='cta-block' style='border-collapse: collapse !important; border-spacing: 0 !important; padding: 0px 0 0px; border: none;'>
    	                <table align = 'left' cellpadding='0' cellspacing='0' border='0' class='mobile-full-width' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
    	                    <tr>
    	                        <td valign = 'top' align='left' style='border-collapse: collapse !important; border-spacing: 0 !important; font-size: 12px; line-height: 12px;  padding: 0px 0px; color: #666666;'>
    	                        Already Shipped?
                                </td>
    	                    </tr>
    	                    <tr>
    	                        <td valign = 'top' align='left' style='border-collapse: collapse !important; border-spacing: 0 !important; font-size: 14px; line-height: 14px;  padding: 0px 0px; '>
                                <a href = 'http://rover.ebay.com/rover/0/e12011.m44.l6573/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FAddTrackingNumber2%26LineID%3DTransactions.152262952392_1463238214005&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;padding: 0px 0px;'>Upload tracking information &gt;</a> 
                                </td>
    	                    </tr>
    	                </table>
    	            </td>
    	        </tr>
            </table>
        </td>
    </tr>
</table> 











    


<table width = '100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#f9f9f9; margin-bottom: 25px'><tr><td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; '>
<table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#f9f9f9' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; padding-bottom: 20px;'>

  <tr>
    <td valign = 'top' class='single-product-block' style='border-collapse: collapse !important; border-spacing: 0 !important; padding-top: 25px; border: none;'>

<table class='single-product-table' width='200' align='left' border='0' cellspacing='0' cellpadding='0' style='float: left; border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
  <tr>
    <td valign = 'middle' width='200' height='200' align='center' class='single-product-image' style='border-collapse: collapse !important; border-spacing: 0 !important; border: 1px solid #dddddd; background-color: #ffffff;'><a href = 'http://rover.ebay.com/rover/0/e12011.m43.l1123/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fwww.ebay.com%2Fulk%2Fitm%2F152262952392&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;'><img src = 'http://i.ebayimg.com/images/g/kgEAAOSwzaJX8G8o/s-b200x200.jpg' alt='Kirkland Signature Men's Crew Neck Tee 6-pack White' class='product-image' border='0' style='display: block; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; margin: 0; border: none; padding: 0;'></a>
    </td>
    <td class='single-product-pad' style='padding: 0 20px 0 0; border-collapse: collapse !important; border-spacing: 0 !important; border: none;'></td>
  </tr>
</table>

<table width = '376' align='left' border='0' cellspacing='0' cellpadding='0' class='twoColumnSixty' style='table-layout: fixed; border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
        <tr>
          <td valign = 'top' style=' border-collapse: collapse !important; border-spacing: 0 !important; border: none; word-wrap: break-word;-ms-word-break: break-all;'>
            <h2 class='product-name' style='font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: 20px; color: #333333; text-align: left; font-size: 16px; margin: 0 0 10px;' align='left'>
              <a href = 'http://rover.ebay.com/rover/0/e12011.m43.l1123/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fwww.ebay.com%2Fulk%2Fitm%2F152262952392&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;'>Kirkland Signature Men's Crew Neck Tee 6-pack White</a>
            </h2>
          </td>
        </tr>
        <tr>

          <td class='product-price' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 18px; line-height: 16px; font-weight: bold; padding-bottom: 10px; border: none;' align='left'>Paid: $10.00
            <br></td>
        </tr>
        <tr>
          <td class='product-bids' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 14px; color: #666666; border: none;' align='left'>Item #: 152262952392
          </td>
        </tr>
        <tr>
          <td class='product-bids' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 14px; color: #666666; border: none;' align='left'>Size: Medium
          </td>
        </tr>

        <tr>
          <td class='product-bids' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 14px; color: #666666; border: none;' align='left'>Date Sold: 10/02/2016
          </td>
        </tr>
    
        <tr>
          <td class='product-bids' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 14px; color: #666666; border: none;' align='left'>Quantity Sold: 1
          </td>
        </tr>

        <tr>
          <td class='product-bids' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 14px; color: #666666; border: none;' align='left'>Quantity Remaining:  5
          </td>
        </tr>
        <tr><td class='product-bids blueLinks' style='border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 14px; color: #666666; border: none;  text-decoration:none !important;' align='left'>
            <span>
                Buyer:&nbsp;<a href = 'http://rover.ebay.com/rover/0/e12011.m43.l7213/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fwww.ebay.com%2Fulk%2Fusr%2Fcoffeebean217&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;'>coffeebean217</a>&nbsp;&nbsp;<a href = 'http://rover.ebay.com/rover/0/e12011.m43.l1441/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fcontact.ebay.com%2Fws%2FeBayISAPI.dll%3FReturnUserEmail%26requested%3Dcoffeebean217%26redirect%3D0%26iid%3D152262952392&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;'>Contact Buyer</a>
               </span>
          </td></tr>
      </table>
      </td>
     </tr>

</table>
<br>
</td>
</tr>
</table> 























  <table width = '100%' border= '0' cellpadding= '0' cellspacing= '0' align= 'center' style= 'border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color:#ffffff' >< tr >< td width= '100%' valign= 'top' style= 'border-collapse: collapse !important; border-spacing: 0 !important; border: none;' >
    < table width= '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#ffffff' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;margin-bottom:40px;margin-top:10px;'>
  <tr>
    <td valign = 'top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>

        <table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#ffffff' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;padding: 0px 0 0px;'>
            <tr>
                <td valign = 'top' class='secondary-headline' style='border-collapse: collapse !important; border-spacing: 0 !important; border-bottom-color: #dddddd; border-bottom-width: 1px; padding: 10px 0 5px;'>
                    <h1 style = 'font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: 26px; color: #333333; text-align: left; font-size: 24px; margin: 0 0 0;' align='left'>Save time and money</h1>
                  </td>
            </tr>
            <tr>
                <td valign = 'top' style= 'border-collapse: collapse !important; border-spacing: 0 !important; border-bottom-color: #dddddd; border-bottom-width: 1px; padding: 5px 0 2px;' >
  
                      < div style= 'border-collapse: collapse !important; border-spacing: 0 !important; font-family: Helvetica, Arial, sans-serif; text-align: left; font-size: 12px;line-height: 13px; color: #666666; border: none;' >
              Print your<a href='http://rover.ebay.com/rover/0/e12011.m354.l1337/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpayments.ebay.com%2Fws%2FeBayISAPI.dll%3FPrintPostage%26transactionid%3D1463238214005%26itemid%3D152262952392&amp;sojTags=bu=bu' style='text-decoration: none; color: #0654ba;padding: 0px 0px;'> eBay Shipping Label</a>.  eBay labels save you up to 25%* on postage and provide free tracking to your buyer.You can also request free mailing supplies for future eBay sales. <a href = 'http://rover.ebay.com/rover/0/e12011.m354.l7959/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpages.ebay.com%2Fsellerinformation%2Fship-smart%2Fshipping-basics%2Febay-labels.html&amp;sojTags=bu=bu' style= 'text-decoration: none; color: #0654ba;padding: 0px 0px;' > Learn more</a>
                      </div>
                </td>
            </tr>
        </table>
        
    </td>
  </tr>
</table>
</td>
</tr>
</table>
     
    
        
<table id = 'area9Container' width= '100%' border= '0' cellpadding= '0' cellspacing= '0' align= 'center' style= 'border-collapse: collapse !important; border-spacing: 0 !important; border: none;background-color:#ffffff' bgcolor= '#ffffff' class='whiteSection dynamic-component'>
          <tr>
            <td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
    <table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#ffffff' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
      <tr>
        <td valign = 'top' class='static-creative-block' style='border-collapse: collapse !important; border-spacing: 0 !important; padding: 20px 0px 20px; border: none;'><a href = 'http://srx.main.ebayrtm.com/clk?RtmClk&amp;lid=2055054&amp;m=559110&amp;pg=0&amp;aii=8671468569960029791&amp;u=1H4sIAAAAAAAAAFMOLs1T8E8uUTAwUjAwtTI1sjKyVPANDlEwMjA04%2BXKzEyxtTAzNzQxszA1s7Q0MzAwsjS3NOTlAgDVpNjfOAAAAA%3D%3D&amp;i=1467794087&amp;g=null&amp;uf=1' alt='Visit our Seller Center for inspiration, advice, and more.' target='_top'>
<img width = '100%' border='0' src='http://rtm.ebaystatic.com/0/RTMS/Image/GR_64822Trg2Pl_V4_580x200.jpg' class='device-width' alt='Visit our Seller Center for inspiration, advice, and more.'>
</a><img src = 'http://rtm.ebay.com/rtm?RtmCmd&amp;a=img&amp;i=1467794087&amp;td=1H4sIAAAAAAAAAFMOLs1T8E8uUTAwUjAwtTI1sjKyVPANDlEwMjA04%2BUqyMktsY3WSM1MibEyBAoZKuQWg9h1CpkgysLM3NDEzMLUzNLSzMDAyNLcEqgAJGFqamloaKCQDFVcANZvCQQKBRAhzVheLgCbqaQvfAAAAA%3D%3D&amp;c=1H4sIAAAAAAAAAEVRsU5EQQjsTfyHl9gbYIFdLqHwA4zFaWfzfOclNsbC%2B39h8e5VC8MwzMLD8fK9vGy%2FC9ACchA6kC3Px9eFAPX%2B7odMHFl7N4bRA8CmjqIVm0VMpMGiZhSQBAKPkNHwCsZwtJGIeuaNAgh5TEj87fj0fgGATEcxhOczYnRm7SpUsFBVW2WYpO3r5LTLkIf%2BrIIPnmx0zgemJNvsZQRfSdt5k8%2F142y8ncCarRT2Nj3jmXhN2qimPpu6VabepkaY3Cf3XkV2yyw4QpyDu1z3wmF%2F5%2FMV7u1GgNuCe321Yy0udysp3MIRsiSot4XrcJkBhsLkt7Kq3fOUWstVcRJhJB2QV7GyrOGE48wakzXcJdQSktZ5h%2Bh%2F2h%2Bj318iOAIAAA%3D%3D&amp;uf=1&amp;ord=1475412749193' width='1' height='1' border='0' alt=''></td>
      </tr>
    </table>
    </td>
    </tr>
    </table>
 <table id = 'area10Container' class='whiteSection' width='100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color: #ffffff'>
    <tr>
      <td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>      
       <table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
       <tr>
        <td valign = 'top' class='email-preferences-block' style='border-collapse: collapse !important; border-spacing: 0 !important; border-bottom-width: 1px; border-bottom-color: #dddddd; padding: 40px 0 30px; border-style: none none solid;'>

   <p style = 'font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: normal; color: #888888; text-align: left; font-size: 11px; margin: 0 0 10px;' align='left'><strong>Update your email preferences</strong><br>

             You are receiving this email based on your eBay account preferences.To change which emails you receive from eBay, go to <a style = 'text-decoration: none; color: #555555;' href= 'http://rover.ebay.com/rover/0/e12011.m2901.l1141/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fmy.ebay.com%2Fws%2FeBayISAPI.dll%3FMyEbayBeta%26CurrentPage%3DMyeBayNextNotificationPreferences&amp;sojTags=bu=bu' target= '_blank' > Communication Preferences</a> in My eBay.
        </p>

        </td>
        </tr></table>
      </td>
    </tr>
  </table> <table id = 'area11Container' class='whiteSection' width='100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none; background-color: #ffffff'>
<tr>
<td width = '100%' valign='top' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
<table width = '600' class='device-width' border='0' cellpadding='0' cellspacing='0' align='center' style='border-collapse: collapse !important; border-spacing: 0 !important; border: none;'>
<tr>
<td class='ebay-footer-block' style='border-collapse: collapse !important; border-spacing: 0 !important; padding: 20px 0 60px; border: none;'>
<div id = 'ReferenceId' >
< p style='font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: normal; color: #888888; text-align: left; font-size: 11px; margin: 0 0 10px;' align='left'><strong>
Email reference id: [#a263fc5eabf94cd0939a2120c6f1f24a#]
</strong></p></div>
<p style='font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: normal; color: #888888; text-align: left; font-size: 11px; margin: 0 0 10px;' align='left'>
We don't check this mailbox, so please don't reply to this message.If you have a question, go to<a style='text-decoration: none; color: #555555;' href='http://rover.ebay.com/rover/0/e12011.m1852.l6369/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Focsnext.ebay.com%2Focs%2Fhome&amp;sojTags=bu=bu' target='_blank'>Help &amp; Contact</a>.
</p>
<p style='font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: normal; color: #888888; text-align: left; font-size: 11px; margin: 0 0 10px;' align='left'>
eBay sent this message to Zhijun Ding(zjding2016). Learn more about<a style='text-decoration: none; color: #555555;' href='http://rover.ebay.com/rover/0/e12011.m1852.l3167/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpages.ebay.com%2Fhelp%2Faccount%2Fprotecting-account.html&amp;sojTags=bu=bu' target='_blank'>account protection</a>.eBay is committed to your privacy.Learn more about our<a style='text-decoration: none; color: #555555;' href='http://rover.ebay.com/rover/0/e12011.m1852.l3168/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpages.ebay.com%2Fhelp%2Fpolicies%2Fprivacy-policy.html&amp;sojTags=bu=bu' target='_blank'>privacy notice</a> and<a style='text-decoration: none; color: #555555;' href='http://rover.ebay.com/rover/0/e12011.m1852.l3165/7?euid=a263fc5eabf94cd0939a2120c6f1f24a&amp;bu=44447377354&amp;loc=http%3A%2F%2Fpages.ebay.com%2Fhelp%2Fpolicies%2Fuser-agreement.html&amp;sojTags=bu=bu' target='_blank'>user agreement</a>.
</p>
<p style='font-family: Helvetica, Arial, sans-serif; font-weight: normal; line-height: normal; color: #888888; text-align: left; font-size: 11px; margin: 0 0 10px;' align='left'>
©2016 eBay Inc., 2145 Hamilton Avenue, San Jose, CA 95125
</p>
</td>
</tr>
</table>
</td>
</tr>
</table></body>
</html>
";

            File.WriteAllText(@"C:\temp\temp.html", body);

            body = body.Replace("\n", "");
            body = body.Replace("\r", "");
            body = body.Replace("\t", "");
            body = body.Replace("\\", "");
            body = body.Replace("\"", "'");

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
            string body = @"<div dir='ltr'><div class='gmail_quote'><div class='HOEnZb'><div class='h5'><div dir='ltr'><div class='gmail_quote'><br>
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





                    <td style='padding:2px 0px 2px 0px;border-top:solid 1px #969696;border-bottom:solid 1px #969696'><b>1. Order Received</b>&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:#969696'>2. Sent to Fulfillment</span>&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:#969696'>3. Shipped</span></td>
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
          596503302</td>
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
          07/03/2016</td>
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
                nanette a crawley</td>
              </tr>
              <tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>1040 buena Rd</td>
              </tr>
              
              <tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>Lake forest, IL &nbsp; 60045</td>
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
<div align='center'><font face='Arial'>1</font></div>
</td>
<td bgcolor='#f0f0f0' width='307' valign='middle'>
<div align='center'>
<font size='2' face='Arial'>

Every Man Jack 2-in-1 Shampoo &#43; Conditioner and Body Wash Combos - Item# 1068097</font></div>
</td>

<td bgcolor='#f0f0f0' width='125' valign='middle'>
<div align='center'>


<font size='2' face='Arial'>Standard Ground</font>

</div>
</td>

<td bgcolor='#f0f0f0' width='104' valign='middle'><div align='center'><font size='2' face='Arial'>$23.99</font></div></td>
<td bgcolor='#f0f0f0' width='125' valign='middle'><div align='center'><font size='2' face='Arial'>$95.96</font></div></td>
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

Standard shipping is via UPS Ground. <strong>The estimated delivery time will be approximately 3 - 5 business days from the time of order.</strong> 
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
                
                Click to see <a href='http://www.costco.com/CatalogSearch?langId=-1&amp;storeId=10301&amp;catalogId=10701&amp;keyword=WhatsNewAZ&amp;sortBy=EnglishAverageRating%7C1&amp;EMID=Transactional_OrderReceived_WhatsNew' target='_blank'><strong>What's New</strong></a> at Costco.com. <br></td>
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
          <font size='2' face='Arial'>$95.96</font>
        </div>
      </td>
    </tr>
    
    <tr>
      <td bgcolor='#cee7ff'>
        <div align='right'>
          <font size='2' face='Arial'>
            <b>Less Promo Code:</b>
          </font>
        </div>
      </td>
      <td bgcolor='#ffffcc'>
        <div align='right'>
          <font size='2' face='Arial'>-$20.00</font>
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
          <font size='2' face='Arial'>$5.76</font>
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
            <b>$81.72</b>
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
          <b>Merchandise:</b> We guarantee your satisfaction on every product we sell with a full refund. The following must be returned within 90 days of purchase for a refund: televisions, projectors, computers, cameras, camcorders, touchscreen tablets, MP3 players and cellular phones.<br>
          <br>
          <b>How to Return:</b> Simply return your purchase at any one of our Costco warehouses worldwide for a refund (including shipping and handling). If you are unable to return your order at one of our warehouses, please contact <a href='https://customerservice.costco.com' target='_blank'>customer service</a> or call our customer service center at <a href='tel:1-800-955-2292' value='&#43;18009552292' target='_blank'>1-800-955-2292</a> for assistance. To expedite the processing of your return, please reference your order number.</font><br>
          <br>
          <p><font face='Arial' style='font-size:9pt'><strong>Costco.com</strong> <a href='https://customerservice.costco.com' target='_blank'>customer service</a> or call <strong><a href='tel:1-800-955-2292' value='&#43;18009552292' target='_blank'>1-800-955-2292</a></strong></font></p>
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
</div></div></div><br></div>
";

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
            string body = @"<div dir='ltr'><div class='gmail_quote'><div lang='EN-US' link='blue' vlink='purple'><div><br>
<div>
<div>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td width='100%' style='width:100.0%;padding:0in 0in 0in 0in'>
<table border='0' cellspacing='3' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td width='1%' nowrap='' style='width:1.0%;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
<p class='MsoNormal'><img width='95' height='39' style='width:.9895in;min-height:.4062in' src='http://q.ebaystatic.com/aw/pics/logos/ebay_95x39.gif' alt='eBay'><u></u><u></u></p>
</td>
<td valign='bottom' style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>
<p class='MsoNormal'><b><span style='font-size:7.5pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>eBay sent this message to Zhijun Ding (zjding2016).<br>
</span></b><span style='font-size:7.5pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>Your registered name is included to show this message originated from eBay.
<a href='http://pages.ebay.com/help/confidence/name-userid-emails.html' target='_blank'>Learn more</a>.</span><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</div>
</div>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;background:#ffe680'>
<tbody>
<tr>
<td width='8' valign='top' style='width:6.0pt;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><img border='0' width='8' height='8' style='width:.0833in;min-height:.0833in' src='http://q.ebaystatic.com/aw/pics/globalAssets/ltCurve.gif'><u></u><u></u></p>
</td>
<td width='100%' valign='bottom' style='width:100.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><b><span style='font-size:14.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Your item has been listed. Sell another item now!</span></b><u></u><u></u></p>
</td>
<td width='8' valign='top' style='width:6.0pt;padding:0in 0in 0in 0in'>
<p class='MsoNormal' align='right' style='text-align:right'><img border='0' width='8' height='8' style='width:.0833in;min-height:.0833in' src='http://p.ebaystatic.com/aw/pics/globalAssets/rtCurve.gif'><u></u><u></u></p>
</td>
</tr>
<tr style='height:3.0pt'>
<td colspan='3' style='background:#ffcc00;padding:0in 0in 0in 0in;height:3.0pt'></td>
</tr>
</tbody>
</table>
<div>
<div>
<p class='MsoNormal'><span style='display:none'><u></u>&nbsp;<u></u></span></p>
<table border='0' cellspacing='3' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Hi zjding2016,<u></u><u></u></span></p>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><img border='0' height='10' style='min-height:.1041in' src='http://q.ebaystatic.com/aw/pics/s.gif' alt=' '><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Your item has been successfully listed on eBay. It may take some time for the item to appear on eBay search results. Here are the listing details:<u></u><u></u></span></p>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><img border='0' height='10' style='min-height:.1041in' src='http://q.ebaystatic.com/aw/pics/s.gif' alt=' '><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
<div>
<p class='MsoNormal'><span style='display:none'><u></u>&nbsp;<u></u></span></p>
<table border='0' cellspacing='3' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td width='100' nowrap='' valign='top' style='width:75.0pt;padding:0in 0in 0in 0in'>
<p class='MsoNormal' align='center' style='text-align:center'><a href='http://rover.ebay.com/rover/0/e12000.m43.l1123/7?euid=b9e583070de44bf39542d75adb833dd3&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D152241585807%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1123' target='_blank'><span style='text-decoration:none'><img border='0' src='http://pics.ebaystatic.com/aw/pics/icon/iconPic_20x20.gif' alt='Adidas Men's Ultimate Core Short'></span></a><u></u><u></u></p>
</td>
<td valign='top' style='padding:0in 0in 0in 0in'>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td colspan='2' style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'><a href='http://rover.ebay.com/rover/0/e12000.m43.l1123/7?euid=b9e583070de44bf39542d75adb833dd3&amp;loc=http%3A%2F%2Fcgi.ebay.com%2Fws%2FeBayISAPI.dll%3FViewItem%26item%3D152241585807%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1123' target='_blank'>Adidas
 Men's Ultimate Core Short</a><u></u><u></u></span></p>
</td>
</tr>
<tr>
<td width='15%' nowrap='' valign='top' style='width:15.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Item Id:<u></u><u></u></span></p>
</td>
<td valign='top' style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>152241585807<u></u><u></u></span></p>
</td>
</tr>
<tr>
<td width='15%' nowrap='' valign='top' style='width:15.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Price:<u></u><u></u></span></p>
</td>
<td valign='top' style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>$30.00<u></u><u></u></span></p>
</td>
</tr>
<tr>
<td width='15%' nowrap='' valign='top' style='width:15.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>End time:<u></u><u></u></span></p>
</td>
<td valign='top' style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Oct-14-16 07:25:20 PDT<u></u><u></u></span></p>
</td>
</tr>
<tr>
<td width='15%' nowrap='' valign='top' style='width:15.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Listing fees:<u></u><u></u></span></p>
</td>
<td valign='top' style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>0<u></u><u></u></span></p>
</td>
</tr>
<tr>
<td colspan='2' style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'><a href='http://rover.ebay.com/rover/0/e12000.m43.l1125/7?euid=b9e583070de44bf39542d75adb833dd3&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws2%2FeBayISAPI.dll%3FUserItemVerification%26%26item%3D152241585807%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1125' target='_blank'>Revise
 item</a> | <a href='http://rover.ebay.com/rover/0/e12000.m43.l1121/7?euid=b9e583070de44bf39542d75adb833dd3&amp;loc=http%3A%2F%2Fmy.ebay.com%2Fws%2FeBayISAPI.dll%3FMyeBay%26%26CurrentPage%3DMyeBaySelling%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1121' target='_blank'>
Go to My eBay</a></span><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</div>
</td>
<td width='185' valign='top' style='width:138.75pt;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
<div>
<p class='MsoNormal'><b><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Ready to List Your Next Item?</span></b><u></u><u></u></p>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td style='padding:0in 0in 0in 0in'>
<p class='MsoNormal'><img border='0' height='4' style='min-height:.0416in' src='http://q.ebaystatic.com/aw/pics/s.gif' alt=' '><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
<p class='MsoNormal'><a href='http://rover.ebay.com/rover/0/e12000.m44.l1127/7?euid=b9e583070de44bf39542d75adb833dd3&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws%2FeBayISAPI.dll%3FSellHub3%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1127' title='http://rover.ebay.com/rover/0/e12000.m44.l1127/7?euid=b9e583070de44bf39542d75adb833dd3&amp;loc=http%3A%2F%2Fcgi5.ebay.com%2Fws%2FeBayISAPI.dll%3FSellHub3%26ssPageName%3DADME%3AL%3ALCA%3AUS%3A1127' target='_blank'><span style='text-decoration:none'><img border='0' width='120' height='32' style='width:1.25in;min-height:.3333in' src='http://p.ebaystatic.com/aw/pics/buttons/btnSellMore.gif'></span></a><br>
<i><span style='font-size:8.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Click to list another item</span></i><u></u><u></u></p>
</div>
</td>
</tr>
</tbody>
</table>
<p class='MsoNormal'><u></u>&nbsp;<u></u></p>
</div>
</div>
<div>
<div>
<div style='border:solid #dedfde 1.0pt;padding:0in 0in 0in 0in;margin-bottom:15.0pt'>
<h3 style='margin:0in;margin-bottom:.0001pt;background:#e7e7e7'><span style='font-size:10.5pt;font-family:&quot;Arial&quot;,sans-serif'>Select your email preferences<u></u><u></u></span></h3>
<div>
<p class='MsoNormal' style='margin-left:7.5pt;line-height:12.0pt'>
<u></u><span style='font-size:10.0pt;font-family:Wingdings'><span>§<span style='font:7.0pt &quot;Times New Roman&quot;'>&nbsp;
</span></span></span><u></u><span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif'>Want to reduce your inbox email volume?
<a href='http://my.ebay.com/ws/eBayISAPI.dll?DigestEmail&amp;emailType=12000' target='_blank'>Receive this email as a daily digest</a>.</span></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333'><br>
<span>For other email digest options, go to <a href='http://my.ebay.com/ws/eBayISAPI.dll?MyEbayBeta&amp;CurrentPage=MyeBayNextNotificationPreferences' target='_blank'>
Notification Preferences</a> in My eBay.</span></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif'><u></u><u></u></span></p>
<p class='MsoNormal' style='margin-left:7.5pt;line-height:12.0pt'>
<u></u><span style='font-size:10.0pt;font-family:Wingdings'><span>§<span style='font:7.0pt &quot;Times New Roman&quot;'>&nbsp;
</span></span></span><u></u><span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif'>Don't want to receive this email?
<a href='http://my.ebay.com/ws/eBayISAPI.dll?EmailUnsubscribe&amp;emailType=12000' target='_blank'>Unsubscribe from this email</a>.</span></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif'><u></u><u></u></span></p>
</div>
</div>
</div>
</div>
<div>
<div>
<table border='1' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border:solid #6b7b91 1.0pt'>
<tbody>
<tr style='height:.75pt'>
<td style='border:none;background:#c9d2dc;padding:0in 0in 0in 0in;height:.75pt'>
<p class='MsoNormal'><img border='0' width='25' height='25' style='width:.2604in;min-height:.2604in' src='http://p.ebaystatic.com/aw/pics/securityCenter/imgShield_25x25.gif' alt='Marketplace Safety Tip'><u></u><u></u></p>
</td>
<td width='20%' nowrap='' style='width:20.0%;border:none;background:#c9d2dc;padding:0in 0in 0in 0in;height:.75pt'>
<p class='MsoNormal'><b><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Marketplace Safety Tip<u></u><u></u></span></b></p>
</td>
<td style='border:none;background:#c9d2dc;padding:0in 0in 0in 0in;height:.75pt'>
<p class='MsoNormal'><img border='0' width='25' height='25' style='width:.2604in;min-height:.2604in' src='http://p.ebaystatic.com/aw/pics/securityCenter/imgTabCorner_25x25.gif'><u></u><u></u></p>
</td>
<td width='80%' style='width:80.0%;border:none;background:#c9d2dc;padding:0in 0in 0in 0in;height:.75pt'>
</td>
</tr>
<tr>
<td colspan='4' style='border:none;padding:0in 0in 0in 0in'>
<ul type='square'>
<li class='MsoNormal' style='color:black;line-height:120%'>
<span style='font-size:10.0pt;line-height:120%;font-family:&quot;Arial&quot;,sans-serif'>If you are contacted about buying a similar item outside of eBay, please do not respond. Outside-of-eBay transactions are against eBay policy, and they are not covered by eBay services
 such as feedback and eBay purchase protection programs.<u></u><u></u></span></li></ul>
</td>
</tr>
<tr>
<td colspan='4' style='border:none;background:#c9d2dc;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><img border='0' width='1' height='1' style='width:.0104in;min-height:.0104in' src='http://q.ebaystatic.com/aw/pics/s.gif'><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
<p class='MsoNormal'><u></u>&nbsp;<u></u></p>
</div>
</div>
<div>
<div>
<div class='MsoNormal' align='center' style='text-align:center'>
<hr size='1' width='100%' align='center'>
</div>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td width='100%' style='width:100.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:8.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black'>Email reference id: [#<wbr>b9e583070de44bf39542d75adb833d<wbr>d3#]</span><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
<p class='MsoNormal'><u></u>&nbsp;<u></u></p>
</div>
<div class='MsoNormal' align='center' style='text-align:center'>
<hr size='1' width='100%' align='center'>
</div>
<table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'>
<tbody>
<tr>
<td width='100%' style='width:100.0%;padding:0in 0in 0in 0in'>
<p class='MsoNormal'><span style='font-size:7.5pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><a href='http://pages.ebay.com/education/spooftutorial/index.html' target='_blank'>Learn More</a> to protect yourself from spoof (fake) emails.<br>
<br>
eBay sent this email to you at <a href='mailto:zjding@outlook.com' target='_blank'>zjding@outlook.com</a> about your account registered on
<a href='http://www.ebay.com' target='_blank'>www.ebay.com</a>.<br>
<br>
eBay sends these emails based on the preferences you set for your account. To unsubscribe from this email, change your
<a href='http://my.ebay.com/ws/eBayISAPI.dll?MyEbayBeta&amp;CurrentPage=MyeBayNextNotificationPreferences' target='_blank'>
communication preferences</a>. Please note that it may take up to 10 days to process your request. Visit our
<a href='http://pages.ebay.com/help/policies/privacy-policy.html' target='_blank'>Privacy Notice</a> and
<a href='http://pages.ebay.com/help/policies/user-agreement.html' target='_blank'>User Agreement</a> if you have any questions.<br>
<br>
Copyright © 2016 eBay Inc. All Rights Reserved. Designated trademarks and brands are the property of their respective owners. eBay and the eBay logo are trademarks of eBay Inc. eBay Inc. is located at 2145 Hamilton Avenue, San Jose, CA 95125.
</span><u></u><u></u></p>
</td>
</tr>
</tbody>
</table>
<p class='MsoNormal'><img border='0' width='1' height='1' style='width:.0104in;min-height:.0104in' src='http://rover.ebay.com/roveropen/0/e12000/7?euid=b9e583070de44bf39542d75adb833dd3&amp;'><u></u><u></u></p>
</div>
</div>
</div>

</div><br></div>";

            string subject = @"Your eBay listing is confirmed: Adidas Men's Ultimate Core Short";

            ProcessListingConfirmEmail(body, subject);
        }

        private void ProcessListingConfirmEmail(string body, string subject)
        {
            body = body.Replace("\n", "");
            body = body.Replace("\t", "");
            //body = body.Replace("\\", "");
            body = body.Replace("\"", "'");

            string productName = subject.Substring(32, subject.Length - 32);

            string sqlString = "SELECT * FROM eBay_ToAdd WHERE name = '" + productName.Replace("'", "''") + "'";

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

            string eBayUrl = SubstringEndBack(body, "' target='_blank'>" + productName, "<a href='", false, false);

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

        private void btnCostcoShipEmail_Click(object sender, EventArgs e)
        {
            string body = @"<div dir='ltr'><div class='gmail_extra'><div class='gmail_quote'><blockquote class='gmail_quote' style='margin:0 0 0 .8ex;border-left:1px #ccc solid;padding-left:1ex'><div dir='ltr'><div class='gmail_extra'><div class='gmail_quote'><br><blockquote class='gmail_quote' style='margin:0 0 0 .8ex;border-left:1px #ccc solid;padding-left:1ex'>
<table border='0' cellpadding='0' cellspacing='0' width='650'>
<tbody><tr>
  <td>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><table border='0' cellspacing='0' cellpadding='0' width='650'>
            <tbody><tr>
              <td><a href='//www.costco.com/?EMID=Transactional_OrderShipped_TopNav_Logo' target='_blank'><img border='0'></a> </td>
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
                    <td style='font-size:19pt'><b>Shipped</b></td>
                  </tr>
                </tbody></table></td>
              <td style='width:149px'></td>
              <td><table border='0' cellspacing='0' cellpadding='0' width='100%'>
                  <tbody><tr>
                    <td style='padding:0px 0px 3px 0px;text-align:left;font-size:14px;color:#1c6699;font-weight:bold;vertical-align:bottom'>STEPS FOR YOUR ORDER:</td>
                  </tr>
                  <tr>
                    <td style='padding:2px 0px 2px 0px;border-top:solid 1px #969696;border-bottom:solid 1px #969696'><span style='color:#969696'>1. Order Received</span>&nbsp;&nbsp;&nbsp;&nbsp;<b>2. Shipped</b>&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:#969696'>3. Delivered</span></td>
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
          596503302</td>
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
          06/23/2016</td>
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
                nanette a crawley</td>
              </tr>
              <tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>1040 buena Rd</td>
              </tr>
              
              <tr style='font-family:Arial,Verdana,Helvetica,sans-serif;background-color:#f0f0f0;color:Black;font-weight:bold;font-size:12px'>

                <td>Lake forest, IL &nbsp; 60045</td>
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
        <td>Thank you for your order from <b><a href='//www.costco.com/?EMID=Transactional_OrderShipped_Main_Intro' target='_blank'>Costco.com</a></b>. The following item(s) have shipped.</td>
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
<tbody><tr valign='top'>
<td width='768'>
<table cellspacing='0' cellpadding='0' border='0'>
<tbody><tr valign='top'>
<td bgcolor='#6495ed' width='760' valign='middle' colspan='6'>
<table cellspacing='0' cellpadding='0' border='0' width='100%'>
<tbody><tr valign='top'>
<td bgcolor='#6495ed' width='9%' valign='middle'><b><font size='2' face='Arial' color='#ffffff'>Shipped</font></b></td>
<td bgcolor='#6495ed' width='91%' valign='middle'>
<div align='right'><font size='2' face='Arial' color='#ffffff'>Orders with multiple items may ship from different locations, and may ship separately.</font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>



<tr valign='top'>
<td bgcolor='#cee7ff' width='310' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Description</font></b>
</div>
</td>
<td bgcolor='#cee7ff' width='40' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Qty</font>
</b></div>
</td>

<td bgcolor='#cee7ff' width='120' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Ship Date</font> </b></div>
</td>
<td bgcolor='#cee7ff' width='120' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Est. Arrival Date</font> </b></div>
</td>
<td bgcolor='#cee7ff' width='167' valign='middle'>
<div align='center'><b><font size='2' face='Arial'>Tracking #</font></b></div>
</td>
<td width='4' valign='middle'></td>
</tr>

<tr valign='top'>
<td bgcolor='#f0f0f0' width='310' valign='middle'>
<div align='center'>
<font size='2' face='Arial'>

Every Man Jack 2-in-1 Shampoo &#43; Conditioner and Body Wash Combos &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;- Item# 1068097</font></div>
</td>
<td bgcolor='#f0f0f0' width='40' valign='middle'>
<div align='center'>
<font size='2' face='Arial'>

1


</font>
</div>
</td>

<td bgcolor='#f0f0f0' width='120' valign='middle'>

<div align='center'>
<font size='2' face='Arial'>06/24/2016</font>
</div>
</td>
<td bgcolor='#f0f0f0' width='120' valign='middle'>
                        
<div align='center'>07/01/2016
<font size='2' face='Arial'> </font>
</div>
</td>
<td bgcolor='#ffffcc' width='167' valign='middle'>
                        
<div align='center'>
<a href='http://wwwapps.ups.com/etracking/tracking.cgi?tracknums_displayed=5&amp;TypeOfInquiryNumber=T&amp;HTMLVersion=4.0&amp;sort_by=status&amp;InquiryNumber1=1ZA929830341798369' rel='nofollow' target='_blank'> <u> <font size='2' face='Arial' color='#0000ff'>1ZA929830341798369</font> </u> </a>

<font size='2' face='Arial'> </font>
</div>
</td>

<td width='4' valign='middle'></td>
</tr>

<tr valign='top'>
<td width='310' valign='middle'></td>
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

<p>View <a href='https://customerservice.costco.com/app/answers/detail/a_id/1191/kw/return%20policy' target='_blank'>Costco's Return Policy</a>.</p><br>

Costco.com products can be returned to any of our more than 600 Costco warehouses worldwide. 
 </font></td>
</tr>
</tbody>
</table>
</td>
</tr>

</tbody></table>
</td>
</tr>
</tbody></table>

<font size='4' face='Times New Roman'> </font>
<p><br>
    
    
    </p><table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><p>Visit <a href='https://www.costco.com/OrderStatusCmd?storeId=10301&amp;catalogId=10701&amp;langId=-1&amp;orderStatusStyle=strong&amp;URL=OrderStatusSummaryView&amp;cm_re=Common-_-Top_Nav-_-Order_Status' target='_blank'>Order Status</a> to track shipped orders. When tracking your order, please note that the tracking information may not be updated immediately.</p>
          <br></td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><table style='border:solid 1px #696969' cellpadding='5' cellspacing='2'>
            <tbody><tr>
              <td><b>Are you missing out on our LIMITED TIME OFFERS?</b> <br>
                Sign up to receive <a href='http://costco.com' target='_blank'>costco.com</a> e-mails on the <a href='https://www.costco.com/LogonForm?langId=-1&amp;storeId=10301&amp;catalogId=10701&amp;URL=LogonForm&amp;cm_re=Common-_-Top_Nav-_-My_Account' target='_blank'>My Account</a> page.<br>
                <br>
                <strong>Costco Office Online</strong> - Save on your office supplies! <a href='//www.costco.com/office-products.html?EMID=Transactional_OrderShipped_OfficeProducts' target='_blank'>Click here</a> to start savings TODAY!<br>
                
                Click to see <a><strong>What's New</strong></a> at Costco.com. <br></td>
            </tr>
          </tbody></table></td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><p><br>
            <font face='Arial' style='font-size:9pt'><b>Shop Confidently</b><br>
            <br>
            <b>Membership:</b> We will refund your membership fee in full at any time if you are dissatisfied.<br>
            <br>
            <b>Merchandise:</b> We guarantee your satisfaction on every product we sell with a full refund. The return policy for televisions, computers, major appliances, tablets, cameras, camcorders, MP3 players, and cellular phones is 90 days from date of purchase.  Manufacturer's warranty service is available on all electronics products. See manufacturer's warranty for specific coverage terms. For TV, Computers (excluding tablets) and Major Appliances, Costco extends the manufacturer's warranty to two years from date of purchase. Please call Costco Concierge @ <a href='tel:1-866-861-0450' value='&#43;18668610450' target='_blank'>1-866-861-0450</a> for warranty assistance, set-up help or technical support.<br>
            <br>
            <b>How to Return:</b> Simply return your purchase at any one of our Costco warehouses worldwide for a refund (including shipping and handling). If you are unable to return your order at one of our warehouses, please contact:</font></p>
          <p><font face='Arial' style='font-size:9pt'><strong>Costco.com</strong> <a href='https://customerservice.costco.com' target='_blank'>customer service</a> or call <strong><a href='tel:1-800-955-2292' value='&#43;18009552292' target='_blank'>1-800-955-2292</a></strong></font></p>
                    <p><font face='Arial' style='font-size:9pt'>To expedite the processing of your return, please reference your order number.</font></p>
          <p><font face='Arial' style='font-size:9pt'>If you request an item be picked up for return, the item must be packaged and   available for pick up in the same manner as it was delivered. <br>
            If your order   was delivered “curb side,” it will need to be available for curb side pick up. <br>
            If the item arrived to you in a box, it will need to be in a box at the time   of pick up. </font> </p>
          </td></tr></tbody></table><p></p>
          <font face='Arial' style='font-size:9pt'> Please <a href='//www.costco.com/returns-replacements-refunds.html' target='_blank'>click here</a> for additional returns information.</font>
          <p></p></td>
      </tr>
    </tbody></table>
    <table border='0'>
      <tbody><tr valign='top' cellspacing='0' cellpadding='0'>
        <td><font face='Arial' style='font-size:9pt'>Please keep this email for future reference, and thank you for shopping at <b>Costco.com</b>.</font></td>
      </tr>
    </tbody></table>



</blockquote></div><br></div></div>
</blockquote></div><br></div></div>
";

            ProcessCostcoShipEmail(body);
        }

        private void ProcessCostcoShipEmail(string body)
        {
            
            try
            {
                body = body.Replace("\r", "");
                body = body.Replace("\t", "");
                body = body.Replace("\n", "");
                string stOrderNumber = SubstringInBetween(body, "Order Number:</td>", "</td>", false, true);
                stOrderNumber = SubstringEndBack(stOrderNumber, "</td>", ">", false, false);
                stOrderNumber = stOrderNumber.Trim();

                string stWorking = SubstringInBetween(body, "Tracking #", "Shipping &amp; Terms", false, false);
                stWorking = TrimTags(stWorking);

                string stProductName = stWorking.Substring(0, stWorking.IndexOf("<"));
                stProductName = stProductName.Trim();

                stWorking = stWorking.Substring(stProductName.Length);
                stWorking = TrimTags(stWorking);

                string stQuatity = stWorking.Substring(0, stWorking.IndexOf("<"));
                stQuatity = stQuatity.Trim();

                stWorking = stWorking.Substring(stQuatity.Length);
                stWorking = TrimTags(stWorking);

                string stShipDate = stWorking.Substring(0, stWorking.IndexOf("<"));
                stShipDate = stShipDate.Trim();

                stWorking = stWorking.Substring(stShipDate.Length);
                stWorking = TrimTags(stWorking);

                string stArriveDate = stWorking.Substring(0, stWorking.IndexOf("<"));
                stArriveDate = stArriveDate.Trim();

                stWorking = stWorking.Substring(stArriveDate.Length);
                stWorking = TrimTags(stWorking);

                string stTrackNum = stWorking.Substring(0, stWorking.IndexOf("<"));
                stTrackNum = stTrackNum.Trim();


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

                string destinationFileName = Convert.ToDateTime(stShipDate).ToString("yyyyMMddHHmmss") + "_" + stOrderNumber + ".pdf";

                File.Delete(@"C:\temp\CostcoShipEmails\" + destinationFileName);

                File.Move(sourceFileFullName, @"C:\temp\CostcoShipEmails\" + destinationFileName);

                // db stuff
                string sqlString;
                bool bExist = false;

                SqlConnection cn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cn.Open();

                sqlString = @"UPDATE eBay_SoldTransactions SET CostcoTrackingNumber = @_costcoTrackingNumber,
                        CostcoShipEmailPdf = @_costcoShipEmailPdf, CostcoShipDate = @_costcoShipDate 
                        WHERE CostcoOrderNumber = @_costcoOrderNumber";

                cmd.CommandText = sqlString;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@_costcoTrackingNumber", stTrackNum);
                cmd.Parameters.AddWithValue("@_costcoShipEmailPdf", destinationFileName);
                cmd.Parameters.AddWithValue("@_costcoOrderNumber", stOrderNumber);
                cmd.Parameters.AddWithValue("@_costcoShipDate", stShipDate);

                cmd.ExecuteNonQuery();

                cn.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
        }

        private void btnCostcoOrder_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"SELECT * FROM eBay_SoldTransactions WHERE eBayItemNumber = @_eBayItemNumber AND BuyerID = @_buyerID";

            cmd.CommandText = sqlString;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@_eBayItemNumber", "152262952392");
            cmd.Parameters.AddWithValue("@_buyerID", "coffeebean217");

            eBaySoldProduct soldProduct = new eBaySoldProduct();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();

                soldProduct.PaypalTransactionID = Convert.ToString(reader["PaypalTransactionID"]);
                soldProduct.PaypalPaidDateTime = Convert.ToDateTime(reader["PaypalPaidDateTime"]);
                soldProduct.PaypalPaidEmailPdf = Convert.ToString(reader["PaypalPaidEmailPdf"]);
                soldProduct.eBayItemNumber = Convert.ToString(reader["eBayItemNumber"]);
                soldProduct.eBaySoldDateTime = Convert.ToDateTime(reader["eBaySoldDateTime"]);
                soldProduct.eBayItemName = Convert.ToString(reader["eBayItemName"]);
                soldProduct.eBaySoldVariation = Convert.ToString(reader["eBaySoldVariation"]);
                soldProduct.eBaySoldQuality = Convert.ToInt16(reader["eBaySoldQuality"]);
                soldProduct.eBaySoldEmailPdf = Convert.ToString(reader["eBaySoldEmailPdf"]);
                soldProduct.BuyerName = Convert.ToString(reader["BuyerName"]);
                soldProduct.BuyerID = Convert.ToString(reader["BuyerID"]);
                soldProduct.BuyerAddress1 = Convert.ToString(reader["BuyerAddress1"]);
                soldProduct.BuyerAddress2 = Convert.ToString(reader["BuyerAddress2"]);
                soldProduct.BuyerCity = Convert.ToString(reader["BuyerCity"]);
                soldProduct.BuyerState = Convert.ToString(reader["BuyerState"]);
                soldProduct.BuyerZip = Convert.ToString(reader["BuyerZip"]);
                soldProduct.BuyerEmail = Convert.ToString(reader["BuyerEmail"]);
                soldProduct.BuyerNote = Convert.ToString(reader["BuyerNote"]);
                soldProduct.CostcoUrlNumber = Convert.ToString(reader["CostcoUrlNumber"]);
                soldProduct.CostcoUrl = Convert.ToString(reader["CostcoUrl"]);
                soldProduct.CostcoPrice = Convert.ToDecimal(reader["CostcoPrice"]);
            }
            reader.Close();

            cn.Close();

            OrderCostcoProduct(soldProduct);
        }

        private bool hasElement(IWebElement webElement, By by)
        {
            try
            {
                webElement.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        private void OrderCostcoProduct(eBaySoldProduct soldProduct)
        {
            IWebDriver driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl("https://www.costco.com/LogonForm");
                driver.FindElement(By.Id("logonId")).SendKeys("zjding@gmail.com");
                driver.FindElement(By.Id("logonPassword")).SendKeys("721123");
                driver.FindElements(By.ClassName("submit"))[0].Click();

                driver.Navigate().GoToUrl("http://www.costco.com/");
                driver.FindElement(By.Id("cart-d")).Click();

                while (driver.FindElements(By.LinkText("Remove from cart")).Count > 0)
                {
                    driver.FindElements(By.LinkText("Remove from cart"))[0].Click();
                    System.Threading.Thread.Sleep(3000);
                }

                driver.Navigate().GoToUrl(soldProduct.CostcoUrl);

                IWebElement eProductDetails = driver.FindElement(By.Id("product-details"));
                if (hasElement(eProductDetails, By.Id("variants")))
                {
                    var eVariants = eProductDetails.FindElement(By.Id("variants"));

                    var productOptions = eVariants.FindElements(By.ClassName("swatchDropdown"));

                    List<string> selectList = new List<string>();

                    foreach (var productOption in productOptions)
                    {
                        selectList.Add(productOption.FindElement(By.TagName("select")).GetAttribute("id").ToString());
                    }

                    if (selectList.Count == 1)
                    {
                        IWebElement selectElement0 = eProductDetails.FindElement(By.Id(selectList[0]));
                        var options0 = selectElement0.FindElements(By.TagName("option"));
                        foreach (IWebElement option0 in options0)
                        {
                            if (options0.IndexOf(option0) > 0)
                            {
                                if (option0.Text.Contains("$"))
                                {
                                    int index = option0.Text.LastIndexOf("- $");
                                    if (option0.Text.Substring(0, index - 1).Trim() == soldProduct.eBaySoldVariation)
                                    {
                                        option0.Click();
                                        break;
                                    }
                                }
                                else
                                {
                                    if (option0.Text.Trim() == soldProduct.eBaySoldVariation)
                                    {
                                        option0.Click();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                driver.FindElement(By.Id("minQtyText")).Clear();
                driver.FindElement(By.Id("minQtyText")).SendKeys("1");
                driver.FindElement(By.Name("add-to-cart")).Click();

                //if (isAlertPresents(ref driver))
                //    driver.SwitchTo().Alert().Accept();

                driver.Navigate().GoToUrl("https://www.costco.com/CheckoutCartView");

                //driver.FindElement(By.Id("cart-d")).Click();

                if (isAlertPresents(ref driver))
                    driver.SwitchTo().Alert().Accept();

                string buyerFirstName = soldProduct.BuyerName.Split(' ')[0];
                string buyerMiddleInitial = soldProduct.BuyerName.Split(' ').Count() == 3 ? soldProduct.BuyerName.Split(' ')[1] : "";
                string buyerLastName = soldProduct.BuyerName.Split(' ')[soldProduct.BuyerName.Split(' ').Count() - 1];


                driver.FindElement(By.Id("shopCartCheckoutSubmitButton")).Click();

                driver.FindElement(By.Id("addressFormInlineFirstName")).SendKeys(buyerFirstName);
                driver.FindElement(By.Id("addressFormInlineMiddleInitial")).SendKeys(buyerMiddleInitial);
                driver.FindElement(By.Id("addressFormInlineLastName")).SendKeys(buyerLastName);
                driver.FindElement(By.Id("addressFormInlineAddressLine1")).SendKeys(soldProduct.BuyerAddress1);
                driver.FindElement(By.Id("addressFormInlineCity")).SendKeys(soldProduct.BuyerCity);

                string state = GetState(soldProduct.BuyerState);

                driver.FindElement(By.XPath("//select[@id='" + "addressFormInlineState" + "']/option[contains(.,'" + state + "')]")).Click();
                driver.FindElement(By.Id("addressFormInlineZip")).SendKeys(soldProduct.BuyerZip);
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

                System.Threading.Thread.Sleep(3000);

                driver.FindElement(By.Id("deliverySubmitButton")).Click();

                driver.FindElement(By.Id("cc_cvc")).SendKeys("819");

                driver.FindElement(By.Id("paymentSubButtonBot")).Click();

                //if (isAlertPresents(ref driver))
                //    driver.SwitchTo().Alert().Accept();

                //driver.FindElement(By.Id("orderButton")).Click();
            }
            catch (Exception e)
            {

            }
            finally
            {
                driver.Dispose();
            }
        }

        public string GetState(string state)
        {
            switch (state.ToUpper())
            {
                case "AL":
                    return "Alabama";

                case "AK":
                    return "Alaska";

                case "AS":
                    return "American Samoa";

                case "AZ":
                    return "Arizona";

                case "AR":
                    return "Arkansas";

                case "CA":
                    return "California";

                case "CO":
                    return "Colorado";

                case "CT":
                    return "Connecticut";

                case "DE":
                    return "Delaware";

                case "DC":
                    return "District of Columbia";

                case "FL":
                    return "Florida";

                case "GA":
                    return "Georgia";

                case "GU":
                    return "Guam";

                case "HI":
                    return "Hawaii";

                case "ID":
                    return "Idaho";

                case "IL":
                    return "Illinois";

                case "IN":
                    return "Indiana";

                case "IA":
                    return "Iowa";

                case "KS":
                    return "Kansas";

                case "KY":
                    return "Kentucky";

                case "LA":
                    return "Louisiana";

                case "ME":
                    return "Maine";

                case "MH":
                    return "Narshall Islands";

                case "MD":
                    return "Maryland";

                case "MA":
                    return "Massachusetts";

                case "MI":
                    return "Michigan";

                case "MN":
                    return "Minnesota";

                case "MS":
                    return "Mississippi";

                case "MO":
                    return "Missouri";

                case "MT":
                    return "Montana";

                case "NE":
                    return "Nebraska";

                case "NV":
                    return "Nevada";

                case "NH":
                    return "New Hampshire";

                case "NJ":
                    return "New Jersey";

                case "NM":
                    return "New Mexico";

                case "NY":
                    return "New York";

                case "NC":
                    return "North Carolina";

                case "ND":
                    return "North Dakota";

                case "OH":
                    return "Ohio";

                case "OK":
                    return "Oklahoma";

                case "OR":
                    return "Oregon";

                case "PW":
                    return "Palau";

                case "PA":
                    return "Pennsylvania";

                case "PR":
                    return "Puerto Rico";

                case "RI":
                    return "Rhode Island";

                case "SC":
                    return "South Carolina";

                case "SD":
                    return "South Dakota";

                case "TN":
                    return "Tennessee";

                case "TX":
                    return "Texas";

                case "UT":
                    return "Utah";

                case "VT":
                    return "Vermont";

                case "VI":
                    return "Virgin Islands";

                case "VA":
                    return "Virginia";

                case "WA":
                    return "Washington";

                case "WV":
                    return "West Virginia";

                case "WI":
                    return "Wisconsin";

                case "WY":
                    return "Wyoming";
            }

            throw new Exception("Not Available");
        }
    }

}
