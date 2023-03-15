using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections;


namespace NetTrackRepository
{
    public class QuoteTemplateRepository
    {
        private ProductModel _ProductModel;
        private DBQuoteTempate _DBProduct;

        public QuoteTemplateRepository()
        {
            this._DBProduct = new DBQuoteTempate();
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
                productModel.ProductTypeName = dr["ProductTypeName"].ToString();
                productModel.DiscountProductTypeId = dr["DiscountProductTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DiscountProductTypeId"]);
                productModel.DiscountProductTypeName = dr["DiscountProductTypeName"] == DBNull.Value ? "" : Convert.ToString(dr["DiscountProductTypeName"]);

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
            return _DBProduct.SaveProduct(model);
        }

        public QuoteProductModel SaveQuoteTemplateProduct(QuoteProductModel model)
        {
            return _DBProduct.SaveQuoteTemplateProduct(model);
        }

        public SalesOrderModel SaveSalesOrder(SalesOrderModel model)
        {
            return _DBProduct.SaveSalesOrder(model);
        }

        public QuoteTemplateModel SaveQuoteTemplate(QuoteTemplateModel model)
        {
            return _DBProduct.SaveQuoteTemplate(model);
        }

        public List<DdlSourceModel> GetSalesPersonList(int sessionId)
        {
            DdlSourceModel userTypeModel = null;
            List<DdlSourceModel> salesPersonList = new List<DdlSourceModel>();

            DataTable dtSalesPerson = _DBProduct.GetSalesPersonList(sessionId);
            foreach (DataRow dr in dtSalesPerson.Rows)
            {
                userTypeModel = new DdlSourceModel();
                userTypeModel.keyfield = dr["keyfield"].ToString();
                userTypeModel.value = dr["value"].ToString();

                salesPersonList.Add(userTypeModel);
            }

            return salesPersonList;
        }

        public List<QuoteTemplateModel> GetQuoteTemplateList(int sessionId)
        {
            QuoteTemplateModel quoteModel = null;
            List<QuoteTemplateModel> quoteModelList = new List<QuoteTemplateModel>();

            DataTable dtQuote = _DBProduct.GetQuoteTemplateList(sessionId);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteModel = new QuoteTemplateModel();
                quoteModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteModel.GroupName = dr["GroupName"] == DBNull.Value ? "" : dr["GroupName"].ToString();
                quoteModel.TemplateName = dr["TemplateName"].ToString();
                quoteModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                //quoteModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteModel.CustomerName = dr["CustomerName"].ToString();
                quoteModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                quoteModel.CreatedDateFormated = quoteModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");
                if (dr["UrlSendDate"] != DBNull.Value)
                    quoteModel.UrlSendDate = Convert.ToDateTime(dr["UrlSendDate"]);

                quoteModel.UrlSendDateFormated = quoteModel.UrlSendDate.HasValue ? quoteModel.UrlSendDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";
                quoteModel.Url = dr["Url"] == DBNull.Value ? "" : dr["Url"].ToString();
                if (dr["LastViewDate"] != DBNull.Value)
                {
                    quoteModel.LastViewDate = Convert.ToDateTime(dr["LastViewDate"]);
                    quoteModel.LastViewDateFormated = quoteModel.LastViewDate.Value.ToString("MM-dd-yyyy hh:mm tt");
                }

                quoteModel.Qty = dr["Qty"].ToString();
                quoteModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);
                quoteModel.BillToBillingEmail = dr["BillToBillingEmail"] == DBNull.Value ? "" : dr["BillToBillingEmail"].ToString();
                quoteModel.IsActive = Convert.ToBoolean(dr["IsActive"]);

                quoteModelList.Add(quoteModel);
            }

            return quoteModelList;
        }

        public List<QuoteModel> GetSalesList(int sessionId)
        {
            QuoteModel quoteModel = null;
            List<QuoteModel> quoteModelList = new List<QuoteModel>();

            DataTable dtQuote = _DBProduct.GetSalesList(sessionId);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteModel = new QuoteModel();
                quoteModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteModel.QuoteNumber = dr["QuoteNumber"].ToString();
                quoteModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                //quoteModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteModel.CustomerName = dr["CustomerName"].ToString();
                quoteModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                quoteModel.CreatedDateFormated = quoteModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");
                if (dr["UrlSendDate"] != DBNull.Value)
                    quoteModel.UrlSendDate = Convert.ToDateTime(dr["UrlSendDate"]);

                quoteModel.UrlSendDateFormated = quoteModel.UrlSendDate.HasValue ? quoteModel.UrlSendDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";
                quoteModel.Url = dr["Url"] == DBNull.Value ? "" : dr["Url"].ToString();
                if (dr["LastViewDate"] != DBNull.Value)
                {
                    quoteModel.LastViewDate = Convert.ToDateTime(dr["LastViewDate"]);
                    quoteModel.LastViewDateFormated = quoteModel.LastViewDate.Value.ToString("MM-dd-yyyy hh:mm tt");
                }

                if (dr["PurchaseDate"] != DBNull.Value)
                    quoteModel.PurchaseDate = Convert.ToDateTime(dr["PurchaseDate"]);

                quoteModel.PurchaseDateFormated = quoteModel.PurchaseDate.HasValue ? quoteModel.PurchaseDate.Value.ToString("MM-dd-yyyy hh:mm tt") : "";

                quoteModel.Qty = dr["Qty"].ToString();
                quoteModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);

                quoteModelList.Add(quoteModel);
            }

            return quoteModelList;
        }

        public QuoteTemplateModel GetQuoteTemplateInfo(QuoteTemplateModel quoteModel)
        {
            DataTable dtQuote = _DBProduct.GetQuoteTemplateInfo(quoteModel);
            foreach (DataRow dr in dtQuote.Rows)
            {
                quoteModel = new QuoteTemplateModel();
                quoteModel.QuoteId = Convert.ToInt32(dr["QuoteId"]);
                quoteModel.TemplateName = dr["TemplateName"].ToString();
                quoteModel.QuoteTemplateGroupId = Convert.ToInt32(dr["QuoteTemplateGroupId"]);
                quoteModel.QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                quoteModel.QuoteDateFormated = quoteModel.QuoteDate.ToString("MM/dd/yyyy");
                quoteModel.SalesPersonId = Convert.ToInt32(dr["SalesPersonId"]);
                quoteModel.SalesPersonName = dr["SalesPersonName"].ToString();

                quoteModel.ContractTerm = Convert.ToInt32(dr["ContractTerm"]);
                quoteModel.ValidUntil = Convert.ToInt32(dr["ValidUntil"]);
                quoteModel.ZohoEntityId = dr["ZohoEntityId"].ToString();
                quoteModel.ZohoEntityType = dr["ZohoEntityType"].ToString();
                quoteModel.BillToCompanyName = dr["BillToCompanyName"].ToString();
                quoteModel.BillToAddress1 = dr["BillToAddress1"].ToString();
                quoteModel.BillToAddress2 = dr["BillToAddress2"].ToString();
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
                /*quoteModel.Purchased = Convert.ToBoolean(dr["Purchased"]);
                quoteModel.Processed = Convert.ToBoolean(dr["Processed"]);*/
            }

            return quoteModel;
        }

        public List<QuoteProductModel> GetQuoteProductTemplateList(QuoteProductModel quoteProductModel)
        {
            QuoteProductModel quoteProduct = null;
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            DataTable dtQuote = _DBProduct.GetQuoteProductTemplateList(quoteProductModel);
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
                salesOrder.OrderId = Convert.ToInt32(dr["QuoteId"]);
                salesOrder.ProductId = Convert.ToInt32(dr["ProductId"]);
                salesOrder.Quantity = Convert.ToInt32(dr["Quantity"]);
                salesOrder.Price = Convert.ToDouble(dr["Price"]);
                salesOrder.PackageId = Convert.ToInt32(dr["PackageId"]);
                salesOrder.ProductCategory = dr["ProductCategory"].ToString();
                salesOrder.SKU = dr["SKU"].ToString();
                salesOrder.ProductDescription = dr["ProductDescription"].ToString();

                salesOrderModelList.Add(salesOrder);
            }

            return salesOrderModelList;
        }

        public QuoteTemplateModel SetQuoteTemplateActive(QuoteTemplateModel model)
        {
            return _DBProduct.SetQuoteTemplateActive(model); ;
        }

        public List<DdlSourceModel> GetQuoteTemplateGroupList()
        {
            DdlSourceModel userTypeModel = null;
            List<DdlSourceModel> salesPersonList = new List<DdlSourceModel>();

            DataTable dtSalesPerson = _DBProduct.GetQuoteTemplateGroupList();
            foreach (DataRow dr in dtSalesPerson.Rows)
            {
                userTypeModel = new DdlSourceModel();
                userTypeModel.keyfield = dr["keyfield"].ToString();
                userTypeModel.value = dr["value"].ToString();

                salesPersonList.Add(userTypeModel);
            }

            return salesPersonList;
        }
    }
}
