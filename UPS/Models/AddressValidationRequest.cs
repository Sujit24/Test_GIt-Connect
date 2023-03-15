using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class AVRequest
    {
        public AVRAccessRequest AccessRequest { get; set; }
        public AVRAddressValidationRequest AddressValidationRequest { get; set; }
    }

    public class AVRAccessRequest
    {
        public string AccessLicenseNumber { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class AVRAddressValidationRequest
    {
        public AVRRequest Request { get; set; }
        public AVRAddress Address { get; set; }
    }

    public class AVRRequest
    {
        public string RequestAction { get; set; }
    }

    public class AVRAddress
    {
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
    }
}
