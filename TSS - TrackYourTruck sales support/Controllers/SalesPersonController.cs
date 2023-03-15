using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSA.Security;
using NetTrackBiz;
using System.Configuration;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class SalesPersonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetSalesPersonList()
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            return Json(tytFacadeBiz.GetSalesPersonList(ConfigurationManager.AppSettings["ClientIdList"]));
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult Save(int id)
        {
            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

                tytFacadeBiz.SaveSalesPerson(id, true);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult Delete(int id)
        {
            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

                tytFacadeBiz.SaveSalesPerson(id, false);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }
    }
}
