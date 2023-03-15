using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetTrackModel;

namespace TSS.Models.ViewModel
{
    public class QuoteViewModel
    {
        public QuoteModel QuoteModel { get; set; }
        public List<QuoteProductModel> QuoteProductModelList { get; set; }

        public QuoteOrderModel QuoteOrderModel { get; set; }
        public List<SalesOrderModel> SalesOrderModelList { get; set; }

        public SessionViewModel SessionViewModel { get; set; }

        public double TotalValue { get; set; }

        public QuoteViewModel()
        {
            this.QuoteProductModelList = new List<QuoteProductModel>();
            this.SalesOrderModelList = new List<SalesOrderModel>();
        }
    }
}