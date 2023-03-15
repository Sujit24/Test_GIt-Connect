using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using GSA.Security;
using GSA.Security.Models;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TSS.Models;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class LoginController : Controller
    {
        private static IMembershipService _MembershipService;
        private static GSAFormsAuthenticationService _GSAFormsAuthenticationService;

        public LoginController()
        {
            _MembershipService = new TytFacadeBiz();
            _GSAFormsAuthenticationService = new GSAFormsAuthenticationService(_MembershipService);
        }

        public ActionResult Index()
        {
            this.Logout();
            //if (!IsCompatibleBrowser())
            //{
            //    return View("BrowserCompatibilityInfo");
            //}
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login login)
        {

            //if (!IsCompatibleBrowser())
            //{
            //    return View("BrowserCompatibilityInfo");
            //}
            ErrorHandlerModel errorModel = new ErrorHandlerModel();
            errorModel.strMethodName = "Index(Login login)";
            errorModel.strMessage = "";
            string loginUrl50 = "";
            try
            {
                AuthStatus authStatus = _GSAFormsAuthenticationService.SignIn(login.UserName, login.Password, true);

                if (authStatus.Code == AuthStatusCode.SUCCESS)
                {
                    ViewBag.UserName = login.UserName;
                    ViewBag.Password = login.Password;

                    SessionVars.CurrentLoggedInUser = (UserModel)authStatus.Data;
                    SessionVars.CurrentLoggedInUser.Login = login.UserName;

                    if (!SessionVars.CurrentLoggedInUser.IsTSSAdmin && !SessionVars.CurrentLoggedInUser.IsTSSUser)
                    {
                        ModelState.AddModelError("", "The user doesn't have access.");
                        Logout();
                        return View();
                    }

                    //Set values to Cookie
                    AddCookies();

                    errorModel.strSessionId = SessionVars.CurrentLoggedInUser.SessionId.ToString();
                    errorModel.strUserId = SessionVars.CurrentLoggedInUser.EmployeeId.ToString();
                    errorModel.strUsername = SessionVars.CurrentLoggedInUser.EmployeeName;
                    errorModel.strLogin = SessionVars.CurrentLoggedInUser.Login;

                    SessionVars.IsSessionAlive = "Yes";

                    if (HttpContext.Request.UrlReferrer.Query.IndexOf("ReturnUrl") > -1)
                    {
                        var queryString = HttpUtility.ParseQueryString(HttpContext.Request.UrlReferrer.Query);
                        return Redirect(queryString["ReturnUrl"]);
                    }
                    else
                    {
                        return RedirectToAction("List", "Quote");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            catch (Exception ex)
            {
                errorModel.strMessage = ex.Message;
                ModelState.AddModelError("", "The user name or password provided is incorrect.");

            }
            finally
            {
                if (errorModel.strMessage == "")
                {
                    errorModel.strMessage = "Successful Login";
                }

            }
            if (errorModel.strMessage == "Blocked Client")
            {
                return Redirect(Url.Content(loginUrl50));
            }
            return View();
        }
        public ActionResult Logout()
        {
            _GSAFormsAuthenticationService.SignOut();
            //ClearCookies();
            CookieManager.RemoveAllCookies();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult Logout2()
        {
            _GSAFormsAuthenticationService.SignOut();
            //ClearCookies();
            CookieManager.RemoveAllCookies();
            return Json(new AjaxResponse { Message = "Success" });
        }
        public static void SessionLogout()
        {
            _GSAFormsAuthenticationService.SignOut();
        }

        public void AddCookies()
        {
            try
            {
                CookieManager.AddCookie("strTSSSessionId", SessionVars.CurrentLoggedInUser.SessionId.ToString());
                CookieManager.AddCookie("strTSSUserId", SessionVars.CurrentLoggedInUser.EmployeeId.ToString());
                CookieManager.AddCookie("strTSSClientId", SessionVars.CurrentLoggedInUser.ClientId.ToString());
                CookieManager.AddCookie("strTSSUsername", SessionVars.CurrentLoggedInUser.EmployeeName);
                CookieManager.AddCookie("strTSSLogin", SessionVars.CurrentLoggedInUser.Login);
                CookieManager.AddCookie("strTSSEmployeeClientId", SessionVars.CurrentLoggedInUser.EmployeeClientId.ToString());
                CookieManager.AddCookie("strTSSIsTSSAdmin", SessionVars.CurrentLoggedInUser.IsTSSAdmin.ToString());
                CookieManager.AddCookie("strTSSIsTSSUser", SessionVars.CurrentLoggedInUser.IsTSSUser.ToString());
                CookieManager.AddCookie("strTSSIsTSSQuoteApproval", SessionVars.CurrentLoggedInUser.IsTSSQuoteApproval.ToString());
            }
            catch (Exception ex) { }
        }
        public void ClearCookies()
        {
            try
            {
                CookieManager.RemoveCookie("strTSSSessionId");
                CookieManager.RemoveCookie("strTSSUserId");
                CookieManager.RemoveCookie("strTSSClientId");
                CookieManager.RemoveCookie("strTSSUsername");
                CookieManager.RemoveCookie("strTSSLogin");
                CookieManager.RemoveCookie("strTSSEmployeeClientId");
                CookieManager.RemoveCookie("strTSSClientName");
                CookieManager.RemoveCookie("strTSSIsTSSAdmin");
                CookieManager.RemoveCookie("strTSSIsTSSUser");
                CookieManager.RemoveCookie("strTSSIsTSSQuoteApproval");
            }
            catch (Exception ex) { }
        }
        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
        /// <summary>      
        /// </summary>
        /// <param name="id">though variable name is "id" but it receive an encrypted string. after decrypting we will get a string formatted like "username pass currentdatetime"</param>
        /// <returns></returns>
        public ActionResult InternalLogin(string id)
        {
            try
            {
                //if (!IsCompatibleBrowser())
                //{
                //    return View("BrowserCompatibilityInfo");
                //}

                string decodedString = DecodeFrom64(id);
                string[] arr = Regex.Split(decodedString, " ");

                Login login = new Login();

                login.UserName = arr[1];
                login.Password = arr[2];

                DateTime tokenCreationTime = DateTime.ParseExact(arr[3], "yyyy-MM-dd-hh-mm", null);


                AuthStatus authStatus = _GSAFormsAuthenticationService.SignIn(login.UserName, login.Password, false);

                if (authStatus.Code == AuthStatusCode.SUCCESS)
                {
                    List<UserModel> userModelList = new List<UserModel>() { authStatus.Data as UserModel };
                    ViewBag.UserName = login.UserName;
                    ViewBag.Password = login.Password;


                    SessionVars.CurrentLoggedInUser = (UserModel)authStatus.Data;
                    SessionVars.CurrentLoggedInUser.Login = login.UserName;
                    //Set values to Cookie
                    AddCookies();


                    if (SessionVars.CurrentLoggedInUser.Roles.Contains("HaveAdmin"))
                    {
                        HttpContext.Cache["HaveAdmin"] = "Yes";
                    }
                    else
                    {
                        HttpContext.Cache["HaveAdmin"] = "No";
                    }
                    SessionVars.IsSessionAlive = "Yes";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");

            }
            return View();
        }

        #region Retrive Login Information

        public ActionResult RetriveLoginIndex()
        {
            return View("RetriveLogin");
        }
        public JsonResult SendEmailLoginInformation(string email)
        {
            string strSendingMailStatus = "";
            try
            {

                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                UserModel userModel = new UserModel() { Email = email };
                userModel = tytFacadeBiz.GetUserLoginInfo(userModel);
                string from, to, bcc, cc, subject, body, displayName;

                from = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                to = ConfigurationManager.AppSettings["EmailTo"].ToString();
                cc = ConfigurationManager.AppSettings["EmailCC"].ToString();
                bcc = ConfigurationManager.AppSettings["EmailBCC"].ToString();
                subject = ConfigurationManager.AppSettings["EmailSubject"].ToString();
                body = ConfigurationManager.AppSettings["EmailBody"].ToString();
                displayName = ConfigurationManager.AppSettings["EmailDisplayName"].ToString();

                if (userModel.Email != "" && userModel.Login != "")
                {
                    to = userModel.Email;
                    subject = "Track Your Truck Account Info";
                    body = "<h3>Dear " + userModel.FirstName + " " + userModel.LastName + ",</h3>";
                    body += "<h4>Your Track Your Truck Account Information is - </h4>";
                    body += "<b>User Name: " + userModel.Login + "</b><br />";
                    body += "<b>Password: " + userModel.Pin + "</b><br /><br /><br />";
                    body += "Thank you, <b>Track Your Truck</b><br />";

                    strSendingMailStatus = EmailSender.SendMailMessage(from, to, cc, bcc, subject, body, displayName);
                }
                else
                {
                    strSendingMailStatus = "Not Found";
                }
            }
            catch (Exception ex) { }
            //return strSendingMailStatus;
            return Json(new AjaxResponse { Message = strSendingMailStatus });
        }

        #endregion

        public bool IsBlockedClient(int clientId)
        {
            bool isBlock = false;
            string strBlockClients = ConfigurationManager.AppSettings["BlockClient"];
            if (!String.IsNullOrEmpty(strBlockClients))
            {
                if (strBlockClients.Contains(","))
                {
                    string[] blockClients = strBlockClients.Split(',');
                    int blockClientId = 0;
                    for (int i = 0; i < blockClients.Length; i++)
                    {
                        try
                        {
                            blockClientId = Int32.Parse(blockClients[i].ToString());
                            if (blockClientId == clientId)
                            {
                                isBlock = true;
                                break;
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return isBlock;
        }

        //public bool IsCompatibleBrowser()
        //{
        //    bool isCompatible =false;
        //    if ( Request.Browser.Type.ToLower().Contains("firefox") || Request.Browser.Type.ToLower().Contains("chrome"))
        //    {
        //        isCompatible = true; 
        //    }
        //    return isCompatible;
        //}
    }
}
