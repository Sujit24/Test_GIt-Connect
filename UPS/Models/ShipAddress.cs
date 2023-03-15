using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class ShipAddress
    {
        public string Name { get; set; }
        public string AttentionName { get; set; }
        //public Phone Phone { get; set; }
        public Address Address { get; set; }
    }

    public class Phone
    {
        public string Number { get; set; }
    }

    public class Address
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }


    public class Shipper : ShipAddress
    {
        //public string TaxIdentificationNumber { get; set; }
        public string ShipperNumber { get; set; }
        public Phone Phone { get; set; }
    }

    public class ShipTo : ShipAddress
    {
        public Phone Phone { get; set; }
    }

    public class ShipFrom : ShipAddress { }
}
