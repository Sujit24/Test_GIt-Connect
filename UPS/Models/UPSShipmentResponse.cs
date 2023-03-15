using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class UPSShipmentResponse
    {
        public ShipmentResponse ShipmentResponse { get; set; }
        public Fault Fault { get; set; }
    }

    
}
