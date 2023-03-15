using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class QuoteEmailUnsubscribeModel
    {
        public int QuoteEmailUnsubscribeId { get; set; }
        public int QuoteId { get; set; }
        public string Email { get; set; }
    }
}
