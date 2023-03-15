using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class BluePayLogModel
    {
        public int BluePayLogId { get; set; }
        public int QuoteId { get; set; }
        public string SubmittedUrl { get; set; }
        public string ReceivedUrl { get; set; }
    }
}
