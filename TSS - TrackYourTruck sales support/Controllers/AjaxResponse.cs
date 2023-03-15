using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace TSS.Controllers
{
    public class AjaxResponse
    {
        public long PrimaryKey { get; set; }
        public string Message { get; set; }
        public bool ShowMsgNewline { get; set; } // !ShowMsgInline
        public bool ShowFullSummary { get; set; }
        public bool ShowFullMessage { get; set; } // !ShowAsteriskOnly
        public Dictionary<string, Attribs> dataState { get; set; }
        /*
        public ContentResult GetContentResutl(List<TruckViewModel> truckViewModelList)
        {
            var serializer = new JavaScriptSerializer();
            // For simplicity just use Int32's max value.
            // You could always read the value from the config section mentioned above.
            serializer.MaxJsonLength = Int32.MaxValue;
            var result = new ContentResult
            {
                Content = serializer.Serialize(truckViewModelList),
                ContentType = "application/json"
            };
            return result;
        }
        public ContentResult GetErrorReustl()
        {
            var serializer = new JavaScriptSerializer();
            // For simplicity just use Int32's max value.
            // You could always read the value from the config section mentioned above.
            // serializer.MaxJsonLength = Int32.MaxValue;
            var resultData = new { Message = "" };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }
        */
    }
        public class Attribs
        {
            public object value { get; set; }
            public List<string> eMsgList { get; set; }
        }
        public static class AjaxResponseExtension
        {
            private static bool AjaxResponse_isInValid;
            public static Dictionary<string, Attribs> ProcessToDataState(this ModelStateDictionary d)
            {
                AjaxResponse_isInValid = false;
                Dictionary<string, Attribs> dataState = new Dictionary<string, Attribs>();
                foreach (KeyValuePair<string, ModelState> x in d)
                {
                    Attribs attr = new Attribs();
                    attr.eMsgList = new List<string>();
                    foreach (ModelError err in x.Value.Errors)
                    {
                        attr.eMsgList.Add(err.ErrorMessage);
                        if (!AjaxResponse_isInValid)
                            AjaxResponse_isInValid = true;
                    }
                    attr.value = x.Value.Value.RawValue;
                    dataState.Add(x.Key, attr);
                }
                return dataState;
            }
        }
    }
