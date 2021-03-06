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
using System.Configuration;

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
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Chrome;
using Microsoft.Win32.TaskScheduler;
using System.Globalization;

namespace CostcoWinForm
{
    public partial class eBayFrontEnd : Form
    {
        DataLayer dl = new DataLayer();

        ScrapingBrowser Browser = new ScrapingBrowser();
        IWebDriver driver;

        List<string> categoryArray = new List<string>();
        List<string> subCategoryArray = new List<string>();
        List<string> productUrlArray = new List<string>();

        List<String> categoryUrlArray = new List<string>();
        List<String> subCategoryUrlArray = new List<string>();
        List<string> productListPages = new List<string>();

        List<String> newProductArray = new List<string>();
        List<String> discontinueddProductArray = new List<string>();
        List<String> priceUpProductArray = new List<string>();
        List<String> priceDownProductArray = new List<string>();
        List<String> stockChangeProductArray = new List<string>();

        List<String> eBayListingDiscontinueddProductArray = new List<string>();
        List<String> eBayListingPriceUpProductArray = new List<string>();
        List<String> eBayListingPriceDownProductArray = new List<string>();

        List<Product> priceChangedProductArray = new List<Product>();

        bool bToAddRefreshing = false;
        bool bValueUpdateing = false;

        bool bCostcoTabEntering = false;
        bool bCostcoTabEnteringGridRefreshed = false;
        bool bToAddTabEnteringGridRefreshed = false;

        int nScanProducts = 0;
        int nImportProducts = 0;
        int nSkipProducts = 0;
        int nImportErrors = 0;

        string emailMessage;

        string destinFileName;

        DateTime startDT;
        DateTime productListEndDT;
        DateTime endDT;

        List<string> firstTry = new List<string>();
        List<string> secondTry = new List<string>();
        List<string> firstTryResult = new List<string>();
        List<string> secondTryResult = new List<string>();

        int nProductListPages;
        int nProductUrlArray;
        int nCategoryUrlArray;

        List<string> selectedItems = new List<string>();

        List<string> selectedListingItems = new List<string>();

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        static string connectionString; // = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        bool bInit = false;

        bool bCheckingAll = false;
        bool bToAddCategoryCheckingAll = false;

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public eBayFrontEnd()
        {
            bToAddRefreshing = true;
            InitializeComponent();
            bToAddRefreshing = false;

            //connectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
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

        private void eBayFrontEnd_Load(object sender, EventArgs e)
        {
            SetConnectionString();

            // TODO: This line of code loads data into the 'dsEBaySold.eBay_SoldTransactions' table. You can move, or remove it, as needed.
            //this.eBay_SoldTransactionsTableAdapter.Fill(this.dsEBaySold.eBay_SoldTransactions);

            bInit = false;



            // TODO: This line of code loads data into the 'ds_eBayToAdd.eBay_ToAdd' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'costcoDataSet4.ProductInfo' table. You can move, or remove it, as needed.
            //this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);
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

            lvCategories_Refresh();

            //this.productInfoTableAdapter1.Fill(this.costcoDataSet4.ProductInfo);

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = (1000) * (3);              // Timer will tick evert second
            timer.Enabled = true;                       // Enable the timer
            timer.Start();

            bInit = true;
        }

        // tpdashboard

        private void btnChangeForEBayListings_Click(object sender, EventArgs e)
        {
            int nEBayListingChangePriceUp = 0;
            int nEBayListingChangePriceDown = 0;
            int nEBayListingChangeDiscontinue = 0;
            int nEBayListingChangeOptions = 0;

            Crawler.Form1 crawler = new Crawler.Form1();
            crawler.CheckEBayListing(out nEBayListingChangePriceUp, out nEBayListingChangePriceDown, out nEBayListingChangeDiscontinue, out nEBayListingChangeOptions);

            this.llEBayDiscontinue.Text = nEBayListingChangeDiscontinue.ToString();
            this.llEBayPriceUp.Text = nEBayListingChangePriceUp.ToString();
            this.llEBayPriceDown.Text = nEBayListingChangePriceDown.ToString();
            this.llEBayOptions.Text = nEBayListingChangeOptions.ToString();

        }

        private void llEBayDiscontinue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "Discontinue");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_Discontinue]";
            cmd.CommandText = sqlString;
            llEBayDiscontinue.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        private void tpDashboard_Enter(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            //SqlDataReader rdr;

            cn.Open();

            string sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_PriceUp]";
            cmd.CommandText = sqlString;
            llEBayPriceUp.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_PriceDown]";
            cmd.CommandText = sqlString;
            llEBayPriceDown.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_Discontinue]";
            cmd.CommandText = sqlString;
            llEBayDiscontinue.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_OptionChange]";
            cmd.CommandText = sqlString;
            llEBayOptions.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_OptionChange]";
            cmd.CommandText = sqlString;
            llEBayOptions.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"select COUNT(1) FROM CostcoInventoryChange_PriceDown n
                                WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                                AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)";
            cmd.CommandText = sqlString;
            llCostcoPriceDown.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"SELECT count(1) FROM CostcoInventoryChange_New n 
                          WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                          AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)";
            cmd.CommandText = sqlString;
            cmd.CommandTimeout = 0;
            llNewProducts.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"SELECT count(1) FROM ProductInfo n 
                          WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                          AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)
                          AND LEN(LTRIM(RTRIM(n.Discount))) > 2 ";
            cmd.CommandText = sqlString;
            llCostcoOnSale.Text = Convert.ToString(cmd.ExecuteScalar());

            sqlString = @"  SELECT count(1) 
                            FROM ProductInfo n 
                            WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                            AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)
                            AND convert(varchar(10), n.Price) like '%.97%' ";
            cmd.CommandText = sqlString;
            llClearanceProducts.Text = Convert.ToString(cmd.ExecuteScalar());
        }

        private void llEBayPriceUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "PriceUp");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_PriceUp]";
            cmd.CommandText = sqlString;
            llEBayPriceUp.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }



        private void llEBayPriceDown_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "PriceDown");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_PriceDown]";
            cmd.CommandText = sqlString;
            llEBayPriceDown.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        private void llEBayOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "OptionChange");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select COUNT(1) from [dbo].[eBayListingChange_OptionChange]";
            cmd.CommandText = sqlString;
            llEBayOptions.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        //private void tpEBayToModify_Enter(object sender, EventArgs e)
        //{
        //    gvChange_Refresh();
        //}



        private void llCostcoPriceDown_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "CostcoPriceDown");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select COUNT(1) FROM CostcoInventoryChange_PriceDown n
                                WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                                AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)";
            cmd.CommandText = sqlString;
            llCostcoPriceDown.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        private void llNewProducts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "CostcoNewProducts");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select count(1) FROM CostcoInventoryChange_New n 
                                WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                                AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)";
            cmd.CommandText = sqlString;
            llNewProducts.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        private void llCostcoOnSale_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "CostcoDiscountProducts");

            frm.ShowDialog();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select count(1) FROM ProductInfo n 
                                WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                                AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)
                                AND LEN(LTRIM(RTRIM(n.Discount))) > 2 ";
            cmd.CommandText = sqlString;
            llCostcoOnSale.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        private void llClearanceProducts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEBayListingChange_Discontinue frm = new frmEBayListingChange_Discontinue(this, connectionString, "CostcoClearanceProducts");

            frm.Show();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            string sqlString = @"select count(1) FROM ProductInfo n 
                                WHERE not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = n.UrlNumber)
                                AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = n.UrlNumber)
                                AND convert(varchar(10), n.Price) like '%.97%'  ";
            cmd.CommandText = sqlString;
            llClearanceProducts.Text = Convert.ToString(cmd.ExecuteScalar());

            cn.Close();
        }

        //  tpCostco 

        private void tpCostco_Enter(object sender, EventArgs e)
        {
            bCostcoTabEntering = true;
            gvProducts_Refresh();
            bCostcoTabEnteringGridRefreshed = true;
            //bCostcoTabEntering = false;
        }

        private void lvCategories_Refresh()
        {
            lvCategories.Items.Clear();

            dl.connectionString = connectionString;
            List<Category> categories = dl.GetCategoryArray();

            List<string> category2s = new List<string>();

            foreach (Category catetory in categories)
            {
                if (catetory.Category1 + catetory.Category2 + catetory.Category3 + catetory.Category4 +
                    catetory.Category5 + catetory.Category6 + catetory.Category7 + catetory.Category8 == "")
                    continue;

                if (category2s.Contains(catetory.Category2))
                    continue;

                ListViewItem item = new ListViewItem();
                item.Checked = true;
                //item.SubItems.Add(catetory.Category1);
                item.SubItems.Add(catetory.Category2);
                //item.SubItems.Add(catetory.Category3);
                //item.SubItems.Add(catetory.Category4);
                //item.SubItems.Add(catetory.Category5);
                //item.SubItems.Add(catetory.Category6);
                //item.SubItems.Add(catetory.Category7);
                //item.SubItems.Add(catetory.Category8);

                category2s.Add(catetory.Category2);

                this.lvCategories.Items.Add(item);
            }

            chkAll.Checked = true;
        }

        private void gvProducts_Refresh()
        {
            List<string> selectedCategories = new List<string>();

            foreach (ListViewItem item in lvCategories.Items)
            {
                if (item.Checked)
                {
                    //string category = "";

                    //for (int i = 0; i < 8; i++)
                    //{
                    //    if (item.SubItems[i].Text.Length > 0)
                    //    {
                    //        category += item.SubItems[i].Text + "|";
                    //    }
                    //}

                    //category = category.Substring(0, category.Length - 1);

                    //selectedCategories.Add(category);

                    selectedCategories.Add("Home|" + item.SubItems[1].Text);
                }
            }

            string selectCategoriesString = "";

            //foreach (string s in selectedCategories)
            //{
            //    selectCategoriesString += "'" + s + "',";
            //}

            //if (selectCategoriesString.Length > 0)
            //    selectCategoriesString = selectCategoriesString.Substring(0, selectCategoriesString.Length - 1);
            //else
            //    selectCategoriesString = "''";

            foreach (string category in selectedCategories)
                selectCategoriesString += "Category like '" + category + "%' OR ";

            string sqlCommand = "";

            if (selectCategoriesString.Length == 0)
            {
                sqlCommand = @"SELECT i.ID, i.Name as CostcoProductName, UrlNumber, ItemNumber, Category, Price, Shipping,
                                Limit, Discount, Details, Specification, Thumb, ImageLink, Url, ImportedDT, eBayCategoryID, NumberofImage
                                FROM ProductInfo i where 1=2";
            }
            else
            {
                selectCategoriesString = selectCategoriesString.Substring(0, selectCategoriesString.Length - 3);

                sqlCommand = @"SELECT i.ID, i.Name as CostcoProductName, UrlNumber, ItemNumber, Category, Price, Shipping,
                                Limit, Discount, Details, Specification, Thumb, ImageLink, Url, ImportedDT, eBayCategoryID, NumberofImage
                                FROM ProductInfo i
                                WHERE (" + selectCategoriesString + ") " +
                                    @" AND not exists (SELECT UrlNumber FROM eBay_ToAdd a WHERE a.UrlNumber = i.UrlNumber and a.DeleteTime is null) 
                                AND not exists (SELECT CostcoUrlNumber FROM eBay_CurrentListings c WHERE c.CostcoUrlNumber = i.UrlNumber and c.DeleteDT is null)";
            }

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand, connectionString);
            DataSet products = new DataSet();
            dataAdapter.SelectCommand.CommandTimeout = 0;
            dataAdapter.Fill(products);
            DataTable productTable = products.Tables[0];

            gvProducts.DataSource = productTable;
            gvProducts.Refresh();

            foreach (DataGridViewRow row in gvProducts.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void btnCostcoCategory_Click(object sender, EventArgs e)
        {
            FrmCostcoCategories frmCategories = new FrmCostcoCategories();
            frmCategories.connectionString = connectionString;
            frmCategories.ShowDialog();
        }

        private void btnCrawl_Click(object sender, EventArgs e)
        {
            Crawler.Form1 crawler = new Crawler.Form1();
            crawler.runCrawl();

            lvCategories_Refresh();
            gvProducts_Refresh();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            bCheckingAll = true;

            foreach (ListViewItem item in this.lvCategories.Items)
            {
                item.Checked = chkAll.Checked;
            }

            bCheckingAll = false;

            gvProducts_Refresh();
        }

        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            gvProducts_Refresh();
        }

        private void gvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.gvProducts.Columns[e.ColumnIndex].Name == "Image")
            {
                string imageUrl = (this.gvProducts.Rows[e.RowIndex].Cells[13]).FormattedValue.ToString();
                if (imageUrl != "")
                {
                    e.Value = GetImageFromUrl(imageUrl);
                }
            }
            //else if (this.gvProducts.Columns[e.ColumnIndex].Name == "CostcoProductName")
            //{
            //    e.Value = (this.gvProducts.Rows[e.RowIndex].Cells[13]).FormattedValue.ToString();
            //}
        }

        public static System.Drawing.Image GetImageFromUrl(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream stream = httpWebReponse.GetResponseStream())
                    {
                        return System.Drawing.Image.FromStream(stream);
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void gvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 3)
            {
                string url = gvProducts.Rows[e.RowIndex].Cells[15].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }
            else if (e.ColumnIndex == 0)
            {
                gvProducts.Rows[e.RowIndex].Cells[0].Value = !Convert.ToBoolean(gvProducts.Rows[e.RowIndex].Cells[0].Value);
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

        private void btnAddPending_Click(object sender, EventArgs e)
        {
            string st = string.Empty;

            foreach (DataGridViewRow row in gvProducts.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Check"].Value) == true)
                {
                    st += "'" + row.Cells["UrlNumber"].Value.ToString() + "',";
                }
            }
            st = st.Substring(0, st.Length - 1);

            AddToEBayToAdd(st);

            gvProducts_Refresh();
        }

        public void AddToEBayToAdd(string urlNumbers)
        {
            List<Product> products = new List<Product>();

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"SELECT  i.Name, i.UrlNumber, i.ItemNumber, i.Category, i.Price, i.Shipping, i.Limit, i.Discount, i.ImageLink, i.Url, i.ImageOptions, i.Options, i.Thumb,
                                         c.eBay_Category_Number
                                FROM ProductInfo i, Costco_eBay_Categories c
                                WHERE i.Category = 
		                                ISNULL(c.Category1, '') + IIF(c.Category2 is NULL, '', '|') + 
		                                ISNULL(c.Category2, '') + IIF(c.Category3 is NULL, '', '|') + 
		                                ISNULL(c.Category3, '') + IIF(c.Category4 is NULL, '', '|') + 
		                                ISNULL(c.Category4, '') + IIF(c.Category5 is NULL, '', '|') + 
		                                ISNULL(c.Category5, '') + IIF(c.Category6 is NULL, '', '|') + 
		                                ISNULL(c.Category6, '') + IIF(c.Category7 is NULL, '', '|') + 
		                                ISNULL(c.Category7, '') + IIF(c.Category8 is NULL, '', '|') + 
		                                ISNULL(c.Category8, '') ";

            sqlString += " AND UrlNumber in (" + urlNumbers + ")";
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
                    product.ImageOptions = Convert.ToString(reader["ImageOptions"]);
                    product.Options = Convert.ToString(reader["Options"]);
                    product.eBayCategoryID = Convert.ToString(reader["eBay_Category_Number"]);
                    product.Thumb = Convert.ToString(reader["Thumb"]);

                    products.Add(product);
                }
            }

            reader.Close();
            cn.Close();


            //driver = new ChromeDriver();
            int descriptionWidth = 0, descriptionHeight = 0, reviewWidth = 0, reviewHeight = 0, ratingWidth = 0, ratingHeight = 0, imageNumber = 0;
            string feature = "", productImages = "";

            cn.Open();

            driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), TimeSpan.FromSeconds(180));

            foreach (Product p in products)
            {
                string eBayReferenceUrl = string.Empty;

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(p.ImageLink.Split('|')[0], @"C:\eBayApp\Files\Screenshots\" + p.UrlNumber + ".jpg");

                    var myBitMap = new Bitmap(@"C:\eBayApp\Files\Screenshots\" + p.UrlNumber + ".jpg");
                    var g = Graphics.FromImage(myBitMap);
                    g.DrawString("No Sales Tax!", new System.Drawing.Font("Tahoma", 35), Brushes.Red, new PointF(0, 0));
                
                    myBitMap.Save(@"C:\eBayApp\Files\Screenshots\" + p.UrlNumber + "_productimage.jpg", ImageFormat.Jpeg);

                    g.Dispose();
                    myBitMap.Dispose();

                    client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                    client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/" + p.UrlNumber + "_productimage.jpg", "STOR", @"C:\eBayApp\Files\Screenshots\" + p.UrlNumber + "_productimage.jpg");
                }

                string categoryIDAndPrice = GetEbayCategoryIDAndPrice(p.Name, ref eBayReferenceUrl, string.IsNullOrEmpty(p.eBayCategoryID));
                if (string.IsNullOrEmpty(p.eBayCategoryID))
                {
                    p.eBayCategoryID = Convert.ToString(categoryIDAndPrice.Split('|')[0]);

                    if (p.eBayCategoryID != "99" && !string.IsNullOrEmpty(p.eBayCategoryID))
                    {
                        sqlString = @"UPDATE Costco_eBay_Categories SET eBay_Category_Number = " + p.eBayCategoryID + " WHERE " +
                                    @"  ISNULL(Category1, '') + IIF(Category2 is NULL, '', '|') + 
                                    ISNULL(Category2, '') + IIF(Category3 is NULL, '', '|') +
                                    ISNULL(Category3, '') + IIF(Category4 is NULL, '', '|') +
                                    ISNULL(Category4, '') + IIF(Category5 is NULL, '', '|') +
                                    ISNULL(Category5, '') + IIF(Category6 is NULL, '', '|') +
                                    ISNULL(Category6, '') + IIF(Category7 is NULL, '', '|') +
                                    ISNULL(Category7, '') + IIF(Category8 is NULL, '', '|') +
                                    ISNULL(Category8, '') = '" + p.Category + "'";
                        cmd.CommandText = sqlString;
                        cmd.ExecuteNonQuery();
                    }
                }

                sqlString = @"SELECT Specifics FROM eBay_Categories WHERE CategoryId = '" + p.eBayCategoryID + "'";
                cmd.CommandText = sqlString;
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    p.Specifics = (rdr["Specifics"].ToString());
                }
                rdr.Close();

                p.eBayReferencePrice = Convert.ToDecimal(categoryIDAndPrice.Split('|')[1]);

                p.eBaySoldNumber = (categoryIDAndPrice.Split('|').Length == 2 || categoryIDAndPrice.Split('|')[2] == "") ? -1 : Convert.ToInt16(categoryIDAndPrice.Split('|')[2].Replace(",", ""));

                if (GetProductInfoWithFirefox(p.Url, p.UrlNumber, out descriptionWidth, out descriptionHeight, out reviewWidth, out reviewHeight, out ratingWidth, out ratingHeight, out feature, out productImages)) ;
                {
                    p.DescriptionImageHeight = descriptionHeight;
                    p.DescriptionImageWidth = descriptionWidth;
                    p.NumberOfImage = imageNumber;

                    string eBayTemplate = @"<div id='ds_div'>";
                    eBayTemplate += @"<link href='http://www.jasondingphotography.com/eBay/etemp.css' rel='stylesheet' type='text/css'>";
                    eBayTemplate += @"<link href='http://www.jasondingphotography.com/eBay/font-awesome.min.css' rel='stylesheet' type='text/css'>";
                    eBayTemplate += @"<link href='http://www.jasondingphotography.com/eBay/slider.css' rel='stylesheet' type='text/css'>";
                    eBayTemplate += @"<div id='etemp'><div id='eheader'><div id='etemp-wrap'><div style='clear:both;'></div><div id='econtent'>";
                    eBayTemplate += @"<div class='slider'>";

                    if (productImages == "")
                    {
                        eBayTemplate += @"<input type='radio' name='slide_switch' id='id1" + "' checked='checked'/>";
                        eBayTemplate += @"<label for='id1" + "'><img src='" + p.ImageLink + "' width='100'/></label>";
                        eBayTemplate += @"<img src='" + p.ImageLink + "'/>";
                    }
                    else
                    {
                        int i = 1;
                        foreach (string image in productImages.Split('|'))
                        {
                            if (i == 1)
                                eBayTemplate += @"<input type='radio' name='slide_switch' id='id" + i.ToString() + "' checked='checked'/>";
                            else
                                eBayTemplate += @"<input type='radio' name='slide_switch' id='id" + i.ToString() + "'/>";
                            eBayTemplate += @"<label for='id" + i.ToString() + "'><img src='" + image + "' width='100'/></label>";
                            eBayTemplate += @"<img src='" + image + "'/>";
                            i++;
                        }
                    }

                    eBayTemplate += "</div>";
                    eBayTemplate += @"<div id='pinfo'><div id='topbox-wrap'><h1 id='ptitle' style='font-size:30px'>";
                    eBayTemplate += p.Name; 
                    eBayTemplate += @"</h1>";
                    eBayTemplate += @"<div style='margin-top:20px'><img src='http://www.jasondingphotography.com/eBay/" + p.UrlNumber + "_rating.jpg' width='" + ratingWidth + "' height='" + ratingHeight + "' alt=''/></div>";
                    eBayTemplate += @"<div style='margin-bottom:20px; margin-top:20px'><span style='font-size:20px'>Price</span><span style='margin-left:20px; font-size:24px'>";
                    eBayTemplate += @"xxx" + @"</span><span style='margin-left:10px'><img src='http://www.jasondingphotography.com/eBay/NoTax.jpg' width='79' height='30' alt='' /></span> </div>";
                    eBayTemplate += @"<div style='margin-bottom:20px'> <span style='font-size:16px'>Features:</span> </div><ul id='itemdesc'>";

                    foreach (string f in feature.Split('|'))
                    {
                        eBayTemplate += @"<li>" + f + @"</li>";
                    }

                    eBayTemplate += @"</ul></div></div><div style='clear:both;' id='midclear'></div>";
                    eBayTemplate += @"<div id='ptabs'><div class='tabslide opentab'><div class='tabhead'><i class='fa fa-minus-circle'></i>Description</div><div class='tabcontent' style='display: block;'>";
                    eBayTemplate += @"<img src='http://www.jasondingphotography.com/eBay/" + p.UrlNumber + "_description.jpg' width='" + descriptionWidth.ToString() + "' height='" + descriptionHeight.ToString() + "' />";
                    eBayTemplate += @"</div><div class='tabslide'><div class='tabhead'><i class='fa fa-plus-circle'></i>Reviews</div><div class='tabcontent'>";
                    eBayTemplate += @"<img src='http://www.jasondingphotography.com/eBay/" + p.UrlNumber + "_review.jpg' width='" + reviewWidth.ToString() + "' height='" + reviewHeight.ToString() + "' /> </div>";
                    eBayTemplate += @"</div><div class='tabslide'><div class='tabhead'><i class='fa fa-plus-circle'></i>Store Policy</div><div class='tabcontent'>";
                    eBayTemplate += @"<strong>Payment</strong><br><br>We accepts payment by PayPal only. With a PayPal account, you can pay using your debit card, major credit card, or a bank account. Don't have a PayPal account yet? <a href='http://paypal.com'>Sign up now.</a><br><br>";
                    eBayTemplate += @"<strong>Sales Tax</strong><br><br>No sales tax!<br><br>";
                    eBayTemplate += @"<strong>Stock</strong><br><br>Items are shipped directly from the warehouse, (No Local Pick Ups!). Items are in stock at time of listing but are offered to other retailers & occasionally go on back order or sell out. Please don't hesitate to ask us prior to ordering whether or not your item is in stock. If for whatever reason your item is either sold out or on back order a full refund will be issued immediately. Please don't leave unfair feedback as I am upfront about the possibility of this happening.<br><br>";
                    eBayTemplate += @"<strong>Descriptions & Errors</strong><br><br>We attempt to describe products as accurately as possible. However, we do not warrant that product descriptions are accurate, complete, reliable, current, or error-free. In the event a product is listed at an incorrect price or with incorrect information due to a typographical error or an error in pricing or product information received from our suppliers, we shall have the right to refuse or cancel any orders placed for products listed at the incorrect price.<br><br>";
                    eBayTemplate += @"<strong>Expiration Date</strong><br><br>Satisfaction guaranteed. It will be fresh and not expired or near expiration. (Usually a few years out.)<br><br>";
                    eBayTemplate += @"<strong>Shipping</strong><br><br>We are not able to deliver to P.O. Boxes, Freight Forwarders, or APO boxes. Shipping is available to the contiguous 48 United States only. We are required to ship purchased item to the address listed with Ebay. Merchandise will be shipped within 2 business days (NOT including weekend and holidays)<br><br>";
                    eBayTemplate += @"<strong>Return</strong><br><br>We have a 14 day return policy on most items (excluding Non-Prescription Remedies, Food, Vitamins, Herbals &amp; Dietary Supplements,  Family Planning Items).Item must be returned in condition received (Tags &amp; Packaging). Buyer is responsible for all return shipping cost.";
                    eBayTemplate += @"</div></div></div><div style='clear:both;' id='midclear'></div></div></div></div></div></div></div>";
                    eBayTemplate += "<script>document.write('<sc' + 'ript src=\"ht' + 'tps://ajax.go' + 'ogleapis.com/ajax/libs/jque' + 'ry/2.1.4/jqu' + 'ery.min.j' + 's\"></scr' + 'ipt>');</script>";
                    eBayTemplate += @"<script>$(document).ready(function(){$('.tabhead').click(function(){$(this).parent('.tabslide').children('.tabcontent').slideToggle('slow');$(this).parent('.tabslide').toggleClass('opentab');$(this).children('.fa').toggleClass('fa-plus-circle');$(this).children('.fa').toggleClass('fa-minus-circle');});";
                    eBayTemplate += @"$('.lwp').click(function(){$(this).parent('.top').children('.tphide').slideToggle('slow');$(this).parent('.top').toggleClass('opennav');});";
                    eBayTemplate += @"$('#thumbs img').click(function(){$('#thumbs img').removeClass('active');$(this).addClass('active');});";
                    eBayTemplate += @"var tflag=0;$('#menutoggle').click(function(){$(this).toggleClass('active');$('#mobilenav').toggle('slow');$('#mobileoverlay').toggle();});";
                    eBayTemplate += @"$('#mobileoverlay').click(function () {$('#menutoggle').toggleClass('active');$('#mobilenav').toggle('slow');$('#mobileoverlay').toggle();});";
                    eBayTemplate += @"$('#searchtoggle').click(function () {$(this).toggleClass('active');$('#searchbar').toggle('fast');});});";
                    eBayTemplate += @"</script><br style='clear:both'><br style='clear:both'><span id='closeHtml'></span>";

                    p.Details = eBayTemplate;

                    double eBayShipping = 0.0;

                    p.eBayListingPrice = Convert.ToDecimal(CalculateListingPrice(Convert.ToDouble(p.Price), Convert.ToDouble(p.Shipping), Convert.ToDouble(p.eBayReferencePrice), out eBayShipping));

                    sqlString = @"INSERT INTO eBay_ToAdd
                                (Name, eBayName, UrlNumber, ItemNumber, Category, Price, Shipping, Limit, Discount, Details, ImageLink, NumberOfImage, Options, ImageOptions, 
                                Url, eBayCategoryID, eBayReferencePrice, eBayListingPrice, DescriptionImageWidth, DescriptionImageHeight, TemplateName, Specifics, InsertTime, eBayReferenceUrl, Thumb, ebaySoldNumber, eBayShipping)
                                VALUES (@_Name, @_eBayName, @_UrlNumber, @_ItemNumber, @_Category, @_Price, @_Shipping, @_Limit, @_Discount, @_Details, @_ImageLink, @_NumberOfImage, @_Options, @_ImageOptions,
                                @_Url, @_eBayCategoryID, @_eBayReferencePrice, @_eBayListingPrice, @_DescriptionImageWidth, @_DescriptionImageHeight, @_TemplateName, @_Specifics, GETDATE(), @_eBayReferenceUrl, @_Thumb, @_eBaySoldNumber, @_eBayShipping)";

                    string eBayName = RemoveSpecialCharacters(p.Name);
                    if (eBayName.Length > 80)
                        eBayName = eBayName.Substring(0, 79);

                    cmd.CommandText = sqlString;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_Name", p.Name);
                    cmd.Parameters.AddWithValue("@_eBayName", eBayName);
                    cmd.Parameters.AddWithValue("@_UrlNumber", p.UrlNumber);
                    cmd.Parameters.AddWithValue("@_ItemNumber", p.ItemNumber);
                    cmd.Parameters.AddWithValue("@_Category", p.Category);
                    cmd.Parameters.AddWithValue("@_Price", p.Price);
                    cmd.Parameters.AddWithValue("@_Shipping", p.Shipping);
                    cmd.Parameters.AddWithValue("@_Limit", p.Limit);
                    cmd.Parameters.AddWithValue("@_Discount", p.Discount);
                    cmd.Parameters.AddWithValue("@_Details", p.Details);
                    cmd.Parameters.AddWithValue("@_ImageLink", productImages == "" ? p.ImageLink : productImages);
                    cmd.Parameters.AddWithValue("@_NumberOfImage", p.NumberOfImage);
                    cmd.Parameters.AddWithValue("@_Options", p.Options);
                    cmd.Parameters.AddWithValue("@_ImageOptions", p.ImageOptions);
                    cmd.Parameters.AddWithValue("@_Url", p.Url);
                    cmd.Parameters.AddWithValue("@_eBayCategoryID", p.eBayCategoryID);
                    cmd.Parameters.AddWithValue("@_eBayReferencePrice", p.eBayReferencePrice);
                    cmd.Parameters.AddWithValue("@_eBayListingPrice", p.eBayListingPrice);
                    cmd.Parameters.AddWithValue("@_DescriptionImageWidth", p.DescriptionImageWidth);
                    cmd.Parameters.AddWithValue("@_DescriptionImageHeight", p.DescriptionImageHeight);
                    cmd.Parameters.AddWithValue("@_TemplateName", p.TemplateName);
                    cmd.Parameters.AddWithValue("@_Specifics", p.Specifics);
                    cmd.Parameters.AddWithValue("@_eBayReferenceUrl", eBayReferenceUrl);
                    cmd.Parameters.AddWithValue("@_Thumb", p.Thumb);
                    cmd.Parameters.AddWithValue("@_eBaySoldNumber", p.eBaySoldNumber == -1 ? (object)DBNull.Value : p.eBaySoldNumber);
                    cmd.Parameters.AddWithValue("@_eBayShipping", eBayShipping);

                    cmd.ExecuteNonQuery();
                }

            }

            cn.Close();
            driver.Close();
            driver.Dispose();
        }

        private string GetEbayCategoryIDAndPrice(string productName, ref string eBayReferenceUrl, bool bCategoryID = true)
        {
            //IWebDriver driver = new ChromeDriver();
            try
            {
                string ebaySearchUrl = "http://www.ebay.com/sch/i.html?LH_Sold=1&LH_ItemCondition=11&_sop=12&rt=nc&LH_BIN=1&_nkw=";

                productName = productName.Replace("  ", " ");
                productName = productName.Replace(" ", "+");
                productName = productName.Replace("%", "");
                productName = productName.Replace("&", "");
                ebaySearchUrl += productName;

                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate(ebaySearchUrl);
                waitTillLoad(this.webBrowser1);

                //driver.Navigate().GoToUrl(ebaySearchUrl);

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                var documentAsIHtmlDocument3 = (mshtml.IHTMLDocument3)webBrowser1.Document.DomDocument;
                StringReader sr = new StringReader(documentAsIHtmlDocument3.documentElement.outerHTML);
                doc.Load(sr);

                var ulNote = doc.DocumentNode.SelectSingleNode("//ul[@id='ListViewInner']");

                if (ulNote == null)
                {
                    ebaySearchUrl = "http://www.ebay.com/sch/i.html?LH_Sold=1&LH_ItemCondition=11&_sop=12&rt=nc&LH_BIN=1&_nkw=";

                    if (productName.Split('+').Length > 2)
                        ebaySearchUrl += productName.Split('+')[0] + "+" + productName.Split('+')[1] + "+" + productName.Split('+')[2];
                    else
                        ebaySearchUrl += productName.Split('+')[0] + "+" + productName.Split('+')[1];

                    webBrowser1.Navigate(ebaySearchUrl);

                    waitTillLoad(this.webBrowser1);

                    documentAsIHtmlDocument3 = (mshtml.IHTMLDocument3)webBrowser1.Document.DomDocument;

                    sr = new StringReader(documentAsIHtmlDocument3.documentElement.outerHTML);

                    doc.Load(sr);

                    ulNote = doc.DocumentNode.SelectSingleNode("//ul[@id='ListViewInner']");

                    if (ulNote == null)
                    {
                        return "99|0";
                    }
                }

                List<HtmlNode> liNotes = ulNote.SelectNodes("li").ToList();

                HtmlNode hrefNote = liNotes[0].SelectSingleNode("//h3[@class='lvtitle']");

                HtmlNode node = hrefNote.Descendants("a").First();
                eBayReferenceUrl = node.Attributes["href"].Value;



                WebPage PageResult = Browser.NavigateToPage(new Uri(eBayReferenceUrl));

                HtmlNode priceNote = PageResult.Html.SelectSingleNode("//span[@itemprop='price']");
                if (priceNote == null)
                {
                    priceNote = PageResult.Html.SelectSingleNode("//span[@id='mm-saleDscPrc']");
                }
                string price = priceNote.InnerText;
                price = price.Substring(4);

                HtmlNode soldNumberNote = PageResult.Html.SelectSingleNode("//span[@class='w2b-sgl']");
                string soldNumber = string.Empty;
                if (soldNumberNote != null && soldNumberNote.InnerText.Contains("sold"))
                {
                    soldNumber = soldNumberNote.InnerText;
                    soldNumber = soldNumber.Replace("sold", "");
                    soldNumber = soldNumber.Trim();
                    soldNumber = soldNumber.Replace(",", "");

                    int value;
                    if (!int.TryParse(soldNumber, out value))
                        soldNumber = string.Empty;
                }

                string categoryID = "";

                if (bCategoryID)
                {
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
                                        "F" + Convert.ToString(categoryList.Count) + "='" + categoryList.ElementAt(categoryList.Count - 2) + "' AND " +
                                        "F" + Convert.ToString(categoryList.Count + 1) + "='" + categoryList.ElementAt(categoryList.Count - 1) + "'";



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
                }

                return categoryID + "|" + price + "|" + soldNumber;
            }
            catch (Exception e)
            {
                return "99|0";
            }
            finally
            {
            }
        }

        private bool GetProductInfoWithFirefox(string productUrl, string UrlNum, out int descriptionWidth, out int descriptionHeight, out int reviewWidth, out int reviewHeight, out int ratingWidth, out int ratingHeight, out string feature, out string stProductImages)
        {
            descriptionWidth = 0;
            descriptionHeight = 0;

            reviewWidth = 0;
            reviewHeight = 0;

            ratingWidth = 0;
            ratingHeight = 0;

            feature = "";
            stProductImages = "";

            try
            {
                driver.Navigate().GoToUrl(productUrl);

                GetDescriptionScreenshot(UrlNum, out descriptionWidth, out descriptionHeight);

                // review
                driver.FindElement(By.Id("pdp-accordion-header-1")).Click();

                IWebElement iReviewText = driver.FindElement(By.Id("reviews-text"));
                iReviewText.Click();

                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                if (hasElement(driver, By.ClassName("bv-content-list-Reviews")))
                {
                    IWebElement iReviews = driver.FindElement(By.ClassName("bv-content-list-Reviews"));

                    GetScreenshot(UrlNum, ref iReviews, "review", out reviewWidth, out reviewHeight);
                }
                // rating
                IWebElement iRating = driver.FindElement(By.ClassName("bv-stars-container"));
                GetScreenshot(UrlNum, ref iRating, "rating", out ratingWidth, out ratingHeight);

                // feature
                if (hasElement(driver, By.ClassName("pdp-features")))
                {
                    IWebElement iFeature = driver.FindElement(By.ClassName("pdp-features"));

                    feature = iFeature.Text;

                    feature = feature.Replace("\r\n", @"|");
                }

                IWebElement iZoomViewer = driver.FindElement(By.Id("zoomViewer"));

                if (hasElement(iZoomViewer, By.ClassName("slick-track")))
                {
                    IWebElement iSlickTrack = iZoomViewer.FindElement(By.ClassName("slick-track"));

                    var iSlickSlides = iSlickTrack.FindElements(By.ClassName("slick-slide"));

                    IWebElement iProductImageContainer = driver.FindElement(By.Id("productImageContainer"));

                    foreach (IWebElement iSlickSlide in iSlickSlides)
                    {
                        iSlickSlide.Click();

                        IWebElement i680Image = iProductImageContainer.FindElement(By.ClassName("RICHFXColorChange"));

                        string image = i680Image.FindElement(By.TagName("img")).GetAttribute("src");

                        stProductImages += image + "|";
                    }

                    stProductImages = stProductImages.Substring(0, stProductImages.Length - 1);
                }
                else
                {
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
        }

        private void GetScreenshot(string UrlNum, ref IWebElement element, string screenshotName, out int width, out int height)
        {
            Size reviewSize = element.Size;
            Point reviewPoint = element.Location;
            System.Drawing.Rectangle cropArea1 = new System.Drawing.Rectangle(reviewPoint, reviewSize);

            width = cropArea1.Width;
            height = cropArea1.Height;

            if (height == 0)
            {
                width = 0;
                return;
            }

            ImageCodecInfo jpgEncoder1 = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder1 = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters1 = new EncoderParameters(1);

            EncoderParameter myEncoderParameter1 = new EncoderParameter(myEncoder1, 85L);
            myEncoderParameters1.Param[0] = myEncoderParameter1;

            var screenshotDriver1 = driver as ITakesScreenshot;
            Screenshot screenshot1 = screenshotDriver1.GetScreenshot();
            var bmpScreen1 = new Bitmap(new MemoryStream(screenshot1.AsByteArray));

            bmpScreen1.Clone(cropArea1, bmpScreen1.PixelFormat).Save(@"C:\temp\Screenshots\" + UrlNum + "_" + screenshotName + ".jpg", jpgEncoder1, myEncoderParameters1);

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/" + UrlNum + "_" + screenshotName + ".jpg", "STOR", @"C:\temp\Screenshots\" + UrlNum + "_" + screenshotName + ".jpg");
            }
        }

        private void GetDescriptionScreenshot(string UrlNum, out int descriptionWidth, out int descriptionHeight)
        {
            // view more
            IWebElement eViewMore = driver.FindElement(By.ClassName("view-more"));
            IWebElement eViewMoreLink = eViewMore.FindElement(By.TagName("a"));
            eViewMoreLink.Click();

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 85L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));

            System.Drawing.Rectangle cropArea;

            IWebElement element = driver.FindElement(By.ClassName("product-info-description"));

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
                Size s = element.Size;
                Point p = element.Location;

                if (hasElement(element, By.XPath("//span[contains(text(),'local warehouse')]")))
                {
                    IWebElement w = element.FindElement(By.XPath("//span[contains(text(),'local warehouse')]"));

                    s.Height = s.Height - w.Size.Height;
                    p.Y = p.Y + w.Size.Height * 2;
                }

                cropArea = new System.Drawing.Rectangle(p, s);
            }

            descriptionWidth = cropArea.Width;
            descriptionHeight = cropArea.Height;

            bmpScreen.Clone(cropArea, bmpScreen.PixelFormat).Save(@"C:\temp\Screenshots\" + UrlNum + "_description.jpg", jpgEncoder, myEncoderParameters);

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/" + UrlNum + "_description.jpg", "STOR", @"C:\temp\Screenshots\" + UrlNum + "_description.jpg");
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

        private double CalculateListingPrice(double productPrice, double costcoShipping, double eBayReferencePrice, out double eBayShipping)
        {
            double profit = 2.0;

            double eBayPrice_Int = System.Math.Truncate(eBayReferencePrice);
            double eBayPrice_Deicmal = eBayReferencePrice - eBayPrice_Int;

            if (eBayPrice_Deicmal > 0.97)
                eBayPrice_Deicmal = 0.97;
            else if (eBayPrice_Deicmal > 0.47)
                eBayPrice_Deicmal = 0.47;
            else
            {
                eBayPrice_Deicmal = 0.97;
                eBayPrice_Int = eBayPrice_Int - 1;
            }

            double eBayListPrice = eBayPrice_Int + eBayPrice_Deicmal;

            eBayShipping = (profit + (productPrice + costcoShipping) * 1.09 + 0.3) / 0.871 - eBayListPrice;

            double eBayShipping_Int = System.Math.Truncate(eBayShipping);
            double eBayShipping_Decimal = eBayShipping - eBayShipping_Int;

            if (eBayShipping_Decimal == 0.00)
                eBayShipping_Decimal = 0.00;
            else if (eBayShipping_Decimal < 0.47)
                eBayShipping_Decimal = 0.47;
            else if (eBayShipping_Decimal < 0.97)
                eBayShipping_Decimal = 0.97;
            else
                eBayShipping_Decimal = 0.99;

            eBayShipping = eBayShipping_Int + eBayShipping_Decimal;

            return eBayListPrice;

            /*
            double profit = 1.5;
            double listingPrice = (0.3 + 0.3 + (costcoShipping + productPrice) * 1.09 + profit) / (1.09 - 1.09 * 0.129);
            //double listingPrice = (0.3 + (shipping + productPrice) * 1.09 + profit) / (1.00 - 0.129);
            //if (eBayReferencePrice < productPrice)
            //{
            //    listingPrice += listingPrice * 0.05;
            //}
            //else
            //{
            //    if ((eBayReferencePrice - productPrice) / eBayReferencePrice < 0.05)
            //    {
            //        listingPrice += listingPrice * 0.05;
            //    }
            //    else
            //    {
            //        listingPrice = eBayReferencePrice - 0.01;
            //    }
            //}

            listingPrice = Math.Round(listingPrice, 2);

            return listingPrice;
            */
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            Crawler.Form1 crawler = new Crawler.Form1();
            crawler.addProduct(txtProductUrl.Text);

            lvCategories_Refresh();
            gvProducts_Refresh();
        }

        private void lvCategories_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (bCheckingAll)
                return;

            if (!bInit)
                return;

            if (bCostcoTabEnteringGridRefreshed)
                return;

            gvProducts_Refresh();
        }

        private void lvCategories_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void lvCategories_Click(object sender, EventArgs e)
        {
            bCostcoTabEnteringGridRefreshed = false;
        }

        private void tpCostco_Leave(object sender, EventArgs e)
        {
            bCostcoTabEnteringGridRefreshed = false;
        }

        private void chkProductAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gvProducts.Rows)
            {
                row.Cells["Check"].Value = chkProductAll.Checked;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            (this.gvProducts.DataSource as DataTable).DefaultView.RowFilter = string.Format("CostcoProductName LIKE '%{0}%'", txtFilter.Text);
        }

        // tpToAdd

        SqlCommand cmdToAdd;
        SqlDataAdapter daToAdd;
        SqlCommandBuilder cmbToAdd;
        DataSet dsToAdd;
        DataTable dtToAdd;

        public void gvAdd_Refresh()
        {
            bToAddRefreshing = true;

            List<string> selectedCategories = new List<string>();


            foreach (ListViewItem item in lvToAddCategory.Items)
            {
                if (item.Checked)
                {
                    selectedCategories.Add("Home|" + item.SubItems[1].Text);
                }
            }


            string sqlString = string.Empty;

            foreach (string category in selectedCategories)
            {
                sqlString += @"SELECT ID, Thumb, ImageLink, Name as ProductName, 0.00 as eBayReferencePriceProfit, Price, eBayListingPrice, eBayShipping, 0.00 as Profit, Shipping, Url, eBayReferencePrice,  eBaySoldNumber, 
                                        UrlNumber, eBayReferenceUrl, Details, Limit, Options, Category, eBayCategoryID, eBayName
                                 FROM eBay_ToAdd 
                                 WHERE DeleteTime is NULL
                                 AND Category like '" + category + "%' UNION ";
            }

            if (sqlString.Length == 0)
                sqlString = @"SELECT ID, Thumb, ImageLink, Name as ProductName, 0.00 as eBayReferencePriceProfit, Price, eBayListingPrice, eBayShipping, 0.00 as Profit, Shipping, Url, eBayReferencePrice,  eBaySoldNumber, 
                                        UrlNumber, eBayReferenceUrl, Details, Limit, Options, Category, eBayCategoryID, eBayName
                                 FROM eBay_ToAdd
                                 WHERE 1 = 2";
            else
            {
                sqlString = sqlString.Substring(0, sqlString.Length - 7);
                sqlString += " ORDER BY eBaySoldNumber desc";
            }

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdToAdd = new SqlCommand(sqlString, connection);
            cmdToAdd.CommandTimeout = 0;
            daToAdd = new SqlDataAdapter(cmdToAdd);
            cmbToAdd = new SqlCommandBuilder(daToAdd);
            dsToAdd = new DataSet();
            daToAdd.Fill(dsToAdd, "tbToAdd");
            dtToAdd = dsToAdd.Tables["tbToAdd"];
            connection.Close();

            gvAdd.DataSource = dsToAdd.Tables["tbToAdd"];
            gvAdd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gvAdd.Columns["ToAddCostcoPrice"].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns["ToAddCostcoPrice"].Width = 50;
            gvAdd.Columns["ToAddShipping"].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns["ToAddShipping"].Width = 50;
            gvAdd.Columns["ToAddReferencePrice"].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns["ToAddReferencePrice"].Width = 50;
            gvAdd.Columns[13].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns[13].Width = 50;
            gvAdd.Columns[8].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns[8].Width = 50;
            gvAdd.Columns[9].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns[9].Width = 50;
            gvAdd.Columns[7].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns[7].Width = 50;
            gvAdd.Columns[10].DefaultCellStyle.Format = "0.00##";
            gvAdd.Columns[10].Width = 50;

            gvAdd.Columns["AddSelect"].Width = 50;
            gvAdd.Columns["ToAddName"].Width = 250;
            gvAdd.Columns["Resize"].Width = 50;
            gvAdd.Columns["ToAddShipping"].Width = 50;

            gvAdd.Columns["UrlNumber"].Visible = false;
            gvAdd.Columns["Limit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gvAdd.Columns["eBayReferenceUrl"].Visible = false;
            gvAdd.Columns["ID"].Visible = false;
            gvAdd.Columns["ImageLink"].Visible = false;
            gvAdd.Columns["Thumb"].Visible = false;
            gvAdd.Columns["eBayName"].Visible = false;
            gvAdd.Columns["Url"].Visible = false;
            gvAdd.Columns["Category"].Visible = false;

            bToAddRefreshing = false;
        }

        private void chkAddAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gvAdd.Rows)
            {
                if (row.Index < gvAdd.Rows.Count - 1)
                    row.Cells["AddSelect"].Value = chkAddAll.Checked;
            }
        }

        private void gvAdd_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            daToAdd.Update(dtToAdd);
        }

        private void gvAdd_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (bToAddRefreshing)
                return;

            if (bValueUpdateing)
                return;

            if (e.ColumnIndex == 9)
            {
                bValueUpdateing = true;
                double costcoPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[8].FormattedValue);
                double eBayListingPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[9].FormattedValue);
                double shipping = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[11].FormattedValue);

                double eBayFee = eBayListingPrice * 0.1;
                double payPalFee = eBayListingPrice * 0.029 + 0.3;
                double costcoCost = costcoPrice * 1.09 + shipping;

                double profit = eBayListingPrice * 1.09 - eBayFee - payPalFee - costcoCost;

                gvAdd.Rows[e.RowIndex].Cells[10].Value = Math.Round(profit, 2);
                bValueUpdateing = false;
            }
            //else if (e.ColumnIndex == 10)
            //{
            //    bValueUpdateing = true;
            //    double costcoPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[8].FormattedValue);
            //    double profit = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[10].Value);
            //    double shipping = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[11].FormattedValue);

            //    double listingPrice = profit + 0.3 + costcoPrice * 1.09 / (1.09 - 0.129);

            //    gvAdd.Rows[e.RowIndex].Cells[9].Value = Math.Round(listingPrice, 2);
            //    bValueUpdateing = false;
            //}

            daToAdd.Update(dtToAdd);
        }

        private void tpToAdd_Enter(object sender, EventArgs e)
        {
            lvCategory_Refresh();
            gvAdd_Refresh();
            bToAddTabEnteringGridRefreshed = true;
        }

        public void lvCategory_Refresh()
        {
            lvToAddCategory.Items.Clear();

            dl.connectionString = connectionString;
            List<Category> categories = dl.GetCategoryArray();

            List<string> category2s = new List<string>();

            foreach (Category catetory in categories)
            {
                if (catetory.Category1 + catetory.Category2 + catetory.Category3 + catetory.Category4 +
                    catetory.Category5 + catetory.Category6 + catetory.Category7 + catetory.Category8 == "")
                    continue;

                if (category2s.Contains(catetory.Category2))
                    continue;

                ListViewItem item = new ListViewItem();
                item.Checked = true;
                item.SubItems.Add(catetory.Category2);

                category2s.Add(catetory.Category2);

                this.lvToAddCategory.Items.Add(item);
            }

            chkToAddCategoryAll.Checked = true;
        }

        private void gvAdd_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = string.Empty;

            cn.Open();

            string st = string.Empty;

            foreach (DataGridViewRow row in gvAdd.Rows)
            {
                if (Convert.ToBoolean(row.Cells["AddSelect"].Value) == true)
                {
                    st += "'" + row.Cells["UrlNumber"].Value.ToString() + "',";
                }
            }


            st = st.Substring(0, st.Length - 1);

            sqlString = @"UPDATE eBay_ToAdd SET DeleteTime = GETDate() WHERE DeleteTime is NULL AND UrlNumber in (" + st + ")";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            gvAdd_Refresh();
        }

        private string RemoveSpecialCharacters(string inputString)
        {
            return inputString.Replace("®", "").Replace("™", "");
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string sqlString = string.Empty;

            cn.Open();

            // deal with resize
            foreach (DataGridViewRow row in gvAdd.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Resize"].Value) == true)
                {
                    string urlNumber = row.Cells["UrlNumber"].Value.ToString();
                    System.Drawing.Image image = System.Drawing.Image.FromFile(@"C:\temp\Screenshots\" + urlNumber + ".jpg");

                    string header = @"<div style='font-family:Arial; font-size:14px; color:#FF0000'><p><strong>No Sales Tax!</strong></p></div>";

                    string detail = "<p><img src='http://www.jasondingphotography.com/eBay/" + urlNumber + ".jpg' width='" +
                                image.Width.ToString() + "' height='" + image.Height.ToString() + "'/></p>";

                    string attach = @"<div style='font-family:Arial; font-size:12px; color:#5D5D5D'><p><strong>Payments</strong></p><p>We accept all major credit cards and instant transfers through PayPal. Payments must be received within 7 days after the end of sale.<br></p><p><strong>Shipping</strong></p><p>We are not able to deliver to P.O. Boxes, Freight Forwarders, or APO boxes. Shipping is available to the contiguous 48 United States only. We are required to ship purchased item to the address listed with Ebay. Merchandise will be shipped within 2 business days (NOT including weekend and holidays)<br></p><p><strong>Stock</strong></p><P>Items are shipped directly from the warehouse, (No Local Pick Ups!). Items are in stock at time of listing but are offered to other retailers &amp; occasionally go on back order or sell out. Please don't hesitate to ask us prior to ordering whether or not your item is in stock.<br>If for whatever reason your item is either sold out or on back order a full refund will be issued immediately. Please don't leave unfair feedback as I am upfront about the possibility of this happening.</P><P> <strong>Descriptions &amp; Errors:</strong></P><p>We attempt to describe products as accurately as possible. However, we do not warrant that product descriptions are accurate, complete, reliable, current, or error-free. In the event a product is listed at an incorrect price or with incorrect information due to a typographical error or an error in pricing or product information received from our suppliers, we shall have the right to refuse or cancel any orders placed for products listed at the incorrect price.</p><p> <strong>Return Policy</strong><br></p><p>We have a 14 day return policy on most items (excluding Non-Prescription Remedies, Food, Vitamins, Herbals &amp; Dietary Supplements,  Family Planning Items).Item must be returned in condition received (Tags & Packaging). Buyer is responsible for all return shipping cost.</p><p><strong>Expiration Date</strong><br></p><p>Satisfaction guaranteed. It will be fresh and not expired or near expiration. (Usually a few years out.)</p><p><strong>About Us</strong></p><p>We are dedicated to providing you the best customer service and a wonderful shopping experience on eBay. Please feel free to email us with any questions you might have. Your satisfaction is extremely important to us and is our highest priority. Please leave your positive feedback and help us build our business. We leave positive feedback for our customers upon shipment of the order.</p></div>";

                    detail = header + detail + attach;
                    //sqlString = @"UPDATE eBay_ToAdd SET DescriptionImageWidth = " + image.Width.ToString() +
                    //            ", DescriptionImageHeight = " + image.Height.ToString() + ", Details = '" + detail + "' WHERE UrlNumber = " + urlNumber + " AND DeleteTime is null";

                    sqlString = "UPDATE eBay_ToAdd SET DescriptionImageWidth = @_width, DescriptionImageHeight = @_height, Details = @_details WHERE UrlNumber = @_urlNumber AND DeleteTime is NULL";
                    cmd.CommandText = sqlString;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@_width", image.Width.ToString());
                    cmd.Parameters.AddWithValue("@_height", image.Height.ToString());
                    cmd.Parameters.AddWithValue("@_details", detail);
                    cmd.Parameters.AddWithValue("@_urlNumber", urlNumber);
                    cmd.ExecuteNonQuery();

                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential("jasondi1", "@Yueding00");
                        client.UploadFile("ftp://jasondingphotography.com/public_html//eBay/" + urlNumber + ".jpg", "STOR", @"C:\temp\Screenshots\" + urlNumber + ".jpg");
                    }
                }
            }


            List<Product> products = new List<Product>();

            Dictionary<string, int> excelColumnsDictionary = new Dictionary<string, int>();

            string st = string.Empty;

            foreach (DataGridViewRow row in gvAdd.Rows)
            {
                if (Convert.ToBoolean(row.Cells["AddSelect"].Value) == true)
                {
                    st += "'" + row.Cells["UrlNumber"].Value.ToString() + "',";
                }
            }
            st = st.Substring(0, st.Length - 1);

            // get products from DB
            sqlString = "SELECT * FROM eBay_ToAdd WHERE DeleteTime is NULL AND UrlNumber in (" + st + ")"; // WHERE shipping = 0.00 and Price < 100";

            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Name = Convert.ToString(reader["Name"]);
                    product.eBayName = Convert.ToString(reader["eBayName"]);
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
                    product.ImageOptions = Convert.ToString(reader["ImageOptions"]);
                    product.Options = Convert.ToString(reader["Options"]);
                    product.eBayCategoryID = Convert.ToString(reader["eBayCategoryID"]);
                    product.eBayReferencePrice = Convert.ToDecimal(reader["eBayReferencePrice"]);
                    product.eBayListingPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
                    product.DescriptionImageHeight = Convert.ToInt16(reader["DescriptionImageHeight"]);
                    product.DescriptionImageWidth = Convert.ToInt16(reader["DescriptionImageWidth"]);
                    product.TemplateName = Convert.ToString(reader["TemplateName"]);
                    product.Specifics = Convert.ToString(reader["Specifics"]);
                    product.eBayShipping = Convert.ToDecimal(reader["eBayShipping"]);

                    products.Add(product);
                }
            }

            reader.Close();
            cn.Close();

            // add to Excel file
            string sourceFileName = "FileExchange.csv";
            string destinFileName = @"C:\eBayApp\Upload\" + sourceFileName.Replace(".csv", "") + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
            File.Copy(@"C:\eBayApp\Files\Templates\FileExchange\" + sourceFileName, destinFileName);

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
            int j = 0;
            foreach (Product product in products)
            {
                string relationshipDetails = string.Empty;
                List<string> relationshipDetailsArray = new List<string>();
                List<string> imageOptionArray = new List<string>();

                if (!string.IsNullOrEmpty(product.Options))
                {
                    GenerateVariations(product.Options, product.ImageOptions, out relationshipDetails, out relationshipDetailsArray, out imageOptionArray);
                }

                if (!string.IsNullOrEmpty(product.Specifics)/* && !bColumnCreated*/)
                {
                    foreach (string spec in product.Specifics.Split(','))
                    {
                        //if (GetColumnIndex(ref oSheet, "C:" + spec) == -1)
                        if (!excelColumnsDictionary.ContainsKey("C:" + spec))
                        {
                            oSheet.Cells[1, 50 + j] = "C:" + spec;
                            excelColumnsDictionary.Add("C:" + spec, 50 + j);
                            j++;
                        }
                    }

                    //bColumnCreated = true;
                }

                //oSheet.Cells[i, 1] = "VerifyAdd";
                oSheet.Cells[i, 1] = "Add";
                oSheet.Cells[i, 2] = product.eBayCategoryID;
                oSheet.Cells[i, 3] = product.eBayName;
                oSheet.Cells[i, 5] = product.Details.Replace("xxx", product.eBayListingPrice.ToString("C", new CultureInfo("en-US")));
                oSheet.Cells[i, 6] = "1000";

                //if (string.IsNullOrEmpty(product.Options))
                //{
                //    //string imageLinks = "";
                //    //string imageLink = product.ImageLink;
                //    //for (int n = 1; n <= product.NumberOfImage; n++)
                //    //{
                //    //    imageLinks += imageLink + n.ToString() + ".jpg|";
                //    //}

                //    //imageLinks = imageLinks.Substring(0, imageLinks.Length - 1);
                //    oSheet.Cells[i, 7] = product.ImageLink;
                //}
                //else
                //{

                //}
                oSheet.Cells[i, 7] = product.ImageLink; //@"http://www.jasondingphotography.com/eBay/" + product.UrlNumber + "_productimage.jpg";
                oSheet.Cells[i, 8] = "10";
                oSheet.Cells[i, 9] = "FixedPrice";
                oSheet.Cells[i, 10] = product.eBayListingPrice;
                oSheet.Cells[i, 12] = "GTC";
                oSheet.Cells[i, 13] = "1";
                oSheet.Cells[i, 14] = "AL";
                oSheet.Cells[i, 16] = "1";
                oSheet.Cells[i, 17] = "zjding@outlook.com";
                oSheet.Cells[i, 22] = "Flat";
                oSheet.Cells[i, 23] = "ShippingMethodStandard";
                oSheet.Cells[i, 24] = Math.Round(product.eBayShipping, 2);
                oSheet.Cells[i, 25] = "1";
                oSheet.Cells[i, 31] = "1";
                oSheet.Cells[i, 33] = "ReturnsNotAccepted";
                oSheet.Cells[i, 43] = relationshipDetails;

                if (!string.IsNullOrEmpty(product.Specifics))
                {
                    foreach (string spec in product.Specifics.Split(','))
                    {
                        //int k = GetColumnIndex(ref oSheet, "C:" + spec);
                        int k = excelColumnsDictionary["C:" + spec];

                        if (spec.Contains("Size Type"))
                            oSheet.Cells[i, k] = "Regular";
                        else if (spec.Contains("Style"))
                            oSheet.Cells[i, k] = "Western";
                        else if (spec.Contains("Brand"))
                            oSheet.Cells[i, k] = product.Name.Split(' ').First();
                        else if (spec.Contains("Inseam"))
                            oSheet.Cells[i, k] = "32";
                        else if (spec.Contains("Size"))
                        {
                            string s = relationshipDetails.Contains('|') ? relationshipDetails.Split('|')[1].Replace("Size=", "") : relationshipDetails.Replace("Size=", "");
                            oSheet.Cells[i, k] = s.Length >= 65 ? s.Substring(0, 64) : s;
                        }
                    }
                }

                i++;

                foreach (string relationshipDetail in relationshipDetailsArray)
                {
                    // Relationship
                    oSheet.Cells[i, 42] = "Variation";
                    // RelationshipDetails
                    oSheet.Cells[i, 43] = relationshipDetail;
                    // *C:Size (Men's)
                    string size = relationshipDetail.Contains('|') ? relationshipDetail.Split('|')[1].Replace("Size=", "") : relationshipDetail.Replace("Size=", "");
                    foreach (string spec in product.Specifics.Split(','))
                    {
                        if (!spec.Contains("Size Type") && spec.Contains("Size"))
                            oSheet.Cells[i, excelColumnsDictionary["C:" + spec]/*GetColumnIndex(ref oSheet, "C:" + spec)*/] = size;
                    }
                    //// C:Color
                    string color = relationshipDetail.Split('|')[0].Replace("Color=", "");
                    //oSheet.Cells[i, 17] = color;
                    // PicURL
                    string colorImage = imageOptionArray.FirstOrDefault(s => s.Contains(color + "="));
                    oSheet.Cells[i, 7] = colorImage;
                    // *StartPrice
                    oSheet.Cells[i, 10] = product.eBayListingPrice;
                    // *Quantity
                    oSheet.Cells[i, 8] = "1";

                    i++;
                }

            }

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\eBayApp\\Curl\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);
        }

        private static void GenerateVariationChange(string oldOptions, string newOptions, string imageOptionsString,
                                             out string relationshipDetails,
                                             out List<string> imageOptionArray,
                                             out List<string> newRelationshipDetailsArray,
                                             out List<string> discontinuedRelationshipDetailsArray
                                             )
        {
            string oldRelationshipDetails = string.Empty;
            string newRelationshipDetails = string.Empty;
            List<string> oldRelationshipDetailsArray = new List<string>();
            newRelationshipDetailsArray = new List<string>();
            List<string> oldImageOptionArray = new List<string>();
            List<string> newImageOptionArray = new List<string>();

            discontinuedRelationshipDetailsArray = new List<string>();
            imageOptionArray = new List<string>();
            relationshipDetails = string.Empty;

            GenerateVariations(oldOptions, imageOptionsString, out oldRelationshipDetails, out oldRelationshipDetailsArray, out oldImageOptionArray);
            GenerateVariations(newOptions, imageOptionsString, out newRelationshipDetails, out newRelationshipDetailsArray, out newImageOptionArray);

            discontinuedRelationshipDetailsArray = oldRelationshipDetailsArray.Except(newRelationshipDetailsArray).ToList();

            imageOptionArray = oldImageOptionArray;
            relationshipDetails = oldRelationshipDetails;

            if (oldRelationshipDetails != newRelationshipDetails)
            {
                List<string> _newRelationshipDetailsArray = new List<string>();

                foreach (string newItem in newRelationshipDetailsArray)
                {
                    if (newItem.Contains("|"))
                    {
                        string _color = newItem.Split('|')[0].Replace("Color=", "");
                        string _size = newItem.Split('|')[1].Replace("Size=", "");

                        if (oldRelationshipDetails.Contains(_color) && oldRelationshipDetails.Contains(_size))
                            _newRelationshipDetailsArray.Add(newItem);
                    }
                    else
                    {
                        string _size = newItem.Replace("Size=", "");

                        if (oldRelationshipDetails.Contains(_size))
                            _newRelationshipDetailsArray.Add(newItem);
                    }
                }

                newRelationshipDetailsArray = _newRelationshipDetailsArray;

                List<string> _discontinuedRelationshipDetailsArray = new List<string>();

                foreach (string discontinuedItem in discontinuedRelationshipDetailsArray)
                {
                    if (discontinuedItem.Contains("|"))
                    {
                        string _color = discontinuedItem.Split('|')[0].Replace("Color=", "");
                        string _size = discontinuedItem.Split('|')[1].Replace("Size=", "");

                        if (oldRelationshipDetails.Contains(_color) && oldRelationshipDetails.Contains(_size))
                            _discontinuedRelationshipDetailsArray.Add(discontinuedItem);
                    }
                    else
                    {
                        string _size = discontinuedItem.Replace("Size=", "");

                        if (oldRelationshipDetails.Contains(_size))
                            _discontinuedRelationshipDetailsArray.Add(discontinuedItem);
                    }
                }

                discontinuedRelationshipDetailsArray = _discontinuedRelationshipDetailsArray;
            }
        }

        private static void GenerateVariations(string inputString, string imageOptionString, out string relationshipDetails, out List<string> relationshipDetailsArray, out List<string> imageOptionArray)
        {
            relationshipDetails = string.Empty;
            relationshipDetailsArray = new List<string>();
            imageOptionArray = new List<string>();

            string _Color = "Color=";
            string _Size = "Size=";

            if (inputString.Contains(":"))
            {
                List<string> sizeList = new List<string>();

                foreach (var colorString in inputString.Split('|').ToList())
                {
                    string color = colorString.Split(':')[0];
                    string sizes = colorString.Split(':')[1];

                    _Color += color + ";";

                    sizeList = sizeList.Union(sizes.Split(';').ToList()).ToList();

                    foreach (var size in sizes.Split(';').ToList())
                    {
                        relationshipDetailsArray.Add("Color=" + color + "|" + "Size=" + size);
                    }
                }

                _Color = _Color.Substring(0, _Color.Length - 1);

                sizeList.Sort();

                foreach (var size in sizeList)
                {
                    _Size += size + ";";
                }

                _Size = _Size.Substring(0, _Size.Length - 1);

                relationshipDetails = _Color + "|" + _Size;
            }
            else
            {
                relationshipDetails = "Size=" + inputString;

                foreach (string size in inputString.Split(';').ToList())
                {
                    relationshipDetailsArray.Add("Size=" + size);
                }
            }

            if (imageOptionString.Contains("~"))
            {
                imageOptionArray = imageOptionString.Split('~').ToList();
            }
            else
            {
                imageOptionArray.Add(imageOptionString);
            }
        }

        private void gvAdd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.RowIndex == gvAdd.Rows.Count - 1)
                return;

            if (e.ColumnIndex == 8)
            {
                string url = gvAdd.Rows[e.RowIndex].Cells[13].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }
            else if (e.ColumnIndex == 6)
            {
                string fileName = gvAdd.Rows[e.RowIndex].Cells[16].FormattedValue.ToString();

                Process.Start(@"C:\temp\Screenshots\" + fileName + ".jpg");
            }
            else if (e.ColumnIndex == 14)
            {
                string url = gvAdd.Rows[e.RowIndex].Cells[17].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }
        }

        private void gvAdd_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex + 1 == this.gvAdd.Rows.Count)
                return;

            if (e.ColumnIndex == 0)
            {
                double eBayReferencePrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[14].Value);
                double eBayListingPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[9].Value);
                if (eBayListingPrice < eBayReferencePrice)
                {
                    gvAdd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
            else if (gvAdd.Columns[e.ColumnIndex].Name == "ToAddImage")
            {
                string imageUrl = string.Empty;

                imageUrl = (this.gvAdd.Rows[e.RowIndex].Cells[5]).FormattedValue.ToString();

                if (imageUrl != "")
                {
                    e.Value = eBayFrontEnd.GetImageFromUrl(imageUrl.Split('|')[0]);
                }
                else
                {
                    e.Value = null;
                }
            }
            else if (gvAdd.Columns[e.ColumnIndex].Name == "Profit")
            {
                double costcoPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[8].Value);
                double eBayListingPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[9].Value);
                double eBayShippingPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[10].Value == DBNull.Value ? 0 : this.gvAdd.Rows[e.RowIndex].Cells[10].Value);
                double shipping = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[11].Value);

                double eBayFee = (eBayListingPrice + eBayShippingPrice) * 0.1;
                double payPalFee = (eBayListingPrice + eBayShippingPrice) * 0.029 + 0.3;
                double costcoCost = (costcoPrice + shipping) * 1.09;

                //double profit = eBayListingPrice * 1.09 - eBayFee - payPalFee - costcoCost;

                double profit = eBayListingPrice + eBayShippingPrice - eBayFee - payPalFee - costcoCost;

                e.Value = Math.Round(profit, 2);
            }
            else if (gvAdd.Columns[e.ColumnIndex].Name == "eBayReferencePriceProfit")
            {
                double costcoPrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[8].Value);
                double eBayReferencePrice = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[14].Value);
                double shipping = Convert.ToDouble(this.gvAdd.Rows[e.RowIndex].Cells[11].Value);

                double eBayFee = eBayReferencePrice * 0.1;
                double payPalFee = eBayReferencePrice * 0.029 + 0.3;
                double costcoCost = costcoPrice * 1.09 + shipping;

                double profit = eBayReferencePrice * 1.09 - eBayFee - payPalFee - costcoCost;

                e.Value = Math.Round(profit, 2);
            }
        }


        //// tpToModify

        //private void btnToChangeUpload_Click(object sender, EventArgs e)
        //{
        //    UpdatePriceChangeListings();
        //    UpdateOptionChangeListings();

        //    gvChange_Refresh();
        //}

        //SqlCommand cmdToChange;
        //SqlDataAdapter daToChange;
        //SqlCommandBuilder cmbToChange;
        //DataSet dsToChange;
        //DataTable dtToChange;

        //public void gvChange_Refresh()
        //{
        //    string sqlString = @"SELECT ID, Name, eBayItemNumber, eBayNewListingPrice, NewImageOptions, NewOptions, eBayReferencePrice, eBayOldListingPrice,  CostcoOldPrice, CostcoNewPrice, OldOptions,  Url, ImageLink, Thumb
        //                         FROM eBay_ToChange 
        //                         WHERE DeleteTime is NULL
        //                         Order by InsertTime DESC";

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    cmdToChange = new SqlCommand(sqlString, connection);
        //    daToChange = new SqlDataAdapter(cmdToChange);
        //    cmbToChange = new SqlCommandBuilder(daToChange);
        //    dsToChange = new DataSet();
        //    daToChange.Fill(dsToChange, "tbToChange");
        //    dtToChange = dsToChange.Tables["tbToChange"];
        //    connection.Close();

        //    gvChange.DataSource = dsToChange.Tables["tbToChange"];
        //    gvChange.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        //    gvChange.Columns["ID"].Visible = false;
        //    gvChange.Columns["ImageLink"].Visible = false;
        //    gvChange.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["eBayItemNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["eBayReferencePrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["eBayOldListingPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["eBayNewListingPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["CostcoOldPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["CostcoNewPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvChange.Columns["OldOptions"].Width = 150;
        //    gvChange.Columns["NewOptions"].Width = 150;
        //    gvChange.Columns["Url"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //}

        //private void gvChange_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (gvChange.Columns[e.ColumnIndex].Name == "ToModifyImage")
        //    {
        //        string imageUrl = string.Empty;

        //        imageUrl = (this.gvChange.Rows[e.RowIndex].Cells[15]).FormattedValue.ToString();

        //        if (imageUrl != "")
        //        {
        //            e.Value = eBayFrontEnd.GetImageFromUrl(imageUrl);
        //        }
        //        else
        //        {
        //            e.Value = null;
        //        }
        //    }
        //}

        //private void chkModifyAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    foreach (DataGridViewRow row in gvChange.Rows)
        //    {
        //        row.Cells["AddSelect"].Value = chkModifyAll.Checked;
        //    }
        //}

        //private void gvChange_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    daToChange.Update(dtToChange);
        //}

        //private void btnToChangeDelete_Click(object sender, EventArgs e)
        //{
        //    for (int i = gvChange.Rows.Count - 1; i >= 0; i--)
        //    {
        //        if (Convert.ToBoolean(gvChange.Rows[i].Cells["ToChangeSelect"].Value) == true)
        //        {
        //            gvChange.Rows.RemoveAt(i);

        //        }
        //    }

        //    daToChange.Update(dtToChange);
        //}

        //private void UpdateOptionChangeListings()
        //{
        //    List<ProductUpdate> products = new List<ProductUpdate>();

        //    //// get products from DB
        //    //SqlConnection cn = new SqlConnection(connectionString);
        //    //SqlCommand cmd = new SqlCommand();
        //    //cmd.Connection = cn;

        //    //string sqlString = @"SELECT c.eBayItemNumber, c.NewOptions, c.NewImageOptions, l.eBayListingPrice
        //    //                    FROM eBay_ToChange c, eBay_CurrentListings l
        //    //                    where c.eBayItemNumber = l.eBayItemNumber
        //    //                    and c.NewOptions is not null 
        //    //                    and c.DeleteTime is null";

        //    //cn.Open();
        //    //cmd.CommandText = sqlString;
        //    //SqlDataReader reader = cmd.ExecuteReader();
        //    //if (reader.HasRows)
        //    //{
        //    //    while (reader.Read())
        //    //    {
        //    //        ProductUpdate p = new ProductUpdate();
        //    //        p.eBayItemNumbr = Convert.ToString(reader["eBayItemNumber"]);
        //    //        p.NewOptions = Convert.ToString(reader["NewOptions"]);
        //    //        p.NewPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
        //    //        p.NewImageOptions = Convert.ToString(reader["NewImageOptions"]);
        //    //        products.Add(p);
        //    //    }
        //    //}

        //    //reader.Close();
        //    //cn.Close();

        //    string eBayItemNumbers = string.Empty;

        //    foreach (DataGridViewRow row in gvChange.Rows)
        //    {
        //        if (Convert.ToBoolean(row.Cells["ToChangeSelect"].Value) == true && row.Cells["NewOptions"].Value.ToString().Trim() != "")
        //        {
        //            ProductUpdate p = new ProductUpdate();
        //            p.eBayItemNumbr = Convert.ToString(row.Cells["eBayItemNumber"].Value);
        //            p.NewOptions = Convert.ToString(row.Cells["NewOptions"].Value);
        //            p.OldOptions = Convert.ToString(row.Cells["OldOptions"].Value);
        //            p.NewImageOptions = Convert.ToString(row.Cells["NewImageOptions"].Value);
        //            p.NewPrice = Convert.ToDecimal(row.Cells["eBayOldListingPrice"].Value);
        //            products.Add(p);

        //            eBayItemNumbers += "'" + p.eBayItemNumbr + "',";
        //        }
        //    }

        //    if (products.Count == 0)
        //        return;

        //    eBayItemNumbers = eBayItemNumbers.Substring(0, eBayItemNumbers.Length - 1);

        //    UploadOptionChanges(products);

        //    SqlConnection cn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cn;

        //    cn.Open();

        //    string sqlString = "UPDATE eBay_ToChange SET DeleteTime = GETDATE() WHERE DeleteTime is null AND eBayItemNumber in (" + eBayItemNumbers + ")";

        //    cmd.CommandText = sqlString;
        //    cmd.ExecuteNonQuery();
        //    cn.Close();
        //}

        public static void UploadOptionChanges(List<ProductUpdate> products)
        {
            if (products.Count == 0)
                return;

            // add to Excel file
            string sourceFileName = @"C:\eBayApp\Files\Templates\FileExchange\" + "FileExchangeOptionsRevise.csv";
            string destinFileName = @"C:\eBayApp\Upload\" + "FileExchangeOptionsRevise-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
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

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString;

            string ebayUrlNumbers = string.Empty;

            int i = 2;
            foreach (ProductUpdate product in products)
            {
                string relationshipDetails = string.Empty;
                List<string> newRelationshipDetailsArray = new List<string>();
                List<string> imageOptionArray = new List<string>();
                List<string> discontinuedRelationshipDetailsArray = new List<string>();

                GenerateVariationChange(product.OldOptions, product.NewOptions, product.NewImageOptions,
                                        out relationshipDetails,
                                        out imageOptionArray,
                                        out newRelationshipDetailsArray,
                                        out discontinuedRelationshipDetailsArray
                                        );

                //GenerateVariations(product.NewOptions, product.NewImageOptions, out relationshipDetails, out relationshipDetailsArray, out imageOptionArray);

                oSheet.Cells[i, 1].value = "Revise";
                oSheet.Cells[i, 2].NumberFormat = "#";
                oSheet.Cells[i, 2].value = product.eBayItemNumbr;
                oSheet.Cells[i, 4].value = relationshipDetails;

                i++;

                foreach (string newRelationshipDetail in newRelationshipDetailsArray)
                {
                    // Relationship
                    oSheet.Cells[i, 3] = "Variation";
                    // RelationshipDetails
                    oSheet.Cells[i, 4] = newRelationshipDetail;

                    //// C:Color
                    string color = newRelationshipDetail.Split('|')[0].Replace("Color=", "");
                    //oSheet.Cells[i, 17] = color;
                    // PicURL
                    string colorImage = imageOptionArray.FirstOrDefault(s => s.Contains(color + "="));
                    oSheet.Cells[i, 7] = colorImage;

                    // *StartPrice
                    oSheet.Cells[i, 6] = product.NewPrice;
                    // *Quantity
                    oSheet.Cells[i, 5] = "1";

                    i++;
                }


                foreach (string discontinuedRelationshipDetail in discontinuedRelationshipDetailsArray)
                {
                    // Relationship
                    oSheet.Cells[i, 3] = "Variation";
                    // RelationshipDetails
                    oSheet.Cells[i, 4] = discontinuedRelationshipDetail;

                    //// C:Color
                    string color = discontinuedRelationshipDetail.Split('|')[0].Replace("Color=", "");
                    //oSheet.Cells[i, 17] = color;
                    // PicURL
                    string colorImage = imageOptionArray.FirstOrDefault(s => s.Contains(color + "="));
                    oSheet.Cells[i, 7] = colorImage;

                    // *StartPrice
                    oSheet.Cells[i, 6] = product.NewPrice;
                    // *Quantity
                    oSheet.Cells[i, 5] = "0";

                    i++;
                }

                sqlString = @" UPDATE eBay_CurrentListings SET PendingChange = '1', CostcoOptions = '" + product.NewOptions +
                            "', ImageOptions = '" + product.NewImageOptions + "'" +
                            ", eBayListingPrice = " + product.NewPrice.ToString() +
                            " WHERE eBayItemNumber = '" + product.eBayItemNumbr + "' AND DeleteDT is null";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();

                ebayUrlNumbers += product.eBayItemNumbr + "~";
            }

            ebayUrlNumbers = ebayUrlNumbers.Substring(0, ebayUrlNumbers.Length - 1);
            DateTime dt = System.DateTime.Now.AddMinutes(10);



            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);

            // task
            string taskName = ebayUrlNumbers + "-" + dt.ToString("yyyyMMddHHmmss");

            TaskService ts = new TaskService();
            TaskDefinition td = ts.NewTask();
            td.Triggers.Add(new TimeTrigger() { StartBoundary = dt });
            td.Actions.Add(new ExecAction(@"C:\Users\Jason Ding\Documents\visual studio 2015\Projects\TaskHandler\TaskHandler\bin\Debug\TaskHandler.exe", taskName, null));
            ts.RootFolder.RegisterTaskDefinition(taskName, td);
            ts.Dispose();

            sqlString = @" INSERT INTO Tasks (TaskName) VALUES ('" + taskName + "')";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        //private void UpdatePriceChangeListings()
        //{
        //    List<ProductUpdate> products = new List<ProductUpdate>();

        //    //string eBayItemNumbers = string.Empty;

        //    foreach (DataGridViewRow row in gvChange.Rows)
        //    {
        //        if (Convert.ToBoolean(row.Cells["ToChangeSelect"].Value) == true && row.Cells["eBayNewListingPrice"].Value.ToString().Trim() != "")
        //        {
        //            ProductUpdate p = new ProductUpdate();
        //            p.eBayItemNumbr = Convert.ToString(row.Cells["eBayItemNumber"].Value);
        //            p.NewPrice = Convert.ToDecimal(row.Cells["eBayNewListingPrice"].Value);
        //            products.Add(p);

        //            //eBayItemNumbers += "'" + p.eBayItemNumbr + "',";
        //        }
        //    }

        //    if (products.Count == 0)
        //        return;

        //    //eBayItemNumbers = eBayItemNumbers.Substring(0, eBayItemNumbers.Length - 1);

        //    UploadPriceChange(products);

        //    //SqlConnection cn = new SqlConnection(connectionString);
        //    //SqlCommand cmd = new SqlCommand();
        //    //cmd.Connection = cn;

        //    //string sqlString = "UPDATE eBay_ToChange SET DeleteTime = GETDATE() WHERE DeleteTime is null AND eBayItemNumber in (" + eBayItemNumbers + ")";

        //    //cmd.CommandText = sqlString;
        //    //cmd.ExecuteNonQuery();
        //    //cn.Close();
        //}

        public static void UploadPriceChange(List<ProductUpdate> products)
        {
            if (products.Count == 0)
                return;

            // add to Excel file
            string sourceFileName = @"C:\eBayApp\Files\Templates\FileExchange\" + "FileExchangeRevise.csv";
            string destinFileName = @"C:\eBayApp\Upload\" + "FileExchangeRevise-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
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

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString;

            string ebayUrlNumbers = string.Empty;

            int i = 2;
            foreach (ProductUpdate product in products)
            {
                oSheet.Cells[i, 1].value = "Revise";
                oSheet.Cells[i, 2].NumberFormat = "#";
                oSheet.Cells[i, 2].value = product.eBayItemNumbr;
                oSheet.Cells[i, 3].value = product.NewPrice;

                sqlString = @" UPDATE eBay_CurrentListings SET PendingChange = '1', eBayListingPrice = " + product.NewPrice.ToString() + " WHERE eBayItemNumber = '" + product.eBayItemNumbr + "' AND DeleteDT is null";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();

                i++;

                ebayUrlNumbers += product.eBayItemNumbr + "~";
            }

            ebayUrlNumbers = ebayUrlNumbers.Substring(0, ebayUrlNumbers.Length - 1);
            DateTime dt = System.DateTime.Now.AddMinutes(10);



            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\ebay\\Upload\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);

            // task
            string taskName = ebayUrlNumbers + "-" + dt.ToString("yyyyMMddHHmmss");

            TaskService ts = new TaskService();
            TaskDefinition td = ts.NewTask();
            td.Triggers.Add(new TimeTrigger() { StartBoundary = dt });
            td.Actions.Add(new ExecAction(@"C:\Users\Jason Ding\Documents\visual studio 2015\Projects\TaskHandler\TaskHandler\bin\Debug\TaskHandler.exe", taskName, null));
            ts.RootFolder.RegisterTaskDefinition(taskName, td);
            ts.Dispose();

            sqlString = @" INSERT INTO Tasks (TaskName) VALUES ('" + taskName + "')";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();
        }


        //// To delete

        //SqlCommand cmdToDelete;
        //SqlDataAdapter daToDelete;
        //SqlCommandBuilder cmbToDelete;
        //DataSet dsToDelete;
        //DataTable dtToDelete;

        //public void gvDelete_Refresh()
        //{
        //    string sqlString = @"SELECT ID, Name, CostcoUrl, CostcoUrlNumber, eBayItemNumber
        //                         FROM eBay_ToRemove 
        //                         WHERE DeleteTime is NULL
        //                         Order by InsertTime DESC";

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    cmdToDelete = new SqlCommand(sqlString, connection);
        //    daToDelete = new SqlDataAdapter(cmdToDelete);
        //    cmbToDelete = new SqlCommandBuilder(daToDelete);
        //    dsToDelete = new DataSet();
        //    daToDelete.Fill(dsToDelete, "tbToDelete");
        //    dtToDelete = dsToDelete.Tables["tbToDelete"];
        //    connection.Close();

        //    gvDelete.DataSource = dsToDelete.Tables["tbToDelete"];
        //    gvDelete.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        //    //gvDelete.Columns["Select"].Width = 20;
        //    gvDelete.Columns["ID"].Visible = false;
        //    gvDelete.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvDelete.Columns["CostcoUrlNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvDelete.Columns["eBayItemNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    gvDelete.Columns["CostcoUrl"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        //}

        //private void tpEbayToDelete_Enter(object sender, EventArgs e)
        //{
        //    gvDelete_Refresh();
        //}

        //private void gvDelete_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    //if (e.RowIndex < 0) return;

        //    //if (e.ColumnIndex == 0)
        //    //{
        //    //    foreach (DataGridViewRow row in gvDelete.Rows)
        //    //    {
        //    //        if (row.Cells["ToDeleteSelect"].Value != null && ((bool)row.Cells["ToDeleteSelect"].Value) == true)
        //    //        {
        //    //            gvDelete.Rows[row.Index].Selected = true;
        //    //            gvDelete.Rows[row.Index].DefaultCellStyle.BackColor = Color.Yellow;
        //    //        }
        //    //        else
        //    //        {
        //    //            gvDelete.Rows[row.Index].Selected = false;
        //    //            gvDelete.Rows[row.Index].DefaultCellStyle.BackColor = Color.White;
        //    //        }

        //    //    }
        //    //}
        //}

        //private void chkDeleteAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    foreach (DataGridViewRow row in gvDelete.Rows)
        //    {
        //        row.Cells["ToDeleteSelect"].Value = chkDeleteAll.Checked;
        //    }
        //}

        //private void btnToDeleteDelete_Click(object sender, EventArgs e)
        //{
        //    for (int i = gvDelete.Rows.Count - 1; i >= 0; i--)
        //    {
        //        if (Convert.ToBoolean(gvDelete.Rows[i].Cells["ToDeleteSelect"].Value) == true)
        //        {
        //            gvDelete.Rows.RemoveAt(i);

        //        }
        //    }

        //    daToDelete.Update(dtToDelete);
        //}

        //private void btnToDeleteUpload_Click(object sender, EventArgs e)
        //{
        //    List<ProductUpdate> products = new List<ProductUpdate>();

        //    foreach (DataGridViewRow row in gvDelete.Rows)
        //    {
        //        if (Convert.ToBoolean(row.Cells["ToDeleteSelect"].Value) == true)
        //        {
        //            ProductUpdate p = new ProductUpdate();
        //            p.eBayItemNumbr = row.Cells["eBayItemNumber"].Value.ToString();

        //            products.Add(p);
        //        }
        //    }

        //    // add to Excel file
        //    //UploadDelete(products);

        //    //gvDelete_Refresh();
        //}

        public static void UploadDelete(string eBayItemNumbers)
        {
            // update db
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @" UPDATE eBay_CurrentListings SET PendingChange = '2' WHERE eBayItemNumber in (" + eBayItemNumbers + ") AND DeleteDT is null";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();

            // upload file
            string sourceFileName = @"C:\eBayApp\Files\Templates\FileExchange\" + "FileExchangeRemove.csv";
            string destinFileName = @"C:\eBayApp\Upload\" + "FileExchangeRemove-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
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
            foreach (string itemNumber in eBayItemNumbers.Split(','))
            {
                oSheet.Cells[i, 1].value = "End";
                oSheet.Cells[i, 2].NumberFormat = "#";
                oSheet.Cells[i, 2].value = itemNumber.Replace("'", "");
                oSheet.Cells[i, 3].value = "NotAvailable";

                i++;
            }

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();

            string command = "c:\\eBayApp\\Curl\\curl -k -o results.txt -F \"token=AgAAAA**AQAAAA**aAAAAA**wsb+Vg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AAloWmAZSCqQudj6x9nY+seQ**GDsDAA**AAMAAA**+d5Az1uG7de9cl6CsLoWYmujVawlxpNTk3Z7fQAMcA+zNQARScFIFVa8AzViTabPRPPq0x5huX5ktlxIAB6kDU3BO4iyuhXEMnTb6DmAHtnORkOlKxD5hZc0pMRCkFjfiyuzc3z+r2XS6tpdFXiRJVx1LmhNp01RUOHjHBj/wVWw6W64u821lyaBn6tcnvHw8lJo8Hfp1f3AtoXdASN+AgB800zCeGNQ+zxD9kVN1cY5ykpeJ70UK0RbAAE3OEXffFurI7BbpO2zv0PHFM3Md5hqnAC4BE54Tr0och/Vm98GPeeivQ4zIsxEL+JwvvpxigszMbeGG0E/ulKvnHT1NkVtUhh7QXhUkEqi9sq3XI/55IjLzOk61iIUiF8vgV1HmoGqbkhIpafJhqotV5HyxVW38PKplihl7mq37aGyx1bRF8XqnJomwLCPOazSf57iTKz7EQlLL9PJ8cRfnJ/TCJUT3EX9Xcu2EIzRFQXapkAU2rY6+KOr3jXwk5Q+VvbFXKF5C9xJmJnXWa+oXSUH4bFor64fB7hdR/k49528rO+/vSZah1Nte+Bbmsai3O2EDZfXQLFGZtinp5JDVXvbmP0vSr+yxX8WBf/T0RHIv6zzEmSo/ZevkJJD4wTRlfh4FIva3P42JU0P4OTUkeff6mXclzWH9/Bedbq9trenh3hZg9Ah4f6NAT99m48YqVvSjBeEotF5kLRoBdz2V3v8RELskReSPDCYJol4g6X89uNwS/iRGZCRkx31K37FQGSR\" -F file=@" + destinFileName + " https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";

            System.Diagnostics.Process.Start("CMD.exe", "/c" + command);

            //sqlString = "UPDATE eBay_ToRemove SET DeleteTime = GETDATE()"; // WHERE shipping = 0.00 and Price < 100";

            //cmd.CommandText = sqlString;
            //cmd.ExecuteNonQuery();
            //cn.Close();
        }


        // Listing

        SqlCommand cmdEBayListing;
        SqlDataAdapter daEBayListing;
        SqlCommandBuilder cmbEBayListing;
        DataSet dsEBayListing;
        DataTable dtEBayListing;

        public void gvListing_Refresh()
        {
            string sqlString = @"SELECT Name as ProductName, eBayItemNumber, CostcoPrice, eBayReferencePrice, '' as ReferencePriceProfit, eBayListingPrice, '' as Profit,   
                                    CostcoOptions, ImageLink, ID, eBayEndTime, CostcoUrl, Thumb, eBayUrl, PendingChange, eBayReferenceUrl, CostcoUrlNumber
                                 FROM eBay_CurrentListings 
                                 WHERE DeleteDT is NULL
                                 Order by PendingChange desc";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdEBayListing = new SqlCommand(sqlString, connection);
            daEBayListing = new SqlDataAdapter(cmdEBayListing);
            cmbEBayListing = new SqlCommandBuilder(daEBayListing);
            dsEBayListing = new DataSet();
            daEBayListing.Fill(dsEBayListing, "tbEBayListing");
            dtEBayListing = dsEBayListing.Tables["tbEBayListing"];
            connection.Close();

            gvListing.DataSource = dsEBayListing.Tables["tbEBayListing"];
            gvListing.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gvListing.Columns["ProductName"].Width = 300;
            gvListing.Columns["CostcoPrice"].Width = 80;
            gvListing.Columns["eBayReferencePrice"].Width = 80;
            gvListing.Columns["eBayListingPrice"].Width = 80;
            gvListing.Columns["ReferencePriceProfit"].Width = 80;
            gvListing.Columns["Profit"].Width = 80;
            gvListing.Columns["CostcoOptions"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gvListing.Columns["eBayItemNumber"].Visible = false;
            gvListing.Columns["ImageLink"].Visible = false;
            gvListing.Columns["ID"].Visible = false;
            gvListing.Columns["eBayEndTime"].Visible = false;
            gvListing.Columns["CostcoUrl"].Visible = false;
            gvListing.Columns["Thumb"].Visible = false;
            gvListing.Columns["eBayUrl"].Visible = false;
            gvListing.Columns["PendingChange"].Visible = false;
            gvListing.Columns["eBayReferenceUrl"].Visible = false;
            gvListing.Columns["CostcoUrlNumber"].Visible = false;

            gvListing.ClearSelection();
            gvListing.CurrentCell = gvListing.Rows[gvListing.Rows.Count - 1].Cells[0];

            gvListing.Rows[gvListing.Rows.Count - 1].Selected = true;


        }

        private void tpCurrentListing_Enter(object sender, EventArgs e)
        {
            gvListing_Refresh();
        }

        private void gvListing_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        private void gvListing_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= gvListing.Rows.Count - 1)
                return;

            if (e.ColumnIndex == 0)
            {
                if (gvListing.Rows[e.RowIndex].Cells["PendingChange"].Value.ToString() == "2")
                {
                    gvListing.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 224);
                }
                else if (gvListing.Rows[e.RowIndex].Cells["PendingChange"].Value.ToString() == "1")
                    gvListing.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 224);
                else
                    gvListing.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            else if (gvListing.Columns[e.ColumnIndex].Name == "Status")
            {
                if (gvListing.Rows[e.RowIndex].Cells["PendingChange"].Value.ToString() == "2")
                    e.Value = "PendingDelete";
                else if (gvListing.Rows[e.RowIndex].Cells["PendingChange"].Value.ToString() == "1")
                    e.Value = "PendingModify";
            }
            else if (gvListing.Columns[e.ColumnIndex].Name == "ReferencePriceProfit")
            {
                double costcoPrice = Convert.ToDouble(gvListing.Rows[e.RowIndex].Cells[5].FormattedValue);
                double eBayReferencePrice = Convert.ToDouble(gvListing.Rows[e.RowIndex].Cells[6].FormattedValue);
                e.Value = (eBayReferencePrice * 1.09 - eBayReferencePrice * 0.119 - 0.3 - costcoPrice * 1.09).ToString();
            }
            else if (gvListing.Columns[e.ColumnIndex].Name == "Profit")
            {
                double costcoPrice = Convert.ToDouble(gvListing.Rows[e.RowIndex].Cells[5].FormattedValue);
                double listingPrice = Convert.ToDouble(gvListing.Rows[e.RowIndex].Cells[8].FormattedValue);
                e.Value = (listingPrice * 1.09 - listingPrice * 0.119 - 0.3 - costcoPrice * 1.09).ToString();
            }
            else if (gvListing.Columns[e.ColumnIndex].Name == "ListingImage")
            {
                string imageUrl = string.Empty;

                imageUrl = (this.gvListing.Rows[e.RowIndex].Cells[15]).FormattedValue.ToString();

                if (imageUrl != "")
                {
                    e.Value = eBayFrontEnd.GetImageFromUrl(imageUrl);
                }
                else
                {
                    e.Value = null;
                }
            }
        }

        private void gvListing_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            daEBayListing.Update(dtEBayListing);
        }

        private void btnListingDelete_Click(object sender, EventArgs e)
        {
            List<ProductUpdate> products = new List<ProductUpdate>();
            string itemNumbers = "";

            foreach (DataGridViewRow row in gvListing.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ListingSelect"].Value) == true)
                {
                    ProductUpdate p = new ProductUpdate();
                    p.eBayItemNumbr = row.Cells["eBayItemNumber"].Value.ToString();
                    products.Add(p);

                    itemNumbers += "'" + row.Cells["eBayItemNumber"].Value.ToString() + "',";
                }
            }

            itemNumbers = itemNumbers.Substring(0, itemNumbers.Length - 1);



            UploadDelete(itemNumbers);

            gvListing_Refresh();
        }

        private void gvListing_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.RowIndex == gvListing.Rows.Count - 1)
                return;

            if (e.ColumnIndex == 5)
            {
                string url = gvListing.Rows[e.RowIndex].Cells[14].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }
            else if (e.ColumnIndex == 3)
            {
                string url = @"www.ebay.com/itm/" + gvListing.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }
            else if (e.ColumnIndex == 6)
            {
                string url = gvListing.Rows[e.RowIndex].Cells[18].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }

        }

        private void btnListingModify_Click(object sender, EventArgs e)
        {
            string itemNumbers = "";

            foreach (DataGridViewRow row in gvListing.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ListingSelect"].Value) == true)
                {
                    itemNumbers += "'" + row.Cells["CostcoUrlNumber"].Value.ToString() + "',";
                }
            }

            itemNumbers = itemNumbers.Substring(0, itemNumbers.Length - 1);

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"select l.eBayItemNumber, p.Options as oldOptions, l.CostcoOptions as newOptions, p.ImageOptions as newImageOptions, l.eBayListingPrice
                                from eBay_CurrentListings l, ProductInfo p
                                where l.CostcoUrlNumber = p.UrlNumber
                                and l.DeleteDT is NULL
                                and l.CostcoUrlNumber in (" + itemNumbers + ")";

            List<ProductUpdate> OptionsChangeProducts = new List<ProductUpdate>();
            List<ProductUpdate> PriceChangeProducts = new List<ProductUpdate>();

            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader["newOptions"].ToString().Trim() != "")
                    {
                        ProductUpdate p = new ProductUpdate();
                        p.eBayItemNumbr = Convert.ToString(reader["eBayItemNumber"]);
                        p.OldOptions = Convert.ToString(reader["oldOptions"]);
                        p.NewOptions = Convert.ToString(reader["newOptions"]);
                        p.NewPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
                        p.NewImageOptions = Convert.ToString(reader["newImageOptions"]);
                        OptionsChangeProducts.Add(p);
                    }
                    else
                    {
                        ProductUpdate p = new ProductUpdate();
                        p.eBayItemNumbr = Convert.ToString(reader["eBayItemNumber"]);
                        p.NewPrice = Convert.ToDecimal(reader["eBayListingPrice"]);
                        PriceChangeProducts.Add(p);
                    }
                }
            }

            reader.Close();

            UploadOptionChanges(OptionsChangeProducts);
            UploadPriceChange(PriceChangeProducts);

            sqlString = @" UPDATE eBay_CurrentListings SET PendingChange = '1' WHERE CostcoUrlNumber in (" + itemNumbers + ") AND DeleteDT is null";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();

            gvListing_Refresh();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            string itemNumbers = "";

            foreach (DataGridViewRow row in gvListing.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ListingSelect"].Value) == true)
                {
                    itemNumbers += "'" + row.Cells["eBayItemNumber"].Value.ToString() + "',";
                }
            }

            itemNumbers = itemNumbers.Substring(0, itemNumbers.Length - 1);

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @" UPDATE eBay_CurrentListings SET PendingChange = '' WHERE eBayItemNumber in (" + itemNumbers + ") AND DeleteDT is null";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();

            gvListing_Refresh();
        }





        // Others

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
                input = input.TrimStart();
            }

            return input;
        }

        private void GetSubCategoryUrls()
        {
            //categoryUrlArray.Clear();
            //categoryUrlArray.Add(@"/mens-clothing.html");
            IWebDriver driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), TimeSpan.FromSeconds(180));

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

                    driver.Navigate().GoToUrl(url);

                    IWebElement ShowMoreOptions = driver.FindElement(By.LinkText("Show More Options"));
                    if (ShowMoreOptions != null)
                        ShowMoreOptions.Click();

                    // level 0
                    IWebElement element = driver.FindElement(By.Id("search-filter"));
                    element = element.FindElement(By.Id("accordion-filter_collapse-1"));
                    var elements = element.FindElements(By.ClassName("style-check"));
                    element = elements[elements.Count - 1];

                    element = element.FindElement(By.TagName("a"));
                    var dimensionid = element.GetAttribute("data-dimensionid");

                    string text = element.Text;

                    if (!(text.Contains("(") && text.Contains(")")))
                        subCategoryArray.Add(url);
                    else
                    {


                    }


                    //WebPage PageResult = Browser.NavigateToPage(new Uri(url));

                    //var SearchFilterNodes = PageResult.Html.CssSelect("#search-filter");

                    //var SearchAccordionNode = SearchFilterNodes.First().SelectSingleNode(".//div[@id='accordion-filter']");

                    //var SearchPanelNodes = SearchAccordionNode.SelectNodes(".//div[@class='panel panel-default']");

                    ///*var CategoryPanelNode = SearchPanelNode.SelectSingleNode("//div[@id='accordion-filter_collapse-1']");*/

                    //var FilterNote = PageResult.Html.SelectSingleNode("//div[@id='search-filter']");

                    //if (FilterNote == null)
                    //    continue;

                    //var SearchAccordionNote = FilterNote.SelectNodes("//div")[1];
                    //var CategoryNote = FilterNote.SelectSingleNode("//div[@class='panel panel-default']");
                    //List<HtmlNode> categoryNodes = CategoryNote.CssSelect(".department_facets").ToList<HtmlNode>();

                    //if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                    //{
                    //    subCategoryArray.Add(url);
                    //}
                    //else
                    //{
                    //    List<HtmlNode> departmentNodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                    //    foreach (HtmlNode departmentNode in departmentNodes)
                    //    {
                    //        if (departmentNode.InnerText.Contains("("))
                    //        {
                    //            HtmlNode node = departmentNode.Descendants("a").First();
                    //            string departmentUrl = node.Attributes["href"].Value;

                    //            // level 1
                    //            PageResult = Browser.NavigateToPage(new Uri(departmentUrl));
                    //            mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                    //            if (mainContentWrapperNote == null)
                    //                continue;
                    //            categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                    //            if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                    //            {
                    //                subCategoryArray.Add(departmentUrl);
                    //            }
                    //            else
                    //            {
                    //                List<HtmlNode> department_1_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                    //                foreach (HtmlNode department_1_Node in department_1_Nodes)
                    //                {
                    //                    if (department_1_Node.InnerText.Contains("("))
                    //                    {
                    //                        HtmlNode node_1 = department_1_Node.Descendants("a").First();
                    //                        string departmentUrl_1 = node_1.Attributes["href"].Value;

                    //                        // level 2
                    //                        PageResult = Browser.NavigateToPage(new Uri(departmentUrl_1));
                    //                        mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                    //                        if (mainContentWrapperNote == null)
                    //                            continue;
                    //                        categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                    //                        if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                    //                        {
                    //                            subCategoryArray.Add(departmentUrl_1);
                    //                        }
                    //                        else
                    //                        {
                    //                            List<HtmlNode> department_2_Nodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                    //                            foreach (HtmlNode department_2_Node in department_2_Nodes)
                    //                            {
                    //                                HtmlNode node_2 = department_2_Node.Descendants("a").First();
                    //                                string department_2_Url = node_2.Attributes["href"].Value;
                    //                                subCategoryArray.Add(department_2_Url);
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        HtmlNode node_1 = departmentNode.Descendants("a").First();
                    //                        string department_1_Url = node_1.Attributes["href"].Value;
                    //                        subCategoryArray.Add(department_1_Url);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            HtmlNode node = departmentNode.Descendants("a").First();
                    //            string departmentUrl = node.Attributes["href"].Value;
                    //            subCategoryArray.Add(departmentUrl);
                    //        }
                    //    }
                    //}
                }
                catch (Exception exception)
                {
                    continue;


                }
            }

            //MessageBox.Show("Get subCategoryUrlArray Done");
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

        void timer_Tick(object sender, EventArgs e)
        {
            if (tpCurrentListing == tabControl1.SelectedTab)
            {

            }
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



















        //private void AddDevTables()
        //{
        //    SqlConnection cn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cn.Open();

        //    string sqlString = "TRUNCATE TABLE Dev_CategoryUrlArray_Staging";
        //    cmd.CommandText = sqlString;
        //    cmd.ExecuteNonQuery();

        //    foreach (var pu in categoryUrlArray)
        //    {
        //        sqlString = @"INSERT INTO Dev_CategoryUrlArray_Staging (Url) VALUES ('" + pu.Replace(@"'", @"''") + "')";
        //        cmd.CommandText = sqlString;
        //        cmd.ExecuteNonQuery();
        //    }

        //    sqlString = "TRUNCATE TABLE Dev_ProductListPages_Staging";
        //    cmd.CommandText = sqlString;
        //    cmd.ExecuteNonQuery();

        //    foreach (var pu in productListPages)
        //    {
        //        sqlString = @"INSERT INTO Dev_ProductListPages_Staging (Url) VALUES ('" + pu.Replace(@"'", @"''") + "')";
        //        cmd.CommandText = sqlString;
        //        cmd.ExecuteNonQuery();
        //    }

        //    sqlString = "TRUNCATE TABLE Dev_ProductUrlArray_Staging";
        //    cmd.CommandText = sqlString;
        //    cmd.ExecuteNonQuery();

        //    foreach (var pu in productUrlArray)
        //    {
        //        sqlString = @"INSERT INTO Dev_ProductUrlArray_Staging (Url) VALUES ('" + pu.Replace(@"'", @"''") + "')";
        //        cmd.CommandText = sqlString;
        //        cmd.ExecuteNonQuery();
        //    }

        //    cn.Close();
        //}

        private void AddProductUrls(string url)
        {
            driver.Navigate().GoToUrl(url);

            if (hasElement(driver, By.ClassName("product-list")))
            {
                var productList = driver.FindElement(By.ClassName("product-list"));

                foreach (var p in productList.FindElements(By.ClassName("product")))
                {
                    productUrlArray.Add(p.FindElement(By.TagName("a")).GetAttribute("href"));
                }
            }
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










        // private void CompareProducts()
        // {
        //     SqlConnection cn = new SqlConnection(connectionString);
        //     SqlCommand cmd = new SqlCommand();
        //     cmd.Connection = cn;

        //     SqlDataReader rdr;

        //     cn.Open();

        //     // price up
        //     string sqlString = @"select s.Name, s.Price as newPrice, p.Price as oldPrice, s.Url 
        //                         from [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
        //                         where s.UrlNumber = p.UrlNumber
        //                         and s.Price > p.Price";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     priceUpProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         priceUpProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["newPrice"].ToString() + "|(" + rdr["oldPrice"].ToString() + ")");
        //     }

        //     rdr.Close();

        //     // price down
        //     sqlString = @"select s.Name, s.Price as newPrice, p.Price as oldPrice, s.Url from 
        //                 [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
        //                 where s.UrlNumber = p.UrlNumber
        //                 and s.Price < p.Price";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     priceDownProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         priceDownProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["newPrice"].ToString() + "|(" + rdr["oldPrice"].ToString() + ")");
        //     }

        //     rdr.Close();

        //     // new products
        //     sqlString = @"select * from Staging_ProductInfo sp
        //                 where 
        //                 not exists
        //                 (select 1 from ProductInfo p  where sp.UrlNumber = p.UrlNumber)";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     newProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         newProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["Price"].ToString());
        //     }

        //     rdr.Close();

        //     // discontinued products
        //     sqlString = @"select * from ProductInfo p 
        //                 where 
        //                 not exists
        //                 (select 1 from Staging_ProductInfo sp where sp.UrlNumber = p.UrlNumber)";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     discontinueddProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         discontinueddProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["Price"].ToString());
        //     }

        //     rdr.Close();

        //     // stockChange products
        //     sqlString = @"select s.Name, s.Url from 
        //                 [dbo].[Staging_ProductInfo] s, [dbo].[ProductInfo] p
        //                 where s.UrlNumber = p.UrlNumber
        //                 and s.Options <> p.Options";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     stockChangeProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         stockChangeProductArray.Add("<a href='" + rdr["Url"].ToString() + "'>" + rdr["Name"].ToString() + "</a>");
        //     }

        //     rdr.Close();

        //     // eBay listing price up
        //     sqlString = @"select s.Name, s.CostcoPrice as OldBasePrice, s.eBayListingPrice as eBayListingPrice, p.Price as NewBasePrice, p.Url as CostcoUrl, s.eBayItemNumber as eBayItemNumber
        //                     from [dbo].[eBay_CurrentListings] s, [dbo].[Staging_ProductInfo] p
        //                     where s.CostcoUrlNumber = p.UrlNumber
        //                     and s.CostcoPrice < p.Price";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     eBayListingPriceUpProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         eBayListingPriceUpProductArray.Add("<a href='" + rdr["CostcoUrl"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["NewBasePrice"].ToString() + "|(" + rdr["OldBasePrice"].ToString() + ")");
        //     }

        //     rdr.Close();

        //     sqlString = @"INSERT INTO [dbo].[eBay_ToChange] (Name, CostcoUrlNumber, eBayItemNumber, eBayOldListingPrice, 
        //eBayNewListingPrice, eBayReferencePrice, 
        //CostcoOldPrice, CostcoNewPrice, PriceChange)
        //                     SELECT l.Name, l.CostcoUrlNumber, l.eBayItemNumber, l.eBayListingPrice, l.eBayListingPrice, 
        //                     l.eBayReferencePrice, l.CostcoPrice, r.Price, 'up'
        //                     FROM [dbo].[eBay_CurrentListings] l, [dbo].[Staging_ProductInfo] r
        //                     WHERE l.CostcoPrice < r.Price 
        //                     AND l.CostcoUrlNumber = r.UrlNumber";

        //     cmd.CommandText = sqlString;
        //     cmd.ExecuteNonQuery();

        //     // eBay listing price down
        //     sqlString = @"select s.Name, s.CostcoPrice as OldBasePrice, s.eBayListingPrice as eBayListingPrice, p.Price as NewBasePrice, p.Url as CostcoUrl, s.eBayItemNumber as eBayItemNumber
        //                     from [dbo].[eBay_CurrentListings] s, [dbo].[Staging_ProductInfo] p
        //                     where s.CostcoUrlNumber = p.UrlNumber
        //                     and s.CostcoPrice > p.Price";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     eBayListingPriceDownProductArray.Clear();

        //     while (rdr.Read())
        //     {
        //         eBayListingPriceDownProductArray.Add("<a href='" + rdr["CostcoUrl"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["NewBasePrice"].ToString() + "|(" + rdr["OldBasePrice"].ToString() + ")");
        //     }

        //     rdr.Close();

        //     sqlString = @"INSERT INTO [dbo].[eBay_ToChange] (Name, CostcoUrlNumber, eBayItemNumber, eBayOldListingPrice, 
        //eBayNewListingPrice, eBayReferencePrice, 
        //CostcoOldPrice, CostcoNewPrice, PriceChange)
        //                     SELECT l.Name, l.CostcoUrlNumber, l.eBayItemNumber, l.eBayListingPrice, l.eBayListingPrice, 
        //                     l.eBayReferencePrice, l.CostcoPrice, r.Price, 'down'
        //                     FROM [dbo].[eBay_CurrentListings] l, [dbo].[Staging_ProductInfo] r
        //                     WHERE l.CostcoPrice < r.Price
        //                     AND l.CostcoUrlNumber = r.UrlNumber";

        //     cmd.CommandText = sqlString;
        //     cmd.ExecuteNonQuery();

        //     // eBay listing discontinused 
        //     sqlString = @"select * from eBay_CurrentListings p 
        //                 where 
        //                 not exists
        //                 (select 1 from Staging_ProductInfo sp where sp.UrlNumber = p.CostcoUrlNumber)";
        //     cmd.CommandText = sqlString;
        //     rdr = cmd.ExecuteReader();

        //     eBayListingDiscontinueddProductArray.Clear();


        //     while (rdr.Read())
        //     {
        //         eBayListingDiscontinueddProductArray.Add("<a href='" + rdr["CostcoUrl"].ToString() + "'>" + rdr["Name"].ToString() + "</a>|" + rdr["CostcoPrice"].ToString());
        //     }

        //     rdr.Close();

        //     sqlString = @"INSERT INTO [dbo].[eBay_ToRemove] (Name, CostcoUrlNumber, eBayItemNumber)
        //                     SELECT l.Name, l.CostcoUrlNumber, l.eBayItemNumber
        //                     FROM [dbo].[eBay_CurrentListings] l
        //                     WHERE not exists 
        //                  (SELECT 1 FROM [dbo].[Staging_ProductInfo] r where r.UrlNumber = l.CostcoUrlNumber)";

        //     cmd.CommandText = sqlString;
        //     cmd.ExecuteNonQuery();

        //     cn.Close();
        // }











        private void btnToChangeUpdate_Click(object sender, EventArgs e)
        {

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

        private void btnCalculate_Click(object sender, EventArgs e)
        {


            string stFrom = dtpFrom.Value.ToString("yyyy/MM/dd");
            string stTo = dtpTo.Value.ToString("yyyy/MM/dd");

            string sqlString = @"SELECT CostcoOrderNumber, CostcoItemName, CostcoOrderDate, CostcoPrice, CostcoTax, BuyerState 
                                 FROM eBay_SoldTransactions 
                                 WHERE CostcoOrderDate between '" + stFrom + "' AND '" + stTo + "'";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sqlString, connection);

            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "tbSoldTransactions");
            connection.Close();
            gvSummary.DataSource = ds;
            gvSummary.DataMember = "tbSoldTransactions";

            gvSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gvSummary.Columns["CostcoOrderNumber"].Width = 100;
            gvSummary.Columns["CostcoItemName"].Width = 200;
        }

        private void btnGenerateFiles_Click(object sender, EventArgs e)
        {

            List<eBaySoldProduct> products = new List<eBaySoldProduct>();
            List<string> states = new List<string>();

            // get products from DB
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            string stFrom = dtpFrom.Value.ToString("yyyy/MM/dd");
            string stTo = dtpTo.Value.ToString("yyyy/MM/dd");

            string sqlString = @"SELECT * 
                                 FROM eBay_SoldTransactions 
                                 WHERE CostcoOrderDate between '" + stFrom + "' AND '" + stTo + "' " +
                                "AND UPPER(BuyerState) not in ('CA', 'CT', 'DC', 'FL', 'HI', 'IL', 'LA', 'MD', 'MA', 'TN')";

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    eBaySoldProduct soldProduct = new eBaySoldProduct();

                    soldProduct.CostcoOrderNumber = Convert.ToString(reader["CostcoOrderNumber"]);
                    soldProduct.CostcoItemName = Convert.ToString(reader["CostcoItemName"]);
                    soldProduct.CostcoOrderDate = Convert.ToDateTime(reader["CostcoOrderDate"]).ToShortDateString();
                    soldProduct.CostcoPrice = Convert.ToDecimal(reader["CostcoPrice"]);
                    soldProduct.CostcoTax = Convert.ToDecimal(reader["CostcoTax"]);
                    soldProduct.BuyerState = Convert.ToString(reader["BuyerState"]);
                    soldProduct.CostcoOrderEmailPdf = Convert.ToString(reader["CostcoOrderEmailPdf"]);
                    soldProduct.CostcoTaxExemptPdf = Convert.ToString(reader["CostcoTaxExemptPdf"]);
                    products.Add(soldProduct);

                    if (!states.Contains(soldProduct.BuyerState))
                    {
                        states.Add(soldProduct.BuyerState.ToUpper());
                    }
                }
            }

            reader.Close();


            // add to Excel file
            string sourceFileName = @"c:\ebay\documents\TaxTemplate.csv";
            string destinFileName = @"c:\temp\TaxExemption\TaxExemption-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + ".csv";
            File.Delete(destinFileName);
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

            List<string> fileNames = new List<string>();

            int i = 2;

            foreach (eBaySoldProduct p in products)
            {
                oSheet.Cells[i, 1].value = p.CostcoOrderNumber;
                oSheet.Cells[i, 2].value = p.CostcoItemName;
                oSheet.Cells[i, 3].value = p.CostcoOrderDate;
                oSheet.Cells[i, 4].value = p.CostcoPrice;
                oSheet.Cells[i, 5].value = p.CostcoTax;
                oSheet.Cells[i, 6].value = p.BuyerState;

                i++;

                fileNames.Add(@"C:\temp\CostcoOrderEmails\" + p.CostcoOrderEmailPdf);
                fileNames.Add(@"C:\temp\TaxExemption\" + p.CostcoTaxExemptPdf);
            }

            if (states.Contains("AL") ||
                states.Contains("CO") ||
                states.Contains("GA") ||
                states.Contains("ID") ||
                states.Contains("IA") ||
                states.Contains("KS") ||
                states.Contains("KY") ||
                states.Contains("MI") ||
                states.Contains("MN") ||
                states.Contains("MO") ||
                states.Contains("NE") ||
                states.Contains("NV") ||
                states.Contains("NJ") ||
                states.Contains("NM") ||
                states.Contains("NC") ||
                states.Contains("ND") ||
                states.Contains("OH") ||
                states.Contains("OK") ||
                states.Contains("SC") ||
                states.Contains("TX") ||
                states.Contains("UT") ||
                states.Contains("VT") ||
                states.Contains("WA") ||
                states.Contains("WI"))
            {
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\Uniform.pdf");
            }
            if (states.Contains("AZ"))
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\Arizona.pdf");
            if (states.Contains("IN"))
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\Indiana.pdf");
            if (states.Contains("NY"))
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\NewYork.pdf");
            if (states.Contains("PA"))
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\Pennsylvania.pdf");
            if (states.Contains("SD"))
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\SouthDakota.pdf");
            if (states.Contains("VA"))
                fileNames.Add(@"C:\ebay\Documents\TaxExemptForms\Vaginia.pdf");

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();


            string finalPDFName = @"C:\temp\tempPDF\TaxExemption-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + ".pdf";
            File.Delete(finalPDFName);
            MergePDFs(fileNames, finalPDFName);

            File.Delete(@"C:\temp\TaxExemption\TaxExemption-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + ".pdf");
            File.Move(finalPDFName, @"C:\temp\TaxExemption\TaxExemption-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + ".pdf");

            sqlString = @"INSERT INTO TaxExemption (FromDate, ToDate, Report) VALUES ('" + stFrom + "',  '" + stTo + "',  '" + "TaxExemption-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + "')";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();
            cn.Close();

            gvTaxExemption_Refresh();
        }

        public void gvTaxExemption_Refresh()
        {
            string sqlString = @"SELECT FromDate, ToDate, Report, TotalSell, TotalTax, RefundableTax, Sent, Refund, ActualRefund 
                                 FROM TaxExemption 
                                 Order by FromDate DESC";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sqlString, connection);

            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "tbTaxExemption");
            connection.Close();
            gvTaxExempt.DataSource = ds;
            gvTaxExempt.DataMember = "tbTaxExemption";

            gvTaxExempt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //gvTaxExempt.Columns["CostcoOrderNumber"].Width = 100;
            //gvTaxExempt.Columns["CostcoItemName"].Width = 200;
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

        private void tpTax_Enter(object sender, EventArgs e)
        {
            gvTaxExemption_Refresh();
        }

        private void btnGenerateSaleTaxReport_Click(object sender, EventArgs e)
        {
            string stFrom = dtpSaleTaxFrom.Value.ToString("yyyy/MM/dd");
            string stTo = dtpSaleTaxTo.Value.ToString("yyyy/MM/dd");

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = @"Insert into SaleTax (FromDate, ToDate, NumberOfTransactions, StateSaleTax, CitySaleTax, CountySaleTax)
                                  Select '" + stFrom + "', '" + stTo +
                                @"', Count(*), SUM(CostcoPrice)*0.03 as StateSaleTax, SUM(CostcoPrice)*0.03 as CitySaleTax, SUM(CostcoPrice)*0.03 as CountySaleTax
                                  From eBay_SoldTransactions
                                  where upper(BuyerState) in ('AL') 
                                  and  PaypalPaidDateTime between '" + stFrom + "' and '" + stTo + "'";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();
            cn.Close();

            gvSaleTaxHistory_Refresh();

            sqlString = @"SELECT PaypalPaidDateTime, PaypalTransactionID, eBayItemName, eBaySoldPrice, PaypalPaidEmailPdf, eBaySoldEmailPdf
                                 FROM eBay_SoldTransactions 
                                 where upper(BuyerState) in ('AL') 
                                 and  PaypalPaidDateTime between '" + stFrom + "' and '" + stTo + "' Order by PaypalPaidDateTime Desc";

            cn.Open();
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();

            List<eBaySoldProduct> products = new List<eBaySoldProduct>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    eBaySoldProduct soldProduct = new eBaySoldProduct();

                    soldProduct.PaypalPaidDateTime = Convert.ToDateTime(reader["PaypalPaidDateTime"]);
                    soldProduct.PaypalTransactionID = Convert.ToString(reader["PaypalTransactionID"]);
                    soldProduct.eBayItemName = Convert.ToString(reader["eBayItemName"]);
                    soldProduct.eBaySoldPrice = Convert.ToDecimal(reader["eBaySoldPrice"]);
                    soldProduct.PaypalPaidEmailPdf = Convert.ToString(reader["PaypalPaidEmailPdf"]);
                    soldProduct.eBaySoldEmailPdf = Convert.ToString(reader["eBaySoldEmailPdf"]);
                    products.Add(soldProduct);
                }
            }

            // add to Excel file
            string sourceFileName = @"C:\eBayApp\Files\Templates\SaleTaxTemplate.csv";
            string destinFileName = @"C:\eBayApp\Files\Tax\SaleTax\SaleTax-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + ".csv";
            File.Delete(destinFileName);
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

            List<string> fileNames = new List<string>();

            int i = 2;

            foreach (eBaySoldProduct p in products)
            {
                oSheet.Cells[i, 1].value = p.PaypalPaidDateTime;
                oSheet.Cells[i, 2].value = p.PaypalTransactionID;
                oSheet.Cells[i, 3].value = p.eBayItemName;
                oSheet.Cells[i, 4].value = p.eBaySoldPrice;

                i++;

                fileNames.Add(@"C:\eBayApp\Files\Emails\eBaySoldEmails\" + p.eBaySoldEmailPdf);
                fileNames.Add(@"C:\eBayApp\Files\Emails\PaypalPaidEmails\" + p.PaypalPaidEmailPdf);
            }

            oWB.Save();
            oWB.Close(true, Type.Missing, Type.Missing);
            oXL.Application.Quit();
            oXL.Quit();


            string finalPDFName = @"C:\eBayApp\Files\Tax\SaleTax\TaxExemption-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd") + ".pdf";
            File.Delete(finalPDFName);
            MergePDFs(fileNames, finalPDFName);
        }

        SqlCommand cmdSaleTax;
        SqlDataAdapter daSaleTax;
        SqlCommandBuilder cmbSaleTax;
        DataSet dsSaleTax;
        DataTable dtSaleTax;

        public void gvSaleTaxHistory_Refresh()
        {
            string sqlString = @"SELECT ReportID, FromDate, ToDate, NumberOfTransactions, StateSaleTax, CitySaleTax, CountySaleTax, StateTaxSubmitted, CityTaxSubmitted, CountyTaxSubmitted 
                                 FROM SaleTax 
                                 Order by FromDate DESC";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdSaleTax = new SqlCommand(sqlString, connection);
            daSaleTax = new SqlDataAdapter(cmdSaleTax);
            cmbSaleTax = new SqlCommandBuilder(daSaleTax);
            dsSaleTax = new DataSet();
            daSaleTax.Fill(dsSaleTax, "tbSaleTax");
            dtSaleTax = dsSaleTax.Tables["tbSaleTax"];
            connection.Close();


            gvSaleTaxHistory.DataSource = dsSaleTax.Tables["tbSaleTax"];
            gvSaleTaxHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gvSaleTaxHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gvSaleTaxHistory.Columns[0].Visible = false;

            //daSaleTax = new SqlDataAdapter(sqlString, connection);

            //dsSaleTax = new DataSet();
            //connection.Open();
            //daSaleTax.Fill(dsSaleTax, "tbSaleTax");
            //dtSaleTax = dsSaleTax.Tables["tbSaleTax"];
            //connection.Close();

            //gvSaleTaxHistory.DataSource = dsSaleTax;
            //gvSaleTaxHistory.DataMember = "tbSaleTax";

            //gvSaleTaxHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //gvTaxExempt.Columns["CostcoOrderNumber"].Width = 100;
            //gvTaxExempt.Columns["CostcoItemName"].Width = 200;
        }

        private void btnSaleTaxSave_Click(object sender, EventArgs e)
        {
            daSaleTax.Update(dtSaleTax);
        }

        private void tpSaleTax_Enter(object sender, EventArgs e)
        {
            gvSaleTaxHistory_Refresh();
        }

        private void ll1a_TextChanged(object sender, EventArgs e)
        {
            ll1.Text = (Convert.ToDecimal(ll1a.Text) + Convert.ToDecimal(ll1b.Text) + Convert.ToDecimal(ll1c.Text)).ToString();
        }

        private void ll1b_TextChanged(object sender, EventArgs e)
        {
            ll1a_TextChanged(sender, e);
        }

        private void ll1c_TextChanged(object sender, EventArgs e)
        {
            ll1a_TextChanged(sender, e);
        }

        private void ll1_TextChanged(object sender, EventArgs e)
        {
            ll3.Text = (Convert.ToDecimal(ll1.Text) - Convert.ToDecimal(ll2.Text)).ToString();
        }

        private void ll2_TextChanged(object sender, EventArgs e)
        {
            ll1_TextChanged(sender, e);
        }

        private void ll3_TextChanged(object sender, EventArgs e)
        {
            ll5.Text = (Convert.ToDecimal(ll3.Text) - Convert.ToDecimal(ll4.Text)).ToString();
        }

        private void ll4_TextChanged(object sender, EventArgs e)
        {
            ll3_TextChanged(sender, e);
        }

        private void ll5_TextChanged(object sender, EventArgs e)
        {
            ll7.Text = (Convert.ToDecimal(ll5.Text) + Convert.ToDecimal(ll6.Text)).ToString();
        }

        private void ll6_TextChanged(object sender, EventArgs e)
        {
            ll5_TextChanged(sender, e);
        }

        private void ll8_TextChanged(object sender, EventArgs e)
        {
            ll28.Text = Convert.ToString(Convert.ToDecimal(ll8.Text) +
                                            Convert.ToDecimal(ll9.Text) +
                                            Convert.ToDecimal(ll10.Text) +
                                            Convert.ToDecimal(ll11.Text) +
                                            Convert.ToDecimal(ll12.Text) +
                                            Convert.ToDecimal(ll13.Text) +
                                            Convert.ToDecimal(ll14.Text) +
                                            Convert.ToDecimal(ll15.Text) +
                                            Convert.ToDecimal(ll16a.Text) +
                                            Convert.ToDecimal(ll16b.Text) +
                                            Convert.ToDecimal(ll17.Text) +
                                            Convert.ToDecimal(ll18.Text) +
                                            Convert.ToDecimal(ll19.Text) +
                                            Convert.ToDecimal(ll20a.Text) +
                                            Convert.ToDecimal(ll20b.Text) +
                                            Convert.ToDecimal(ll21.Text) +
                                            Convert.ToDecimal(ll22.Text) +
                                            Convert.ToDecimal(ll23.Text) +
                                            Convert.ToDecimal(ll24a.Text) +
                                            Convert.ToDecimal(ll24b.Text) +
                                            Convert.ToDecimal(ll25.Text) +
                                            Convert.ToDecimal(ll26.Text) +
                                            Convert.ToDecimal(ll27.Text));
        }

        private void ll9_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll10_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll11_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll12_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll13_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll14_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll15_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll16a_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll16b_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll17_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll18_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll19_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll20a_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll20b_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll21_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll22_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll23_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll24a_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll24b_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll25_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll26_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll27_TextChanged(object sender, EventArgs e)
        {
            ll8_TextChanged(sender, e);
        }

        private void ll7_TextChanged(object sender, EventArgs e)
        {
            ll28_TextChanged(sender, e);
        }

        private void ll28_TextChanged(object sender, EventArgs e)
        {
            ll29.Text = (Convert.ToDecimal(ll7.Text) - Convert.ToDecimal(ll28.Text)).ToString();
        }

        private void ll29_TextChanged(object sender, EventArgs e)
        {
            ll31.Text = (Convert.ToDecimal(ll29.Text) - Convert.ToDecimal(ll30.Text)).ToString();
        }

        private void ll30_TextChanged(object sender, EventArgs e)
        {
            ll29_TextChanged(sender, e);
        }

        private void tpIncomeTax_Enter(object sender, EventArgs e)
        {
            cmbIncomeTaxYear.Text = DateTime.Now.Year.ToString();
        }

        private void btnIncomeTaxCalculate_Click(object sender, EventArgs e)
        {
            string year = cmbIncomeTaxYear.Text;

            string yearStart = year + "/1/1";
            string yearEnd = year + "/12/31";

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            // 1a - Sales
            string sqlString = @" Select Sum(eBaySoldPrice)  from eBay_SoldTransactions where eBaySoldDateTime between
                                  '" + yearStart + "' and '" + yearEnd + "'";

            cmd.CommandText = sqlString;
            ll1a.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("#,##0.00");

            // 1b - Sales Tax Collected
            sqlString = @" Select Sum(eBaySaleTax) from eBay_SoldTransactions where eBaySoldDateTime between
                                  '" + yearStart + "' and '" + yearEnd + "'";

            cmd.CommandText = sqlString;
            ll1b.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("#,##0.00");

            // 2 - Returns and allowances
            sqlString = @" Select Sum(CostcoTax) from eBay_SoldTransactions where eBaySoldDateTime between
                                  '" + yearStart + "' and '" + yearEnd + "'";

            cmd.CommandText = sqlString;
            ll2.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("#,##0.00");

            // 4 - Cost of goods sold
            sqlString = @" Select Sum(CostcoPrice + CostcoTax) from eBay_SoldTransactions where eBaySoldDateTime between
                                  '" + yearStart + "' and '" + yearEnd + "'";

            cmd.CommandText = sqlString;
            ll4.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("#,##0.00");

            List<string> expenseCategories = new List<string> { "8", "9", "10", "11", "12", "13", "14", "15", "16a", "16b", "17", "18", "19",
                                                                "20a", "20b", "21", "22", "23", "24a", "24b", "25", "26", "27" };

            foreach (string c in expenseCategories)
            {
                sqlString = @" Select ISNULL(Sum(Amount), 0.00) from BookKeeping where Date between
                                  '" + yearStart + "' and '" + yearEnd + "' and CategoryCode = '" + c + "' and Expense = 1";

                cmd.CommandText = sqlString;
                var result = cmd.ExecuteScalar();
                this.Controls.Find("ll" + c, true)[0].Text = Convert.ToDecimal(result).ToString("#,##0.00");
            }

            cn.Close();
        }


        private void ll1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void ll_LinkClicked(string categoryCode, string year)
        {
            frmBookKeeping frmBK = new frmBookKeeping();
            frmBK.connectionString = connectionString;
            frmBK.m_CategoryCode = categoryCode;
            frmBK.m_Year = year;

            frmBK.ShowDialog();
        }

        private void ll8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ll_LinkClicked(8.ToString(), cmbIncomeTaxYear.Text);
        }

        private void btnResearch_Click(object sender, EventArgs e)
        {
            string sqlString = string.Empty;
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string storeName = cmbStore.Text;

            string storeUrl = "http://www.ebay.com/sch/***/m.html?_nkw=&_armrs=1&_ipg=&_from=";

            storeUrl = storeUrl.Replace("***", storeName);

            sqlString = @"DELETE FROM eBay_ProductsResearch WHERE eBayUserId = '" + storeName + "'";

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            List<string> storeLinkList = new List<string>();

            //driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), TimeSpan.FromSeconds(180));
            driver = new ChromeDriver();

            driver.Navigate().GoToUrl(storeUrl);

            IWebElement eProductCount = driver.FindElement(By.ClassName("rcnt"));
            int nProductCount = Convert.ToInt32(eProductCount.Text.Replace(",", ""));

            int nPage = Convert.ToInt16(Math.Round(nProductCount / 50.00 + 0.5));

            if (hasElement(driver, By.ClassName("pages")))
            {
                for (int i = 1; i <= nPage; i++)
                {
                    storeUrl = @"http://www.ebay.com/sch/m.html?_nkw=&_armrs=1&_from=&_ssn=" + storeName + @"&_pgn=" + i.ToString() + @"&_skc=" + ((i - 1) * 50).ToString() + @"&rt=nc";

                    storeLinkList.Add(storeUrl);
                }
            }
            else
            {
                storeLinkList.Add(storeUrl);
            }

            ChromeDriver costcoDriver = new ChromeDriver();
            costcoDriver.Navigate().GoToUrl("https://www.costco.com/LogonForm");
            IWebElement logonForm = costcoDriver.FindElement(By.Id("LogonForm"));
            logonForm.FindElement(By.Id("logonId")).SendKeys("zjding@outlook.com");
            logonForm.FindElement(By.Id("logonPassword")).SendKeys("721123");
            logonForm.FindElement(By.ClassName("submit")).Click();

            string itemName = string.Empty;
            string itemURL = string.Empty;
            string itemPrice = string.Empty;
            string trending = string.Empty;
            string numberSold = string.Empty;

            foreach (string url in storeLinkList)
            {

                driver.Navigate().GoToUrl(url);

                if (hasElement(driver, By.Id("ResultSetItems")))
                {
                    var lis = driver.FindElement(By.Id("ResultSetItems")).FindElements(By.ClassName("lvresult"));

                    foreach (IWebElement li in lis)
                    {
                        itemName = string.Empty;
                        itemURL = string.Empty;
                        itemPrice = string.Empty;
                        trending = string.Empty;
                        numberSold = string.Empty;

                        try
                        {
                            itemName = li.FindElement(By.ClassName("lvtitle")).Text;
                            itemName = itemName.Replace("NEW LISTING", "");
                            itemURL = li.FindElement(By.ClassName("lvtitle")).FindElement(By.TagName("a")).GetAttribute("href");
                            itemPrice = li.FindElement(By.ClassName("lvprice")).FindElement(By.ClassName("bold")).Text;
                            if (hasElement(li.FindElement(By.ClassName("lvprice")).FindElement(By.ClassName("bold")), By.ClassName("medprc")))
                                trending = li.FindElement(By.ClassName("lvprice")).FindElement(By.ClassName("bold")).FindElement(By.ClassName("medprc")).Text;
                            if (!string.IsNullOrEmpty(trending))
                                itemPrice = itemPrice.Replace(trending, "").Replace("$", "").Replace("\r", "").Replace("\n", "");
                            else
                                itemPrice = itemPrice.Replace("$", "").Replace("\r", "").Replace("\n", "");

                            string extra = string.Empty;
                            IWebElement lvextras;

                            if (hasElement(li, By.ClassName("lvextras")))
                            {
                                lvextras = li.FindElement(By.ClassName("lvextras"));

                                if (hasElement(lvextras, By.ClassName("red")))
                                    lvextras = lvextras.FindElement(By.ClassName("red"));

                                extra = lvextras.Text;
                            }

                            if (extra.Contains("sold"))
                            {
                                numberSold = extra.Replace("sold", "").Replace("+", "").Trim();
                            }

                            //string searchUrl = itemName.Replace(" ", "+");
                            //searchUrl = searchUrl.Replace("%", "%25");
                            //searchUrl = "https://www.google.com/?gws_rd=ssl#q=" + searchUrl + "+site:costco.com";

                            //costcoDriver.Navigate().GoToUrl(searchUrl);

                            //costcoDriver.Navigate().GoToUrl("https://www.google.com");
                            //costcoDriver.FindElement(By.Id("lst-ib")).SendKeys(itemName + " site:costco.com");
                            //costcoDriver.FindElement(By.Name("btnG")).Click();

                            //costcoDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

                            costcoDriver.Navigate().GoToUrl("http://www.costco.com");
                            costcoDriver.FindElement(By.Id("search-field")).SendKeys(itemName);
                            costcoDriver.FindElement(By.ClassName("co-search-thin")).Click();
                            costcoDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                            string cs = string.Empty;
                            string price = string.Empty;
                            string productUrl = string.Empty;

                            if (hasElement(costcoDriver, By.Id("product-details")))
                            {
                                IWebElement eProductDetails = costcoDriver.FindElement(By.Id("product-details"));
                                IWebElement ePrice = eProductDetails.FindElements(By.ClassName("form-group"))[0];
                                IWebElement eYourPrice = ePrice.FindElement(By.ClassName("your-price")).FindElement(By.ClassName("value"));
                                price = eYourPrice.Text.Replace(",", "");
                                cs = "C";
                            } 
                            else if (!hasElement(costcoDriver, By.Id("no-results"))&& hasElement(costcoDriver, By.ClassName("product-list")))
                            {
                                IWebElement eProductList = costcoDriver.FindElement(By.ClassName("product-list"));
                                if (hasElement(eProductList, By.ClassName("product")))
                                {
                                    IWebElement eProduct = eProductList.FindElement(By.ClassName("product"));
                                    IWebElement ePrice = eProduct.FindElement(By.ClassName("price"));
                                    price = ePrice.Text.Replace("$", "").Replace(",", "");
                                    cs = "C";
                                }
                            }


                            //if (hasElement(costcoDriver, By.Id("res")))
                            //{
                            //    IWebElement eRes = costcoDriver.FindElement(By.Id("res"));

                                //    if (hasElement(eRes, By.ClassName("srg")))
                                //    {
                                //        IWebElement eSrg = eRes.FindElement(By.ClassName("srg"));

                                //        if (hasElement(eSrg, By.ClassName("g")))
                                //        {
                                //            var eGs = eSrg.FindElements(By.ClassName("g"));

                                //            productUrl = eGs[0].FindElement(By.TagName("h3")).FindElement(By.TagName("a")).GetAttribute("href");

                                //            costcoDriver.Navigate().GoToUrl(productUrl);

                                //            IWebElement eProductDetails = costcoDriver.FindElement(By.Id("product-details"));
                                //            IWebElement ePrice = eProductDetails.FindElements(By.ClassName("form-group"))[0];
                                //            IWebElement eYourPrice = ePrice.FindElement(By.ClassName("your-price")).FindElement(By.ClassName("value"));
                                //            price = eYourPrice.Text.Replace(",", "");

                                //            cs = "C";
                                //        }
                                //    }
                                //}

                            if (string.IsNullOrEmpty(price))
                            {
                                //searchUrl = searchUrl.Replace("costco", "samsclub");

                                //costcoDriver.Navigate().GoToUrl(searchUrl);

                                //costcoDriver.Navigate().GoToUrl("https://www.google.com");
                                //costcoDriver.FindElement(By.Id("lst-ib")).SendKeys(itemName + " site:samsclub.com");
                                //costcoDriver.FindElement(By.Name("btnG")).Click();
                                //costcoDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

                                //if (hasElement(costcoDriver, By.Id("res")))
                                //{
                                //    IWebElement eRes = costcoDriver.FindElement(By.Id("res"));

                                //    if (hasElement(eRes, By.ClassName("srg")))
                                //    {
                                //        IWebElement eSrg = eRes.FindElement(By.ClassName("srg"));

                                //        if (hasElement(eSrg, By.ClassName("g")))
                                //        {
                                //            var eGs = eSrg.FindElements(By.ClassName("g"));

                                //            productUrl = eGs[0].FindElement(By.TagName("h3")).FindElement(By.TagName("a")).GetAttribute("href");

                                //            costcoDriver.Navigate().GoToUrl(productUrl);

                                //            IWebElement eMoneyBox = costcoDriver.FindElement(By.Id("itemPageMoneyBox"));

                                //            IWebElement ePrice = eMoneyBox.FindElement(By.ClassName("pricingInfo"));

                                //            price = ePrice.FindElement(By.ClassName("price")).Text + "." + ePrice.FindElements(By.ClassName("superscript"))[1].Text;

                                //            cs = "S";
                                //        }
                                //    }
                                //}

                                costcoDriver.Navigate().GoToUrl("http://www.samsclub.com");
                                costcoDriver.FindElement(By.Id("id-search-bar")).SendKeys(itemName);
                                costcoDriver.FindElement(By.ClassName("search-ico")).Click();

                                if (hasElement(costcoDriver, By.ClassName("sc-product-grid")))
                                {
                                    IWebElement eProductCard = costcoDriver.FindElement(By.ClassName("sc-product-grid")).FindElement(By.ClassName("sc-product-card"));
                                    price = eProductCard.FindElement(By.ClassName("sc-dollars")).Text.Replace(",", "");
                                    cs = "S";
                                }
                                else if (hasElement(costcoDriver, By.Id("itemPageMoneyBox")))
                                {
                                    IWebElement eMoneyBox = costcoDriver.FindElement(By.Id("itemPageMoneyBox"));
                                    IWebElement ePrice = eMoneyBox.FindElement(By.ClassName("pricingInfo"));
                                    price = ePrice.FindElement(By.ClassName("price")).Text + "." + ePrice.FindElements(By.ClassName("superscript"))[1].Text;

                                    cs = "S";
                                }
                            }

                            sqlString = @"INSERT INTO eBay_ProductsResearch (Name, eBayUrl, eBayPrice, eBaySoldNumber, productUrl, productPrice, SamsOrCostco, eBayUserId) VALUES ('" +
                            itemName.Replace("'", "") + "','" + itemURL.Replace("'", "") + "','" + itemPrice + "'," + (string.IsNullOrEmpty(numberSold) ? "NULL" : numberSold) +
                            ", '" + productUrl + "', " + price + ", '" + cs + "', '" + storeName.Replace("'", "") + "')";

                            cmd.CommandText = sqlString;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            sqlString = @"INSERT INTO eBay_ProductsResearch (Name, eBayUrl, eBayPrice, eBaySoldNumber, eBayUserId) VALUES ('" +
                                    itemName.Replace("'", "") + "','" + itemURL.Replace("'", "") + "','" + itemPrice + "'," + (string.IsNullOrEmpty(numberSold) ? "NULL" : numberSold) +
                                    ", '" + storeName.Replace("'", "") + "')";

                            cmd.CommandText = sqlString;
                            cmd.ExecuteNonQuery();

                            continue;
                        }
                    }
                }
            }

            cn.Close();

            driver.Dispose();
            costcoDriver.Dispose();

            gvEBayResearch_Refresh();
        }

        private void tpResearch_Enter(object sender, EventArgs e)
        {
            string sqlString = @"select distinct eBayUserId from [Costco].[dbo].[eBay_ProductsResearch]";
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            cmbStore.Items.Add("All");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cmbStore.Items.Add(Convert.ToString(reader["eBayUserId"]));
                }
            }

            reader.Close();
            cn.Close();
        }

        private void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvEBayResearch_Refresh();
        }

        public void gvEBayResearch_Refresh()
        {
            string sqlString = @"SELECT Name, eBayUrl, eBayPrice, eBaySoldNumber, eBayUserId
                                 FROM eBay_ProductsResearch ";

            if (cmbStore.Text != "All")
                sqlString += "WHERE eBayUserId = '" + cmbStore.Text + "'";


            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdSaleTax = new SqlCommand(sqlString, connection);
            daSaleTax = new SqlDataAdapter(cmdSaleTax);
            cmbSaleTax = new SqlCommandBuilder(daSaleTax);
            dsSaleTax = new DataSet();
            daSaleTax.Fill(dsSaleTax, "tbEBayResearch");
            dtSaleTax = dsSaleTax.Tables["tbEBayResearch"];
            connection.Close();


            gvEBayResearch.DataSource = dsSaleTax.Tables["tbEBayResearch"];
            gvEBayResearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gvEBayResearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataGridViewLinkColumn lnk = new DataGridViewLinkColumn();
            gvEBayResearch.Columns.Insert(0, lnk);
            lnk.UseColumnTextForLinkValue = false;
            lnk.HeaderText = "eBayItemName";
            lnk.Name = "eBayItemName";
            lnk.Width = 500;

            gvEBayResearch.Columns[1].Visible = false;
            gvEBayResearch.Columns[2].Visible = false;

            foreach (DataGridViewRow row in gvEBayResearch.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void gvEBayResearch_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gvEBayResearch.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void gvEBayResearch_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.gvEBayResearch.Columns[e.ColumnIndex].Name == "eBayItemName")
            {
                string name = (this.gvEBayResearch.Rows[e.RowIndex].Cells["Name"]).FormattedValue.ToString();
                ((DataGridViewLinkCell)(gvEBayResearch.Rows[e.RowIndex].Cells["eBayItemName"])).Value = name;
            }
        }

        private void gvEBayResearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                string url = gvEBayResearch.Rows[e.RowIndex].Cells["eBayUrl"].FormattedValue.ToString();

                Process.Start(@"chrome", url);
            }
        }

        private void btnEBayItemSpecifics_Click(object sender, EventArgs e)
        {
            string fileName = @"C:\eBayApp\Instructions\eBayCSA_ItemSpecifics.xls";

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            string str;
            int rCnt = 0;
            int cCnt = 0;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = string.Empty;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;

            string category = string.Empty;
            string specifics = string.Empty;
            int iLastLeftBracket = 0;
            int iLastRightBracket = 0;
            string categoryId = string.Empty;

            for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
            {
                for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                {
                    if (cCnt == 1)
                        category = (string)(range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                    if (cCnt == 2)
                        specifics = (string)(range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                }

                iLastLeftBracket = category.LastIndexOf('(');
                iLastRightBracket = category.LastIndexOf(')');

                categoryId = category.Substring(iLastLeftBracket + 1, iLastRightBracket - iLastLeftBracket - 1);
                categoryId = categoryId.Replace("#", "");

                if (specifics != "No Specifics Required")
                {
                    sqlString = @"UPDATE eBay_Categories SET Specifics = '" + specifics.Replace("'", "''") + "' WHERE CategoryId = '" + categoryId + "'";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();
                }
            }

            cn.Close();

        }







        private int GetColumnIndex(ref Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet, string columnName)
        {
            Microsoft.Office.Interop.Excel.Range range = xlWorkSheet.UsedRange;

            int cCnt = 0;

            for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
            {
                try
                {

                    //if ((range.Cells[1, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2 != null && 
                    //    (range.Cells[1, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2.ToString() == columnName)
                    //    return cCnt;

                    string v = xlWorkSheet.Cells[1, cCnt].Value.ToString();

                    if (v == columnName)
                        return cCnt;
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            return -1;
        }

        private void btnImportEBayCatetories_Click(object sender, EventArgs e)
        {
            string fileName = @"C:\eBayApp\Instructions\CategoryIDs-US.xls";

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            string str;
            int rCnt = 0;
            int cCnt = 0;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = string.Empty;

            sqlString = @"truncate table ebay_categories";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;

            string categoryId = string.Empty;
            string categoryName = string.Empty;
            string categoryHierarchy = string.Empty;
            string columnsString = string.Empty;
            string valuesString = string.Empty;

            for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
            {
                categoryId = xlWorkSheet.Cells[rCnt, 1].Value.ToString();
                categoryName = xlWorkSheet.Cells[rCnt, 2].Value.ToString().Replace("'", "''");
                categoryHierarchy = xlWorkSheet.Cells[rCnt, 3].Value.ToString().Replace("'", "''");


                columnsString = "CategoryId,Category,";
                valuesString = "'" + categoryId + "','" + categoryName + "',";

                int i = 2;
                foreach (string categoryNode in categoryHierarchy.Split('>'))
                {
                    string node = categoryNode.Substring(0, categoryNode.LastIndexOf('(') - 1).Trim();

                    columnsString += "F" + i.ToString() + ",";
                    valuesString += "'" + node + "',";

                    i++;
                }

                columnsString = columnsString.Substring(0, columnsString.Length - 1);
                valuesString = valuesString.Substring(0, valuesString.Length - 1);


                sqlString = @"INSERT INTO eBay_Categories (" + columnsString + ") VALUES (" + valuesString + ")";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();

            }

            cn.Close();
        }

        private void label99_Click(object sender, EventArgs e)
        {

        }

        private void txtSell_TextChanged(object sender, EventArgs e)
        {
            if (txtCost.Text != "")
            {
                txt9Profit.Text = (Convert.ToDouble(txtSell.Text) * 1.09 - (Convert.ToDouble(txtSell.Text) * 0.119 + 0.3 + Convert.ToDouble(txtCost.Text) * 1.09)).ToString();
                txt6Profit.Text = (Convert.ToDouble(txtSell.Text) * 1.09 - (Convert.ToDouble(txtSell.Text) * 0.119 + 0.3 + Convert.ToDouble(txtCost.Text) * 1.06)).ToString();
                txt0Profit.Text = (Convert.ToDouble(txtSell.Text) * 1.09 - (Convert.ToDouble(txtSell.Text) * 0.119 + 0.3 + Convert.ToDouble(txtCost.Text) * 1.00)).ToString();
            }
        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {
            if (txtSell.Text != "")
            {
                txt9Profit.Text = (Convert.ToDouble(txtSell.Text) * 1.09 - (Convert.ToDouble(txtSell.Text) * 0.119 + 0.3 + Convert.ToDouble(txtCost.Text) * 1.09)).ToString();
                txt6Profit.Text = (Convert.ToDouble(txtSell.Text) * 1.09 - (Convert.ToDouble(txtSell.Text) * 0.119 + 0.3 + Convert.ToDouble(txtCost.Text) * 1.06)).ToString();
                txt0Profit.Text = (Convert.ToDouble(txtSell.Text) * 1.09 - (Convert.ToDouble(txtSell.Text) * 0.119 + 0.3 + Convert.ToDouble(txtCost.Text) * 1.00)).ToString();
            }
        }

        private void lvToAddCategory_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (bToAddCategoryCheckingAll)
                return;

            if (!bInit)
                return;

            if (bToAddTabEnteringGridRefreshed)
                return;

            gvAdd_Refresh();
        }

        private void tpToAdd_Leave(object sender, EventArgs e)
        {
            bToAddTabEnteringGridRefreshed = false;
        }

        private void chkToAddCategoryAll_CheckedChanged(object sender, EventArgs e)
        {
            bToAddCategoryCheckingAll = true;

            foreach (ListViewItem item in this.lvToAddCategory.Items)
            {
                item.Checked = chkToAddCategoryAll.Checked;
            }

            bToAddCategoryCheckingAll = false;

            gvAdd_Refresh();
        }

        private void lvToAddCategory_Click(object sender, EventArgs e)
        {
            bToAddTabEnteringGridRefreshed = false;
        }

        
    }
}
