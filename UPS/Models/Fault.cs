using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class Fault
    {
        [JsonProperty("faultcode")]
        public string FaultCode { get; set; }

        [JsonProperty("faultstring")]
        public string FaultString { get; set; }

        [JsonProperty("detail")]
        public FaultDetail Detail { get; set; }
    }

    public class FaultDetail
    {
        public Errors Errors { get; set; }
    }

    public class Errors
    {
        public ErrorDetail ErrorDetail { get; set; }
    }

    public class ErrorDetail
    {
        public string Severity { get; set; }
        public PrimaryErrorCode PrimaryErrorCode { get; set; }
    }

    public class PrimaryErrorCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
