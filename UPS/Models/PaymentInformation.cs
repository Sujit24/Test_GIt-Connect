using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class PaymentInformation
    {
        public ShipmentCharge ShipmentCharge { get; set; }
    }

    public class ShipmentCharge
    {
        /// <summary>
        /// 01 = Transportation
        /// 02 = Duties and Taxes
        /// </summary>
        public string Type { get; set; }
        public BillShipper BillShipper { get; set; }
    }

    public class BillShipper
    {
        public string AccountNumber { get; set; }
    }
}
