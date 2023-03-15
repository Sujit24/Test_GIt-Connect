using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class TransportationCharges : Charges { }
    public class ServiceOptionsCharges : Charges { }
    public class TotalCharges : Charges { }

    public class ShipmentCharges
    {
        public TransportationCharges TransportationCharges { get; set; }
        public ServiceOptionsCharges ServiceOptionsCharges { get; set; }
        public TotalCharges TotalCharges { get; set; }
    }
}
