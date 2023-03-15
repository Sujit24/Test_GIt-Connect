using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class UgClientsModel
    {
        public int SessionId { get; set; }

        public int clientid { get; set; }

        public string ClientName { get; set; }
        
        public string AccID { get; set; }

        public string MailingAddress { get; set; }

        public string ShippingAddress { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string MobilePhone { get; set; }

        public string Comments { get; set; }

        public string ClientStatus { get; set; }

        public string MapAccessPin { get; set; }

        public string DataServicePin { get; set; }

        public int MapExpireHours { get; set; }

        public int DataStorageDays { get; set; }

        public string AllowReadOnlyOn { get; set; }

        public DateTime DateCreated { get; set; }

        public string FTIN { get; set; }
    }
}