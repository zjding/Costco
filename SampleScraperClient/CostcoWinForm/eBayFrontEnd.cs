﻿
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
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CostcoWinForm
{
    public partial class eBayFrontEnd : Form
    {
        DataLayer dl = new DataLayer();

        ScrapingBrowser Browser = new ScrapingBrowser();

        List<string> categoryArray = new List<string>();
        List<string> subCategoryArray = new List<string>();
        List<string> productUrlArray = new List<string>();

        List<String> categoryUrlArray = new List<string>();
        List<String> subCategoryUrlArray = new List<string>();

        List<String> newProductArray = new List<string>();
        List<String> discontinueddProductArray = new List<string>();
        List<String> priceUpProductArray = new List<string>();
        List<String> priceDownProductArray = new List<string>();

        List<String> eBayListingDiscontinueddProductArray = new List<string>();
        List<String> eBayListingPriceUpProductArray = new List<string>();
        List<String> eBayListingPriceDownProductArray = new List<string>();

        List<Product> priceChangedProductArray = new List<Product>();

        string emailMessage;

        string destinFileName;

        DateTime startDT;
        DateTime endDT;

        List<string> selectedItems = new List<string>();

        List<string> selectedListingItems = new List<string>();

        Timer timer = new Timer();

        string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        bool bInit = false;

        bool bCheckingAll = false;

        public eBayFrontEnd()
        {
            InitializeComponent();
        }

        private void eBayFrontEnd_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsEBaySold.eBay_SoldTransactions' table. You can move, or remove it, as needed.
            this.eBay_SoldTransactionsTableAdapter.Fill(this.dsEBaySold.eBay_SoldTransactions);
            // TODO: This line of code loads data into the 'costcoDataSet6.eBay_ToChange' table. You can move, or remove it, as needed.
            this.eBay_ToChangeTableAdapter.Fill(this.costcoDataSet6.eBay_ToChange);
            bInit = false;

            // TODO: This line of code loads data into the 'costcoDataSet5.eBay_ToRemove' table. You can move, or remove it, as needed.
            this.eBay_ToRemoveTableAdapter.Fill(this.ds_eBayToRemove.eBay_ToRemove);
            // TODO: This line of code loads data into the 'dseBayCurrentListings.eBay_CurrentListings' table. You can move, or remove it, as needed.
            this.eBay_CurrentListingsTableAdapter.Fill(this.dseBayCurrentListings.eBay_CurrentListings);
            // TODO: This line of code loads data into the 'ds_eBayToAdd.eBay_ToAdd' table. You can move, or remove it, as needed.
            this.eBay_ToAddTableAdapter.Fill(this.ds_eBayToAdd.eBay_ToAdd);
            // TODO: This line of code loads data into the 'costcoDataSet4.ProductInfo' table. You can move, or remove it, as needed.
            this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);
            //this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;

            //List<Department> categories = dl.GetDepartmentArray();

            //foreach (Department catetory in categories)
            //{
            //    ListViewItem item = new ListViewItem();
            //    item.Checked = catetory.bInclude;
            //    item.SubItems.Add(catetory.DepartmentName);
            //    item.SubItems.Add(catetory.CategoryName);
            //    item.SubItems.Add(catetory.Url);

            //    this.lvCategories.Items.Add(item);
            //}

            List<Category> categories = dl.GetCategoryArray();

            foreach (Category catetory in categories)
            {
                ListViewItem item = new ListViewItem();
                item.Checked = true;
                item.SubItems.Add(catetory.Category1);
                item.SubItems.Add(catetory.Category2);
                item.SubItems.Add(catetory.Category3);
                item.SubItems.Add(catetory.Category4);
                item.SubItems.Add(catetory.Category5);
                item.SubItems.Add(catetory.Category6);
                item.SubItems.Add(catetory.Category7);
                item.SubItems.Add(catetory.Category8);

                this.lvCategories.Items.Add(item);
            }

            chkAll.Checked = true;

            this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = (1000) * (3);              // Timer will tick evert second
            timer.Enabled = true;                       // Enable the timer
            timer.Start();

            bInit = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (tpPendingChanges == tabControl1.SelectedTab)
            {
                eBayToAddBindingSource.ResetBindings(false);
                this.eBay_ToAddTableAdapter.Fill(this.ds_eBayToAdd.eBay_ToAdd);
                this.gvToAdd.Refresh();

                eBayToChangeBindingSource.ResetBindings(false);
                this.eBay_ToChangeTableAdapter.Fill(this.costcoDataSet6.eBay_ToChange);
                this.gvToChange.Refresh();

                eBayToRemoveBindingSource.ResetBindings(false);
                this.eBay_ToRemoveTableAdapter.Fill(this.ds_eBayToRemove.eBay_ToRemove);
                this.gvToDelete.Refresh();
            } else if (tpCurrentListing == tabControl1.SelectedTab)
            {
                eBayCurrentListingsBindingSource.ResetBindings(false);
                eBay_CurrentListingsTableAdapter.Fill(this.dseBayCurrentListings.eBay_CurrentListings);
                gvCurrentListing.Refresh();
            }
        }

        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            RefreshProductsGrid();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            bCheckingAll = true;

            foreach (ListViewItem item in this.lvCategories.Items)
            {
                item.Checked = chkAll.Checked;
            }

            bCheckingAll = false;

            RefreshProductsGrid();
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
            GetDepartmentArray();

            GetSubCategoryUrls();

            GetProductUrls();

            GetProductInfo();

            SecondTry();

            GetProductInfo(false);

            SecondTry();

            GetProductInfo(false);

            PopulateTables();

            CompareProducts();

            ArchieveProducts();

            SendEmail();
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
                categoryTemp = categoryTemp.Replace("'", "''");
                categoryList.Add(categoryTemp);
            }

            //string subCategory = categoryList.ElementAt(categoryList.Count - 1);

            //if (subCategory.Contains("Other"))
            //{
            //    subCategory = "Other";
            //}

            string sqlString = "SELECT CategoryId FROM eBay_Categories WHERE " +
                                "F" + Convert.ToString(categoryList.Count) + "='" + categoryList.ElementAt(categoryList.Count - 2).Replace("'", "''") + "' AND " +
                                "F" + Convert.ToString(categoryList.Count + 1) + "='" + categoryList.ElementAt(categoryList.Count - 1).Replace("'", "''") + "'";

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

                if (imageNumber == 0)
                    imageNumber = 1;

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

        private void btnListingDelete_Click(object sender, EventArgs e)
        {
            string itemNumbers = "";

            foreach (string n in this.selectedListingItems)
            {
                itemNumbers += "'" + n + "',";
            }

            itemNumbers = itemNumbers.Substring(0, itemNumbers.Length - 1);

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"INSERT INTO eBay_ToRemove(eBayListingName, eBayItemNumber) 
                                 SELECT eBayListingName, eBayItemNumber 
                                 FROM eBay_CurrentListings
                                 WHERE eBayItemNumber in (" + itemNumbers + ")";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();

            // add to Excel file
            /*
            string sourceFileName = @"c:\ebay\documents\" + "FileExchangeRemove.csv";
            string destinFileName = @"c:\ebay\upload\" + "FileExchangeRemove-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
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
            foreach (string n in this.selectedListingItems)
            {
                    oSheet.Cells[i, 1].value = "End";
                    oSheet.Cells[i, 2].NumberFormat = "#";
                    oSheet.Cells[i, 2].value = n;
                    oSheet.Cells[i, 3].value = "NotAvailable";

                    i++;
            }

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);
            */
        }

        private void gvCurrentListing_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.gvCurrentListing.Rows[e.RowIndex].Cells[0].Value = !Convert.ToBoolean(gvProducts.Rows[e.RowIndex].Cells[0].Value);
                if (Convert.ToBoolean(gvCurrentListing.Rows[e.RowIndex].Cells[0].Value))
                {
                    gvCurrentListing.Rows[e.RowIndex].Selected = true;
                    gvCurrentListing.Rows[e.RowIndex].DefaultCellStyle.BackColor = SystemColors.Highlight;

                    selectedListingItems.Add(gvCurrentListing.Rows[e.RowIndex].Cells[5].Value.ToString());
                }
                else
                {
                    gvCurrentListing.Rows[e.RowIndex].Selected = false;
                    gvCurrentListing.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                    selectedListingItems.Remove(gvCurrentListing.Rows[e.RowIndex].Cells[5].Value.ToString());
                }
            }
        }

        private void btnReloadCurrentListing_Click(object sender, EventArgs e)
        {
            eBayCurrentListingsBindingSource.ResetBindings(false);
            this.eBay_CurrentListingsTableAdapter.Fill(this.dseBayCurrentListings.eBay_CurrentListings);
            //this.gvToAdd.Update();
            this.gvCurrentListing.Refresh();
        }

        private void lvCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> selectedCategories = new List<string>();

            foreach(ListViewItem item in lvCategories.Items)
            {
                if (item.Checked)
                {
                    string category = "";

                    for (int i = 0; i < 8; i++)
                    {
                        category += item.SubItems[i].Text + "|";
                    }

                    category = category.Substring(0, category.Length - 1);

                    selectedCategories.Add(category);
                }
            }

            string selectCategoriesString = "";

            foreach(string s in selectedCategories)
            {
                selectCategoriesString += "'" + s + "',";
            }

            selectCategoriesString = selectCategoriesString.Substring(0, selectCategoriesString.Length - 1);

            string sqlCommand = @"SELECT ID, Name, UrlNumber, ItemNumber, Category, Price, Shipping,
                                Limit, Discount, Details, Specification, ImageLink, Url, ImportedDT, eBayCategoryID, NumberofImage
                                FROM ProductInfo
                                WHERE Category in (" + selectCategoriesString + ")";
            
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand, connectionString);
            CostcoDataSet4.ProductInfoDataTable table = new CostcoDataSet4.ProductInfoDataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            //dataAdapter.Fill(table);

            //this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);

            productInfoBindingSource.ResetBindings(false);
            productInfoTableAdapter1.Fill(table);
            this.gvProducts.Refresh();

            //eBayCurrentListingsBindingSource.ResetBindings(false);
            //this.eBay_CurrentListingsTableAdapter.Fill(this.dseBayCurrentListings.eBay_CurrentListings);
            ////this.gvToAdd.Update();
            //this.gvCurrentListing.Refresh();
        }

        private void lvCategories_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (bInit)
            {

                

                //CostcoDataSet4.ProductInfoDataTable table = new CostcoDataSet4.ProductInfoDataTable();
                //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                ////dataAdapter.Fill(table);

                ////this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);

                //productInfoBindingSource.ResetBindings(false);
                //productInfoTableAdapter1.Fill(table);
                //this.gvProducts.Refresh();
            }
        }

        private void lvCategories_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (bCheckingAll)
                return;

            if (!bInit)
                return;

            RefreshProductsGrid();   
        }

        private void RefreshProductsGrid()
        {
            List<string> selectedCategories = new List<string>();

            foreach (ListViewItem item in lvCategories.Items)
            {
                if (item.Checked)
                {
                    string category = "";

                    for (int i = 0; i < 8; i++)
                    {
                        if (item.SubItems[i].Text.Length > 0)
                        {
                            category += item.SubItems[i].Text + "|";
                        }
                    }

                    category = category.Substring(0, category.Length - 1);

                    selectedCategories.Add(category);
                }
            }

            string selectCategoriesString = "";

            foreach (string s in selectedCategories)
            {
                selectCategoriesString += "'" + s + "',";
            }

            if (selectCategoriesString.Length > 0)
                selectCategoriesString = selectCategoriesString.Substring(0, selectCategoriesString.Length - 1);
            else
                selectCategoriesString = "''";

            string sqlCommand = @"SELECT ID, Name, UrlNumber, ItemNumber, Category, Price, Shipping,
                                Limit, Discount, Details, Specification, ImageLink, Url, ImportedDT, eBayCategoryID, NumberofImage
                                FROM ProductInfo
                                WHERE Category in (" + selectCategoriesString + ")" + txtFilter.Text;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand, connectionString);
            DataSet products = new DataSet();
            dataAdapter.Fill(products);
            DataTable productTable = products.Tables[0];

            gvProducts.DataSource = productTable;
            gvProducts.Refresh();
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

        private void GetSubCategoryUrls()
        {
            subCategoryArray.Clear();

            foreach (var categoryUrl in categoryUrlArray)
            {
                try
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
                catch (Exception exception)
                {
                    continue;


                }
            }

            //MessageBox.Show("Get subCategoryUrlArray Done");
        }

        private void GetProductUrls()
        {
            productUrlArray.Clear();

            foreach (string url in subCategoryArray)
            {
                try
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
                catch (Exception exception)
                {
                    continue;


                }
            }
                

            //MessageBox.Show("Get productUrlArray Done");
        }

        private void SecondTry()
        {
            productUrlArray.Clear();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select * from ProductInfo p 
                        where 
                        not exists
                        (select 1 from Raw_ProductInfo sp where sp.UrlNumber = p.UrlNumber)";
            cmd.CommandText = sqlString;
            SqlDataReader  rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                productUrlArray.Add(rdr["Url"].ToString());
            }

            rdr.Close();
            cn.Close();
        }

        private void GetProductInfo(bool bTruncate = true)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString;

            if (bTruncate)
            {
                startDT = DateTime.Now;

                sqlString = "TRUNCATE TABLE Raw_ProductInfo";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();

                sqlString = "TRUNCATE TABLE Costco_Categories";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();

                sqlString = "TRUNCATE TABLE Import_Errors";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();
            }

            //IWebDriver driver = new FirefoxDriver();
            WebPage PageResult;

            int i = 1;

            foreach (string pu in productUrlArray)
            {
                try
                {

                    i++;

                    string productUrl = pu.Replace("%2c", ",");

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
                    {
                        discount = discountNote.InnerText.Replace("?", "");
                        discount = discountNote.InnerText.Replace("'", "");
                    }

                    string productName = ((topReviewPanelNode[0]).SelectNodes("h1"))[0].InnerText;
                    productName = productName.Replace("???", "");
                    productName = productName.Replace("??", "");
                    productName = productName.Trim();

                    List<HtmlNode> col1Node = productInfo.CssSelect(".col1").ToList<HtmlNode>();
                    string itemNumber = (col1Node[0].SelectNodes("p")[0]).InnerText;
                    if (itemNumber.ToUpper().Contains("ITEM") && itemNumber.Length > 6)
                        itemNumber = itemNumber.Substring(6);
                    else
                        itemNumber = "";

                    discountNote = col1Node[0].CssSelect(".merchandisingText").FirstOrDefault();

                    if (discountNote != null)
                    {
                        discount = discount.Length == 0 ? discountNote.InnerText.Replace("?", "") : discount + "; " + discountNote.InnerText.Replace("?", "");
                        discount = discount.Replace("?", "");
                        discount = discount.Replace("'", "");
                    }
                        

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
                        if (productSHNode.InnerText.ToUpper().Contains("OPTIONS"))
                        {
                            continue;
                        }
                        else if (productSHNode.InnerText.ToUpper().Contains("INCLUDED") || productSHNode.InnerText.ToUpper().Contains("INLCUDED"))
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

                    /*
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
                    */

                    HtmlNode imageColumnNode = html.CssSelect(".image-column").ToList<HtmlNode>().First();

                    //HtmlNode carouselWrapNode = imageColumnNode.CssSelect(".product-image-carousel").ToList<HtmlNode>().First();
                    //var thumbHolderNode = carouselWrapNode.SelectNodes("//ul[@id='thumb_holder']");

                    //var thumbNodes = thumbHolderNode.SelectNodes("/li");

                    HtmlNode imageNode = imageColumnNode.SelectSingleNode("//img[@itemprop='image']");

                    string imageUrl = (imageNode.Attributes["src"]).Value;

                    sqlString = "INSERT INTO Raw_ProductInfo (Name, UrlNumber, ItemNumber, Category, Price, Shipping, Discount,  ImageLink, Url) VALUES ('" + productName + "','" + UrlNum + "','" + itemNumber + "','" + stSubCategories + "'," + price + "," + shipping + "," + "'" + discount + "','"  + imageUrl + "','" + productUrl + "')";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();

                    sqlString = "INSERT INTO Costco_Categories (" + columns + ") VALUES (" + values + ")";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    sqlString = "INSERT INTO Import_Errors (Url, Exception) VALUES ('" + pu + "','" + exception.Message + "')";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();

                    continue;
                }
            }

            cn.Close();

            endDT = DateTime.Now;

            //driver.Dispose();

            //MessageBox.Show("Start: " + startDT.ToLongTimeString() + "; End: " + endDT.ToLongTimeString());
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
            string sqlString = @"select s.Name, s.Price as newPrice, p.Price as oldPrice, s.Url 
                                from [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
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
            sqlString = @"select s.Name, s.Price as newPrice, p.Price as oldPrice, s.Url from 
                        [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
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

            // eBay listing price up
            sqlString = @"select s.Name, s.CostcoPrice as OldBasePrice, s.eBayListingPrice as eBayListingPrice, p.Price as NewBasePrice, p.Url as CostcoUrl, s.eBayItemNumber as eBayItemNumber
                            from [dbo].[eBay_CurrentListings] s, [dbo].[Staging_ProductInfo] p
                            where s.CostcoUrlNumber = p.UrlNumber
                            and s.CostcoPrice < p.Price";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                eBayListingPriceUpProductArray.Add("<a href='" + rdr["CostcoUrl"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["NewBasePrice"].ToString() + "|(" + rdr["OldBasePrice"].ToString() + ")");
            }

            rdr.Close();

            sqlString = @"INSERT INTO [dbo].[eBay_ToChange] (Name, CostcoUrlNumber, eBayItemNumber, eBayOldListingPrice, 
							eBayNewListingPrice, eBayReferencePrice, 
							CostcoOldPrice, CostcoNewPrice, PriceChange)
                            SELECT l.Name, l.CostcoUrlNumber, l.eBayItemNumber, l.eBayListingPrice, l.eBayListingPrice, 
                            l.eBayReferencePrice, l.CostcoPrice, r.Price, 'up'
                            FROM [dbo].[eBay_CurrentListings] l, [dbo].[Staging_ProductInfo] r
                            WHERE l.CostcoPrice < r.Price 
                            AND l.CostcoUrlNumber = r.UrlNumber";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            // eBay listing price down
            sqlString = @"select s.Name, s.CostcoPrice as OldBasePrice, s.eBayListingPrice as eBayListingPrice, p.Price as NewBasePrice, p.Url as CostcoUrl, s.eBayItemNumber as eBayItemNumber
                            from [dbo].[eBay_CurrentListings] s, [dbo].[Staging_ProductInfo] p
                            where s.CostcoUrlNumber = p.UrlNumber
                            and s.CostcoPrice > p.Price";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                eBayListingPriceDownProductArray.Add("<a href='" + rdr["CostcoUrl"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["NewBasePrice"].ToString() + "|(" + rdr["OldBasePrice"].ToString() + ")");
            }

            rdr.Close();

            sqlString = @"INSERT INTO [dbo].[eBay_ToChange] (Name, CostcoUrlNumber, eBayItemNumber, eBayOldListingPrice, 
							eBayNewListingPrice, eBayReferencePrice, 
							CostcoOldPrice, CostcoNewPrice, PriceChange)
                            SELECT l.Name, l.CostcoUrlNumber, l.eBayItemNumber, l.eBayListingPrice, l.eBayListingPrice, 
                            l.eBayReferencePrice, l.CostcoPrice, r.Price, 'down'
                            FROM [dbo].[eBay_CurrentListings] l, [dbo].[Staging_ProductInfo] r
                            WHERE l.CostcoPrice < r.Price
                            AND l.CostcoUrlNumber = r.UrlNumber";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            // eBay listing discontinused 
            sqlString = @"select * from eBay_CurrentListings p 
                        where 
                        not exists
                        (select 1 from Staging_ProductInfo sp where sp.UrlNumber = p.CostcoUrlNumber)";
            cmd.CommandText = sqlString;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                eBayListingDiscontinueddProductArray.Add("<a href='" + rdr["CostcoUrl"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["CostcoPrice"].ToString());
            }

            rdr.Close();

            sqlString = @"INSERT INTO [dbo].[eBay_ToRemove] (Name, CostcoUrlNumber, eBayItemNumber)
                            SELECT l.Name, l.CostcoUrlNumber, l.eBayItemNumber
                            FROM [dbo].[eBay_CurrentListings] l
                            WHERE not exists 
	                        (SELECT 1 FROM [dbo].[Staging_ProductInfo] r where r.UrlNumber = l.CostcoUrlNumber)";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

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
            emailMessage =  "<p>Start: " + startDT.ToLongTimeString() + "</p></br>";
            emailMessage += "<p>End: " + endDT.ToLongTimeString() + "</p></br>";

            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>Price up products: (" + priceUpProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "<h3>Price down products: (" + priceDownProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "<h3>New products: (" + newProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "<h3>Discontinued products: (" + discontinueddProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "<h3>eBay listing price up products: (" + eBayListingPriceUpProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "<h3>eBay listing price down products: (" + eBayListingPriceDownProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "<h3>eBay listing discontinued products: (" + eBayListingDiscontinueddProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>Price up products: (" + priceUpProductArray.Count.ToString() + ")</h3>" + "</br>";
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

            emailMessage += "<h3>eBay listing price up products: (" + eBayListingPriceUpProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (eBayListingPriceUpProductArray.Count == 0)
                emailMessage += "<p>No eBay listing price up product</p>" + "</br>";
            else
            {
                foreach (string priceUpProduct in eBayListingPriceUpProductArray)
                {
                    emailMessage += "<p>" + priceUpProduct + "</p></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>eBay listing price down products: (" + eBayListingPriceDownProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (eBayListingPriceDownProductArray.Count == 0)
                emailMessage += "<p>No eBay listing price down product</p>" + "</br>";
            else
            {
                foreach (string priceDownProduct in eBayListingPriceDownProductArray)
                {
                    emailMessage += "<p>" + priceDownProduct + "</p></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

            emailMessage += "<h3>eBay listing discontinued products: (" + eBayListingDiscontinueddProductArray.Count.ToString() + ")</h3>" + "</br>";
            emailMessage += "</br>";
            if (eBayListingDiscontinueddProductArray.Count == 0)
                emailMessage += "<p>No eBay listing discontinued product</p>" + "</br>";
            else
            {
                foreach (string priceDiscontinuedProduct in eBayListingDiscontinueddProductArray)
                {
                    emailMessage += "<p>" + priceDiscontinuedProduct + "</p></br>";
                }
            }
            emailMessage += "</br>";
            emailMessage += "</br>";

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

        private void btnToChangeUpload_Click(object sender, EventArgs e)
        {
            List<ProductUpdate> products = new List<ProductUpdate>();

            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = "  SELECT * FROM eBay_ToChange"; // WHERE shipping = 0.00 and Price < 100";

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductUpdate p = new ProductUpdate();
                    p.eBayItemNumbr = Convert.ToString(reader["eBayItemNumber"]);
                    p.NewPrice = Convert.ToDecimal(reader["eBayNewListingPrice"]);
                    products.Add(p);
                }
            }

            reader.Close();
            cn.Close();

            // add to Excel file
            string sourceFileName = @"c:\ebay\documents\" + "FileExchangeRevise.csv";
            string destinFileName = @"c:\ebay\upload\" + "FileExchangeRevise-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
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
            foreach (ProductUpdate product in products)
            {
                oSheet.Cells[i, 1].value = "Revise";
                oSheet.Cells[i, 2].NumberFormat = "#";
                oSheet.Cells[i, 2].value = product.eBayItemNumbr;        
                oSheet.Cells[i, 3].value = product.NewPrice;
               
                i++;
            }

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);
        }

        private void btnToDeleteUpload_Click(object sender, EventArgs e)
        {
            List<ProductUpdate> products = new List<ProductUpdate>();

            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = "  SELECT * FROM eBay_ToRemove"; // WHERE shipping = 0.00 and Price < 100";

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductUpdate p = new ProductUpdate();
                    p.eBayItemNumbr = Convert.ToString(reader["eBayItemNumber"]);
                    
                    products.Add(p);
                }
            }

            reader.Close();
            cn.Close();

            // add to Excel file
            string sourceFileName = @"c:\ebay\documents\" + "FileExchangeRemove.csv";
            string destinFileName = @"c:\ebay\upload\" + "FileExchangeRemove-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
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
            foreach (ProductUpdate product in products)
            {
                oSheet.Cells[i, 1].value = "End";
                oSheet.Cells[i, 2].NumberFormat = "#";
                oSheet.Cells[i, 2].value = product.eBayItemNumbr;
                oSheet.Cells[i, 3].value = "NotAvailable";

                i++;
            }
            
            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);
        }

        private void btnToChangeDelete_Click(object sender, EventArgs e)
        {
            gvToChange.Rows.RemoveAt(gvToChange.CurrentRow.Index);
            eBay_ToChangeTableAdapter.Update(costcoDataSet6.eBay_ToChange);
        }

        private void btnToChangeUpdate_Click(object sender, EventArgs e)
        {
            eBayToChangeBindingSource.EndEdit();
            eBay_ToChangeTableAdapter.Update(costcoDataSet6.eBay_ToChange);
        }

        private void btnListingModify_Click(object sender, EventArgs e)
        {
            string itemNumbers = "";

            foreach (string n in this.selectedListingItems)
            {
                itemNumbers += "'" + n + "',";
            }

            itemNumbers = itemNumbers.Substring(0, itemNumbers.Length - 1);

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"INSERT INTO eBay_ToChange(Name, eBayItemNumber, eBayOldListingPrice, eBayNewListingPrice) 
                                 SELECT eBayListingName, eBayItemNumber, eBayListingPrice, eBayListingPrice
                                 FROM eBay_CurrentListings
                                 WHERE eBayItemNumber in (" + itemNumbers + ")";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        private void gvToChange_SelectionChanged(object sender, EventArgs e)
        {
            //timer.Stop();
        }

        private void gvToChange_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //timer.Stop();
        }

        private void gvToChange_Enter(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void gvToChange_Leave(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void tpPendingChanges_Enter(object sender, EventArgs e)
        {
            timer.Start();
        }
    }
}