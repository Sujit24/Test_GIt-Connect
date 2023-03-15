using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class EmailTemplateModel
    {
        public int EmailTemplateId { get; set; }
        public string Title { get; set; }
        public string Template { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormatted { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateFormatted { get; set; }
    }
}
