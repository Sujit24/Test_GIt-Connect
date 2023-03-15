using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class UgUserModel
    {
        public int SessionId { get; set; }

        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Login { get; set; }
    }
}