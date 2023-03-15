using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace TSS.Helper
{
    public class CrystalReportPrefetch
    {
        public static void Prefetch(HttpServerUtilityBase serverUtility)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string reportPath = serverUtility.MapPath("~/Reports/");

                    using (ReportDocument reportDoc = new ReportDocument())
                    {
                        reportDoc.Load(reportPath + "QuoteProducts.rpt");
                        reportDoc.Load(reportPath + "SalesOrder.rpt");
                    }
                }
                catch (Exception ex)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(ex); /// due issue 
                }
            });
        }
    }
}