using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTrackRepository;
using NetTrackModel;
using System.Configuration;

namespace NetTrackBiz
{
    internal class ProductBiz
    {
        private ProductRepository _ProductRepository;
        private List<ProductModel> _ProductModelList;
        private List<QuoteModel> _QuoteModelList;
        private List<DdlSourceModel> _ddlSourceModelList;
        public ProductBiz()
        {
            _ProductRepository = new ProductRepository();
            _ProductModelList = new List<ProductModel>();
        }

        public List<ProductModel> GetProductList(int sessionId)
        {
            _ProductRepository = new ProductRepository();
            _ProductModelList = _ProductRepository.GetProductList(sessionId);

            return _ProductModelList;
        }

        public ProductModel GetDetailProductInfo(ProductModel productModel)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetDetailProductInfo(productModel);
        }

        public List<DdlSourceModel> GetProductTypeList(ProductModel productModel)
        {
            _ProductRepository = new ProductRepository();
            _ddlSourceModelList = _ProductRepository.GetProductTypeList(productModel);

            return _ddlSourceModelList;
        }

        public ProductModel SaveProduct(ProductModel model)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.SaveProduct(model);
        }

        public QuoteProductModel SaveQuoteProduct(QuoteProductModel model)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.SaveQuoteProduct(model);
        }

        public SalesOrderModel SaveSalesOrder(SalesOrderModel model)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.SaveSalesOrder(model);
        }

        public QuoteModel SaveQuote(QuoteModel model)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.SaveQuote(model);
        }

        public void ResetQuoteStartDate(int quoteId)
        {
            _ProductRepository = new ProductRepository();
            _ProductRepository.ResetQuoteStartDate(quoteId);
        }

        public void ApproveQuote(QuoteModel model)
        {
            _ProductRepository = new ProductRepository();
            _ProductRepository.ApproveQuote(model);
        }

        public QuoteOrderModel SaveQuoteOrder(QuoteOrderModel model)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.SaveQuoteOrder(model);
        }

        public List<SalesPerson> GetSalesPersonList(int sessionId)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetSalesPersonList(sessionId);
        }

        public List<SalesPerson> GetSalesPersonList(string clientIdList)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetSalesPersonList(clientIdList);
        }

        public void SaveSalesPerson(int employeeId, bool isSet)
        {
            new ProductRepository().SaveSalesPerson(employeeId, isSet);
        }

        public List<QuoteModel> GetQuoteList(QuoteModel model)
        {
            _ProductRepository = new ProductRepository();
            _QuoteModelList = _ProductRepository.GetQuoteList(model);

            return _QuoteModelList;
        }

        public List<QuoteOrderModel> GetSalesList(QuoteOrderModel model)
        {
            _ProductRepository = new ProductRepository();

            return _ProductRepository.GetSalesList(model);
        }

        public QuoteModel GetQuoteInfo(QuoteModel quoteModel)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetQuoteInfo(quoteModel);
        }

        public QuoteOrderModel GetQuoteOrderInfo(QuoteOrderModel quoteOrderModel)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetQuoteOrderInfo(quoteOrderModel);
        }

        public List<QuoteProductModel> GetQuoteProductList(QuoteProductModel quoteProductModel)
        {
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            _ProductRepository = new ProductRepository();
            quoteProductModelList = _ProductRepository.GetQuoteProductList(quoteProductModel);

            return quoteProductModelList;
        }

        public List<SalesOrderModel> GetSalesOrderList(SalesOrderModel salesOrderModel)
        {
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            _ProductRepository = new ProductRepository();
            salesOrderModelList = _ProductRepository.GetSalesOrderList(salesOrderModel);
            return salesOrderModelList;
        }

        public List<ClientModel> GetNetTrackClientList(ClientModel clientModel)
        {
            _ProductRepository = new ProductRepository();

            return _ProductRepository.GetNetTrackClientList(clientModel);
        }

        public List<DdlSourceModel> GetOrderStatusList(int sessionId)
        {
            _ProductRepository = new ProductRepository();
            _ddlSourceModelList = _ProductRepository.GetOrderStatusList(sessionId);

            return _ddlSourceModelList;
        }

        public List<QuotePaymentMethodModel> GetQuotePaymentMethodList()
        {
            return _ProductRepository.GetQuotePaymentMethodList();
        }

        public void SetQuoteOrderStatus(QuoteOrderModel model)
        {
            _ProductRepository.SetQuoteOrderStatus(model);
        }

        public List<QuoteLastViewModel> GetQuoteLastViewList(int quoteId)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetQuoteLastViewList(quoteId); ;
        }

        public List<TSSSentEmailModel> GetSentEmailList(int quoteId)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetSentEmailList(quoteId); ;
        }

        public List<TSSSentEmailModel> GetSentEmailReportList(TSSSentEmailModel sentEmailModel)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.GetSentEmailReportList(sentEmailModel); ;
        }

        public List<TSSSentEmailModel> GetResendEmailList()
        {
            return new ProductRepository().GetResendEmailList();
        }

        public TSSSentEmailModel SaveTSSSentEmail(TSSSentEmailModel model)
        {
            _ProductRepository = new ProductRepository();
            return _ProductRepository.SaveTSSSentEmail(model); ;
        }

        #region OrderStatusHistory

        public OrderStatusHistory SaveOrderStatusHistory(int quoteOrderId, int orderStatusId)
        {
            OrderStatusHistory model = new OrderStatusHistory();
            model.QuoteOrderId = quoteOrderId;
            model.OrderStatusId = orderStatusId;
            model.StatusDate = DateTime.Now;

            return _ProductRepository.SaveOrderStatusHistory(model);
        }

        public List<OrderStatusHistory> GetOrderStatusHistoryList(int quoteOrderId)
        {
            return _ProductRepository.GetOrderStatusHistoryList(quoteOrderId);
        }

        #endregion

        #region QuoteEmailUnsubscribe

        public QuoteEmailUnsubscribeModel SaveQuoteEmailUnsubscribe(QuoteEmailUnsubscribeModel model)
        {
            return _ProductRepository.SaveQuoteEmailUnsubscribe(model);
        }

        public List<QuoteEmailUnsubscribeModel> GetQuoteEmailUnsubscribeList(int quoteId)
        {
            return _ProductRepository.GetQuoteEmailUnsubscribeList(quoteId);
        }

        #endregion

        #region QuoteLinkMapping

        public QuoteLinkMappingModel GetQuoteLinkMapping(string encryptValue)
        {
            return _ProductRepository.GetQuoteLinkMapping(encryptValue);
        }

        public void SaveQuoteLinkMapping(QuoteLinkMappingModel model)
        {
            _ProductRepository.SaveQuoteLinkMapping(model);
        }

        #endregion

        #region TSS Shipping

        public void SaveTssShipping(TssShippingModel model)
        {
            _ProductRepository.SaveTssShipping(model);
        }

        public List<TssShippingModel> GetTssShippingHistory(TssShippingModel model)
        {
            return _ProductRepository.GetTssShippingHistory(model);
        }

        public void UpdateTssShipSentEmail(TssShippingModel model)
        {
            _ProductRepository.UpdateTssShipSentEmail(model);
        }

        public List<TssShippingEmailLogModel> GetTssShippingEmailHistory(TssShippingModel model)
        {
            return _ProductRepository.GetTssShippingEmailHistory(model);
        }

        #endregion
    }
}
