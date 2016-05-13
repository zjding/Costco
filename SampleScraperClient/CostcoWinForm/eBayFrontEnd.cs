
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
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using OpenQA.Selenium;
using System.Drawing.Imaging;
using OpenQA.Selenium.Firefox;

namespace CostcoWinForm
{
    public partial class eBayFrontEnd : Form
    {
        DataLayer dl = new DataLayer();

        ScrapingBrowser Browser = new ScrapingBrowser();

        List<string> categoryArray = new List<string>();
        List<string> subCategoryArray = new List<string>();
        List<string> productUrlArray = new List<string>();

        List<string> selectedItems = new List<string>();

        string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public eBayFrontEnd()
        {
            InitializeComponent();
        }

        private void eBayFrontEnd_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'ds_eBayToAdd.eBay_ToAdd' table. You can move, or remove it, as needed.
            this.eBay_ToAddTableAdapter.Fill(this.ds_eBayToAdd.eBay_ToAdd);
            // TODO: This line of code loads data into the 'costcoDataSet4.ProductInfo' table. You can move, or remove it, as needed.
            this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);
            //this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;

            List<Category> categories = dl.GetCategoryArray();

            foreach (Category catetory in categories)
            {
                ListViewItem item = new ListViewItem();
                item.Checked = catetory.bInclude;
                item.SubItems.Add(catetory.DepartmentName);
                item.SubItems.Add(catetory.CategoryName);
                item.SubItems.Add(catetory.Url);

                this.lvCategories.Items.Add(item);
            }

            this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);

        }

        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            // populate categoryArray
            categoryArray.Clear();
            foreach (ListViewItem item in this.lvCategories.Items)
            {
                if (item.Checked)
                {
                    categoryArray.Add(item.SubItems[3].Text);
                }
            }

            GetSubCategoryUrls();
            GetProductUrls();


        }

        private void GetSubCategoryUrls()
        {
            subCategoryArray.Clear();

            foreach (var categoryUrl in categoryArray)
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
                    subCategoryArray.Add(url);
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
                                subCategoryArray.Add(departmentUrl);
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
                                            subCategoryArray.Add(departmentUrl_1);
                                        }
                                        else
                                        {
                                            List<HtmlNode> department_2_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                                            foreach (HtmlNode department_2_Node in department_2_Nodes)
                                            {
                                                HtmlNode node_2 = department_2_Node.Descendants("a").First();
                                                string department_2_Url = node_2.Attributes["href"].Value;
                                                subCategoryArray.Add(department_2_Url);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HtmlNode node_1 = departmentNode.Descendants("a").First();
                                        string department_1_Url = node_1.Attributes["href"].Value;
                                        subCategoryArray.Add(department_1_Url);
                                    }
                                }
                            }
                        }
                        else
                        {
                            HtmlNode node = departmentNode.Descendants("a").First();
                            string departmentUrl = node.Attributes["href"].Value;
                            subCategoryArray.Add(departmentUrl);
                        }
                    }
                }
            }

            //MessageBox.Show("Get subCategoryUrlArray Done");
        }

        private void GetProductUrls()
        {
            foreach (string url in subCategoryArray)
            {
                WebPage PageResult = Browser.NavigateToPage(new Uri(url));
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

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvCategories.Items)
            {
                item.Checked = chkAll.Checked;
            }
        }

        private void gvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.gvProducts.Columns[e.ColumnIndex].Name == "Image")
            {
                string imageUrl = (this.gvProducts.Rows[e.RowIndex].Cells[11]).FormattedValue.ToString();
                if (imageUrl != "")
                {
                    e.Value = GetImageFromUrl(imageUrl);
                }
            } 
        }

        public static Image GetImageFromUrl(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            // if you have proxy server, you may need to set proxy details like below 
            //httpWebRequest.Proxy = new WebProxy("proxyserver",port){ Credentials = new NetworkCredential(){ UserName ="uname", Password = "pw"}};

            using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (Stream stream = httpWebReponse.GetResponseStream())
                {
                    return System.Drawing.Image.FromStream(stream);
                }
            }
        }

        private void gvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                string url = gvProducts.Rows[e.RowIndex].Cells[12].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            } else if (e.ColumnIndex == 0)
            {
                gvProducts.Rows[e.RowIndex].Cells[0] .Value = !Convert.ToBoolean(gvProducts.Rows[e.RowIndex].Cells[0].Value);
                if (Convert.ToBoolean(gvProducts.Rows[e.RowIndex].Cells[0].Value))
                {
                    gvProducts.Rows[e.RowIndex].Selected = true;
                    gvProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = SystemColors.Highlight;

                    selectedItems.Add(gvProducts.Rows[e.RowIndex].Cells[4].Value.ToString());
                }
                else
                {
                    gvProducts.Rows[e.RowIndex].Selected = false;
                    gvProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                    selectedItems.Remove(gvProducts.Rows[e.RowIndex].Cells[4].Value.ToString());
                }
            }
        }

        public void runCrawl()
        {
            MessageBox.Show("Hi");
        }

        private void btnCrawl_Click(object sender, EventArgs e)
        {
            runCrawl();
        }

        private void btnAddPending_Click(object sender, EventArgs e)
        {
            string urlNumbers = "";

            foreach (string n in this.selectedItems)
            {
                urlNumbers += "'" + n + "',";
            }

            urlNumbers = urlNumbers.Substring(0, urlNumbers.Length - 1);

            List<Product> products = new List<Product>();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"SELECT  Name, UrlNumber, ItemNumber, Category, Price, Shipping, Limit, Discount, ImageLink, Url
                                FROM ProductInfo
                                WHERE UrlNumber in (";
            sqlString += urlNumbers + ")";
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
                    product.Limit = Convert.ToString(reader["Limit"]);
                    product.Discount = Convert.ToString(reader["Discount"]);
                    product.ImageLink = Convert.ToString(reader["ImageLink"]);
                    product.Url = Convert.ToString(reader["Url"]);

                    products.Add(product);
                }
            }

            reader.Close();
            cn.Close();

            IWebDriver driver = new FirefoxDriver();
            int screenshotWidth, screenshotHeight, imageNumber;

            cn.Open();

            foreach (Product p in products)
            {
                string categoryIDAndPrice = GetEbayCategoryIDAndPrice(p.Name);
                p.eBayCategoryID = Convert.ToString(categoryIDAndPrice.Split('|')[0]);
                p.eBayReferencePrice = Convert.ToDecimal(categoryIDAndPrice.Split('|')[1]);

                if (GetProductInfoWithFirefox(ref driver, p.Url, p.UrlNumber, out screenshotWidth, out screenshotHeight, out imageNumber))
                {

                    p.DescriptionImageHeight = screenshotHeight;
                    p.DescriptionImageWidth = screenshotWidth;
                    p.NumberOfImage = imageNumber;

                    p.Details = "<p><img src='http://www.jasondingphotography.com/eBay/" + p.UrlNumber + ".jpg' width='" +
                                p.DescriptionImageWidth.ToString() + "' height='" + p.DescriptionImageHeight.ToString() + "'/></p>";

                    p.eBayListingPrice = Convert.ToDecimal(CalculateListingPrice(Convert.ToDouble(p.Price), Convert.ToDouble(p.eBayReferencePrice)));

                    sqlString = @"INSERT INTO eBay_ToAdd
                                (Name, UrlNumber, ItemNumber, Category, Price, Shipping, Limit, Discount, Details, ImageLink, NumberOfImage, 
                                Url, eBayCategoryID, eBayReferencePrice, eBayListingPrice, DescriptionImageWidth, DescriptionImageHeight)
                                VALUES (@_Name, @_UrlNumber, @_ItemNumber, @_Category, @_Price, @_Shipping, @_Limit, @_Discount, @_Details, @_ImageLink, @_NumberOfImage,
                                @_Url, @_eBayCategoryID, @_eBayReferencePrice, @_eBayListingPrice, @_DescriptionImageWidth, @_DescriptionImageHeight)";

                    cmd.CommandText = sqlString;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_Name", p.Name);
                    cmd.Parameters.AddWithValue("@_UrlNumber", p.UrlNumber);
                    cmd.Parameters.AddWithValue("@_ItemNumber", p.ItemNumber);
                    cmd.Parameters.AddWithValue("@_Category", p.Category);
                    cmd.Parameters.AddWithValue("@_Price", p.Price);
                    cmd.Parameters.AddWithValue("@_Shipping", p.Shipping);
                    cmd.Parameters.AddWithValue("@_Limit", p.Limit);
                    cmd.Parameters.AddWithValue("@_Discount", p.Discount);
                    cmd.Parameters.AddWithValue("@_Details", p.Details);
                    cmd.Parameters.AddWithValue("@_ImageLink", p.ImageLink);
                    cmd.Parameters.AddWithValue("@_NumberOfImage", p.NumberOfImage);
                    cmd.Parameters.AddWithValue("@_Url", p.Url);
                    cmd.Parameters.AddWithValue("@_eBayCategoryID", p.eBayCategoryID);
                    cmd.Parameters.AddWithValue("@_eBayReferencePrice", p.eBayReferencePrice);
                    cmd.Parameters.AddWithValue("@_eBayListingPrice", p.eBayListingPrice);
                    cmd.Parameters.AddWithValue("@_DescriptionImageWidth", p.DescriptionImageWidth);
                    cmd.Parameters.AddWithValue("@_DescriptionImageHeight", p.DescriptionImageHeight);

                    cmd.ExecuteNonQuery();
                }

            }

            cn.Close();
            driver.Dispose();
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

        private bool GetProductInfoWithFirefox(ref IWebDriver driver, string productUrl, string UrlNum, out int screenshotWidth, out int screenshotHeight, out int imageNumber)
        {
            imageNumber = 0;
            screenshotWidth = 0;
            screenshotHeight = 0;

            try
            {

                driver.Navigate().GoToUrl(productUrl);

                IWebElement element = driver.FindElement(By.Id("product-tab1"));

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

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 85L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                var screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));

                System.Drawing.Rectangle cropArea;

                if (element.FindElements(By.Id("wc-power-page")).Count > 0)
                {
                    IWebElement e = element.FindElement(By.Id("wc-power-page"));
                    IWebElement eCaption = e.FindElement(By.ClassName("wc-ppp-caption"));

                    int hCaption = 0;

                    if (eCaption != null)
                    {
                        hCaption = eCaption.Size.Height;
                    }

                    Size s = e.Size;
                    s.Height = s.Height - hCaption;
                    Point p = e.Location;
                    p.Y = p.Y + hCaption;
                    cropArea = new System.Drawing.Rectangle(p, s);
                }
                else if (element.FindElements(By.Id("sp_inline_product")).Count > 0)
                {
                    IWebElement e = element.FindElement(By.Id("sp_inline_product"));

                    Size s = e.Size;
                    s.Height = s.Height - 18;

                    Point p = e.Location;
                    p.Y = p.Y + 18;

                    cropArea = new System.Drawing.Rectangle(p, s);
                }
                else
                {
                    IWebElement e = element.FindElement(By.Id("product-tab1"));

                    Size s = e.Size;
                    s.Height = s.Height - 5;

                    Point p = e.Location;
                    p.Y = p.Y + 5;

                    cropArea = new System.Drawing.Rectangle(p, s);
                }

                screenshotWidth = cropArea.Width;
                screenshotHeight = cropArea.Height;

                bmpScreen.Clone(cropArea, bmpScreen.PixelFormat).Save(@"C:\temp\Screenshots\" + UrlNum + ".jpg", jpgEncoder, myEncoderParameters);

                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                    client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/" + UrlNum + ".jpg", "STOR", @"C:\temp\Screenshots\" + UrlNum + ".jpg");
                }

                IWebElement imageElement = driver.FindElement(By.Id("thumb_holder"));

                if (imageElement.FindElements(By.TagName("li")) != null)
                    imageNumber = imageElement.FindElements(By.TagName("li")).ToList().Count;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            eBayToAddBindingSource.ResetBindings(false);
            this.eBay_ToAddTableAdapter.Fill(this.ds_eBayToAdd.eBay_ToAdd);
            //this.gvToAdd.Update();
            this.gvToAdd.Refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            eBayToAddBindingSource.EndEdit();
            eBay_ToAddTableAdapter.Update(ds_eBayToAdd.eBay_ToAdd);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.gvToAdd.Rows.RemoveAt(gvToAdd.CurrentRow.Index);
            eBay_ToAddTableAdapter.Update(ds_eBayToAdd.eBay_ToAdd);
        }

        private double CalculateListingPrice(double productPrice, double eBayReferencePrice)
        {
            double listingPrice = productPrice;

            if (eBayReferencePrice < productPrice)
            {
                listingPrice += listingPrice * 0.05;
            }
            else
            {
                if ((eBayReferencePrice - productPrice) / eBayReferencePrice < 0.05)
                {
                    listingPrice += listingPrice * 0.05;
                }
                else
                {
                    listingPrice = eBayReferencePrice - 0.01;
                }
            }

            return listingPrice;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            List<Product> products = new List<Product>();

            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = "  SELECT * FROM eBay_ToAdd"; // WHERE shipping = 0.00 and Price < 100";

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
                    product.Limit = Convert.ToString(reader["Limit"]);
                    product.Discount = Convert.ToString(reader["Discount"]);
                    product.Details = Convert.ToString(reader["Details"]);
                    product.Specification = Convert.ToString(reader["Specification"]);
                    product.ImageLink = Convert.ToString(reader["ImageLink"]);
                    product.NumberOfImage = Convert.ToInt16(reader["NumberOfImage"]);
                    product.Url = Convert.ToString(reader["Url"]);
                    product.eBayCategoryID = Convert.ToString(reader["eBayCategoryID"]);
                    product.eBayReferencePrice = Convert.ToDecimal(reader["eBayReferencePrice"]);
                    product.eBayListingPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
                    product.DescriptionImageHeight = Convert.ToInt16(reader["DescriptionImageHeight"]);
                    product.DescriptionImageWidth = Convert.ToInt16(reader["DescriptionImageWidth"]);

                    products.Add(product);
                }
            }

            reader.Close();
            cn.Close();

            // add to Excel file
            string sourceFileName = @"c:\ebay\documents\" + "FileExchange.csv";
            string destinFileName = @"c:\ebay\upload\" + "FileExchange-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
            File.Copy(sourceFileName, destinFileName);

            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Range oRange;

            //oXL.Visible = true;
            oXL.DisplayAlerts = false;

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
                //oSheet.Cells[i, 1] = "VerifyAdd";
                oSheet.Cells[i, 1] = "Add";

                oSheet.Cells[i, 2] = product.eBayCategoryID;

                oSheet.Cells[i, 3] = product.Name;
                oSheet.Cells[i, 5] = product.Details;
                oSheet.Cells[i, 6] = "1000";

                string imageLinks = "";
                string imageLink = product.ImageLink.Substring(0, product.ImageLink.Length - 5);
                for (int j = 1; j <= product.NumberOfImage; j++)
                {
                    imageLinks += imageLink + j.ToString() + ".jpg|";
                }

                imageLinks = imageLinks.Substring(0, imageLinks.Length - 1);

                oSheet.Cells[i, 7] = imageLinks;
                oSheet.Cells[i, 8] = "1";
                oSheet.Cells[i, 9] = "FixedPrice";
                oSheet.Cells[i, 10] = product.eBayListingPrice;
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

            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);
        }
    }
}
