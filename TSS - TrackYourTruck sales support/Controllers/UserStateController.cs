using System;
using System.Web.Mvc;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TYT.Helper;

namespace TSS.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [RequireHttpsCustomAttribute]
    public class UserStateController : Controller
    {
        // GET: /UserState/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult MaintainUserState(string loadType)
        {
            UserStateModel userStateModel = new UserStateModel();
            try
            {
                if (SessionVars.CurrentLoggedInUser == null)
                {
                    UserModel userModel = CookieManager.ReloadSessionFromCookie();
                    SessionVars.CurrentLoggedInUser = userModel;
                }
            }
            catch (Exception) { }

            try
            {
                if (SessionVars.CurrentLoggedInUser != null)
                {
                    UserStateModel tempUserStateModel = new UserStateModel();
                    tempUserStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    tempUserStateModel.UserId = SessionVars.CurrentLoggedInUser.EmployeeId;
                    tempUserStateModel.ClientId = SessionVars.CurrentLoggedInUser.ClientId;
                    tempUserStateModel.LoadType = loadType;
                    tempUserStateModel.IsAdmin = "0";
                    tempUserStateModel.DbAction = "S";
                    bool isAdmin = false;
                    try
                    {
                        if (System.Web.HttpContext.Current.Cache["HaveAdmin"].ToString() == "Yes")
                        {
                            isAdmin = true;
                            tempUserStateModel.IsAdmin = "1";
                        }
                    }
                    catch (Exception) { }

                    TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                    userStateModel = tytFacadeBiz.MaintainUserState(tempUserStateModel);
                    userStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    userStateModel.Login = SessionVars.CurrentLoggedInUser.Login;
                    if (userStateModel.ClientId == 0)
                    {
                        try
                        {
                            userStateModel.ClientId = Int32.Parse(CookieManager.GetCookie("strClientId"));
                        }
                        catch (Exception)
                        {
                            userStateModel.ClientId = 18;
                        }
                    }
                   
                    // This condition only for cemtek admin users
                    if ((SessionVars.CurrentLoggedInUser.ClientId != userStateModel.ClientId) && (isAdmin))
                    {
                        SessionVars.CurrentLoggedInUser.ClientId = userStateModel.ClientId;
                    }
                    if (SessionVars.CurrentLoggedInUser.SubscriptionLevelId == 1)
                    {
                        userStateModel.FollowMe = "0";
                    }
                }
            }
            catch (Exception)
            {
                //Elmah.ErrorSignal.FromCurrentContext();
            }
            return Json(userStateModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MaintainUserStateGridPage(string loadType)
        {
            UserStateModel userStateModel = new UserStateModel();
            try
            {
                if (SessionVars.CurrentLoggedInUser == null)
                {
                    UserModel userModel = CookieManager.ReloadSessionFromCookie();
                    SessionVars.CurrentLoggedInUser = userModel;
                }
            }
            catch (Exception) { }

            try
            {
                if (SessionVars.CurrentLoggedInUser != null)
                {
                    UserStateModel tempUserStateModel = new UserStateModel();
                    tempUserStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    tempUserStateModel.UserId = SessionVars.CurrentLoggedInUser.EmployeeId;
                    tempUserStateModel.ClientId = SessionVars.CurrentLoggedInUser.ClientId;
                    tempUserStateModel.LoadType = loadType;
                    tempUserStateModel.IsAdmin = "0";
                    tempUserStateModel.DbAction = "S";
                    bool isAdmin = false;
                    try
                    {
                        if (System.Web.HttpContext.Current.Cache["HaveAdmin"].ToString() == "Yes")
                        {
                            isAdmin = true;
                            tempUserStateModel.IsAdmin = "1";
                        }
                    }
                    catch (Exception) { }

                    TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                    userStateModel = tytFacadeBiz.MaintainUserStateGridPage(tempUserStateModel);
                    userStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    userStateModel.Login = SessionVars.CurrentLoggedInUser.Login;
                    if (userStateModel.ClientId == 0)
                    {
                        try
                        {
                            userStateModel.ClientId = Int32.Parse(CookieManager.GetCookie("strClientId"));
                        }
                        catch (Exception)
                        {
                            userStateModel.ClientId = 18;
                        }
                    }

                    // This condition only for cemtek admin users
                    if ((SessionVars.CurrentLoggedInUser.ClientId != userStateModel.ClientId) && (isAdmin))
                    {
                        SessionVars.CurrentLoggedInUser.ClientId = userStateModel.ClientId;
                    }
                    if (SessionVars.CurrentLoggedInUser.SubscriptionLevelId == 1)
                    {
                        userStateModel.FollowMe = "0";
                    }
                }
            }
            catch (Exception)
            {
                //Elmah.ErrorSignal.FromCurrentContext();
            }
            return Json(userStateModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPut]
        public JsonResult MaintainUserState(UserStateModel userStateModel)
        {
            try
            {
                if (SessionVars.CurrentLoggedInUser == null)
                {
                    UserModel userModel = CookieManager.ReloadSessionFromCookie();
                    SessionVars.CurrentLoggedInUser = userModel;
                }
            }
            catch (Exception) { }
            try
            {
                if (SessionVars.CurrentLoggedInUser != null)
                {

                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                userStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                userStateModel.UserId = SessionVars.CurrentLoggedInUser.EmployeeId;
                string strMiniMap = "";
                if (userStateModel.LeftMapModelList != null)
                {
                    foreach (LeftMapModel lmModel in userStateModel.LeftMapModelList)
                    {
                        strMiniMap += lmModel.OrderId + "_" + lmModel.TruckId + ",";
                    }
                }
                userStateModel.MiniMaps = strMiniMap;

                userStateModel.DbAction = "U";
                userStateModel = tytFacadeBiz.MaintainUserState(userStateModel);
                if (userStateModel.SessionId == 0)
                {
                    userStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                }
                if (userStateModel.UserId == 0)
                {
                    userStateModel.UserId = SessionVars.CurrentLoggedInUser.EmployeeId;
                }
                if (userStateModel.ClientId == 0)
                {
                    try
                    {
                        userStateModel.ClientId = Int32.Parse(CookieManager.GetCookie("strClientId"));
                    }
                    catch (Exception)
                    {
                        userStateModel.ClientId = 18;
                    }
                }
                if (SessionVars.CurrentLoggedInUser.SubscriptionLevelId == 1)
                {
                    userStateModel.FollowMe = "0";
                }
                }
            }
            catch (Exception)
            {
                //Elmah.ErrorSignal.FromCurrentContext();
            }
            return Json(userStateModel, JsonRequestBehavior.AllowGet);
        }       
        [HttpPut]
        public JsonResult MaintainUserStateGridPage(UserStateModel userStateModel)
        {
            try
            {
                if (SessionVars.CurrentLoggedInUser == null)
                {
                    UserModel userModel = CookieManager.ReloadSessionFromCookie();
                    SessionVars.CurrentLoggedInUser = userModel;
                }
            }
            catch (Exception) { }
            try
            {
                if (SessionVars.CurrentLoggedInUser != null)
                {

                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                userStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                userStateModel.UserId = SessionVars.CurrentLoggedInUser.EmployeeId;
               
                
                

                userStateModel.DbAction = "U";
                userStateModel = tytFacadeBiz.MaintainUserStateGridPage(userStateModel);
                if (userStateModel.SessionId == 0)
                {
                    userStateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                }
                if (userStateModel.UserId == 0)
                {
                    userStateModel.UserId = SessionVars.CurrentLoggedInUser.EmployeeId;
                }
                if (userStateModel.ClientId == 0)
                {
                    try
                    {
                        userStateModel.ClientId = Int32.Parse(CookieManager.GetCookie("strClientId"));
                    }
                    catch (Exception)
                    {
                        userStateModel.ClientId = 18;
                    }
                }
                
                }
            }
            catch (Exception)
            {
                //Elmah.ErrorSignal.FromCurrentContext();
            }
            return Json(userStateModel, JsonRequestBehavior.AllowGet);
        }        
    }
}
