using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetTrackModel;
using NetTrackDBContext;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using TSS.Helper;
using CrystalDecisions.Shared;
using System.IO;
using GSA.Security;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class SalesOrderRptController : Controller
    {
        [GSAAuthorizeAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        public void SalesOrderPDF(string id)
        {
            SalesOrderRpt(int.Parse(Cipher.Decrypt(id)));
        }

        [GSAAuthorizeAttribute()]
        public void ShowSalesOrderRpt(int id)
        {
            SalesOrderRpt(id);
        }

        private void SalesOrderRpt(int quoteOrderId)
        {
            try
            {
                QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
                quoteOrderModel.SessionId = 0;
                quoteOrderModel.QuoteOrderId = quoteOrderId;

                DBProduct dbProduct = new DBProduct();
                DataSet dsRpt = dbProduct.GetSalesOrderRpt(quoteOrderModel);
                dsRpt.Tables[0].TableName = "Quote";
                dsRpt.Tables[1].TableName = "OuoteProduct";

                /*string schema = Server.MapPath("~/ReportSchema/rptQuoteProduct.xsd");
                dsRpt.WriteXmlSchema(schema);*/

                using (ReportDocument ReportDoc = new ReportDocument())
                {
                    string ReportPath = Server.MapPath("~/Reports/SalesOrder.rpt");
                    ReportDoc.Load(ReportPath);
                    ReportDoc.SetDataSource(dsRpt);

                    //'Directory for PDF
                    string appDir = Server.MapPath("~/PdfReports/");
                    if (!Directory.Exists(appDir))
                        Directory.CreateDirectory(appDir);
                    string fileName = "SalesOrder_" + quoteOrderId.ToString();
                    string filePath = appDir + fileName;

                    ReportDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, fileName);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}
