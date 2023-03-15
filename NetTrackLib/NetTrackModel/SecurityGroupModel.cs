using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class SecurityGroupModel
    {
        public long SecurityGroupId { get; set; }
        public string GroupName { get; set; }

        public string Action { get; set; }
    }
}
