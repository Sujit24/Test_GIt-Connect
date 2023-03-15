using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TSS.Models.ViewModel;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        public ActionResult LoadDDL(string spName)
        {
            TytFacadeBiz _biz = new TytFacadeBiz();
            CommonDDLmodel dModel = new CommonDDLmodel(SessionVars.CurrentLoggedInUser.SessionId, spName);
            DDLViewModel vm = new DDLViewModel();

            vm.List = _biz.GetDdlDataSource(dModel);
            return View("DDL", vm);
        }

        public JsonResult LoadDDLJson(string spName)
        {
            TytFacadeBiz _biz = new TytFacadeBiz();
            CommonDDLmodel dModel = new CommonDDLmodel(SessionVars.CurrentLoggedInUser.SessionId, spName);

            return Json(_biz.GetDdlDataSource(dModel));
        }

        public ActionResult LoadDDLWithParam(string spName,string parameters)
        {
            TytFacadeBiz _biz = new TytFacadeBiz();
            DDLViewModel vm = new DDLViewModel();
            vm.List = _biz.GetDdlDataSource(spName,parameters);
            return View("DDL", vm);
        }

        public JsonResult LoadDDLWithParamJson(string spName, string parameters)
        {
            TytFacadeBiz _biz = new TytFacadeBiz();
            return Json(_biz.GetDdlDataSource(spName, parameters), JsonRequestBehavior.AllowGet);
        }

    }
}
