using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel.Params
{
    public class OutParams
    {
        public int NewID { get; private set; }
        public int StatusCode { get; private set; }
        public string StatusMsg { get; private set; }
        public string SystemStatusMsg { get; private set; }
        public OutParams(int statusCode, string statusMsg, string sytemStatusMsg, int newId = -1)
        {
            NewID = newId;
            StatusCode = statusCode;
            StatusMsg = statusMsg;
            SystemStatusMsg = sytemStatusMsg;
        }
        public string ToJsonString()
        {
            StringBuilder r = new StringBuilder();
            r.Append("{ \"StatusCode\":\"" + StatusCode.ToString()+"\" , ");
            r.Append("\"StatusMsg\":\"" + StatusCode + "\" , }");
            r.Append("\"SystemStatusMsg\":\"" + SystemStatusMsg + "\" }");
            return r.ToString();
        }
    }
}
