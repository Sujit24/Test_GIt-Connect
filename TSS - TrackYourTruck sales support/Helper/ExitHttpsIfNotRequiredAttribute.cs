using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TYT.Attributes
{

    public class ExitHttpsIfNotRequiredAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // abort if it's not a secure connection
            if (!filterContext.HttpContext.Request.IsSecureConnection || filterContext.HttpContext.Request.IsLocal || System.Configuration.ConfigurationManager.AppSettings["ApplyHttpsChange"] != "true") return;

            // abort if a [RequireHttps] attribute is applied to controller or action
            if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RequireHttpsAttribute), true).Length > 0) return;
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequireHttpsAttribute), true).Length > 0) return;
            
            // abort if it's not a GET request -
            // we don't want to be redirecting on a form post
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod,"GET", StringComparison.OrdinalIgnoreCase)) return;

            // abort if it's a child action
            if (filterContext.IsChildAction) return;

            // redirect to HTTP
            string url = "http://" + filterContext.HttpContext.Request.Url.Host +filterContext.HttpContext.Request.RawUrl;

            filterContext.Result = new RedirectResult(url);

        }
    }


}
