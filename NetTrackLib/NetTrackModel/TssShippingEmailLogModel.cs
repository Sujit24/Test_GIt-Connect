using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class TssShippingEmailLogModel
    {
        public int TssShippingId { get; set; }
        public string MessageId { get; set; }
        public string Recipent { get; set; }
        public string Event { get; set; }
        public DateTime EventTime { get; set; }
        public string EventTimeFormated { get; set; }
    }
}
