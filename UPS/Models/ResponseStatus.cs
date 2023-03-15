using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public enum Status
    {
        Successful = 1,
        Failure = 0
    }

    public class ResponseStatus
    {
        [JsonProperty("Code")]
        public Status Status { get; set; }

        public string Description { get; set; }
    }
}
