using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class UPSShippingRequest
    {
        public UPSSecurity UPSSecurity { get; set; }
        public ShipmentRequest ShipmentRequest { get; set; }
    }

    public class ShipmentRequest
    {
        public Request Request { get; set; }
        public Shipment Shipment { get; set; }
        public LabelSpecification LabelSpecification { get; set; }
    }
}
