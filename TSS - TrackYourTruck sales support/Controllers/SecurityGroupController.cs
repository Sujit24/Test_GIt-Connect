using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSA.Security;
using NetTrackBiz;
using System.Configuration;
using NetTrackModel;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class SecurityGroupController : Controller
    {
        private TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, GSAAuthorize]
        public JsonResult GetSecurityGroupList()
        {
            return Json(tytFacadeBiz.GetAllSecurityGroup());
        }

        [HttpPost, GSAAuthorize]
        public JsonResult Save(SecurityGroupModel model)
        {
            try
            {
                tytFacadeBiz.SaveSecurityGroup(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorize]
        public JsonResult Delete(SecurityGroupModel model)
        {
            try
            {
                tytFacadeBiz.DeleteSecurityGroup(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorize]
        public JsonResult GetSalesPersonList(SecurityGroupSalesPersonModel model)
        {
            model.ClientIds = ConfigurationManager.AppSettings["ClientIdList"];

            return Json(tytFacadeBiz.GetSalesPersonsBySecurityGroupId(model));
        }

        [HttpPost, GSAAuthorize]
        public JsonResult SaveSalesPerson(SecurityGroupSalesPersonModel model)
        {
            try
            {
                tytFacadeBiz.SaveSalesPersonSecurityGroup(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorize]
        public JsonResult DeleteSalesPerson(SecurityGroupSalesPersonModel model)
        {
            try
            {
                tytFacadeBiz.DeleteSalesPersonSecurityGroup(model);
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