using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostcoWinForm
{
    public partial class ProductInfoForm : Form
    {
        public ProductInfoForm()
        {
            InitializeComponent();
        }

        private void ProductInfoForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'costcoDataSet.ProductInfo' table. You can move, or remove it, as needed.
            this.productInfoTableAdapter.Fill(this.costcoDataSet.ProductInfo);

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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                string imageUrl = (this.dataGridView1.Rows[e.RowIndex].Cells[8]).FormattedValue.ToString(); 
                if (imageUrl != "")
                {
                    e.Value = GetImageFromUrl(imageUrl);
                }
                    
            }
        }
    }
}
