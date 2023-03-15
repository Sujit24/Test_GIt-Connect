using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace Zoho
{
    public class ZohoAPI
    {
        private ZohoAccessManager _ZohoAccessManager;
        private string _AuthToken;

        public ZohoAPI(ZohoAccessManager zohoAccessManager)
        {
            _ZohoAccessManager = zohoAccessManager;
            _AuthToken = "Zoho-oauthtoken " + _ZohoAccessManager.GetNewToken();
        }

        public string SearchLead(string companyName, int fromIndex = 0, int toIndex = 200)
        {
            int page = toIndex / 200;

            RestRequest request = new RestRequest();
            request.Method = Method.GET;
            request.AddParameter("criteria", "(Company:starts_with:*" + companyName + ")");
            request.AddParameter("page", page);
            request.AddHeader("Authorization", _AuthToken);

            var client = new RestClient("https://www.zohoapis.com/crm/v2/Leads/search");
            var response = client.Execute(request);

            return response.Content;
        }

        public string SearchContact(string accountId, int fromIndex = 0, int toIndex = 200)
        {
            int page = toIndex / 200;

            RestRequest request = new RestRequest();
            request.Method = Method.GET;
            request.AddParameter("criteria", "(Account_Name:equals:" + accountId + ")");
            request.AddParameter("page", page);
            request.AddHeader("Authorization", _AuthToken);

            var client = new RestClient("https://www.zohoapis.com/crm/v2/Contacts/search");
            var response = client.Execute(request);

            return response.Content;

        }
        public string SearchAccount(string accountName, int fromIndex = 0, int toIndex = 200)
        {
            int page = toIndex / 200;

            RestRequest request = new RestRequest();
            request.Method = Method.GET;
            request.AddParameter("criteria", "(Account_Name:starts_with:*" + accountName + ")");
            request.AddParameter("page", page);
            request.AddHeader("Authorization", _AuthToken);

            var client = new RestClient("https://www.zohoapis.com/crm/v2/Accounts/search");
            var response = client.Execute(request);

            return response.Content;
        }
        public void AddNote(string noteTitle, string noteContent, string parent_Id)
        {
            var client = new RestClient("https://www.zohoapis.com/crm/v2/Notes");
            var request = new RestRequest(Method.POST);

            ZohoNoteRequest req = new ZohoNoteRequest();
            req.data.Add(new Zoho.ZohoNote
            {
                Note_Title = noteTitle,
                Note_Content = noteContent,
                Parent_Id = parent_Id,
                se_module = "Accounts"
            });

            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", _AuthToken);
            //request.AddParameter("application/json", 
            //    "{\r\n    " +
            //        "\"data\": [\r\n       " +
            //            "{\r\n            " +
            //                 "\"Note_Title\": \""+noteTitle+"\"," +
            //                 "\r\n            " +
            //                 "\"Note_Content\": \""+noteContent+"\"," +
            //                 "\r\n            " +
            //                 "\"Parent_Id\": \""+parent_Id+"\"," +
            //                 "\r\n            " +
            //                 "\"se_module\": \"Accounts\"" +
            //                 "\r\n        " +
            //                 "}\r\n    " +
            //            "]" +
            //      "\r\n}", ParameterType.RequestBody);
            request.AddParameter("application/json", JsonConvert.SerializeObject(req), ParameterType.RequestBody);

            //response object is for test purpose
            IRestResponse response = client.Execute(request);

        }
    }

    public class ZohoNoteRequest
    {
        public List<ZohoNote> data { get; set; }

        public ZohoNoteRequest()
        {
            data = new List<ZohoNote>();
        }
    }

    public class ZohoNote
    {
        public string Note_Title { get; set; }
        public string Note_Content { get; set; }
        public string Parent_Id { get; set; }
        public string se_module { get; set; }
    }
}
