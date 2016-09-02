using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
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
    public partial class FrmCostcoCategories : Form
    {
        SqlCommand cmdCostcoCategories;
        SqlDataAdapter daCostcoCategories;
        SqlCommandBuilder cmbCostcoCategories;
        DataSet dsCostcoCategories;
        DataTable dtCostcoCategories;
        //string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public string connectionString; // = "Server=tcp:zjding.database.windows.net,1433;Initial Catalog=Costco;Persist Security Info=False;User ID=zjding;Password=G4indigo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public FrmCostcoCategories()
        {
            InitializeComponent();
        }

        private void FrmCostcoCategories_Load(object sender, EventArgs e)
        {
            string sqlString = @"SELECT * 
                                 FROM Costco_Departments";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmdCostcoCategories = new SqlCommand(sqlString, connection);
            daCostcoCategories = new SqlDataAdapter(cmdCostcoCategories);
            cmbCostcoCategories = new SqlCommandBuilder(daCostcoCategories);
            dsCostcoCategories = new DataSet();
            daCostcoCategories.Fill(dsCostcoCategories, "tbCostcoCategories");
            dtCostcoCategories = dsCostcoCategories.Tables["tbCostcoCategories"];
            connection.Close();


            gvCostcoCategories.DataSource = dsCostcoCategories.Tables["tbCostcoCategories"];
            gvCostcoCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gvCostcoCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gvCostcoCategories.Columns[0].Visible = false;
        }

        private void btnImportCategories_Click(object sender, EventArgs e)
        {
            IWebDriver driver;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cn.Open();

            string sqlString = "TRUNCATE TABLE Costco_Departments";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), TimeSpan.FromSeconds(180));

            driver.Navigate().GoToUrl("http://www.costco.com/view-more.html");

            var viewMoreColumns = driver.FindElements(By.ClassName("viewmore-column"));

            foreach (var v in viewMoreColumns)
            {
                var viewMoreHeader = v.FindElement(By.ClassName("viewmore-column-header"));

                string department = viewMoreHeader.Text;

                var lis = v.FindElements(By.TagName("li"));

                foreach (var l in lis)
                {
                    var a = l.FindElement(By.TagName("a"));

                    string url = a.GetAttribute("href");
                    string category = a.Text;

                    sqlString = @"INSERT INTO Costco_Departments (DepartmentName, CategoryName, CategoryUrl) VALUES ('" + department + "','" + url + "','" + category.Replace("'", "''") + "')";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();
                }
            }

            cn.Close();
            driver.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvCostcoCategories_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            daCostcoCategories.Update(dtCostcoCategories);
        }
    }
}
