using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class QuoteTemplateModel : QuoteModel
    {
        public string TemplateName { get; set; }
        public bool IsActive { get; set; }
        public int QuoteTemplateGroupId { get; set; }
        public string GroupName { get; set; }
    }
}
