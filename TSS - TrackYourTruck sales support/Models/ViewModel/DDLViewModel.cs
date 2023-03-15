using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetTrackModel;

namespace TSS.Models.ViewModel
{
    public class DDLViewModel
    {
        public List<IDdlSourceModel> List { get; set; }

        public string SelVal { get; set; }
    }
}