using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class ShipperAddressModel
    {
        public string ShipperName { get; set; }

        public string ShipperAddressLine { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperState { get; set; }
        public string ShipperZip { get; set; }

        public string _ShipperCityStateZip;
        public string ShipperCityStateZip
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.ShipperCity) || !string.IsNullOrWhiteSpace(this.ShipperState) || !string.IsNullOrWhiteSpace(this.ShipperZip))
                    return this.ShipperCity + ", " + this.ShipperState + ", " + this.ShipperZip;
                return this._ShipperCityStateZip;
            }
            set
            {
                this._ShipperCityStateZip = value;
            }
        }


        public string ShipperAddressLine2 { get; set; }
        public string ShipperCity2 { get; set; }
        public string ShipperState2 { get; set; }
        public string ShipperZip2 { get; set; }

        public string _ShipperCityStateZip2;
        public string ShipperCityStateZip2
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.ShipperCity2) || !string.IsNullOrWhiteSpace(this.ShipperState2) || !string.IsNullOrWhiteSpace(this.ShipperZip2))
                    return this.ShipperCity2 + ", " + this.ShipperState2 + ", " + this.ShipperZip2;
                return this._ShipperCityStateZip2;
            }
            set
            {
                this._ShipperCityStateZip2 = value;
            }
        }
    }
}
