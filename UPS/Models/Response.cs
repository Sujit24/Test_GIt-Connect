using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class Response
    {
        public ResponseStatus ResponseStatus { get; set; }
        public TransactionReference TransactionReference { get; set; }
    }
}
