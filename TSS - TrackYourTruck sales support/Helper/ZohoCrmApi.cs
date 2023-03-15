using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;


namespace TSS.Helper
{
    public class ZohoCrmApi
    {
        public static string zohocrmurl = "https://crm.zoho.com/crm/private/xml/";
        private string _zohoAuthToken = ConfigurationManager.AppSettings["_zohoAuthToken"];
        //'https://crm.zoho.com/crm/private/json/Accounts/getSearchRecords?authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(Account Name|contains|*' + $('#Search-Zuhu-pattern').val() + '*)';


        public static String APIMethod(string modulename, string methodname, string recordId)
        {
            string uri = zohocrmurl + modulename + "/" + methodname + "?";
            /* Append your parameters here */
            string postContent = "scope=crmapi";
            postContent = postContent + "&authtoken=0ac32dc177c4918eca902fd290a92f4a";//Give your authtoken
            if (methodname.Equals("insertRecords") || methodname.Equals("updateRecords"))
            {
                postContent = postContent + "&xmlData=" + HttpUtility.UrlEncode("Your CompanyHannahSmithtesting@testing.com");
            }
            if (methodname.Equals("updateRecords") || methodname.Equals("deleteRecords") || methodname.Equals("getRecordById"))
            {
                postContent = postContent + "&id=" + recordId;
            }
            string result = AccessCRM(uri, postContent);
            return result;
        }

        public static string AccessCRM(string url, string postcontent)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(postcontent);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        public static string AccessCRM(string url, string postcontent, string xml)
        {
            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            string postData = postcontent + "&xmlData=" + xml;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);


            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
    }
}