using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSS.Models.ViewModel
{
    public class ShippingAndHandlingViewModel
    {
        public string ShipToAddress { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountry { get; set; }

        public int TotalProduct { get; set; }
        public decimal PackageWeight { get; set; }
        public decimal PackageInsuredValue { get; set; }
    }
}