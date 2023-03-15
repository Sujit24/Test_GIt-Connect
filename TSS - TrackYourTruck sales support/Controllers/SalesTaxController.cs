using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSS.Helper;
using NetTrackModel;
using NetTrackBiz;
using System.Web.Script.Serialization;
using GSA.Security;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class SalesTaxController : Controller
    {
        public ActionResult Index()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            return View();
        }

        [HttpPost, ActionName("GetSalesTaxList")]
        public ContentResult GetSalesTaxList(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<SalesTaxModel> salesTaxList = new List<SalesTaxModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                salesTaxList = tytFacadeBiz.GetSalesTaxList();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return LargeJsonResult(salesTaxList);
        }

        public ContentResult LargeJsonResult(List<SalesTaxModel> salesTaxList)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here  
            ContentResult result = new ContentResult();
            result.Content = serializer.Serialize(salesTaxList);
            result.ContentType = "application/json";

            return result;
        }

        [HttpPost, ActionName("Save"), GSAAuthorizeAttribute()]
        public ActionResult Save(List<SalesTaxModel> model)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                foreach (SalesTaxModel item in model)
                {
                    tytFacadeBiz.SaveSalesTax(item);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { Message = "Successfully Saved." });
        }
    }
}
