using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel.Params
{
    public class InputParams
    {
        public int SessionID { get; private set; }
        public string action { get; set; }
        public InputParams(int sessionId) 
        { 
            SessionID = sessionId; 
        }
    }
}
