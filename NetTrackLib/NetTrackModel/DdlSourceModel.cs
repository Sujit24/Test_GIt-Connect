using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class DdlSourceModel
    {
        public string keyfield { get; set; }

        public string value { get; set; }

        public DdlSourceModel()
        {
            
        }

        public DdlSourceModel(string text,string key)
        {
            keyfield = key;
            value = text;
        }
    }
}
