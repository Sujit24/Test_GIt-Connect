using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class SalesOrderModel
    {
        public int SessionId { get; set; }
        public string Action { get; set; }

        public int SalesOrderId { get; set; }
        public int QuoteOrderId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public int PackageId { get; set; }
        public string ProductCategory { get; set; }
        public int UserQuantity { get; set; }
        public bool Purchased { get; set; }
        public bool Processed { get; set; }

        public DateTime? PurchaseDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string SKU { get; set; }
        public string ProductDescription { get; set; }

        public string PurchaseDateFormated { get; set; }
        public string UpdatedDateFormated { get; set; }

        public bool Deleted { get; set; }
        
        public int SortOrder { get; set; }
    }
}
