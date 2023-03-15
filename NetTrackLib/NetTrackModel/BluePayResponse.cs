using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public enum BluePayStatus
    {
        Success = 1,
        Failed = 0
    }

    public class BluePayResponse
    {
        public string Message { get; set; }
        public BluePayStatus Status { get; set; }
    }
}
