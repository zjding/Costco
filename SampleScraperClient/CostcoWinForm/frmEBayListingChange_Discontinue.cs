using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostcoWinForm
{
    public partial class frmEBayListingChange_Discontinue : Form
    {
        public string connectionString = string.Empty;
        public string mode = string.Empty;
        public eBayFrontEnd parent;

        SqlCommand cmdEBayListingChange_Discontinue;
        SqlDataAdapter daEBayListingChange_Discontinue;
        SqlCommandBuilder cmbEBayListingChange_Discontinue;
        DataSet dsEBayListingChange_Discontinue;
        DataTable dtEBayListingChange_Discontinue;

        public frmEBayListingChange_Discontinue()
        {
            InitializeComponent();
        }

        public frmEBayListingChange_Discontinue(eBayFrontEnd parent, string connectionString, string mode)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.mode = mode;
            this.parent = parent;
        }

        private void frmEBayListingChange_Discontinue_Load(object sender, EventArgs e)
        {
            gvEBayListingChangeDiscontinue_Refresh();
        }

        public void gvEBayListingChangeDiscontinue_Refresh()
        {
            string sqlString = string.Empty;

            if (mode == "Discontinue")
            {
                sqlString = @"SELECT * FROM eBayListingChange_Discontinue ";
            }
            else if (mode == "PriceUp")
            {
                sqlString = @"SELECT * FROM eBayListingChange_PriceUp ";
            }
            else if (mode == "PriceDown")
            {
                sqlString = @"SELECT * FROM eBayListingChange_PriceDown ";
            }
            else if (mode == "OptionChange")
            {
                sqlString = @"SELECT * FROM eBayListingChange_OptionChange ";
            }
            else if (mode == "CostcoPriceDown")
            {
                sqlString = @"  SELECT * FROM CostcoInventoryChange_PriceDown n
                                WHERE n.UrlNumber NOT IN (SELECT CostcoUrlNumber FROM eBay_CurrentListings) 
                                AND n.UrlNumber NOT IN (SELECT UrlNumber FROM eBay_ToAdd)";
            }
            else if (mode == "CostcoNewProducts")
            {
                sqlString = @"  SELECT * FROM CostcoInventoryChange_New n 
                                WHERE n.UrlNumber NOT IN (SELECT CostcoUrlNumber FROM eBay_CurrentListings) 
                                AND n.UrlNumber NOT IN (SELECT UrlNumber FROM eBay_ToAdd)";
            }
            else if (mode == "CostcoDiscountProducts")
            {
                sqlString = @"  SELECT * FROM ProductInfo n 
                                WHERE n.UrlNumber NOT IN (SELECT CostcoUrlNumber FROM eBay_CurrentListings) 
                                AND n.UrlNumber NOT IN (SELECT UrlNumber FROM eBay_ToAdd) 
                                AND LEN(LTRIM(RTRIM(n.Discount))) > 2";
            }
            else if (mode == "CostcoClearanceProducts")
            {
                sqlString = @"  SELECT *
                                FROM ProductInfo n 
                                WHERE n.UrlNumber NOT IN (Select CostcoUrlNumber FROM eBay_CurrentListings) 
                                AND n.UrlNumber not in (Select UrlNumber FROM eBay_ToAdd) 
                                AND convert(varchar(10), n.Price) like '%.97%'  ";
            }

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdEBayListingChange_Discontinue = new SqlCommand(sqlString, connection);
            daEBayListingChange_Discontinue = new SqlDataAdapter(cmdEBayListingChange_Discontinue);
            cmbEBayListingChange_Discontinue = new SqlCommandBuilder(daEBayListingChange_Discontinue);
            dsEBayListingChange_Discontinue = new DataSet();
            daEBayListingChange_Discontinue.Fill(dsEBayListingChange_Discontinue, "tbEBayListingChange_Discontinue");
            dtEBayListingChange_Discontinue = dsEBayListingChange_Discontinue.Tables["tbEBayListingChange_Discontinue"];
            connection.Close();

            gvEBayListingChangeDiscontinue.DataSource = dsEBayListingChange_Discontinue.Tables["tbEBayListingChange_Discontinue"];

            gvEBayListingChangeDiscontinue.Columns["Select"].Width = 20;
            gvEBayListingChangeDiscontinue.Columns["ID"].Visible = false;
            gvEBayListingChangeDiscontinue.Columns["Name"].Width = 250;
            gvEBayListingChangeDiscontinue.Columns["UrlNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //gvEBayListingChangeDiscontinue.Columns["CostcoUrl"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (mode == "Discontinue")
            {
                gvEBayListingChangeDiscontinue.Columns["eBayItemNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            else if (mode == "PriceUp" || mode == "PriceDown")
            {
                gvEBayListingChangeDiscontinue.Columns["eBayItemNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gvEBayListingChangeDiscontinue.Columns["CostcoOldPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["CostcoNewPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["eBayListingOldPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["eBayListingNewPrice"].Width = 50;
            }
            else if (mode == "OptionChange")
            {
                gvEBayListingChangeDiscontinue.Columns["eBayItemNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                gvEBayListingChangeDiscontinue.Columns["CostcoOldOptions"].Width = 150;
                gvEBayListingChangeDiscontinue.Columns["CostcoNewOptions"].Width = 150;
            }
            else if (mode == "CostcoPriceUp" || mode == "CostcoPriceDown")
            {
                gvEBayListingChangeDiscontinue.Columns["CostcoOldPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["CostcoNewPrice"].Width = 50;
            }
            else if (mode == "CostcoNewProducts")
            {
                gvEBayListingChangeDiscontinue.Columns["Details"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["Specification"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["ImageLink"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["NumberOfImage"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["CostcoUrl"].Width = 150;
            }
            else if (mode == "CostcoDiscountProducts" || mode == "CostcoClearanceProducts")
            {
                gvEBayListingChangeDiscontinue.Columns["Details"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["Specification"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["ImageLink"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["NumberOfImage"].Visible = false;
                gvEBayListingChangeDiscontinue.Columns["Url"].Width = 150;
            }

            gvEBayListingChangeDiscontinue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.SelectedRows)
            {
                gvEBayListingChangeDiscontinue.Rows.RemoveAt(row.Index);
            }

            daEBayListingChange_Discontinue.Update(dtEBayListingChange_Discontinue);
        }

        private void gvEBayListingChangeDiscontinue_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            //if (e.ColumnIndex == 0)
            //{
            //    foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.Rows)
            //    {
            //        if (row.Cells["Select"].Value != null && ((bool)row.Cells["Select"].Value) == true)
            //        {
            //            this.gvEBayListingChangeDiscontinue.Rows[row.Index].Selected = true;
            //            gvEBayListingChangeDiscontinue.Rows[row.Index].DefaultCellStyle.BackColor = Color.Yellow;
            //        }
            //        else
            //        {
            //            this.gvEBayListingChangeDiscontinue.Rows[row.Index].Selected = false;
            //            gvEBayListingChangeDiscontinue.Rows[row.Index].DefaultCellStyle.BackColor = Color.White;
            //        }
                    
            //    }
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            daEBayListingChange_Discontinue.Update(dtEBayListingChange_Discontinue);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string st = string.Empty;

            foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                {
                    st += "'" + row.Cells["UrlNumber"].Value.ToString() + "',";
                }
            }
            st = st.Substring(0, st.Length - 1);

            string sqlString = string.Empty;

            if (mode == "Discontinue")
            {
                eBayFrontEnd.UploadDelete(st);
            }
            else if (mode == "PriceUp" || mode == "PriceDown")
            {
                List<ProductUpdate> products = new List<ProductUpdate>();

                string eBayItemNumbers = string.Empty;

                foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].Value) == true && row.Cells["eBayListingNewPrice"].Value.ToString().Trim() != "")
                    {
                        ProductUpdate p = new ProductUpdate();
                        p.eBayItemNumbr = Convert.ToString(row.Cells["eBayItemNumber"].Value);
                        p.NewPrice = Convert.ToDecimal(row.Cells["eBayListingNewPrice"].Value);
                        products.Add(p);

                        eBayItemNumbers += "'" + p.eBayItemNumbr + "',";
                    }
                }

                eBayFrontEnd.UploadPriceChange(products);
            }
            else if (mode == "OptionChange")
            {
                sqlString = @"INSERT INTO eBay_ToChange (Name, CostcoUrlNumber, eBayItemNumber, Url, OldOptions, NewOptions, NewImageOptions, ImageLink, eBayOldListingPrice, Thumb) 
                                 SELECT o.Name, o.UrlNumber, o.eBayItemNumber, o.CostcoUrl, o.CostcoOldOptions, o.CostcoNewOptions, o.CostcoNewImageOptions, o.ImageLink, c.eBayListingPrice, o.Thumb
                                 FROM eBayListingChange_OptionChange o, eBay_CurrentListings c
                                 WHERE o.eBayItemNumber = c.eBayItemNumber
                                 AND UrlNumber in (" + st + ")";
            }
            else if (mode == "CostcoPriceDown" || mode == "CostcoNewProducts" || mode == "CostcoDiscountProducts" || mode == "CostcoClearanceProducts")
            {
                parent.AddToEBayToAdd(st);
            }

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            if (!string.IsNullOrEmpty(sqlString))
            {
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();
            }

            if (mode == "Discontinue")
            {
                sqlString = @"DELETE FROM eBayListingChange_Discontinue
                          WHERE UrlNumber in (" + st + ")";
            }
            else if (mode == "PriceUp")
            {
                sqlString = @"DELETE FROM eBayListingChange_PriceUp
                          WHERE UrlNumber in (" + st + ")";
            }
            else if (mode == "PriceDown")
            {
                sqlString = @"DELETE FROM eBayListingChange_PriceDown
                          WHERE UrlNumber in (" + st + ")";
            }
            else if (mode == "OptionChange")
            {
                sqlString = @"DELETE FROM eBayListingChange_OptionChange
                          WHERE UrlNumber in (" + st + ")";
            }

            if (!string.IsNullOrEmpty(sqlString))
            {
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();
            }

            this.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string st = string.Empty;

            foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                {
                    st += "'" + row.Cells["UrlNumber"].Value.ToString() + "',";
                }
            }
            st = st.Substring(0, st.Length - 1);

            string sqlString = string.Empty;

            if (mode == "OptionChange")
            {
                sqlString  = @" UPDATE ProductInfo 
                                SET ProductInfo.Options = eBay_ToChange.NewOptions,   
                                    ProductInfo.ImageOptions = eBay_ToChange.NewImageOptions
                                FROM ProductInfo, eBay_ToChange
                                WHERE ProductInfo.UrlNumber = eBay_ToChange.CostcoUrlNumber
                                AND ProductInfo.UrlNumber in (" + st + "); ";

                sqlString += @"  UPDATE eBay_ToChange SET DeleteTime = GETDATE()
                                WHERE CostcoUrlNumber in (" + st + ") AND DeleteTime is NULL; ";

                sqlString += @" DELETE eBayListingChange_OptionChange WHERE UrlNumber in (" + st + "); ";
            }
            else if (mode == "PriceUp")
            {
                sqlString = @" UPDATE ProductInfo 
                                SET ProductInfo.Price = eBay_ToChange.CostcoNewPrice
                                FROM ProductInfo, eBay_ToChange
                                WHERE ProductInfo.UrlNumber = eBay_ToChange.CostcoUrlNumber
                                AND ProductInfo.UrlNumber in (" + st + "); ";

                sqlString = @" UPDATE eBay_CurrentListings 
                                SET eBay_CurrentListings.eBayListingPrice = eBay_ToChange.eBayNewListingPrice
                                FROM eBay_CurrentListings, eBay_ToChange
                                WHERE eBay_CurrentListings.CostcoUrlNumber = eBay_ToChange.CostcoUrlNumber
                                AND eBay_CurrentListings.CostcoUrlNumber in (" + st + "); ";

                sqlString += @"  UPDATE eBay_ToChange SET DeleteTime = GETDATE()
                                WHERE CostcoUrlNumber in (" + st + ") AND DeleteTime is NULL; ";

                sqlString += @" DELETE eBayListingChange_PriceUp WHERE UrlNumber in (" + st + "); ";

            }
            else if (mode == "PriceDown")
            {
                sqlString = @" UPDATE ProductInfo 
                                SET ProductInfo.Price = eBay_ToChange.CostcoNewPrice
                                FROM ProductInfo, eBay_ToChange
                                WHERE ProductInfo.UrlNumber = eBay_ToChange.CostcoUrlNumber
                                AND ProductInfo.UrlNumber in (" + st + "); ";

                sqlString = @" UPDATE eBay_CurrentListings 
                                SET eBay_CurrentListings.eBayListingPrice = eBay_ToChange.eBayNewListingPrice
                                FROM eBay_CurrentListings, eBay_ToChange
                                WHERE eBay_CurrentListings.CostcoUrlNumber = eBay_ToChange.CostcoUrlNumber
                                AND eBay_CurrentListings.CostcoUrlNumber in (" + st + "); ";

                sqlString += @"  UPDATE eBay_ToChange SET DeleteTime = GETDATE()
                                WHERE CostcoUrlNumber in (" + st + ") AND DeleteTime is NULL; ";

                sqlString += @" DELETE eBayListingChange_PriceDown WHERE UrlNumber in (" + st + "); ";

            }

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            cn.Close();

            this.Close();
        }

        private void gvEBayListingChangeDiscontinue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gvEBayListingChangeDiscontinue.Columns[e.ColumnIndex].Name == "Image")
            {
                string imageUrl = string.Empty;

                if (mode == "CostcoNewProducts" || mode == "CostcoDiscountProducts" || mode == "CostcoClearanceProducts")
                    imageUrl = (this.gvEBayListingChangeDiscontinue.Rows[e.RowIndex].Cells[15]).FormattedValue.ToString();
                else if (mode == "CostcoPriceDown")
                    imageUrl = (this.gvEBayListingChangeDiscontinue.Rows[e.RowIndex].Cells[9]).FormattedValue.ToString();
                else if (mode == "OptionChange")
                    imageUrl = (this.gvEBayListingChangeDiscontinue.Rows[e.RowIndex].Cells[11]).FormattedValue.ToString();
                else if (mode == "PriceUp" || mode == "PriceDown")
                    imageUrl = (this.gvEBayListingChangeDiscontinue.Rows[e.RowIndex].Cells[12]).FormattedValue.ToString();

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

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.Rows)
            {
                row.Cells["Select"].Value = chkAll.Checked;
            }
        }
    }
}
