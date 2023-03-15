using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class UgsClientModel
    {
        [Required]
        [DisplayName("Client ID:")]
        public int ClientID { get; set; }

        [DisplayName("Accounting ID:")]
        public string AccID { get; set; }

        [Required]
        [DisplayName("Client Name:")]
        public string ClientName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Fax { get; set; }

        public string EMail { get; set; }

        [DisplayName("Date Created:")]
        public DateTime DateCreated { get; set; }

        //[Required]
        [DisplayName("Phone:")]
        public string ContactPhones { get; set; }

        //[Required]
        [DisplayName("Contact Name:")]
        public string ContactPerson { get; set; }

       // [Required]
        public string ClientStatus { get; set; }

        [DisplayName("Data Viewable for:")]
        public int? Retention { get; set; }

        [DisplayName("Data Retention Days:")]
        public int? DaysToKeepDataFor { get; set; }

        public string Comments { get; set; }

        public int? ProxyClientID { get; set; }

        [DisplayName("Map Expire(hours):")]
        public int? MapExpirationHours { get; set; }

        public string ShippingAddress1 { get; set; }

        public string ShippingAddress2 { get; set; }

        public string ShippingCity { get; set; }

        public string ShippingState { get; set; }

        public string ShippingZip { get; set; }

        [DisplayName("Mobile:")]
        public string MobilePhone { get; set; }

        public string FTIN { get; set; }

        [DisplayName("Map Access PIN:")]
        public string MapAccessPIN { get; set; }

        [DisplayName("Data Service PIN:")]
        public string DataServicePIN { get; set; }

        [DisplayName("Password Expiration (days):")]
        public int? PasswordExpire { get; set; }

        public string clienttype { get; set; }

        [DisplayName("Vehicle Name:")]
        public string unitname { get; set; }

        public int? masterclientid { get; set; }

        [DisplayName("Mobile Map URL:")]
        public string iaccessurl { get; set; }

        [DisplayName("Default Point Radius:")]
        public int? defaultradius { get; set; }

        public int? showkmlnames { get; set; }

        [DisplayName("Distance Measurement:")]
        public int? distanceunit { get; set; }

        public int? allowemergencypopup { get; set; }

        public int? SessionId { get; set; }

        [DisplayName("From Date:")]
        public DateTime FromDate { get; set; }

        [DisplayName("To Date:")]
        public DateTime ToDate { get; set; }

        [DisplayName("Available For(Days):")]
        public int? AvailableForDays { get; set; }

        [DisplayName("Restore History:")]
        public string RestoreHistory { get; set; }

        [DisplayName("Client KML URL:")]
        public string KML { get; set; }

        [DisplayName("User Quantity")]
        public int UsrQty { get; set; }

        public string action { get; set; }

        public string TYTVer { get; set; }

        public int? SubscriptionLevelId { get; set; }

        [DisplayName("Zoho Account ID")]
        public string ZohoAccountID { get; set; }

        [DisplayName("Zoho Contact ID")]
        public string ZohoContactID { get; set; }

        public string ZohoUserTz { get; set; }
        public int OrderId { get; set; }
    }
}