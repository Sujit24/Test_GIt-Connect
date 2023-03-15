using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class LabelSpecification
    {
        public ImageFormat LabelImageFormat { get; set; }
        public LabelStockSize LabelStockSize { get; set; }
        public LabelPrintMethod LabelPrintMethod { get; set; }
    }
}
