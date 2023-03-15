using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class BluePaySettingsModel
    {
        public string ServerName { get; set; }
        public string AccountName { get; set; }
        public string AccountId { get; set; }
        public string SecretKey { get; set; }
        public string CurrentMode { get; set; }
        public string LiveUrl { get; set; }
        public string TestUrl { get; set; }
    }
}
