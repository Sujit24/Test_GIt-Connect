
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
//using Common.Models;

namespace GSA.Security
{
    public class GSAAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private bool _isAuthenticated = false;
        private bool _isAuthorized = false;
        
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            _isAuthorized = false;

            if (_isAuthenticated)
            {
                if (this.Roles.Length > 0)
                {
                    string[] roles = this.Roles.Split(',');
                    GenericPrincipal user = httpContext.User as GenericPrincipal;
                    if (user != null)
                    {
                        foreach (string role in roles)
                        {
                            if (user.IsInRole(role))
                            {
                                _isAuthorized = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    _isAuthorized = true;
                }
            }
            
            return _isAuthorized;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            _isAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string returnUrl = string.Format("/{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
            
            /*if (IsAjaxRequest(filterContext.HttpContext.Request))
            {
                StatusMessage statusMessage = new StatusMessage();
                if (!_isAuthenticated)
                {
                    statusMessage.Data = new { errorCode = -1, returnUrl = returnUrl };
                    statusMessage.Message = "Please sign in.";
                }
                else if (!_isAuthorized)
                {
                    statusMessage.Data = new { errorCode = -2, returnUrl = returnUrl };
                    statusMessage.Message = "Unauthorized access attempt!";
                }
                JsonResult jsonResult = new JsonResult();
                jsonResult.Data = statusMessage;
                filterContext.Result = jsonResult;
            }*/
            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
                
            //    if (!_isAuthenticated)
            //    {
            //        string message = string.Empty;
            //        message = "Please sign in.";
            //        JsonResult jsonResult = new JsonResult();
            //        jsonResult.Data = message;
            //        filterContext.Result = jsonResult;
            //    }
               
               
            //}
            //else
            //{
                filterContext.HttpContext.Session["ReturnUrl"] = returnUrl;

                if (!_isAuthenticated)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else if (!_isAuthorized)
                {
                    ViewResult viewResult = new ViewResult();
                    viewResult.ViewName = "UnauthorizedAccess";
                    filterContext.Result = viewResult;
                }
            //}
        }

        private static bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (request["X-Requested-With"] == "XMLHttpRequest" || request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return false;
        }
    }
}
