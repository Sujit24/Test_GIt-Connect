using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class LabelStockSize
    {
        /// <summary>
        /// Only valid values are 6 or 8
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Only valid values is 4
        /// </summary>
        public string Width { get; set; }
    }
}
