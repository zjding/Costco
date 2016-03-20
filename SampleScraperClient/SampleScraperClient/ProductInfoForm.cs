using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleScraperClient
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
    }
}
