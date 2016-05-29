using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostcoWinForm
{
    class Product
    {
        public string Name { get; set; } = "";
        public string UrlNumber { get; set; } = "";
        public string ItemNumber { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; } 
        public decimal Shipping { get; set; }
        public string Discount { get; set; } = "";
        public string Limit { get; set; } = "";
        public string Details { get; set; } = "";
        public string Specification { get; set; } = "";
        public string ImageLink { get; set; } = "";
        public string Url { get; set; } = "";
        public int NumberOfImage { get; set; }

        public int DescriptionImageWidth { get; set; }
        public int DescriptionImageHeight { get; set; }

        public decimal eBayReferencePrice { get; set; }

        public string eBayItemNumber { get; set; } = "";
        public string eBayUrl { get; set; } = "";
        public string eBayCategoryID { get; set; } = "";
        public decimal eBayListingPrice { get; set; } 

    }
}
