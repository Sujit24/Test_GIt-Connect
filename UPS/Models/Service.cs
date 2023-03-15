using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class Service
    {
        /// <summary>
        /// 01 = Next Day Air
        /// 02 = 2nd Day Air
        /// 03 = Ground
        /// 07 = Express
        /// 08 = Expedited
        /// 11 = UPS Standard
        /// 12 = 3 Day Select
        /// 13 = Next Day Air Saver
        /// 14 = UPS Next Day Air Early
        /// 54 = Express Plus
        /// 59 = 2nd Day Air A.M.
        /// 65 = UPS Saver 
        /// 70 = UPS Access Point™ Economy
        /// M2 = First Class Mail
        /// M3 = Priority Mail 
        /// M4 = Expedited MaiI Innovations
        /// M5 = Priority Mail Innovations
        /// M6 = Economy Mail Innovations
        /// M7 = MaiI Innovations (MI) Returns
        /// 82 = UPS Today Standard
        /// 83 = UPS Today Dedicated Courier
        /// 
        /// The following Services are not available to return shipment:
        /// 13 - Next Day Air Saver
        /// 59 - 2nd Day Air A.M.
        /// 82 = UPS Today Standard
        /// 83 = UPS Today Dedicated Courier
        /// 84 = UPS Today Intercity
        /// 85 = UPS Today Express
        /// 86 = UPS Today Express Saver
        /// </summary>
        public string Code { get; set; }
        //public string Description { get; set; }
    }
}
