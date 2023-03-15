using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSS.Models
{
    public class UPSShippingRequestModel
    {
        public int QuoteOrderId { get; set; }
        public string ShippingType { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }

        public int ShipFromId { get; set; }
        public string LabelImageType { get; set; }
    }
}