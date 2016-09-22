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
        public string eBayName { get; set; } = "";
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
        public string ImageOptions { get; set; } = "";
        public string Options { get; set; } = "";
        public string Thumb { get; set; } = "";

        public int DescriptionImageWidth { get; set; }
        public int DescriptionImageHeight { get; set; }

        public decimal eBayReferencePrice { get; set; }

        public string eBayItemNumber { get; set; } = "";
        public string eBayUrl { get; set; } = "";
        public string eBayCategoryID { get; set; } = "";
        public decimal eBayListingPrice { get; set; }

        public string TemplateName { get; set; } = "";
        public string Specifics { get; set; } = "";
    }

    public class ProductUpdate
    {
        public string eBayItemNumbr { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public string NewOptions { get; set; }
        public string OldOptions { get; set; }
        public string NewImageOptions { get; set; }
    }

    class eBaySoldProduct
    {
        public string PaypalTransactionID { get; set; }
        public DateTime PaypalPaidDateTime { get; set; }
        public string PaypalPaidEmailPdf { get; set; }
        public string eBayItemNumber { get; set; }
        public DateTime eBaySoldDateTime { get; set; }
        public string eBayItemName { get; set; }
        public int eBayListingQuality { get; set; }
        public int eBaySoldQuality { get; set; }
        public int eBayRemainingQuality { get; set; }
        public decimal eBaySoldPrice { get; set; }
        public string eBayUrl { get; set; }
        public string eBaySoldEmailPdf { get; set; }
        public string BuyerName { get; set; }
        public string BuyerID { get; set; }
        public string BuyerAddress1 { get; set; }
        public string BuyerAddress2 { get; set; }
        public string BuyerCity { get; set; }
        public string BuyerState { get; set; }
        public string BuyerZip { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerNote { get; set; }
        public string CostcoUrlNumber { get; set; }
        public string CostcoUrl { get; set; }
        public decimal CostcoPrice { get; set; }
        public decimal CostcoTax { get; set; }
        public string CostcoOrderNumber { get; set; }
        public string CostcoOrderDate { get; set; }
        public string CostcoItemName { get; set; }
        public string CostcoItemNumber { get; set; }
        public string CostcoTrackingNumber { get; set; }
        public string CostcoShipDate { get; set; }
        public string CostcoTaxExemptPdf { get; set; }
        public string CostcoOrderEmailPdf { get; set; }
        public string CostcoShipEmailPdf { get; set; }
    }
}
