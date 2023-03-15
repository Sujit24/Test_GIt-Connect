using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class SalesTaxModel
    {
        public int SalesTaxId { get; set; }
        public string Country { get; set; }
        public string StateFullName { get; set; }
        public string StateShortName { get; set; }
        public double TaxRate { get; set; }
    }
}
