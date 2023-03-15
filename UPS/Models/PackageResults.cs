using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class PackageResults
    {
        public string TrackingNumber { get; set; }
        public ServiceOptionsCharges ServiceOptionsCharges { get; set; }
        public ShippingLabel ShippingLabel { get; set; }
    }
}
