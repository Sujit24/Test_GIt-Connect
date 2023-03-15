using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel
{
    public class QuoteOrderModel
    {
        public int SessionId { get; set; }
        public string Action { get; set; }

        public int QuoteOrderId { get; set; }
        public string SearchQuoteOrderId
        {
            get
            {
                return this.QuoteOrderId.ToString();
            }
        }

        public int QuoteId { get; set; }
        public string SearchQuoteId
        {
            get
            {
                return this.QuoteId.ToString();
            }
        }

        public string QuoteNumber { get; set; }
        public DateTime QuoteDate { get; set; }
        public string QuoteDateFormated { get; set; }
        public int ContractTerm { get; set; }
        public int ValidUntil { get; set; }
        public int SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string ZohoEntityId { get; set; }
        public string ZohoEntityType { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public string BillToCompanyName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }

        public string _BillToCityStateZip;
        public string BillToCityStateZip
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.BillToCity) || !string.IsNullOrWhiteSpace(this.BillToState) || !string.IsNullOrWhiteSpace(this.BillToZip))
                    return this.BillToCity + ", " + this.BillToState + ", " + this.BillToZip;
                return this._BillToCityStateZip;
            }
            set
            {
                this._BillToCityStateZip = value;
            }
        }
        public string BillToCity { get; set; }
        public string BillToState { get; set; }
        public string BillToZip { get; set; }
        public string BillToCountry { get; set; }
        public string BillToBillingContact { get; set; }
        public string BillToBillingEmail { get; set; }
        public string BillToPhone { get; set; }

        public bool IsShipSameAsBill { get; set; }

        public string ShipToCompanyName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }

        private string _ShipToCityStateZip;
        public string ShipToCityStateZip
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.ShipToCity) || !string.IsNullOrWhiteSpace(this.ShipToState) || !string.IsNullOrWhiteSpace(this.ShipToZip))
                    return this.ShipToCity + ", " + this.ShipToState + ", " + this.ShipToZip;
                return this._ShipToCityStateZip;
            }
            set
            {
                this._ShipToCityStateZip = value;
            }
        }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountry { get; set; }
        public string ShipToBillingContact { get; set; }
        public string ShipToBillingEmail { get; set; }
        public string ShipToPhone { get; set; }

        public double ShippingAndHandling { get; set; }
        public string ShippingAndHandlingType { get; set; }
        public string ShippingAndHandlingTypeTitle { get; set; }

        public string CustomerName { get; set; }
        public string Qty { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UrlSendDate { get; set; }
        public string Url { get; set; }
        public DateTime? LastViewDate { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public string CreatedDateFormated { get; set; }
        public string UrlSendDateFormated { get; set; }
        public string LastViewDateFormated { get; set; }
        public string PurchaseDateFormated { get; set; }

        public string PdfToken { get; set; }

        public Boolean Purchased { get; set; }
        public Boolean Processed { get; set; }

        public int OrderStatusId { get; set; }
        public string StatusTitle { get; set; }

        public bool IsAccepted { get; set; }
        public string AcceptanceName { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public string AcceptanceDateFormated { get; set; }

        public double SalesTax { get; set; }

        public int QuotePaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodComment { get; set; }

        public DateTime SearchFromDate { get; set; }
        public DateTime SearchToDate { get; set; }

        public int NettrackClientStatusId { get; set; }
        public string NettrackStatus { get; set; }

        public string SalesPersonEmail { get; set; }

        public string SalesPersonCellPhone { get; set; }

        public string TransactionId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpire { get; set; }

        public string Note { get; set; }
        public bool InvalidShippingAndHandling { get; set; }

        public bool IsDemo { get; set; }
        public double TotalWeight { get; set; }

        public string SalesOrderRowColor { get; set; }
        public bool IsShipped { get; set; }

        public int TssOrderTypeId { get; set; }
        public string OrderTypeTitle { get; set; }

        public string ShippingBoxSize { get; set; }
    }
}
