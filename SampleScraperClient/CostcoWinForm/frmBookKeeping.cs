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
    public partial class frmBookKeeping : Form
    {
        public string m_CategoryCode;
        public string m_Year;

        SqlCommand cmdBookKeeping;
        SqlDataAdapter daBookKeeping;
        SqlCommandBuilder cmbBookKeeping;
        DataSet dsBookKeeping;
        DataTable dtBookKeeping;

        string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        bool bIniting = false;

        public frmBookKeeping()
        {
            InitializeComponent();
        }


        private void frmBookKeeping_Load(object sender, EventArgs e)
        {
            bIniting = true;

            foreach (var item in cmbCategory.Items)
            {
                if (item.ToString().Contains(m_CategoryCode + " - "))
                {
                    cmbCategory.SelectedItem = item;
                    break;
                }
            }

            cmbYear.SelectedText = m_Year;

            gvBookKeeping_Refresh(m_CategoryCode, m_Year);

            bIniting = false;
        }

        private void gvBookKeeping_Refresh(string categoryCode, string year)
        {
            string yearStart = year + "/1/1";
            string yearEnd = year + "/12/31";

            string sqlString = @"SELECT ID, Date, Name, CategoryCode, Amount, Note, Receipt, Expense
                                 FROM BookKeeping ";

            sqlString += @"WHERE  Date between '" + yearStart + "' and '" + yearEnd + "' ";

            if (categoryCode != "All")
                sqlString += @"AND CategoryCode = '" + categoryCode + "' ";

            sqlString += @"Order by Date DESC";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdBookKeeping = new SqlCommand(sqlString, connection);
            daBookKeeping = new SqlDataAdapter(cmdBookKeeping);
            cmbBookKeeping = new SqlCommandBuilder(daBookKeeping);
            dsBookKeeping = new DataSet();
            daBookKeeping.Fill(dsBookKeeping, "tbBookKeeping");
            dtBookKeeping = dsBookKeeping.Tables["tbBookKeeping"];
            connection.Close();


            gvBookKeeping.DataSource = dsBookKeeping.Tables["tbBookKeeping"];
            gvBookKeeping.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gvBookKeeping.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gvBookKeeping.Columns[0].Visible = false;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bIniting)
                return;

            string selectedCategoryCode = cmbCategory.Text == "All" ? "All" : cmbCategory.Text.Split('-')[0].Trim();

            gvBookKeeping_Refresh(selectedCategoryCode, cmbYear.Text);
        }
    }
}
