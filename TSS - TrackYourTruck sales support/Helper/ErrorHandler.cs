using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using System.Configuration;
using NetTrackModel;

namespace TSS.Helper
{
    public static class ErrorHandler
    {        
        public static void WriteError(ErrorHandlerModel errorModel)
        {
            try
            {
                string strEnableLogin = ConfigurationManager.AppSettings["enableLogin"];
                if (strEnableLogin != "true") { return; }
                if (String.IsNullOrEmpty(errorModel.strSessionId)) { return; }

                if (errorModel.strUsername.Contains(","))
                {
                    errorModel.strUsername = errorModel.strUsername.Replace(",", "");
                }
                //string filePath = ConfigurationManager.AppSettings["errorLog"];

                string filePath = "~/ErrorLog/" + errorModel.strLogin + "_" + errorModel.strSessionId + "_" + DateTime.Now.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(filePath)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(filePath)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(filePath)))
                {
                    w.WriteLine("\r\nLog Entry: ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "SessionId: " + errorModel.strSessionId +
                                ". UserId: " + errorModel.strUserId +
                                ". Username: " + errorModel.strUsername +
                                ". Function: " + errorModel.strMethodName;
                    if (!String.IsNullOrEmpty(errorModel.strMessage))
                    {
                        if (errorModel.strMessage == "Successful Login")
                        {
                            err = err + ". Message: " + errorModel.strMessage + " by user " + errorModel.strUsername;
                        }
                        else if (errorModel.strMessage.Contains("Your Session has been cancelled"))
                        {
                            err = err + ". Message: " + errorModel.strMessage;
                        }
                        else
                        {
                            err = err + ". Error Message:" + errorModel.strMessage +
                                ". Error in: " + System.Web.HttpContext.Current.Request.Url.ToString();
                        }
                    }
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                errorModel.strMessage = ex.Message;
                WriteError(errorModel);
            }
        }        
    }
}