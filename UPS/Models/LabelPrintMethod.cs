using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UPS.Models
{
    public enum PrintMethodType
    {
        GIF,
        PNG,
        EPL, //EPL2
        SPL,
        ZPL
    }

    public class LabelPrintMethod
    {
        [JsonProperty("Code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PrintMethodType PrintMethodType { get; set; }

        //public string Description { get; set; }
    }
}
