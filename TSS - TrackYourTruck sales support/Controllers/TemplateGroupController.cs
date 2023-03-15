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
    public class TemplateGroupController : Controller
    {
        private TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, GSAAuthorize]
        public JsonResult GetTemplateGroupList()
        {
            return Json(tytFacadeBiz.GetAllTemplateGroup());
        }

        [HttpPost, GSAAuthorize]
        public JsonResult Save(TemplateGroupModel model)
        {
            try
            {
                tytFacadeBiz.SaveTemplateGroup(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorize]
        public JsonResult Delete(TemplateGroupModel model)
        {
            try
            {
                tytFacadeBiz.DeleteTemplateGroup(model);
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