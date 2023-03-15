using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GSA.Security;
using NetTrackDBContext;
using NetTrackModel;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class QuoteProductRptController : Controller
    {
        private HttpContextBase _Context;

        public QuoteProductRptController() { }

        public QuoteProductRptController(HttpContextBase context)
        {
            this._Context = context;
        }

        [GSAAuthorizeAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        [GSAAuthorizeAttribute()]
        public void ShowQuoteProductRpt(string id)
        {
            try
            {
                //int quoteId = int.Parse(Cipher.Decrypt(id));
                int quoteId = int.Parse(id);
                string fileName = "QuoteProduct_" + id.ToString();

                ReportDocument reportDoc = GetReport(quoteId);
                reportDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, fileName);
                reportDoc.Close();
                reportDoc.Dispose();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public ReportDocument GetReport(int quoteId)
        {
            QuoteModel quoteModel = new QuoteModel();
            quoteModel.SessionId = 0;
            quoteModel.QuoteId = quoteId;

            DBProduct dbProduct = new DBProduct();
            DataSet dsRpt = dbProduct.GetQuoteProductRpt(quoteModel);
            dsRpt.Tables[0].TableName = "Quote";
            dsRpt.Tables[1].TableName = "OuoteProduct";

            /*string schema = Server.MapPath("~/ReportSchema/rptQuoteProduct.xsd");
            dsRpt.WriteXmlSchema(schema);*/

            ReportDocument reportDoc = new ReportDocument();
            string ReportPath = (HttpContext ?? _Context).Server.MapPath("~/Reports/QuoteProducts.rpt");
            reportDoc.Load(ReportPath);
            reportDoc.SetDataSource(dsRpt);

            return reportDoc;
        }

        public Stream GetPDFStream(int quoteId, string orderNowUrl)
        {
            Stream pdfStream = null;

            try
            {
                QuoteModel quoteModel = new QuoteModel();
                quoteModel.SessionId = 0;
                quoteModel.QuoteId = quoteId;

                DBProduct dbProduct = new DBProduct();
                DataSet dsRpt = dbProduct.GetQuoteProductRpt(quoteModel);
                dsRpt.Tables[0].TableName = "Quote";
                dsRpt.Tables[1].TableName = "OuoteProduct";

                /*string schema = Server.MapPath("~/ReportSchema/rptQuoteProduct.xsd");
                dsRpt.WriteXmlSchema(schema);*/

                using (ReportDocument reportDoc = new ReportDocument())
                {
                    string ReportPath = (HttpContext ?? _Context).Server.MapPath("~/Reports/QuotePDF.rpt");
                    reportDoc.Load(ReportPath);
                    reportDoc.SetDataSource(dsRpt);
                    reportDoc.SetParameterValue("OrderNow", orderNowUrl);

                    pdfStream = reportDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                }
            }
            catch { }

            return pdfStream;
        }
    }
}
