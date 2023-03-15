using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class QuoteLastViewModel
    {
        public int QuoteLastViewId { get; set; }
        public int QuoteId { get; set; }
        public DateTime LastViewDate { get; set; }
        public string LastViewDateFormated { get; set; }
    }
}
