using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class AVSingleResponse
    {
        public AddressValidationSingleResponse AddressValidationResponse { get; set; }
    }
    public class AddressValidationSingleResponse
    {
        public AVRResponse Response { get; set; }
        public AVRAddressValidationResult AddressValidationResult { get; set; }
    }


    public class AVResponse
    {
        public AddressValidationResponse AddressValidationResponse { get; set; }
    }

    public class AddressValidationResponse
    {
        public AVRResponse Response { get; set; }
        public List<AVRAddressValidationResult> AddressValidationResult { get; set; }

        public AddressValidationResponse()
        {
            AddressValidationResult = new List<AVRAddressValidationResult>();
        }
    }

    public class AVRResponse
    {
        public string ResponseStatusCode { get; set; }
        public string ResponseStatusDescription { get; set; }
    }

    public class AVRAddressValidationResult
    {
        public string Rank { get; set; }
        public string Quality { get; set; }
        public AVRAddress Address { get; set; }
        public string PostalCodeLowEnd { get; set; }
        public string PostalCodeHighEnd { get; set; }
    }
}
