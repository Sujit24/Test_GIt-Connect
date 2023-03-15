using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class ClientModel
    {
        public int SessionID { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhones { get; set; }
        public string ContactPerson { get; set; }
        public string Comments { get; set; }
        public string ClientStatus { get; set; }
        public int Retention { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string EMail { get; set; }
        public string Fax { get; set; }
        public DateTime DateCreated { get; set; }
        public int proxyclientid { get; set; }
        public int MapExpirationHours { get; set; }
        public string ShippingAddress1 { get; set; }
        public string ShippingAddress2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZip { get; set; }
        public string MobilePhone { get; set; }
        public string FTIN { get; set; }
        public string DataServicePIN { get; set; }
        public string MapAccessPIN { get; set; }
        public DateTime RestoreFrom { get; set; }
        public int password_expire { get; set; }
        public string unitname { get; set; }
        public string clienttype { get; set; }
        public int masterclientid { get; set; }
        public int defaultradius { get; set; }
        public string KML { get; set; }
        public int showKMLnames { get; set; }
        public int distanceunit { get; set; }
        public int allowemergencypopup { get; set; }


        //Default constructor
        public ClientModel() { }
        public ClientModel(int clientId, string clientName) {
            this.ClientID = clientId;
            this.ClientName = clientName;
        }
    }
}
