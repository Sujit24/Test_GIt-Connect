using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class SecurityGroupSalesPersonModel
    {
        public int SecurityGroupSalesPersonId { get; set; }
        public int SecurityGroupId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WebLogin { get; set; }
        public bool IsInGroup { get; set; }

        public string ClientIds { get; set; }
        public string Action { get; set; }
    }
}
