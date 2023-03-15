using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class Package
    {
        //public string Description { get; set; }
        public Packaging Packaging { get; set; }
        public Dimensions Dimensions { get; set; }
        public PackageWeight PackageWeight { get; set; }
    }

    public class Packaging
    {
        /// <summary>
        /// Package types. Values are:
        /// 01 = UPS Letter
        /// 02 = Customer Supplied Package
        /// 03 = Tube
        /// 04 = PAK
        /// 21 = UPS Express Box
        /// 24 = UPS 25KG Box
        /// 25 = UPS 10KG Box
        /// 30 = Pallet
        /// 2a = Small Express Box
        /// 2b = Medium Express Box
        /// 2c = Large Express Box
        /// 56 = Flats 57 = Parcels
        /// 58 = BPM 59 = First Class
        /// 60 = Priority
        /// 61 = Machinables
        /// 62 = Irregulars
        /// 63 = Parcel Post
        /// 64 = BPM Parcel
        /// 65 = Media Mail
        /// 66 = BPM Flat
        /// 67 = Standard Flat
        /// 
        /// Package type 24 or 25 is only allowed for shipment without return service Packaging type must be valid for all the following: 
        /// ShipTo country, ShipFrom country, a shipment going from ShipTo country to ShipFrom country, all accessorial at both the shipment
        /// and package level, and the shipment service type. UPS will not accept raw wood pallets and please refer the UPS packaging 
        /// guidelines for pallets on UPS.com.
        /// </summary>
        public string Code { get; set; }
        //public string Description { get; set; }
    }

    public class Dimensions
    {
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class PackageWeight
    {
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }
}
