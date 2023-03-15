using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class ShipmentResults
    {
        public ShipmentCharges ShipmentCharges { get; set; }
        public BillingWeight BillingWeight { get; set; }
        public string ShipmentIdentificationNumber { get; set; }
        public PackageResults PackageResults { get; set; }
    }
}
