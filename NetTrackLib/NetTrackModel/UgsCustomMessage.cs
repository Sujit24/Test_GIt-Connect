using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class UgsCustomMessage
    {
        public int sessionid { get; set; }

        public string formmode { get; set; }

        public int? custommsgid { get; set; }

        public string custommessage { get; set; }
    }
}