using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class ShipmentResponse
    {
        public Response Response { get; set; }
        public ShipmentResults ShipmentResults { get; set; }
    }
}
