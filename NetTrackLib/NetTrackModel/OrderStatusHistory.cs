using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class OrderStatusHistory
    {
        public int OrderStatusHistoryId { get; set; }
        public int QuoteOrderId { get; set; }
        public int OrderStatusId { get; set; }
        public string StatusTitle { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusDateFormated { get; set; }
    }
}
