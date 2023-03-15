using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class QuoteProductModel
    {
        public int SessionId { get; set; }
        public string Action { get; set; }

        public int QuoteProductId { get; set; }
        public int QuoteId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public int PackageId { get; set; }
        public string ProductCategory { get; set; }

        public int UserQuantity { get; set; }

        public string SKU { get; set; }
        public string ProductDescription { get; set; }

        public bool Deleted { get; set; }

        public int SortOrder { get; set; }
    }
}
