using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSS.Models.ViewModel
{
    public class MailViewModel
    {
        public int QuoteId { get; set; }
        public string MailTo { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string OrderUrl { get; set; }
        public string ApproveUrl { get; set; }

        public List<HttpPostedFileBase> EmailAttachment { get; set; }

        public bool IsLocalMail { get; set; }
        public bool IsAttachDoc { get; set; }
    }
}