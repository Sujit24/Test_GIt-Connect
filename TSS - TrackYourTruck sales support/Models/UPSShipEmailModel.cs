using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSS.Models
{
    public class UPSShipEmailModel
    {
        public int TssShippingId { get; set; }
        public int QuoteOrderId { get; set; }
        public string TrackingNo { get; set; }
        public string ShippingMailTo { get; set; }

        [AllowHtml]
        public string ShippingMailBody { get; set; }
    }
}