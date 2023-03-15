using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public enum RequestOption
    {
        /// <summary>
        /// No street level address validation would be performed, but Postal Code/State combination validation would still be performed.
        /// </summary>
        nonvalidate,

        /// <summary>
        /// No street level address validation would be performed, but City/State/Postal Code combination validation would still be performed.
        /// </summary>
        validate
    }

    public class Request
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestOption RequestOption { get; set; }

        public TransactionReference TransactionReference { get; set; }
    }
}
