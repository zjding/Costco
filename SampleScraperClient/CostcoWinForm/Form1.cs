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
            string body = File.ReadAllText(@"C:\eBayApp\TestFiles\152262952392_paid.txt");

            body = body.Replace("\n", "");
            body = body.Replace("\t", "");
            body = body.Replace("\\", "");
            body = body.Replace("\"", "'");

            ProcessPaymentReceivedEmail(body);
        }

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

        private void ProcessPaymentReceivedEmail(string html)
        {
            {
                string body = html;

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

                //// Generate PDF for email
                //File.WriteAllText(@"C:\temp\temp.html", body);

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

                //string stUrl = SubstringEndBack(body, ">" + stItemName, "<a href =", false, false);
                //stUrl = SubstringInBetween(stUrl, "'", "'", false, false);

                string stPaid = SubstringInBetween(body, "Paid:", @"<br>", false, false);
                stPaid = stPaid.Replace("$", "");
                stPaid = stPaid.Trim();

                string stColor = string.Empty;
                if (body.Contains(@"Color:"))
                {
                    stColor = SubstringInBetween(body, @"Color:", @"</td>", false, false);
                    stColor = stColor.Trim();
                }

                string stSize = string.Empty;
                if (body.Contains(@"Size:"))
                {
                    stSize = SubstringInBetween(body, @"Size:", @"</td>", false, false);
                    stSize = stSize.Trim();
                }

                string stVariation = string.Empty;

                if (string.IsNullOrEmpty(stColor))
                {
                    if (string.IsNullOrEmpty(stSize))
                    {

                    } else
                    {
                        stVariation = stSize;
                    }
                } else
                {
                    if (string.IsNullOrEmpty(stSize))
                    {
                        stVariation = stColor;
                    }
                    else
                    {
                        stVariation = stColor + ";" + stSize;
                    }
                }

                string stDateSold = SubstringInBetween(body, @"Date Sold:", @"</td>", false, false);
                stDateSold = stDateSold.Trim();

                string stQuantitySold = SubstringInBetween(body, @"Quantity Sold:", @"</td>", false, false);
                stQuantitySold = stQuantitySold.Trim();

                string stBuyer = SubstringEndBack(body, "Contact Buyer", "Buyer", false, false);
                stBuyer = stBuyer.Replace(@"&nbsp", "");
                stBuyer = TrimTags(stBuyer);
                stBuyer = stBuyer.Substring(0, stBuyer.IndexOf("<"));

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
                              (eBayItemNumber, eBaySoldDateTime, eBayItemName, eBaySoldVariation, eBaySoldPrice, eBaySoldQuality, eBaySoldEmailPdf,
                               BuyerID, CostcoUrlNumber, CostcoItemNumber, CostcoUrl, CostcoPrice)
                              VALUES (@_eBayItemNumber, @_eBaySoldDateTime, @_eBayItemName, @_eBaySoldVariation, @_eBayPrice, @_eBaySoldQuality,  @_eBaySoldEmailPdf,
                               @_BuyerID, @_CostcoUrlNumber, @_CostcoItemNumber, @_CostcoUrl, @_CostcoPrice)";

                    cmd.CommandText = sqlString;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_eBayItemNumber", stItemNum);
                    cmd.Parameters.AddWithValue("@_eBaySoldDateTime", Convert.ToDateTime(stDateSold));
                    cmd.Parameters.AddWithValue("@_eBayItemName", stItemName);
                    cmd.Parameters.AddWithValue("@_eBaySoldVariation", stVariation);
                    cmd.Parameters.AddWithValue("@_eBayPrice", Convert.ToDecimal(eBayProduct.eBayListingPrice));
                    cmd.Parameters.AddWithValue("@_eBaySoldQuality", Convert.ToInt16(stQuantitySold));
                    cmd.Parameters.AddWithValue("@_eBaySoldEmailPdf", destinationFileName);
                    cmd.Parameters.AddWithValue("@_BuyerID", stBuyer);
                    cmd.Parameters.AddWithValue("@_CostcoUrlNumber", eBayProduct.CostcoUrlNumber);
                    cmd.Parameters.AddWithValue("@_CostcoItemNumber", eBayProduct.CostcoItemNumber);
                    cmd.Parameters.AddWithValue("@_CostcoUrl", eBayProduct.CostcoUrl);
                    cmd.Parameters.AddWithValue("@_CostcoPrice", eBayProduct.CostcoPrice);

                    cmd.ExecuteNonQuery();
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

                stBuyerName = stBuyerName.Trim();

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
                
                if (!bExist)
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
            //string subject = "Your eBay item sold! Kirkland Signature Men's Pima Cotton Slub Knit T-Shirt 152262902432";
            //string body = File.ReadAllText(@"C:\eBayApp\TestFiles\152262902432_sold.txt");

            string subject = "Your eBay item sold! Kirkland Signature Men's Crew Neck Tee 6-pack White 152262952392";
            string body = File.ReadAllText(@"C:\eBayApp\TestFiles\152262952392_sold.txt");

            //File.WriteAllText(@"C:\temp\temp.html", body);

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
            string body = File.ReadAllText(@"C:\eBayApp\TestFiles\152262902432_order.txt");

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

        private bool hasElement(IWebDriver webDriver, By by)
        {
            try
            {
                webDriver.FindElement(by);
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
            //IWebDriver driver = new FirefoxDriver();

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

                string select0 = string.Empty;
                string select1 = string.Empty;

                if (soldProduct.eBaySoldVariation.Length > 0)
                {
                    if (soldProduct.eBaySoldVariation.Contains(";"))
                    {
                        select0 = soldProduct.eBaySoldVariation.Split(';')[0];
                        select1 = soldProduct.eBaySoldVariation.Split(';')[1];
                    } else
                    {
                        select0 = soldProduct.eBaySoldVariation;
                    }
                }

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
                    } else if (selectList.Count == 2)
                    {
                        IWebElement selectElement0 = eProductDetails.FindElement(By.Id(selectList[0]));
                        var options0 = selectElement0.FindElements(By.TagName("option"));
                        foreach (IWebElement option0 in options0)
                        {
                            if (options0.IndexOf(option0) > 0)
                            {
                                if (option0.Text.Trim() == select0)
                                {
                                    option0.Click();
                                    break;
                                }
                            }
                        }

                        IWebElement selectElement1 = eProductDetails.FindElement(By.Id(selectList[1]));
                        var options1 = selectElement1.FindElements(By.TagName("option"));
                        foreach (IWebElement option1 in options1)
                        {
                            if (options1.IndexOf(option1) > 0)
                            {
                                if (option1.Text.Contains("$"))
                                {
                                    int index = option1.Text.LastIndexOf("- $");
                                    if (option1.Text.Substring(0, index - 1).Trim() == select1)
                                    {
                                        option1.Click();
                                        break;
                                    }
                                }
                                else
                                {
                                    if (option1.Text.Trim() == select1)
                                    {
                                        option1.Click();
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

                if (isAlertPresents(ref driver))
                    driver.SwitchTo().Alert().Accept();

                System.Threading.Thread.Sleep(3000);

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

                if (hasElement(driver, By.Id("deliverySubmitButton")))
                    driver.FindElement(By.Id("deliverySubmitButton")).Click();

                driver.FindElement(By.Id("cc_cvc")).SendKeys("819");

                driver.FindElement(By.Id("paymentSubButtonBot")).Click();

                System.Threading.Thread.Sleep(3000);

                if (isAlertPresents(ref driver))
                    driver.SwitchTo().Alert().Accept();

                driver.FindElement(By.Id("orderButton")).Click();
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
