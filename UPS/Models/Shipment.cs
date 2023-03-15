using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class Shipment
    {
        public string Description { get; set; }
        public Shipper Shipper { get; set; }
        public ShipTo ShipTo { get; set; }
        public ShipFrom ShipFrom { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public Service Service { get; set; }
        public Package Package { get; set; }
        public ShipmentServiceOptions ShipmentServiceOptions { get; set; }
        public InvoiceLineTotal InvoiceLineTotal { get; set; }
    }

    public class InvoiceLineTotal
    {
        public string CurrencyCode { get; set; }
        public string MonetaryValue { get; set; }
    }
}
