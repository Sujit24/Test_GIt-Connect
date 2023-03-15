using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class TSSSentEmailModel
    {
        public int TSSSentEmailId { get; set; }
        public int QuoteId { get; set; }
        public string CustomerName { get; set; }
        public string EmailId { get; set; }
        public int SentById { get; set; }
        public string Recipent { get; set; }
        public string SenderName { get; set; }
        public DateTime SentDate { get; set; }
        public string SentDateFormated { get; set; }

        public string EmailStatus { get; set; }
        public DateTime? EmailStatusTime { get; set; }
        public string EmailStatusTimeFormated { get; set; }

        public DateTime SearchFromDate { get; set; }
        public DateTime SearchToDate { get; set; }

        public string MessageBody { get; set; }
        public int Resend { get; set; }

        public string SalesPersonName { get; set; }
        public string SalesPersonEmail { get; set; }
    }
}
