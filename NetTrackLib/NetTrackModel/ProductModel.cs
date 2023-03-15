using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NetTrackModel
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public int ClientId { get; set; }
        
        public string SKU { get; set; }
        
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        
        public double Price { get; set; }

        public double Weight { get; set; }
        
        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }

        public int DiscountProductTypeId { get; set; }

        public string DiscountProductTypeName { get; set; }

        public int SessionId { get; set; }
        public string Action { get; set; }

        public string Carrier { get; set; }
        public string Notes { get; set; }
        public string ProductImageFileName { get; set; }
    }
}
