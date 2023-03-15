using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetTrackModel;

namespace TSS.Helper
{
    public static class SessionVars
    {
        public static UserModel CurrentLoggedInUser
        {
            get
            {
                return HttpContext.Current.Session["CurrentLoggedInUser"] as UserModel;
            }
            set
            {
                HttpContext.Current.Session["CurrentLoggedInUser"] = value;
            }
        }

        public static string IsSessionAlive
        {
            get
            {
                return HttpContext.Current.Session["IsSessionAlive"]!=null?HttpContext.Current.Session["IsSessionAlive"].ToString():"No";
            }
            set
            {
                HttpContext.Current.Session["IsSessionAlive"] = value;
            }
        }
    }
}