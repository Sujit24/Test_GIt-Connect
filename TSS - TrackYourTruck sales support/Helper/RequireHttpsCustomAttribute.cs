using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TYT.Helper
{
    public class RequireHttpsCustomAttribute : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsLocal && System.Configuration.ConfigurationManager.AppSettings["ApplyHttpsChange"] == "true")
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        }

    }
}
