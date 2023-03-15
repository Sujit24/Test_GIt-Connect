using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class TssShippingModel
    {
        public int TssShippingId { get; set; }
        public int QuoteOrderId { get; set; }
        public int QuoteId { get; set; }
        public string ShippingType { get; set; }
        public double ShippingCost { get; set; }
        public double Weight { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public string TrackingNo { get; set; }
        public string LabelImage { get; set; }
        public DateTime? SentEmailDate { get; set; }
        public string SentEmailDateFormated { get; set; }
        public string ShipToCountry { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormated { get; set; }

        public string ShipToCompanyName { get; set; }
        public string ShipToBillingContact { get; set; }
        public string ShipToBillingEmail { get; set; }
    }
}
