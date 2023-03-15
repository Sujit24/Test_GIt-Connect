using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSS.Models.ViewModel
{
    public class EmailTemplateViewModel
    {
        public int EmailTemplateId { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Template { get; set; }
    }
}