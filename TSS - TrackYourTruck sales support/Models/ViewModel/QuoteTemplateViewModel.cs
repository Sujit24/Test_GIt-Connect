using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetTrackModel;

namespace TSS.Models.ViewModel
{
    public class QuoteTemplateViewModel
    {
        public QuoteTemplateModel QuoteTemplateModel { get; set; }
        public List<QuoteProductModel> QuoteProductModelList { get; set; }
        public List<SalesOrderModel> SalesOrderModelList { get; set; }

        public QuoteTemplateViewModel()
        {
            this.QuoteProductModelList = new List<QuoteProductModel>();
            this.SalesOrderModelList = new List<SalesOrderModel>();
        }
    }
}