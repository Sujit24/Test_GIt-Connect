using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public interface IDdlSourceModel
    {
        string keyfield { get; set; }

        string value { get; set; }

        string SpName { get; }

        int sessionid { get; set; }
        string groupValue { get; set; }
    }
}