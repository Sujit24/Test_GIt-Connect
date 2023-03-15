using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class UgsNewZohoContacts
    {
        public string ZohoAccID { get; set; }
        public List<newZohoUser> newUserList { get; set; }

    }

    public class newZohoUser
    {
        public string LogIn { get; set; }
        public string Pin { get; set; }
    }
}
