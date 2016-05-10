using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostcoWinForm
{
    class DataLayer
    {
        private string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<Category> GetCategoryArray()
        {
            List<Category> categories = new List<Category>();

            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();
            sqlString = "SELECT * FROM Categories";
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Category category = new Category();
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

    }
}
