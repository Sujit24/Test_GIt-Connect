using System;
using System.Collections.Generic;

namespace NetTrackModel
{
    public class UserSessionModel
    {
        // user state property
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string SessionAlive { get; set; }
    }
}
