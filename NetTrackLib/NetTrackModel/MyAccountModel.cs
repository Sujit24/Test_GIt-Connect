using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class MyAccountModel
    {
        public int MyAccountId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string CardNumber { get; set; }
        public string CVV2 { get; set; }
        public string CardExpireYear { get; set; }
        public string CardExpireMonth { get; set; }
        public string CardHolderFirstName { get; set; }
        public string CardHolderLastName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
