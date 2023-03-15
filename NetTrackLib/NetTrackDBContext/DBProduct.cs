using System;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;

namespace NetTrackDBContext
{
    public class DBProduct : DBContext
    {
        #region private property

        private SqlDataReader _dataReader;
        private SqlParameter[] _spParameters;
        private DataTable _dataTable;
        private DataSet _dataSet;
        private string _spName;

        #endregion

        #region Constructor

        // default constructor
        public DBProduct()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion

        #region DBProductConfig public method

        public DataTable GetProductList(int sessionId)
        {
            _spName = "ug_products";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", sessionId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }
        public DataTable GetDetailProductInfo(ProductModel productModel)
        {
            _spName = "ug_products";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", productModel.SessionId),
                              new SqlParameter("@productid", productModel.ProductId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetProductTypeList(ProductModel productModel)
        {
            _spName = "ug_Product_type";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", productModel.SessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }
        public ProductModel SaveProduct(ProductModel model)
        {
            SqlParameter spemp = new SqlParameter("@ProductId", model.ProductId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_products";
            // _spName = "us_alert";
            _spParameters = new SqlParameter[]{

                                          new SqlParameter("@sessionid",model.SessionId),
                                          spemp,
                                          new SqlParameter("@SKU",model.SKU),
                                          new SqlParameter("@ProductName",model.ProductName),
                                          new SqlParameter("@ProductDescription",model.ProductDescription),
                                          new SqlParameter("@Price",model.Price),
                                          new SqlParameter("@Weight",model.Weight),
                                          new SqlParameter("@ProductTypeId",model.ProductTypeId),
                                          new SqlParameter("@DiscountProductTypeId",model.DiscountProductTypeId),
                                          new SqlParameter("@Carrier",model.Carrier),
                                          new SqlParameter("@Notes",model.Notes),
                                          new SqlParameter("@ProductImageFileName",model.ProductImageFileName),
                                          new SqlParameter("@action",model.Action),

                        };
            int result = ExecuteNoResult(_spName, _spParameters);


            model.ProductId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public QuoteProductModel SaveQuoteProduct(QuoteProductModel model)
        {
            SqlParameter spemp = new SqlParameter("@QuoteProductId", model.QuoteProductId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_QuoteProduct";
            _spParameters = new SqlParameter[]{
                                          new SqlParameter("@sessionid",model.SessionId),
                                          spemp,
                                          new SqlParameter("@QuoteId",model.QuoteId),
                                          new SqlParameter("@ProductId",model.ProductId),
                                          new SqlParameter("@Quantity",model.Quantity),
                                          new SqlParameter("@Price",model.Price),
                                          new SqlParameter("@PackageId",model.PackageId),
                                          new SqlParameter("@ProductCategory",model.ProductCategory),
                                          new SqlParameter("@UserQuantity",model.UserQuantity),
                                          new SqlParameter("@SortOrder",model.SortOrder),
                                          new SqlParameter("@action",model.Action),

                        };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.QuoteProductId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public SalesOrderModel SaveSalesOrder(SalesOrderModel model)
        {
            //SqlParameter spemp = new SqlParameter("@QuoteProductId", model.QuoteProductId);
            //spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_SalesOrder";
            _spParameters = new SqlParameter[]{
                                          new SqlParameter("@sessionid",model.SessionId),
                                          //spemp,
										  new SqlParameter("@SalesOrderId",model.SalesOrderId),
                                          new SqlParameter("@QuoteOrderId",model.QuoteOrderId),
                                          new SqlParameter("@ProductId",model.ProductId),
                                          new SqlParameter("@Quantity",model.Quantity),
                                          new SqlParameter("@Price",model.Price),
                                          new SqlParameter("@PackageId",model.PackageId),
                                          new SqlParameter("@ProductCategory",model.ProductCategory),
                                          new SqlParameter("@UserQuantity",model.UserQuantity),
                                          new SqlParameter("@SortOrder",model.SortOrder),
                                          new SqlParameter("@action",model.Action)
                        };

            int result = ExecuteNoResult(_spName, _spParameters);
            //model.QuoteProductId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public QuoteModel SaveQuote(QuoteModel model)
        {
            SqlParameter spemp = new SqlParameter("@QuoteId", model.QuoteId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_Quote";
            _spParameters = new SqlParameter[]{
                                          new SqlParameter("@sessionid",model.SessionId),
                                          spemp,
                                          new SqlParameter("@TssOrderTypeId",model.TssOrderTypeId),
                                          new SqlParameter("@QuoteNumber",model.QuoteNumber),
                                          new SqlParameter("@QuoteDate",model.QuoteDate),
                                          new SqlParameter("@ContractTerm",model.ContractTerm),
                                          new SqlParameter("@ValidUntil",model.ValidUntil),
                                          new SqlParameter("@SalesPersonId",model.SalesPersonId),
                                          new SqlParameter("@ZohoEntityId",model.ZohoEntityId),
                                          new SqlParameter("@ZohoEntityType",model.ZohoEntityType),
                                          new SqlParameter("@ClientID",model.ClientId),
                                          new SqlParameter("@BillToCompanyName",model.BillToCompanyName),
                                          new SqlParameter("@BillToAddress1",model.BillToAddress1),
                                          new SqlParameter("@BillToAddress2",model.BillToAddress2),
                                          new SqlParameter("@BillToCity",model.BillToCity),
                                          new SqlParameter("@BillToState",model.BillToState),
                                          new SqlParameter("@BillToZip",model.BillToZip),
                                          new SqlParameter("@BillToCountry",model.BillToCountry),
                                          new SqlParameter("@BillToBillingContact",model.BillToBillingContact),
                                          new SqlParameter("@BillToBillingEmail",model.BillToBillingEmail),
                                          new SqlParameter("@BillToPhone",model.BillToPhone),
                                          new SqlParameter("@IsShipSameAsBill",model.IsShipSameAsBill),
                                          new SqlParameter("@ShipToCompanyName",model.ShipToCompanyName),
                                          new SqlParameter("@ShipToAddress1",model.ShipToAddress1),
                                          new SqlParameter("@ShipToAddress2",model.ShipToAddress2),
                                          new SqlParameter("@ShipToCity",model.ShipToCity),
                                          new SqlParameter("@ShipToState",model.ShipToState),
                                          new SqlParameter("@ShipToZip",model.ShipToZip),
                                          new SqlParameter("@ShipToCountry",model.ShipToCountry),
                                          new SqlParameter("@ShipToBillingContact",model.ShipToBillingContact),
                                          new SqlParameter("@ShipToBillingEmail",model.ShipToBillingEmail),
                                          new SqlParameter("@ShipToPhone",model.ShipToPhone),
                                          new SqlParameter("@ShippingAndHandling",model.ShippingAndHandling),
                                          new SqlParameter("@ShippingAndHandlingType",model.ShippingAndHandlingType),
                                          new SqlParameter("@UrlSendDate",model.UrlSendDate),
                                          new SqlParameter("@LastViewDate",model.LastViewDate),
                                          new SqlParameter("@SalesTax",model.SalesTax),
                                          new SqlParameter("@IsQuantitySyncDisabled",model.IsQuantitySyncDisabled),
                                          new SqlParameter("@Note",model.Note),
                                          new SqlParameter("@IsApproved",model.IsApproved),
                                          new SqlParameter("@IsDemo",model.IsDemo),
                                          new SqlParameter("@action",model.Action),

                        };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.QuoteId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public void ResetQuoteStartDate(int quoteId)
        {
            _spName = "us_Quote";
            _spParameters = new SqlParameter[] {
                new SqlParameter("@QuoteId", quoteId),
                new SqlParameter("@action", "RQD")
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        public void ApproveQuote(QuoteModel model)
        {
            _spName = "us_Quote";
            _spParameters = new SqlParameter[] {
                new SqlParameter("@QuoteId", model.QuoteId),
                new SqlParameter("@IsApproved", model.IsApproved),
                new SqlParameter("@ApprovedBy", model.ApprovedBy),
                new SqlParameter("@ApprovedDate", model.ApprovedDate),
                new SqlParameter("@action", "APR")
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        public QuoteOrderModel SaveQuoteOrder(QuoteOrderModel model)
        {
            SqlParameter spemp = new SqlParameter("@QuoteOrderId", model.QuoteOrderId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_QuoteOrder";
            _spParameters = new SqlParameter[]{
                                          new SqlParameter("@sessionid",model.SessionId),
                                          spemp,
                                          new SqlParameter("@QuoteId",model.QuoteId),
                                          new SqlParameter("@TssOrderTypeId",model.TssOrderTypeId),
                                          new SqlParameter("@QuoteDate",model.QuoteDate.CompareTo(new DateTime()) == 0 ? DateTime.Now : model.QuoteDate),
                                          new SqlParameter("@ContractTerm",model.ContractTerm),
                                          new SqlParameter("@ValidUntil",model.ValidUntil),
                                          new SqlParameter("@SalesPersonId",model.SalesPersonId),
                                          new SqlParameter("@ZohoEntityId",model.ZohoEntityId),
                                          new SqlParameter("@ZohoEntityType",model.ZohoEntityType),
                                          new SqlParameter("@ClientID",model.ClientId),
                                          new SqlParameter("@BillToCompanyName",model.BillToCompanyName),
                                          new SqlParameter("@BillToAddress1",model.BillToAddress1),
                                          new SqlParameter("@BillToAddress2",model.BillToAddress2),
                                          new SqlParameter("@BillToCity",model.BillToCity),
                                          new SqlParameter("@BillToState",model.BillToState),
                                          new SqlParameter("@BillToZip",model.BillToZip),
                                          new SqlParameter("@BillToCountry",model.BillToCountry),
                                          new SqlParameter("@BillToBillingContact",model.BillToBillingContact),
                                          new SqlParameter("@BillToBillingEmail",model.BillToBillingEmail),
                                          new SqlParameter("@BillToPhone",model.BillToPhone),
                                          new SqlParameter("@IsShipSameAsBill",model.IsShipSameAsBill),
                                          new SqlParameter("@ShipToCompanyName",model.ShipToCompanyName),
                                          new SqlParameter("@ShipToAddress1",model.ShipToAddress1),
                                          new SqlParameter("@ShipToAddress2",model.ShipToAddress2),
                                          new SqlParameter("@ShipToCity",model.ShipToCity),
                                          new SqlParameter("@ShipToState",model.ShipToState),
                                          new SqlParameter("@ShipToZip",model.ShipToZip),
                                          new SqlParameter("@ShipToCountry",model.ShipToCountry),
                                          new SqlParameter("@ShipToBillingContact",model.ShipToBillingContact),
                                          new SqlParameter("@ShipToBillingEmail",model.ShipToBillingEmail),
                                          new SqlParameter("@ShipToPhone",model.ShipToPhone),
                                          new SqlParameter("@ShippingAndHandling",model.ShippingAndHandling),
                                          new SqlParameter("@ShippingAndHandlingType",model.ShippingAndHandlingType),
                                          new SqlParameter("@UrlSendDate",model.UrlSendDate),
                                          new SqlParameter("@LastViewDate",model.LastViewDate),
                                          new SqlParameter("@OrderStatusId",model.OrderStatusId),
                                          new SqlParameter("@IsAccepted",model.IsAccepted),
                                          new SqlParameter("@AcceptanceName",model.AcceptanceName),
                                          new SqlParameter("@AcceptanceDate",model.AcceptanceDate),
                                          new SqlParameter("@SalesTax",model.SalesTax),
                                          new SqlParameter("@QuotePaymentMethodId",model.QuotePaymentMethodId),
                                          new SqlParameter("@PaymentMethodComment",model.PaymentMethodComment),
                                          new SqlParameter("@NettrackClientStatusId",model.NettrackClientStatusId),
                                          new SqlParameter("@TransactionId",model.TransactionId),
                                          new SqlParameter("@Note",model.Note),
                                          new SqlParameter("@action",model.Action),

                        };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.QuoteOrderId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public DataTable GetSalesPersonList(int sessionId)
        {
            _spName = "ug_SalesPerson";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", sessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetSalesPersonList(string clientIdList)
        {
            _spName = "ug_SalesPersonList";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@ClientId", clientIdList) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public void SaveSalesPerson(int employeeId, bool isSet)
        {
            _spName = "us_SalesPerson";
            _spParameters = new SqlParameter[] {
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@Delete", !isSet)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        public DataTable GetQuoteList(QuoteModel model)
        {
            _spName = "ug_quote_list";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", model.SessionId),
                              new SqlParameter("@SearchFromDate", model.SearchFromDate),
                              new SqlParameter("@SearchToDate", model.SearchToDate)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetSalesList(QuoteOrderModel model)
        {
            _spName = "ug_SalesOrder_list";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", model.SessionId),
                              new SqlParameter("@SearchFromDate", model.SearchFromDate),
                              new SqlParameter("@SearchToDate", model.SearchToDate)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetQuoteInfo(QuoteModel quoteModel)
        {
            _spName = "ug_Quote";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteModel.SessionId),
                              new SqlParameter("@QuoteId", quoteModel.QuoteId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetQuoteOrderInfo(QuoteOrderModel quoteOrderModel)
        {
            _spName = "ug_quoteOrder";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteOrderModel.SessionId),
                              new SqlParameter("@QuoteOrderId", quoteOrderModel.QuoteOrderId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetQuoteProductList(QuoteProductModel quoteProductModel)
        {
            _spName = "ug_QuoteProduct";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteProductModel.SessionId),
                              new SqlParameter("@QuoteId", quoteProductModel.QuoteId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetSalesOrderList(SalesOrderModel salesOrderModel)
        {
            _spName = "ug_SalesOrder";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", salesOrderModel.SessionId),
                              new SqlParameter("@QuoteOrderId", salesOrderModel.QuoteOrderId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataSet GetQuoteProductRpt(QuoteModel quoteModel)
        {
            _spName = "ug_QuoteProduct_Rpt";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteModel.SessionId),
                              new SqlParameter("@QuoteId",  quoteModel.QuoteId)
                        };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }

        public DataSet GetSalesOrderRpt(QuoteOrderModel quoteOrderModel)
        {
            _spName = "ug_SalesOrder_Rpt";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteOrderModel.SessionId),
                              new SqlParameter("@QuoteOrderId",  quoteOrderModel.QuoteOrderId)
                        };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }

        public DataTable GetNetTrackClientList(ClientModel clientModel)
        {
            _spName = "ug_quote_NetTrack_client";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", clientModel.SessionID),
                              new SqlParameter("@clientname", clientModel.ClientName)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetOrderStatusList(int sessionId)
        {
            _spName = "ug_OrderStatus";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", sessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetQuotePaymentMethodList()
        {
            _spName = "ug_QuotePaymentMethod";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public void SetQuoteOrderStatus(QuoteOrderModel model)
        {
            _spName = "us_QuoteOrderStatus";
            _spParameters = new SqlParameter[]{
                            new SqlParameter("@QuoteOrderId",model.QuoteOrderId),
                            new SqlParameter("@OrderStatusId",model.OrderStatusId),
                        };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        public DataTable GetQuoteLastViewList(int quoteId)
        {
            _spName = "ug_QuoteLastView";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@QuoteId", quoteId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetSentEmailList(int quoteId)
        {
            _spName = "ug_TSSSentEmail";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@QuoteId", quoteId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetSentEmailReportList(TSSSentEmailModel sentEmailModel)
        {
            _spName = "ug_TSSSentEmailReport";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@SearchFromDate", sentEmailModel.SearchFromDate),
                              new SqlParameter("@SearchToDate", sentEmailModel.SearchToDate),
                              new SqlParameter("@EmailStatus", sentEmailModel.EmailStatus)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        #endregion

        public DataTable GetResendEmailList()
        {
            _spName = "sp_ResendTssEmail";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public TSSSentEmailModel SaveTSSSentEmail(TSSSentEmailModel model)
        {
            SqlParameter spemp = new SqlParameter("@TSSSentEmailId", model.TSSSentEmailId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_TSSSentEmail";
            _spParameters = new SqlParameter[]{
                spemp,
                new SqlParameter("@QuoteId", model.QuoteId),
                new SqlParameter("@EmailId", model.EmailId),
                new SqlParameter("@SentBy", model.SentById),
                new SqlParameter("@SentDate", model.SentDate),
                new SqlParameter("@Recipent", model.Recipent),
                new SqlParameter("@MessageBody", model.MessageBody),
                new SqlParameter("@Resend", model.Resend),
            };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.TSSSentEmailId = Convert.ToInt32(spemp.Value);

            return model;
        }

        #region OrderStatusHistory

        public OrderStatusHistory SaveOrderStatusHistory(OrderStatusHistory model)
        {
            SqlParameter spemp = new SqlParameter("@OrderStatusHistoryId", model.OrderStatusHistoryId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_OrderStatusHistory";
            _spParameters = new SqlParameter[]{
                spemp,
                new SqlParameter("@QuoteOrderId", model.QuoteOrderId),
                new SqlParameter("@OrderStatusId", model.OrderStatusId),
                new SqlParameter("@StatusDate", model.StatusDate)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.OrderStatusHistoryId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public DataTable GetOrderStatusHistoryList(int quoteOrderId)
        {
            _dataTable = new DataTable();

            _spName = "ug_OrderStatusHistory";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@QuoteOrderId", quoteOrderId)
            };

            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        #endregion

        #region QuoteEmailUnsubscribe

        public QuoteEmailUnsubscribeModel SaveQuoteEmailUnsubscribe(QuoteEmailUnsubscribeModel model)
        {
            SqlParameter spemp = new SqlParameter("@QuoteEmailUnsubscribeId", model.QuoteEmailUnsubscribeId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_QuoteEmailUnsubscribe";
            _spParameters = new SqlParameter[]{
                spemp,
                new SqlParameter("@QuoteId", model.QuoteId),
                new SqlParameter("@Email", model.Email)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.QuoteEmailUnsubscribeId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public DataTable GetQuoteEmailUnsubscribeList(int quoteId)
        {
            _dataTable = new DataTable();

            _spName = "ug_QuoteEmailUnsubscribe";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@QuoteId", quoteId)
            };

            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        #endregion

        #region UPS Log
        public void SaveUPSLog(string message)
        {
            _spName = "us_UPSLog";
            _spParameters = new SqlParameter[] { new SqlParameter("@Message", message) };

            int result = ExecuteNoResult(_spName, _spParameters);
        }
        #endregion

        #region QuoteLinkMapping

        public DataTable GetQuoteLinkMapping(string encryptValue)
        {
            _dataTable = new DataTable();

            _spName = "ug_QuoteLinkMapping";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@EncryptValue", encryptValue)
            };

            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public void SaveQuoteLinkMapping(QuoteLinkMappingModel model)
        {
            _spName = "us_QuoteLinkMapping";
            _spParameters = new SqlParameter[] {
                new SqlParameter("@QuoteId", model.QuoteId),
                new SqlParameter("@EncryptValue", model.EncryptValue),
                new SqlParameter("@DecryptValue", model.DecryptValue),
                new SqlParameter("@HasError", model.HasError),
                new SqlParameter("@Error", model.Error),
                new SqlParameter("@Recipient", model.Recipient)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        #endregion

        #region UPS Shipping

        public void SaveTssShipping(TssShippingModel model)
        {
            _spName = "us_TssShipping";
            _spParameters = new SqlParameter[]{
                            new SqlParameter("@QuoteOrderId", model.QuoteOrderId),
                            new SqlParameter("@ShippingType", model.ShippingType),
                            new SqlParameter("@ShippingCost", model.ShippingCost),
                            new SqlParameter("@Weight", model.Weight),
                            new SqlParameter("@Height", model.Height),
                            new SqlParameter("@Width", model.Width),
                            new SqlParameter("@Length", model.Length),
                            new SqlParameter("@TrackingNo", model.TrackingNo),
                            new SqlParameter("@LabelImage", model.LabelImage),
                            new SqlParameter("@Action", "I")
                        };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        public DataTable GetTssShippingHistory(TssShippingModel model)
        {
            _spName = "ug_TssShippingHistory";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@QuoteOrderId", model.QuoteOrderId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public void UpdateTssShipSentEmail(TssShippingModel model)
        {
            _spName = "us_TssShipping";
            _spParameters = new SqlParameter[]{
                            new SqlParameter("@TssShippingId", model.TssShippingId),
                            new SqlParameter("@Action", "U_SED")
                        };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        public DataTable GetTssShippingEmailHistory(TssShippingModel model)
        {
            _spName = "ug_TssShippingEmailLog";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@TssShippingId", model.TssShippingId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        #endregion
    }
}
