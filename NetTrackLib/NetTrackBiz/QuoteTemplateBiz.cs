using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTrackRepository;
using NetTrackModel;
using System.Configuration;

namespace NetTrackBiz
{
    internal class QuoteTemplateBiz
    {
        private QuoteTemplateRepository _quoteTemplateRepository;
        private List<ProductModel> _ProductModelList;
        private List<QuoteModel> _QuoteModelList;
        private List<QuoteTemplateModel> _QuoteTemplateModelList;
        private List<DdlSourceModel> _ddlSourceModelList;
        public QuoteTemplateBiz()
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _ProductModelList = new List<ProductModel>();
        }

        public List<ProductModel> GetProductList(int sessionId)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _ProductModelList = _quoteTemplateRepository.GetProductList(sessionId);

            return _ProductModelList;
        }

        public ProductModel GetDetailProductInfo(ProductModel productModel)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.GetDetailProductInfo(productModel);
        }

        public List<DdlSourceModel> GetProductTypeList(ProductModel productModel)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _ddlSourceModelList = _quoteTemplateRepository.GetProductTypeList(productModel);

            return _ddlSourceModelList;
        }

        public ProductModel SaveProduct(ProductModel model)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.SaveProduct(model);
        }

        public QuoteProductModel SaveQuoteTemplateProduct(QuoteProductModel model)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.SaveQuoteTemplateProduct(model);
        }

        public SalesOrderModel SaveSalesOrder(SalesOrderModel model)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.SaveSalesOrder(model);
        }

        public QuoteTemplateModel SaveQuoteTemplate(QuoteTemplateModel model)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.SaveQuoteTemplate(model);
        }

        public List<DdlSourceModel> GetSalesPersonList(int sessionId)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _ddlSourceModelList = _quoteTemplateRepository.GetSalesPersonList(sessionId);

            return _ddlSourceModelList;
        }

        public List<QuoteTemplateModel> GetQuoteTemplateList(int sessionId)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _QuoteTemplateModelList = _quoteTemplateRepository.GetQuoteTemplateList(sessionId);

            return _QuoteTemplateModelList;
        }

        public List<QuoteModel> GetSalesList(int sessionId)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _QuoteModelList = _quoteTemplateRepository.GetSalesList(sessionId);

            return _QuoteModelList;
        }

        //public QuoteTemplateModel GetQuoteTemplteInfo(QuoteTemplateModel quoteModel)
        //{
        //    _quoteTemplateRepository = new QuoteTemplateRepository();
        //    return _quoteTemplateRepository.GetQuoteTemplteInfo(quoteModel);
        //}

        public List<QuoteProductModel> GetQuoteProductTemplateList(QuoteProductModel quoteProductModel)
        {
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            _quoteTemplateRepository = new QuoteTemplateRepository();
            quoteProductModelList=  _quoteTemplateRepository.GetQuoteProductTemplateList(quoteProductModel);

            return quoteProductModelList;
        }

        public List<SalesOrderModel> GetSalesOrderList(SalesOrderModel salesOrderModel)
        {
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            _quoteTemplateRepository = new QuoteTemplateRepository();
            salesOrderModelList = _quoteTemplateRepository.GetSalesOrderList(salesOrderModel);
            return salesOrderModelList;
        }
        public QuoteTemplateModel GetQuoteTemplateInfo(QuoteTemplateModel quoteModel)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.GetQuoteTemplateInfo(quoteModel);
        }

        internal QuoteTemplateModel SetQuoteTemplateActive(QuoteTemplateModel model)
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            return _quoteTemplateRepository.SetQuoteTemplateActive(model);
        }

        public List<DdlSourceModel> GetQuoteTemplateGroupList()
        {
            _quoteTemplateRepository = new QuoteTemplateRepository();
            _ddlSourceModelList = _quoteTemplateRepository.GetQuoteTemplateGroupList();

            return _ddlSourceModelList;
        }
    }
}
