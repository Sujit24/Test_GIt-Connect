using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections;


namespace NetTrackRepository
{
    public class ProductRepository
    {
        private ProductModel _ProductModel;
        private DBProduct _DBProduct;

        public ProductRepository()
        {
            this._DBProduct = new DBProduct();
        }

        public List<ProductModel> GetProductList(int sessionId)
        {
            ProductModel productModel = null;
            List<ProductModel> productModelList = new List<ProductModel>();

            DataTable dtProduct = _DBProduct.GetProductList(sessionId);
            foreach (DataRow dr in dtProduct.Rows)
            {
                productModel = new ProductModel();
                productModel.ProductId = Convert.ToInt32(dr["ProductId"]);
                productModel.ClientId = Convert.ToInt32(dr["ClientId"]);
                productModel.SKU = dr["SKU"].ToString();
                productModel.ProductName = dr["ProductName"].ToString();
                productModel.ProductDescription = dr["ProductDescription"].ToString();
                productModel.Price = Convert.ToDouble(dr["Price"]);
                productModel.Weight = dr["Weight"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["Weight"]);
                productModel.ProductTypeId = Convert.ToInt32(dr["ProductTypeId"]);
                productModel.DiscountProductTypeId = dr["DiscountProductTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DiscountProductTypeId"]);
                productModel.DiscountProductTypeName = dr["DiscountProductTypeName"] == DBNull.Value ? "" : Convert.ToString(dr["DiscountProductTypeName"]);
                productModel.ProductTypeName = dr["ProductTypeName"].ToString();
                productModel.Carrier = dr["Carrier"] == DBNull.Value ? "" : Convert.ToString(dr["Carrier"]);
                productModel.Notes = dr["Notes"] == DBNull.Value ? "" : Convert.ToString(dr["Notes"]);
                productModel.ProductImageFileName = dr["ProductImageFileName"] == DBNull.Value ? "" : Convert.ToString(dr["ProductImageFileName"]);

                productModelList.Add(productModel);
            }

            return productModelList;
        }

        public ProductModel GetDetailProductInfo(ProductModel productModel)
        {
            DataTable dtProduct = _DBProduct.GetDetailProductInfo(productModel);
            foreach (DataRow dr in dtProduct.Rows)
            {
                productModel = new ProductModel();
                productModel.ProductId = Convert.ToInt32(dr["ProductId"]);
                productModel.ClientId = Convert.ToInt32(dr["ClientId"]);
                productModel.SKU = dr["SKU"].ToString();
                productModel.ProductName = dr["ProductName"].ToString();
                productModel.ProductDescription = dr["ProductDescription"].ToString();
                productModel.Price = Convert.ToDouble(dr["Price"]);
                productModel.Weight = dr["Weight"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["Weight"]);
                productModel.ProductTypeId = Convert.ToInt32(dr["ProductTypeId"]);
                productModel.ProductTypeName = dr["ProductTypeName"].ToString();
                productModel.DiscountProductTypeId = dr["DiscountProductTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DiscountProductTypeId"]);
                productModel.DiscountProductTypeName = dr["DiscountProductTypeName"] == DBNull.Value ? "" : Convert.ToString(dr["DiscountProductTypeName"]);
                productModel.Carrier = dr["Carrier"] == DBNull.Value ? "" : Convert.ToString(dr["Carrier"]);
                productModel.Notes = dr["Notes"] == DBNull.Value ? "" : Convert.ToString(dr["Notes"]);
                productModel.ProductImageFileName = dr["ProductImageFileName"] == DBNull.Value ? "" : Convert.ToString(dr["ProductImageFileName"]);
            }

            return productModel;
        }

        public List<DdlSourceModel> GetProductTypeList(ProductModel productModel)
        {
            DdlSourceModel userTypeModel = null;
            List<DdlSourceModel> productTypeModelList = new List<DdlSourceModel>();

            DataTable dtProductType = _DBProduct.GetProductTypeList(productModel);
            foreach (DataRow dr in dtProductType.Rows)
            {
                userTypeModel = new DdlSourceModel();
                userTypeModel.keyfield = dr["keyfield"].ToString();
                userTypeModel.value = dr["value"].ToString();

                productTypeModelList.Add(userTypeModel);
            }

            return productTypeModelList;
        }

        public ProductModel SaveProduct(ProductModel model)
        {
            return _DBProduct.SaveProduct(model); ;
        }

        public QuoteProductModel SaveQuoteProduct(QuoteProductModel model)
        {
            return _DBProduct.SaveQuoteProduct(model); ;
        }

        public SalesOrderModel SaveSalesOrder(SalesOrderModel model)
        {
            return _DBProduct.SaveSalesOrder(model); ;
        }

        public QuoteModel SaveQuote(QuoteModel model)
        {
            return _DBProduct.SaveQuote(model); ;
        }

        public void ResetQuoteStartDate(int quoteId)
        {
            _DBProduct.ResetQuoteStartDate(quoteId);
        }

        public void ApproveQuote(QuoteModel model)
        {
            _DBProduct.ApproveQuote(model);
        }

        public QuoteOrderModel SaveQuoteOrder(QuoteOrderModel model)
        {
            return _DBProduct.SaveQuoteOrder(model); ;
        }

        public List<SalesPerson> GetSalesPersonList(int sessionId)
        {
            SalesPerson salesPerson = null;
            List<SalesPerson> salesPersonList = new List<SalesPerson>();

            DataTable dtSalesPerson = _DBProduct.GetSalesPersonList(sessionId);
            foreach (DataRow dr in dtSalesPerson.Rows)
            {
                salesPerson = new SalesPerson();
                salesPerson.EmployeeId = dr["EmployeeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EmployeeID"]);
                salesPerson.Name = dr["Name"] == DBNull.Value ? "" : Convert.ToString(dr["Name"]);
                salesPerson.Email = dr["UserEmail"] == DBNull.Value ? "" : Convert.ToString(dr["UserEmail"]);
                salesPerson.CellPhone = dr["CellPhoneAddr"] == DBNull.Value ? "" : Convert.ToString(dr["CellPhoneAddr"]);

                salesPersonList.Add(salesPerson);
            }

            return salesPersonList;
        }

        public List<SalesPerson> GetSalesPersonList(string clientIdList)
        {
            SalesPerson salesPerson = null;
            List<SalesPerson> salesPersonList = new List<SalesPerson>();

            DataTable dtSalesPerson = _DBProduct.GetSalesPersonList(clientIdList);
            foreach (DataRow dr in dtSalesPerson.Rows)
            {
                salesPerson = new SalesPerson();
                salesPerson.EmployeeId = dr["EmployeeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EmployeeID"]);
                salesPerson.Name = dr["Name"] == DBNull.Value ? "" : Convert.ToString(dr["Name"]);
                salesPerson.FirstName = dr["First_Name"] == DBNull.Value ? "" : Convert.ToString(dr["First_Name"]);
                salesPerson.LastName = dr["Last_Name"] == DBNull.Value ? "" : Convert.ToString(dr["Last_Name"]);
                salesPerson.Login = dr["Login"] == DBNull.Value ? "" : Convert.ToString(dr["Login"]);
                salesPerson.Email = dr["UserEmail"] == DBNull.Value ? "" : Convert.ToString(dr["UserEmail"]);
                salesPerson.CellPhone = dr["CellPhoneAddr"] == DBNull.Value ? "" : Convert.ToString(dr["CellPhoneAddr"]);
                salesPerson.IsSalesPerson = Convert.ToBoolean(dr["IsSalesPerson"]);

                salesPersonList.Add(salesPerson);
            }

            return salesPersonList;
        }

        public void SaveSalesPerson(int employeeId, bool isSet)
        {
            _DBProduct.SaveSalesPerson(employeeId, isSet);
        }

        public List<QuoteModel> GetQuoteList(QuoteModel model)
        {
            QuoteModel quoteModel = null;
            List<QuoteModel> quoteModelList = new List<QuoteModel>();

            DataTable dtQuote = _DBProduct.GetQuoteList(model);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteModel = new QuoteModel();
                quoteModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteModel.QuoteNumber = dr["QuoteNumber"].ToString();
                quoteModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                quoteModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteModel.CompanyName = dr["CompanyName"] == DBNull.Value ? "" : Convert.ToString(dr["CompanyName"]);
                quoteModel.CustomerName = dr["CustomerName"] == DBNull.Value ? "" : Convert.ToString(dr["CustomerName"]);
                quoteModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                quoteModel.CreatedDateFormated = quoteModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");
                if (dr["UrlSendDate"] != DBNull.Value)
                    quoteModel.UrlSendDate = Convert.ToDateTime(dr["UrlSendDate"]);

                quoteModel.UrlSendDateFormated = quoteModel.UrlSendDate.HasValue ? quoteModel.UrlSendDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";
                quoteModel.Url = dr["Url"] == DBNull.Value ? "" : dr["Url"].ToString();
                quoteModel.LastViewCount = Convert.ToInt32(dr["LastViewCount"]);
                if (dr["LastViewDate"] != DBNull.Value)
                {
                    quoteModel.LastViewDate = Convert.ToDateTime(dr["LastViewDate"]);
                    quoteModel.LastViewDateFormated = quoteModel.LastViewDate.Value.ToString("MM-dd-yyyy hh:mm tt");// + " (" + quoteModel.LastViewCount.ToString() + ")";
                }

                quoteModel.Qty = dr["Qty"].ToString();
                quoteModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);
                quoteModel.BillToBillingEmail = dr["BillToBillingEmail"] == DBNull.Value ? "" : dr["BillToBillingEmail"].ToString();

                quoteModel.ShipToAddress1 = dr["ShipToAddress1"] == DBNull.Value ? "" : dr["ShipToAddress1"].ToString();
                quoteModel.ShipToAddress2 = dr["ShipToAddress2"] == DBNull.Value ? "" : dr["ShipToAddress2"].ToString();
                quoteModel.ShipToCity = dr["ShipToCity"] == DBNull.Value ? "" : dr["ShipToCity"].ToString();
                quoteModel.ShipToState = dr["ShipToState"] == DBNull.Value ? "" : dr["ShipToState"].ToString();
                quoteModel.ShipToZip = dr["ShipToZip"] == DBNull.Value ? "" : dr["ShipToZip"].ToString();

                quoteModel.StatusTitle = dr["StatusTitle"] == DBNull.Value ? "" : dr["StatusTitle"].ToString();

                quoteModel.ShippingAndHandling = Convert.ToDouble(dr["ShippingAndHandling"]);
                quoteModel.ShippingAndHandlingType = dr["ShippingAndHandlingType"] == DBNull.Value ? "" : dr["ShippingAndHandlingType"].ToString();

                quoteModel.UnsubscribeCount = Convert.ToInt32(dr["UnsubscribeCount"]);

                quoteModel.Note = dr["Note"] == DBNull.Value ? "" : dr["Note"].ToString();

                quoteModel.IsApproved = dr["IsApproved"] == DBNull.Value ? "" : dr["IsApproved"].ToString();
                quoteModel.ApprovedUserName = dr["ApprovedUserName"] == DBNull.Value ? "" : dr["ApprovedUserName"].ToString();
                if (dr["ApprovedDate"] != DBNull.Value)
                    quoteModel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                quoteModel.ApprovedDateFormated = quoteModel.ApprovedDate.HasValue ? quoteModel.ApprovedDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                quoteModel.OrderTypeTitle = dr["OrderTypeTitle"] == DBNull.Value ? "" : dr["OrderTypeTitle"].ToString();

                quoteModel.MailGunEvent = dr["MailGunEvent"] == DBNull.Value ? "" : dr["MailGunEvent"].ToString();

                quoteModelList.Add(quoteModel);
            }

            return quoteModelList;
        }

        public List<QuoteOrderModel> GetSalesList(QuoteOrderModel model)
        {
            QuoteOrderModel quoteOrderModel = null;
            List<QuoteOrderModel> quoteModelList = new List<QuoteOrderModel>();

            DataTable dtQuote = _DBProduct.GetSalesList(model);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteOrderModel = new QuoteOrderModel();
                quoteOrderModel.QuoteOrderId = Convert.ToInt32(dr["QuoteOrderId"]);
                quoteOrderModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                //quoteModel.QuoteNumber = dr["QuoteNumber"].ToString();
                quoteOrderModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                quoteOrderModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteOrderModel.CustomerName = dr["CustomerName"].ToString();
                quoteOrderModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                quoteOrderModel.CreatedDateFormated = quoteOrderModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");
                if (dr["UrlSendDate"] != DBNull.Value)
                    quoteOrderModel.UrlSendDate = Convert.ToDateTime(dr["UrlSendDate"]);

                quoteOrderModel.UrlSendDateFormated = quoteOrderModel.UrlSendDate.HasValue ? quoteOrderModel.UrlSendDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";
                quoteOrderModel.Url = dr["Url"] == DBNull.Value ? "" : dr["Url"].ToString();
                if (dr["LastViewDate"] != DBNull.Value)
                {
                    quoteOrderModel.LastViewDate = Convert.ToDateTime(dr["LastViewDate"]);
                    quoteOrderModel.LastViewDateFormated = quoteOrderModel.LastViewDate.Value.ToString("MM-dd-yyyy hh:mm tt");
                }

                if (dr["PurchaseDate"] != DBNull.Value)
                    quoteOrderModel.PurchaseDate = Convert.ToDateTime(dr["PurchaseDate"]);

                quoteOrderModel.PurchaseDateFormated = quoteOrderModel.PurchaseDate.HasValue ? quoteOrderModel.PurchaseDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                quoteOrderModel.Qty = dr["Qty"].ToString();
                quoteOrderModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);

                quoteOrderModel.OrderStatusId = dr["OrderStatusId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrderStatusId"]);
                quoteOrderModel.StatusTitle = dr["StatusTitle"] == DBNull.Value ? "" : dr["StatusTitle"].ToString();
                quoteOrderModel.PaymentMethod = dr["PaymentMethod"] == DBNull.Value ? "" : dr["PaymentMethod"].ToString();
                quoteOrderModel.PaymentMethodComment = dr["PaymentMethodComment"] == DBNull.Value ? "" : dr["PaymentMethodComment"].ToString();

                quoteOrderModel.NettrackClientStatusId = dr["NettrackClientStatusId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["NettrackClientStatusId"]);
                quoteOrderModel.NettrackStatus = dr["NettrackStatus"] == DBNull.Value ? "" : dr["NettrackStatus"].ToString();

                quoteOrderModel.TransactionId = dr["TransactionId"] == DBNull.Value ? "" : dr["TransactionId"].ToString();
                quoteOrderModel.CardNumber = dr["CardNumber"] == DBNull.Value ? "" : dr["CardNumber"].ToString();
                quoteOrderModel.CardExpire = dr["CardExpire"] == DBNull.Value ? "" : dr["CardExpire"].ToString();

                quoteOrderModel.Note = dr["Note"] == DBNull.Value ? "" : dr["Note"].ToString();
                quoteOrderModel.InvalidShippingAndHandling = Convert.ToBoolean(dr["InvalidShippingAndHandling"]);
                quoteOrderModel.IsShipped = Convert.ToInt32(dr["IsShipped"]) == 0 ? false : true;

                quoteOrderModel.OrderTypeTitle = dr["OrderTypeTitle"] == DBNull.Value ? "" : dr["OrderTypeTitle"].ToString();

                quoteOrderModel.ShippingAndHandlingType = dr["ShippingAndHandlingType"] == DBNull.Value ? "" : dr["ShippingAndHandlingType"].ToString();
                if (!string.IsNullOrWhiteSpace(quoteOrderModel.ShippingAndHandlingType))
                {
                    if (quoteOrderModel.ShippingAndHandlingType == "2Day")
                    {
                        quoteOrderModel.ShippingAndHandlingTypeTitle = "2nd day AIR";
                    }
                    else if (quoteOrderModel.ShippingAndHandlingType == "NextAir")
                    {
                        quoteOrderModel.ShippingAndHandlingTypeTitle = "Next day AIR";
                    }
                    else
                    {
                        quoteOrderModel.ShippingAndHandlingTypeTitle = "UPS " + quoteOrderModel.ShippingAndHandlingType;
                    }
                }

                quoteModelList.Add(quoteOrderModel);
            }

            return quoteModelList;
        }

        public QuoteModel GetQuoteInfo(QuoteModel quoteModel)
        {
            DataTable dtQuote = _DBProduct.GetQuoteInfo(quoteModel);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteModel = new QuoteModel();
                quoteModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteModel.TssOrderTypeId = Convert.ToInt32(dr["TssOrderTypeId"]);
                quoteModel.OrderTypeTitle = dr["OrderTypeTitle"] == DBNull.Value ? "" : dr["OrderTypeTitle"].ToString();
                quoteModel.QuoteNumber = dr["QuoteNumber"].ToString();
                quoteModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                quoteModel.QuoteDateFormated = quoteModel.QuoteDate.ToString("MM/dd/yyyy");
                quoteModel.QuoteStartDate = dr["QuoteStartDate"] == DBNull.Value ? quoteModel.QuoteDate : Convert.ToDateTime(dr["QuoteStartDate"]);
                quoteModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteModel.SalesPersonName = dr["SalesPersonName"].ToString();
                quoteModel.SalesPersonEmail = dr["SalesPersonEmail"] == DBNull.Value ? "" : dr["SalesPersonEmail"].ToString();
                quoteModel.SalesPersonCellPhone = dr["SalesPersonCellPhone"] == DBNull.Value ? "" : dr["SalesPersonCellPhone"].ToString();
                quoteModel.ClientId = dr["ClientID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ClientID"]);
                quoteModel.ClientName = dr["ClientName"].ToString();

                quoteModel.ContractTerm = Convert.ToInt32(dr["ContractTerm"]);
                quoteModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);
                quoteModel.ZohoEntityId = dr["ZohoEntityId"].ToString();
                quoteModel.ZohoEntityType = dr["ZohoEntityType"].ToString();
                quoteModel.BillToCompanyName = dr["BillToCompanyName"].ToString();
                quoteModel.BillToAddress1 = dr["BillToAddress1"].ToString();
                quoteModel.BillToAddress2 = dr["BillToAddress2"].ToString();
                //quoteModel.BillToCityStateZip = dr["BillToCityStateZip"].ToString();
                quoteModel.BillToCity = dr["BillToCity"].ToString();
                quoteModel.BillToState = dr["BillToState"].ToString();
                quoteModel.BillToZip = dr["BillToZip"].ToString();
                quoteModel.BillToCountry = dr["BillToCountry"] == DBNull.Value ? "" : dr["BillToCountry"].ToString();
                quoteModel.BillToBillingContact = dr["BillToBillingContact"].ToString();
                quoteModel.BillToBillingEmail = dr["BillToBillingEmail"].ToString();
                quoteModel.BillToPhone = dr["BillToPhone"].ToString();
                quoteModel.IsShipSameAsBill = Convert.ToBoolean(dr["IsShipSameAsBill"]);
                quoteModel.ShipToCompanyName = dr["ShipToCompanyName"].ToString();
                quoteModel.ShipToAddress1 = dr["ShipToAddress1"].ToString();
                quoteModel.ShipToAddress2 = dr["ShipToAddress2"].ToString();
                //quoteModel.ShipToCityStateZip = dr["ShipToCityStateZip"].ToString();
                quoteModel.ShipToCity = dr["ShipToCity"].ToString();
                quoteModel.ShipToState = dr["ShipToState"].ToString();
                quoteModel.ShipToZip = dr["ShipToZip"].ToString();
                quoteModel.ShipToCountry = dr["ShipToCountry"] == DBNull.Value ? "" : dr["ShipToCountry"].ToString();
                quoteModel.ShipToBillingContact = dr["ShipToBillingContact"].ToString();
                quoteModel.ShipToBillingEmail = dr["ShipToBillingEmail"].ToString();
                quoteModel.ShipToPhone = dr["ShipToPhone"].ToString();
                quoteModel.ShippingAndHandling = Convert.ToDouble(dr["ShippingAndHandling"]);
                quoteModel.ShippingAndHandlingType = dr["ShippingAndHandlingType"] == DBNull.Value ? "" : dr["ShippingAndHandlingType"].ToString();
                quoteModel.LastViewDate = dr["LastViewDate"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["LastViewDate"]);
                quoteModel.Purchased = Convert.ToBoolean(dr["Purchased"]);
                quoteModel.Processed = Convert.ToBoolean(dr["Processed"]);

                quoteModel.IsAccepted = dr["IsAccepted"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAccepted"]);
                quoteModel.AcceptanceName = dr["AcceptanceName"] == DBNull.Value ? "" : dr["AcceptanceName"].ToString();
                quoteModel.AcceptanceDate = dr["AcceptanceDate"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["AcceptanceDate"]);
                quoteModel.AcceptanceDateFormated = quoteModel.AcceptanceDate.HasValue ? quoteModel.AcceptanceDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                quoteModel.SalesTax = dr["SalesTax"] == DBNull.Value ? 0 : Convert.ToDouble(dr["SalesTax"]);

                quoteModel.IsQuantitySyncDisabled = Convert.ToBoolean(dr["IsQuantitySyncDisabled"]);

                quoteModel.Note = dr["Note"] == DBNull.Value ? "" : dr["Note"].ToString();

                quoteModel.IsApproved = dr["IsApproved"] == DBNull.Value ? "" : dr["IsApproved"].ToString();

                quoteModel.IsDemo = Convert.ToBoolean(dr["IsDemo"]);
            }

            return quoteModel;
        }

        public QuoteOrderModel GetQuoteOrderInfo(QuoteOrderModel quoteOrderModel)
        {
            DataTable dtQuote = _DBProduct.GetQuoteOrderInfo(quoteOrderModel);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteOrderModel = new QuoteOrderModel();
                quoteOrderModel.QuoteOrderId = Convert.ToInt32(dr["QuoteOrderId"]);
                quoteOrderModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteOrderModel.TssOrderTypeId = Convert.ToInt32(dr["TssOrderTypeId"]);
                quoteOrderModel.OrderTypeTitle = dr["OrderTypeTitle"] == DBNull.Value ? "" : dr["OrderTypeTitle"].ToString();
                quoteOrderModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                quoteOrderModel.QuoteDateFormated = quoteOrderModel.QuoteDate.ToString("MM/dd/yyyy");
                quoteOrderModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteOrderModel.SalesPersonName = dr["SalesPersonName"].ToString();
                quoteOrderModel.SalesPersonEmail = dr["SalesPersonEmail"] == DBNull.Value ? "" : dr["SalesPersonEmail"].ToString();
                quoteOrderModel.SalesPersonCellPhone = dr["SalesPersonCellPhone"] == DBNull.Value ? "" : dr["SalesPersonCellPhone"].ToString();

                quoteOrderModel.ClientId = dr["ClientID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ClientID"]);
                quoteOrderModel.ClientName = dr["ClientName"].ToString();

                quoteOrderModel.ContractTerm = Convert.ToInt32(dr["ContractTerm"]);
                quoteOrderModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);
                quoteOrderModel.ZohoEntityId = dr["ZohoEntityId"].ToString();
                quoteOrderModel.ZohoEntityType = dr["ZohoEntityType"].ToString();
                quoteOrderModel.BillToCompanyName = dr["BillToCompanyName"].ToString();
                quoteOrderModel.BillToAddress1 = dr["BillToAddress1"].ToString();
                quoteOrderModel.BillToAddress2 = dr["BillToAddress2"].ToString();
                quoteOrderModel.BillToCity = dr["BillToCity"].ToString();
                quoteOrderModel.BillToState = dr["BillToState"].ToString();
                quoteOrderModel.BillToZip = dr["BillToZip"].ToString();
                quoteOrderModel.BillToCountry = dr["BillToCountry"] == DBNull.Value ? "" : dr["BillToCountry"].ToString();
                quoteOrderModel.BillToBillingContact = dr["BillToBillingContact"].ToString();
                quoteOrderModel.BillToBillingEmail = dr["BillToBillingEmail"].ToString();
                quoteOrderModel.BillToPhone = dr["BillToPhone"].ToString();
                quoteOrderModel.IsShipSameAsBill = Convert.ToBoolean(dr["IsShipSameAsBill"]);
                quoteOrderModel.ShipToCompanyName = dr["ShipToCompanyName"].ToString();
                quoteOrderModel.ShipToAddress1 = dr["ShipToAddress1"].ToString();
                quoteOrderModel.ShipToAddress2 = dr["ShipToAddress2"].ToString();
                quoteOrderModel.ShipToCity = dr["ShipToCity"].ToString();
                quoteOrderModel.ShipToState = dr["ShipToState"].ToString();
                quoteOrderModel.ShipToZip = dr["ShipToZip"].ToString();
                quoteOrderModel.ShipToCountry = dr["ShipToCountry"] == DBNull.Value ? "" : dr["ShipToCountry"].ToString();
                quoteOrderModel.ShipToBillingContact = dr["ShipToBillingContact"].ToString();
                quoteOrderModel.ShipToBillingEmail = dr["ShipToBillingEmail"].ToString();
                quoteOrderModel.ShipToPhone = dr["ShipToPhone"].ToString();
                quoteOrderModel.ShippingAndHandling = Convert.ToDouble(dr["ShippingAndHandling"]);
                quoteOrderModel.ShippingAndHandlingType = dr["ShippingAndHandlingType"] == DBNull.Value ? "" : dr["ShippingAndHandlingType"].ToString();
                quoteOrderModel.LastViewDate = dr["LastViewDate"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["LastViewDate"]);
                quoteOrderModel.Purchased = Convert.ToBoolean(dr["Purchased"]);
                quoteOrderModel.Processed = Convert.ToBoolean(dr["Processed"]);

                quoteOrderModel.OrderStatusId = Convert.ToInt32(dr["OrderStatusId"]);
                quoteOrderModel.StatusTitle = dr["StatusTitle"].ToString();

                quoteOrderModel.IsAccepted = dr["IsAccepted"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAccepted"]);
                quoteOrderModel.AcceptanceName = dr["AcceptanceName"] == DBNull.Value ? "" : dr["AcceptanceName"].ToString();
                quoteOrderModel.AcceptanceDate = dr["AcceptanceDate"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["AcceptanceDate"]);
                quoteOrderModel.AcceptanceDateFormated = quoteOrderModel.AcceptanceDate.HasValue ? quoteOrderModel.AcceptanceDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                quoteOrderModel.SalesTax = dr["SalesTax"] == DBNull.Value ? 0 : Convert.ToDouble(dr["SalesTax"]);

                quoteOrderModel.PaymentMethod = dr["PaymentMethod"] == DBNull.Value ? "" : Convert.ToString(dr["PaymentMethod"]);
                quoteOrderModel.PaymentMethodComment = dr["PaymentMethodComment"] == DBNull.Value ? "" : Convert.ToString(dr["PaymentMethodComment"]);

                quoteOrderModel.NettrackClientStatusId = dr["NettrackClientStatusId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["NettrackClientStatusId"]);
                quoteOrderModel.NettrackStatus = dr["NettrackStatus"] == DBNull.Value ? "" : Convert.ToString(dr["NettrackStatus"]);

                quoteOrderModel.Note = dr["Note"] == DBNull.Value ? "" : Convert.ToString(dr["Note"]);

                if (dr["PurchaseDate"] != DBNull.Value)
                    quoteOrderModel.PurchaseDate = Convert.ToDateTime(dr["PurchaseDate"]);

                quoteOrderModel.PurchaseDateFormated = quoteOrderModel.PurchaseDate.HasValue ? quoteOrderModel.PurchaseDate.Value.ToString("MM/dd/yyyy") : "";
            }

            return quoteOrderModel;
        }

        public List<QuoteProductModel> GetQuoteProductList(QuoteProductModel quoteProductModel)
        {
            QuoteProductModel quoteProduct = null;
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            DataTable dtQuote = _DBProduct.GetQuoteProductList(quoteProductModel);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteProduct = new QuoteProductModel();
                quoteProduct.QuoteProductId = Convert.ToInt32(dr["QuoteProductId"]);
                quoteProduct.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteProduct.ProductId = Convert.ToInt32(dr["ProductId"]);
                quoteProduct.Quantity = Convert.ToInt32(dr["Quantity"]);
                quoteProduct.Price = Convert.ToDouble(dr["Price"]);
                quoteProduct.Weight = dr["Weight"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["Weight"]);
                quoteProduct.PackageId = Convert.ToInt32(dr["PackageId"]);
                quoteProduct.ProductCategory = dr["ProductCategory"].ToString();
                quoteProduct.SKU = dr["SKU"].ToString();
                quoteProduct.ProductDescription = dr["ProductDescription"].ToString();

                quoteProductModelList.Add(quoteProduct);
            }

            return quoteProductModelList;
        }

        public List<SalesOrderModel> GetSalesOrderList(SalesOrderModel salesOrderModel)
        {
            SalesOrderModel salesOrder = null;
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            DataTable dtQuote = _DBProduct.GetSalesOrderList(salesOrderModel);
            foreach (DataRow dr in dtQuote.Rows)
            {
                salesOrder = new SalesOrderModel();
                salesOrder.SalesOrderId = Convert.ToInt32(dr["SalesOrderId"]);
                salesOrder.QuoteOrderId = Convert.ToInt32(dr["QuoteOrderId"]);
                salesOrder.ProductId = Convert.ToInt32(dr["ProductId"]);
                salesOrder.Quantity = Convert.ToInt32(dr["Quantity"]);
                salesOrder.Price = Convert.ToDouble(dr["Price"]);
                salesOrder.Weight = dr["Weight"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["Weight"]);
                salesOrder.PackageId = Convert.ToInt32(dr["PackageId"]);
                salesOrder.ProductCategory = dr["ProductCategory"].ToString();
                salesOrder.SKU = dr["SKU"].ToString();
                salesOrder.ProductDescription = dr["ProductDescription"].ToString();

                salesOrderModelList.Add(salesOrder);
            }

            return salesOrderModelList;
        }

        public List<ClientModel> GetNetTrackClientList(ClientModel clientModel)
        {
            ClientModel item = null;
            List<ClientModel> clientModelList = new List<ClientModel>();

            DataTable dtProduct = _DBProduct.GetNetTrackClientList(clientModel);
            foreach (DataRow dr in dtProduct.Rows)
            {
                item = new ClientModel();
                item.ClientID = Convert.ToInt32(dr["ClientId"]);
                item.ClientName = dr["ClientName"].ToString();

                clientModelList.Add(item);
            }

            return clientModelList;
        }

        public List<DdlSourceModel> GetOrderStatusList(int sessionId)
        {
            DdlSourceModel userTypeModel = null;
            List<DdlSourceModel> orderStatusList = new List<DdlSourceModel>();

            DataTable dtOrderStatus = _DBProduct.GetOrderStatusList(sessionId);
            foreach (DataRow dr in dtOrderStatus.Rows)
            {
                userTypeModel = new DdlSourceModel();
                userTypeModel.keyfield = dr["keyfield"].ToString();
                userTypeModel.value = dr["value"].ToString();

                orderStatusList.Add(userTypeModel);
            }

            return orderStatusList;
        }

        public List<QuotePaymentMethodModel> GetQuotePaymentMethodList()
        {
            QuotePaymentMethodModel item = null;
            List<QuotePaymentMethodModel> quotePaymentMethodList = new List<QuotePaymentMethodModel>();

            DataTable dtQuotePaymentMethod = _DBProduct.GetQuotePaymentMethodList();
            foreach (DataRow dr in dtQuotePaymentMethod.Rows)
            {
                item = new QuotePaymentMethodModel();
                item.QuotePaymentMethodId = Convert.ToInt32(dr["QuotePaymentMethodId"]);
                item.PaymentMethod = dr["PaymentMethod"].ToString();

                quotePaymentMethodList.Add(item);
            }

            return quotePaymentMethodList;
        }

        public void SetQuoteOrderStatus(QuoteOrderModel model)
        {
            _DBProduct.SetQuoteOrderStatus(model);
        }

        public List<QuoteLastViewModel> GetQuoteLastViewList(int quoteId)
        {
            QuoteLastViewModel quoteLastViewModel = null;
            List<QuoteLastViewModel> quoteLastViewModelList = new List<QuoteLastViewModel>();

            DataTable dtProduct = _DBProduct.GetQuoteLastViewList(quoteId);
            foreach (DataRow dr in dtProduct.Rows)
            {
                quoteLastViewModel = new QuoteLastViewModel();
                quoteLastViewModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteLastViewModel.LastViewDate = Convert.ToDateTime(dr["LastViewDate"]);
                quoteLastViewModel.LastViewDateFormated = quoteLastViewModel.LastViewDate.ToString("MM-dd-yyyy hh:mm:ss tt");

                quoteLastViewModelList.Add(quoteLastViewModel);
            }

            return quoteLastViewModelList;
        }

        public List<TSSSentEmailModel> GetSentEmailList(int quoteId)
        {
            TSSSentEmailModel tssSentEmailModel = null;
            List<TSSSentEmailModel> tssSentEmailModelList = new List<TSSSentEmailModel>();

            DataTable dtProduct = _DBProduct.GetSentEmailList(quoteId);
            foreach (DataRow dr in dtProduct.Rows)
            {
                tssSentEmailModel = new TSSSentEmailModel();
                tssSentEmailModel.TSSSentEmailId = Convert.ToInt32(dr["TSSSentEmailId"]);
                tssSentEmailModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                tssSentEmailModel.EmailId = Convert.ToString(dr["EmailId"]);
                tssSentEmailModel.EmailId = tssSentEmailModel.EmailId.Split('@')[0];
                tssSentEmailModel.Recipent = Convert.ToString(dr["Recipent"]);
                tssSentEmailModel.SentById = Convert.ToInt32(dr["SentById"]);
                tssSentEmailModel.SenderName = Convert.ToString(dr["SenderName"]);
                tssSentEmailModel.SentDate = Convert.ToDateTime(dr["SentDate"]);
                tssSentEmailModel.SentDateFormated = tssSentEmailModel.SentDate.ToString("MM-dd-yyyy hh:mm tt");

                tssSentEmailModel.EmailStatus = dr["EmailStatus"] == DBNull.Value ? "" : Convert.ToString(dr["EmailStatus"]);
                if (!string.IsNullOrWhiteSpace(tssSentEmailModel.EmailStatus))
                {
                    tssSentEmailModel.EmailStatus = tssSentEmailModel.EmailStatus[0].ToString().ToUpper() + tssSentEmailModel.EmailStatus.Substring(1);
                }
                tssSentEmailModel.EmailStatusTime = dr["EmailStatusTime"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["EmailStatusTime"]);
                tssSentEmailModel.EmailStatusTimeFormated = tssSentEmailModel.EmailStatusTime.HasValue ? tssSentEmailModel.EmailStatusTime.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                tssSentEmailModel.MessageBody = dr["MessageBody"] == DBNull.Value ? "" : Convert.ToString(dr["MessageBody"]);

                tssSentEmailModelList.Add(tssSentEmailModel);
            }

            return tssSentEmailModelList;
        }

        public List<TSSSentEmailModel> GetSentEmailReportList(TSSSentEmailModel sentEmailModel)
        {
            TSSSentEmailModel tssSentEmailModel = null;
            List<TSSSentEmailModel> tssSentEmailModelList = new List<TSSSentEmailModel>();

            DataTable dtProduct = _DBProduct.GetSentEmailReportList(sentEmailModel);
            foreach (DataRow dr in dtProduct.Rows)
            {
                tssSentEmailModel = new TSSSentEmailModel();
                tssSentEmailModel.TSSSentEmailId = Convert.ToInt32(dr["TSSSentEmailId"]);
                tssSentEmailModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                tssSentEmailModel.CustomerName = dr["CustomerName"] == DBNull.Value ? "" : Convert.ToString(dr["CustomerName"]);
                tssSentEmailModel.EmailId = Convert.ToString(dr["EmailId"]);
                tssSentEmailModel.EmailId = tssSentEmailModel.EmailId.Split('@')[0];
                tssSentEmailModel.Recipent = Convert.ToString(dr["Recipent"]);
                tssSentEmailModel.SentById = Convert.ToInt32(dr["SentById"]);
                tssSentEmailModel.SenderName = Convert.ToString(dr["SenderName"]);
                tssSentEmailModel.SentDate = Convert.ToDateTime(dr["SentDate"]);
                tssSentEmailModel.SentDateFormated = tssSentEmailModel.SentDate.ToString("MM-dd-yyyy hh:mm tt");

                tssSentEmailModel.EmailStatus = dr["EmailStatus"] == DBNull.Value ? "" : Convert.ToString(dr["EmailStatus"]);
                if (!string.IsNullOrWhiteSpace(tssSentEmailModel.EmailStatus))
                {
                    tssSentEmailModel.EmailStatus = tssSentEmailModel.EmailStatus[0].ToString().ToUpper() + tssSentEmailModel.EmailStatus.Substring(1);
                }
                tssSentEmailModel.EmailStatusTime = dr["EmailStatusTime"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["EmailStatusTime"]);
                tssSentEmailModel.EmailStatusTimeFormated = tssSentEmailModel.EmailStatusTime.HasValue ? tssSentEmailModel.EmailStatusTime.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                tssSentEmailModelList.Add(tssSentEmailModel);
            }

            return tssSentEmailModelList;
        }

        public List<TSSSentEmailModel> GetResendEmailList()
        {
            TSSSentEmailModel tssSentEmailModel = null;
            List<TSSSentEmailModel> tssSentEmailModelList = new List<TSSSentEmailModel>();

            DataTable dtEmail = _DBProduct.GetResendEmailList();
            foreach (DataRow dr in dtEmail.Rows)
            {
                tssSentEmailModel = new TSSSentEmailModel();
                tssSentEmailModel.TSSSentEmailId = Convert.ToInt32(dr["TSSSentEmailId"]);
                tssSentEmailModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                tssSentEmailModel.Recipent = Convert.ToString(dr["Recipent"]);
                tssSentEmailModel.MessageBody = Convert.ToString(dr["MessageBody"]);
                tssSentEmailModel.Resend = Convert.ToInt32(dr["Resend"]);

                tssSentEmailModel.SalesPersonName = dr["SalesPersonName"] == DBNull.Value ? "" : Convert.ToString(dr["SalesPersonName"]);
                tssSentEmailModel.SalesPersonEmail = dr["SalesPersonEmail"] == DBNull.Value ? "" : Convert.ToString(dr["SalesPersonEmail"]);

                tssSentEmailModelList.Add(tssSentEmailModel);
            }

            return tssSentEmailModelList;
        }

        public TSSSentEmailModel SaveTSSSentEmail(TSSSentEmailModel model)
        {
            return _DBProduct.SaveTSSSentEmail(model);
        }

        #region OrderStatusHistory

        public OrderStatusHistory SaveOrderStatusHistory(OrderStatusHistory model)
        {
            return _DBProduct.SaveOrderStatusHistory(model);
        }

        public List<OrderStatusHistory> GetOrderStatusHistoryList(int quoteOrderId)
        {
            OrderStatusHistory orderStatusHistory = null;
            List<OrderStatusHistory> qrderStatusHistoryList = new List<OrderStatusHistory>();

            DataTable dtOrderStatusHistory = _DBProduct.GetOrderStatusHistoryList(quoteOrderId);
            foreach (DataRow dr in dtOrderStatusHistory.Rows)
            {
                orderStatusHistory = new OrderStatusHistory();
                orderStatusHistory.OrderStatusHistoryId = Convert.ToInt32(dr["OrderStatusHistoryId"]);
                orderStatusHistory.QuoteOrderId = Convert.ToInt32(dr["QuoteOrderId"]);
                orderStatusHistory.OrderStatusId = Convert.ToInt32(dr["OrderStatusId"]);
                orderStatusHistory.StatusTitle = Convert.ToString(dr["StatusTitle"]);
                orderStatusHistory.StatusDate = Convert.ToDateTime(dr["StatusDate"]);
                orderStatusHistory.StatusDateFormated = orderStatusHistory.StatusDate.ToString("MM-dd-yyyy hh:mm tt");

                qrderStatusHistoryList.Add(orderStatusHistory);
            }

            return qrderStatusHistoryList;
        }

        #endregion

        #region QuoteEmailUnsubscribe

        public QuoteEmailUnsubscribeModel SaveQuoteEmailUnsubscribe(QuoteEmailUnsubscribeModel model)
        {
            return _DBProduct.SaveQuoteEmailUnsubscribe(model);
        }

        public List<QuoteEmailUnsubscribeModel> GetQuoteEmailUnsubscribeList(int quoteId)
        {
            QuoteEmailUnsubscribeModel quoteEmailUnsubscribe = null;
            List<QuoteEmailUnsubscribeModel> quoteEmailUnsubscribeList = new List<QuoteEmailUnsubscribeModel>();

            DataTable dtQuoteEmailUnsubscribe = _DBProduct.GetQuoteEmailUnsubscribeList(quoteId);
            foreach (DataRow dr in dtQuoteEmailUnsubscribe.Rows)
            {
                quoteEmailUnsubscribe = new QuoteEmailUnsubscribeModel();
                quoteEmailUnsubscribe.QuoteEmailUnsubscribeId = Convert.ToInt32(dr["QuoteEmailUnsubscribeId"]);
                quoteEmailUnsubscribe.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteEmailUnsubscribe.Email = Convert.ToString(dr["Email"]);

                quoteEmailUnsubscribeList.Add(quoteEmailUnsubscribe);
            }

            return quoteEmailUnsubscribeList;
        }

        #endregion

        #region QuoteLinkMapping

        public QuoteLinkMappingModel GetQuoteLinkMapping(string encryptValue)
        {
            QuoteLinkMappingModel quoteLinkMappingModel = null;

            DataTable dt = _DBProduct.GetQuoteLinkMapping(encryptValue);
            foreach (DataRow dr in dt.Rows)
            {
                quoteLinkMappingModel = new QuoteLinkMappingModel();
                quoteLinkMappingModel.QuoteLinkMappingId = Convert.ToInt32(dr["QuoteLinkMappingId"]);
                quoteLinkMappingModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteLinkMappingModel.EncryptValue = dr["EncryptValue"] == DBNull.Value ? "" : dr["EncryptValue"].ToString();
                quoteLinkMappingModel.DecryptValue = dr["DecryptValue"] == DBNull.Value ? "" : dr["DecryptValue"].ToString();
                quoteLinkMappingModel.HasError = Convert.ToBoolean(dr["HasError"]);
                quoteLinkMappingModel.Error = dr["Error"] == DBNull.Value ? "" : dr["Error"].ToString();
                quoteLinkMappingModel.Recipient = dr["Recipient"] == DBNull.Value ? "" : dr["Recipient"].ToString();

                break;
            }

            return quoteLinkMappingModel;
        }

        public void SaveQuoteLinkMapping(QuoteLinkMappingModel model)
        {
            _DBProduct.SaveQuoteLinkMapping(model);
        }

        #endregion

        #region TSS Shipping

        public void SaveTssShipping(TssShippingModel model)
        {
            _DBProduct.SaveTssShipping(model);
        }

        public List<TssShippingModel> GetTssShippingHistory(TssShippingModel model)
        {
            TssShippingModel tssShippingModel = null;
            List<TssShippingModel> tssShippingModelList = new List<TssShippingModel>();

            DataTable dtHistory = _DBProduct.GetTssShippingHistory(model);
            foreach (DataRow dr in dtHistory.Rows)
            {
                tssShippingModel = new TssShippingModel();
                tssShippingModel.TssShippingId = Convert.ToInt32(dr["TssShippingId"]);
                tssShippingModel.QuoteOrderId = Convert.ToInt32(dr["QuoteOrderId"]);
                tssShippingModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                tssShippingModel.ShippingType = Convert.ToString(dr["ShippingType"]);

                tssShippingModel.ShipToCompanyName = Convert.ToString(dr["ShipToCompanyName"]);
                tssShippingModel.ShipToBillingContact = Convert.ToString(dr["ShipToBillingContact"]);
                tssShippingModel.ShipToBillingEmail = Convert.ToString(dr["ShipToBillingEmail"]);

                tssShippingModel.ShipToCountry = Convert.ToString(dr["ShipToCountry"]);
                if (tssShippingModel.ShipToCountry == "US")
                {
                    if (tssShippingModel.ShippingType == "Ground")
                    {
                        tssShippingModel.ShippingType = "UPS Ground";
                    }
                    else if (tssShippingModel.ShippingType == "2Day")
                    {
                        tssShippingModel.ShippingType = "UPS 2nd Air";
                    }
                    else if (tssShippingModel.ShippingType == "NextAir")
                    {
                        tssShippingModel.ShippingType = "UPS Next Air";
                    }
                }
                else
                {
                    if (tssShippingModel.ShippingType == "Ground")
                    {
                        tssShippingModel.ShippingType = "UPS Standard";
                    }
                    else if (tssShippingModel.ShippingType == "2Day")
                    {
                        tssShippingModel.ShippingType = "UPS Expedited";
                    }
                    else if (tssShippingModel.ShippingType == "NextAir")
                    {
                        tssShippingModel.ShippingType = "UPS Saver";
                    }
                }

                tssShippingModel.ShippingCost = Convert.ToDouble(dr["ShippingCost"]);
                tssShippingModel.Weight = Convert.ToDouble(dr["Weight"]);
                tssShippingModel.Height = Convert.ToInt32(dr["Height"]);
                tssShippingModel.Width = Convert.ToInt32(dr["Width"]);
                tssShippingModel.Length = Convert.ToInt32(dr["Length"]);
                tssShippingModel.TrackingNo = Convert.ToString(dr["TrackingNo"]);
                tssShippingModel.LabelImage = Convert.ToString(dr["LabelImage"]);

                tssShippingModel.SentEmailDate = dr["SentEmailDate"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(dr["SentEmailDate"]);
                if (tssShippingModel.SentEmailDate.HasValue)
                {
                    tssShippingModel.SentEmailDateFormated = tssShippingModel.SentEmailDate.Value.ToString("MM-dd-yyyy hh:mm tt");
                }

                tssShippingModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                tssShippingModel.CreatedDateFormated = tssShippingModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");

                tssShippingModelList.Add(tssShippingModel);
            }

            return tssShippingModelList;
        }

        public void UpdateTssShipSentEmail(TssShippingModel model)
        {
            _DBProduct.UpdateTssShipSentEmail(model);
        }

        public List<TssShippingEmailLogModel> GetTssShippingEmailHistory(TssShippingModel model)
        {
            TssShippingEmailLogModel tssShippingEmailLog = null;
            List<TssShippingEmailLogModel> tssShippingEmailLogList = new List<TssShippingEmailLogModel>();

            DataTable dtHistory = _DBProduct.GetTssShippingEmailHistory(model);
            foreach (DataRow dr in dtHistory.Rows)
            {
                tssShippingEmailLog = new TssShippingEmailLogModel();
                tssShippingEmailLog.TssShippingId = Convert.ToInt32(dr["TssShippingId"]);
                tssShippingEmailLog.MessageId = Convert.ToString(dr["MessageId"]);
                if (!string.IsNullOrWhiteSpace(tssShippingEmailLog.MessageId))
                {
                    tssShippingEmailLog.MessageId = tssShippingEmailLog.MessageId.Split('@')[0];
                }
                tssShippingEmailLog.Recipent = Convert.ToString(dr["Recipent"]);
                tssShippingEmailLog.Event = Convert.ToString(dr["Event"]);
                tssShippingEmailLog.EventTime = Convert.ToDateTime(dr["EventTime"]);
                tssShippingEmailLog.EventTimeFormated = tssShippingEmailLog.EventTime.ToString("MM-dd-yyyy hh:mm tt");

                tssShippingEmailLogList.Add(tssShippingEmailLog);
            }

            return tssShippingEmailLogList;
        }

        #endregion
    }
}
