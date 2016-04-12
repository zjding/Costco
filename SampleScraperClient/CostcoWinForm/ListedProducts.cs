using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostcoWinForm
{
    public partial class ListedProducts : Form
    {
        public ListedProducts()
        {
            InitializeComponent();
        }

        private void ListedProducts_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'costcoDataSet1.eBay_CurrentListings' table. You can move, or remove it, as needed.
            this.eBay_CurrentListingsTableAdapter.Fill(this.costcoDataSet1.eBay_CurrentListings);
            // TODO: This line of code loads data into the 'dataSet_eBayCurrentListing.eBay_CurrentListings' table. You can move, or remove it, as needed.
            this.eBay_CurrentListingsTableAdapter.Fill(this.dataSet_eBayCurrentListing.eBay_CurrentListings);

        }
    }
}
