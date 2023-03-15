using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class BluePayACHTransactionModel
    {
        public int BluePayTransId { get; set; }
        public long TransactionId { get; set; }
        public int OrderId { get; set; }

        public string routingNum { get; set; }
        public string accountNum { get; set; }
        public string accountType { get; set; }
        public string docType { get; set; }
        public double Amount { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public DateTime PaymentDate { get; set; }
        public bool IsFundTransferred { get; set; }
        public string BackendId { get; set; }

    }
}
