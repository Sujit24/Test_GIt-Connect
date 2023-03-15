using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class QuoteLinkMappingModel
    {
        public int QuoteLinkMappingId { get; set; }
        public int QuoteId { get; set; }
        public string EncryptValue { get; set; }
        public string DecryptValue { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
        public string Recipient { get; set; }
    }
}
