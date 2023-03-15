using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel.Params
{
    public class TerminalInputParams:InputParams
    {
        public int terminalid { get; private set; }
        public string action  { get; set; }
        public TerminalInputParams(int sessionId,int terminalId = 0)
            : base(sessionId)
        {
            terminalid = terminalId;
        }
    }
}
