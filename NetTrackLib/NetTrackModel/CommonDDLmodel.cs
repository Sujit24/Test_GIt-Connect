using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class CommonDDLmodel : IDdlSourceModel
    {
        public CommonDDLmodel()
        {
        }
        public CommonDDLmodel(int sessionid, string spName)
        {
            this.sessionid = sessionid;
            this.SpName = spName;
        }

        public string keyfield { get; set; }
        

        public string value { get; set; }
       
        public string SpName { get; set; }

        public int sessionid { get; set; }

        public string groupValue { get; set; }
    }
}
