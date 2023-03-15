using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public enum ImageFormatType
    {
        PDF,
        GIF,
        PNG,
        EPL, //EPL2
        SPL,
        ZPL
    }

    public class ImageFormat
    {
        [JsonProperty("Code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ImageFormatType ImageFormatType { get; set; }

        //public string Description { get; set; }
    }
}
