using System;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;

namespace NetTrackDBContext
{
    public class DBQuoteTempate : DBContext
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
        public DBQuoteTempate()
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
                                          new SqlParameter("@action",model.Action),

                        };
            int result = ExecuteNoResult(_spName, _spParameters);


            model.ProductId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public QuoteProductModel SaveQuoteTemplateProduct(QuoteProductModel model)
        {
            SqlParameter spemp = new SqlParameter("@QuoteProductId", model.QuoteProductId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_QuoteTemplateProduct";
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
                                          new SqlParameter("@QuoteId",model.OrderId),
                                          new SqlParameter("@ProductId",model.ProductId),
                                          new SqlParameter("@Quantity",model.Quantity),
                                          new SqlParameter("@Price",model.Price),
                                          new SqlParameter("@PackageId",model.PackageId),
                                          new SqlParameter("@ProductCategory",model.ProductCategory),
                                          new SqlParameter("@UserQuantity",model.UserQuantity),
                                          new SqlParameter("@action",model.Action),

                        };

            int result = ExecuteNoResult(_spName, _spParameters);
            //model.QuoteProductId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public QuoteTemplateModel SaveQuoteTemplate(QuoteTemplateModel model)
        {
            SqlParameter spemp = new SqlParameter("@QuoteId", model.QuoteId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_QuoteTemplate";
            _spParameters = new SqlParameter[]{
                                          new SqlParameter("@sessionid",model.SessionId),
                                          spemp,
                                          new SqlParameter("@TemplateName",model.TemplateName),
                                          new SqlParameter("@QuoteTemplateGroupId",model.QuoteTemplateGroupId),
                                          new SqlParameter("@QuoteDate",model.QuoteDate),
                                          new SqlParameter("@ContractTerm",model.ContractTerm),
                                          new SqlParameter("@ValidUntil",model.ValidUntil),
                                          new SqlParameter("@SalesPersonId",model.SalesPersonId),
                                          new SqlParameter("@ZohoEntityId",model.ZohoEntityId),
                                          new SqlParameter("@ZohoEntityType",model.ZohoEntityType),
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
                                          new SqlParameter("@action",model.Action),

                        };

            int result = ExecuteNoResult(_spName, _spParameters);
            model.QuoteId = Convert.ToInt32(spemp.Value);

            return model;
        }

        public DataTable GetSalesPersonList(int sessionId)
        {
            _spName = "ug_SalesPerson";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", sessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetQuoteTemplateList(int sessionId)
        {
            _spName = "ug_quoteTemplate_list";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", sessionId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetSalesList(int sessionId)
        {
            _spName = "ug_SalesOrder_list";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", sessionId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetQuoteTemplateInfo(QuoteTemplateModel quoteModel)
        {
            _spName = "ug_quoteTemplate";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteModel.SessionId),
                              new SqlParameter("@QuoteId", quoteModel.QuoteId)
                        };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public DataTable GetQuoteProductTemplateList(QuoteProductModel quoteProductModel)
        {
            _spName = "ug_QuoteProductTemplate";
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
                              new SqlParameter("@QuoteId", salesOrderModel.OrderId)
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

        public DataSet GetSalesOrderRpt(QuoteModel quoteModel)
        {
            _spName = "ug_SalesOrder_Rpt";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                              new SqlParameter("@sessionid", quoteModel.SessionId),
                              new SqlParameter("@QuoteId",  quoteModel.QuoteId)
                        };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }

        public QuoteTemplateModel SetQuoteTemplateActive(QuoteTemplateModel model)
        {
            _spName = "us_QuoteTemplateActive";
            _spParameters = new SqlParameter[] {
                                new SqlParameter("@QuoteId", model.QuoteId),
                                new SqlParameter("@IsActive", model.IsActive),
                            };

            int result = ExecuteNoResult(_spName, _spParameters);

            return model;
        }

        public DataTable GetQuoteTemplateGroupList()
        {
            _spName = "ug_QuoteTemplateGroup";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        #endregion
    }
}
