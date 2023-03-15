using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public enum UnitType
    {
        LBS,    //Pounds
        KGS,    //Kilograms
        IN,     //Inches
        CM      //Centimeters
    }

    public class UnitOfMeasurement
    {
        [JsonProperty("Code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UnitType UnitType { get; set; }

        //public string Description { get; set; }
    }
}
