using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostcoWinForm
{
    class DataLayer
    {
        public string connectionString; // = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DataLayer ()
        {
            
        }

        public List<Department> GetDepartmentsArray()
        {
            List<Department> categories = new List<Department>();

            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            sqlString = "SELECT * FROM Costco_Departments";
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Department category = new Department();
                    category.DepartmentName = reader["DepartmentName"].ToString();
                    category.CategoryName = reader["CategoryName"].ToString();
                    category.Url = reader["CategoryUrl"].ToString();
                    category.bInclude = Convert.ToBoolean(reader["bInclude"]);

                    categories.Add(category);
                }
            }
            reader.Close();
            cn.Close();

            return categories;
        }

        public List<Category> GetCategoryArray()
        {
            List<Category> categories = new List<Category>();

            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            sqlString = @"SELECT DISTINCT * FROM Costco_Categories
                            order by Category1, Category2, Category3, Category4, 
                            Category5, Category6, Category7, Category8";
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Category category = new Category();
                    category.Category1 = reader["Category1"].ToString();
                    category.Category2 = reader["Category2"] == null ? "" : reader["Category2"].ToString();
                    category.Category3 = reader["Category3"] == null ? "" : reader["Category3"].ToString();
                    category.Category4 = reader["Category4"] == null ? "" : reader["Category4"].ToString();
                    category.Category5 = reader["Category5"] == null ? "" : reader["Category5"].ToString();
                    category.Category6 = reader["Category6"] == null ? "" : reader["Category6"].ToString();
                    category.Category7 = reader["Category7"] == null ? "" : reader["Category7"].ToString();
                    category.Category8 = reader["Category8"] == null ? "" : reader["Category8"].ToString();

                    categories.Add(category);
                }
            }
            reader.Close();
            cn.Close();

            return categories;
        }

    }
}
