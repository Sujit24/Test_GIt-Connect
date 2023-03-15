using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel.Params
{
    public class StatusCodesInputParams:InputParams
    {
        public int TruckStatus { get; private set; }
        public StatusCodesInputParams(int sessionId, int statusCode = 0)
            : base(sessionId)
        {
            TruckStatus = statusCode;
        }
    }
}
