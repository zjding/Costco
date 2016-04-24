using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostcoWinForm
{
    class eBayListingProduct
    {
        public string Name { get; set; }
        public string eBayListingName { get; set; }
        public string eBayCategoryID { get; set; }
        public string eBayItemNumber { get; set; }
        public decimal eBayListingPrice { get; set; }
        public string eBayDescription { get; set; }
        public DateTime eBayListingDT { get; set; }
        public string eBayUrl { get; set; }
        public string CostcoUrlNumber { get; set; }
        public string CostcoItemNumber { get; set; }
        public string CostcoUrl { get; set; }
        public decimal CostcoPrice { get; set; }
        public string ImageLink { get; set; }
    }
}
