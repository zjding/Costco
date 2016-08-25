﻿using System;
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

        SqlCommand cmdEBayListingChange_Discontinue;
        SqlDataAdapter daEBayListingChange_Discontinue;
        SqlCommandBuilder cmbEBayListingChange_Discontinue;
        DataSet dsEBayListingChange_Discontinue;
        DataTable dtEBayListingChange_Discontinue;

        public frmEBayListingChange_Discontinue()
        {
            InitializeComponent();
        }

        public frmEBayListingChange_Discontinue(string connectionString, string mode)
        {
            InitializeComponent();

            this.connectionString = connectionString;
            this.mode = mode;
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
            gvEBayListingChangeDiscontinue.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gvEBayListingChangeDiscontinue.Columns["UrlNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gvEBayListingChangeDiscontinue.Columns["eBayItemNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gvEBayListingChangeDiscontinue.Columns["CostcoUrl"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (mode == "Discontinue")
            {
                
            }
            else if (mode == "PriceUp" || mode == "PriceDown")
            {
                gvEBayListingChangeDiscontinue.Columns["CostcoOldPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["CostcoNewPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["eBayListingOldPrice"].Width = 50;
                gvEBayListingChangeDiscontinue.Columns["eBayListingNewPrice"].Width = 50;
            }
            else if (mode == "OptionChange")
            {
                gvEBayListingChangeDiscontinue.Columns["CostcoOldOptions"].Width = 150;
                gvEBayListingChangeDiscontinue.Columns["CostcoNewOptions"].Width = 150;
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

            if (e.ColumnIndex == 0)
            {
                foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.Rows)
                {
                    if (row.Cells["Select"].Value != null && ((bool)row.Cells["Select"].Value) == true)
                    {
                        this.gvEBayListingChangeDiscontinue.Rows[row.Index].Selected = true;
                        gvEBayListingChangeDiscontinue.Rows[row.Index].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        this.gvEBayListingChangeDiscontinue.Rows[row.Index].Selected = false;
                        gvEBayListingChangeDiscontinue.Rows[row.Index].DefaultCellStyle.BackColor = Color.White;
                    }
                    
                }
            }
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

            foreach (DataGridViewRow row in gvEBayListingChangeDiscontinue.SelectedRows)
            {
                st += row.Cells["UrlNumber"].Value.ToString() + ",";
            }
            st = st.Substring(0, st.Length - 1);

            string sqlString = string.Empty;

            if (mode == "Discontinue")
            {
                sqlString = @"INSERT INTO eBay_ToRemove (Name, CostcoUrlNumber, eBayItemNumber, CostcoUrl, InsertTime) 
                                 SELECT Name, UrlNumber, eBayItemNumber, CostcoUrl, GETDATE()
                                 FROM eBayListingChange_Discontinue 
                                 WHERE UrlNumber in (" + st + ")";
            }
            else if (mode == "PriceUp")
            {
                sqlString = @"INSERT INTO eBay_ToChange (Name, CostcoUrlNumber, eBayItemNumber, Url, CostcoOldPrice, CostcoNewPrice, eBayOldListingPirce, eBayNewListingPrice) 
                                 SELECT Name, UrlNumber, eBayItemNumber, CostcoUrl, CostcoOldPrice, CostcoNewPrice, eBayListingOldPrice, eBayListingNewPrice
                                 FROM eBayListingChange_PriceUp 
                                 WHERE UrlNumber in (" + st + ")";
            }
            else if (mode == "PriceDown")
            {
                sqlString = @"INSERT INTO eBay_ToChange (Name, CostcoUrlNumber, eBayItemNumber, Url, CostcoOldPrice, CostcoNewPrice, eBayOldListingPirce, eBayNewListingPrice) 
                                 SELECT Name, UrlNumber, eBayItemNumber, CostcoUrl, CostcoOldPrice, CostcoNewPrice, eBayListingOldPrice, eBayListingNewPrice
                                 FROM eBayListingChange_PriceDown 
                                 WHERE UrlNumber in (" + st + ")";
            }
            else if (mode == "OptionChange")
            {
                sqlString = @"INSERT INTO eBay_ToChange (Name, CostcoUrlNumber, eBayItemNumber, Url, OldOptions, NewOptions) 
                                 SELECT Name, UrlNumber, eBayItemNumber, CostcoUrl, CostcoOldOptions, CostcoNewOptions
                                 FROM eBayListingChange_OptionChange 
                                 WHERE UrlNumber in (" + st + ")";
            }

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

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

            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            this.Close();
        }
    }
}
