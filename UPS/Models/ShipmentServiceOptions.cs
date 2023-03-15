using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class ShipmentServiceOptions
    {
        //public Notification Notification { get; set; }
        public string DirectDeliveryOnlyIndicator { get; set; }
    }

    public class Notification
    {
        /// <summary>
        /// 5 - QV In-transit Notification
        /// 6 - QV Ship Notification
        /// 7 - QV Exception Notification
        /// 8 - QV Delivery Notification
        /// 2 - Return Notification or Label Creation Notification
        /// 012 - Alternate Delivery Location Notification
        /// 013 - UAP Shipper Notification.
        /// </summary>
        public string NotificationCode { get; set; }

        public EMail EMail { get; set; }
    }

    public class EMail
    {
        public string EMailAddress { get; set; }
        //public string FromEMailAddress { get; set; }
    }
}
