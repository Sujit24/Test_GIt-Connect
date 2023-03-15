using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using GSA.Security;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TSS.Models.ViewModel;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;
using System.IO;

using System.Configuration;
using MailGun;
using RestSharp;
using Zoho;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ClientController : Controller
    {
        private ZohoAccessManager _ZohoAccessManager;
        private ZohoAPI _ZohoAPI;

        #region zoho
        [HttpPost, ActionName("SearchZoho"), GSAAuthorizeAttribute()]
        public JsonResult SearchZoho(string searchText, int fromIndex = 0, int toIndex = 200)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchAccount(searchText, fromIndex, toIndex);

            //https://crm.zoho.com/crm/private/json/Accounts/getSearchRecords?authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(Account Name|contains|*' + $('#Search-Zuhu-pattern').val() + '*)';
            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Accounts/getSearchRecords?fromIndex=" + fromIndex.ToString() + "&toIndex=" + toIndex.ToString() + "&", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchCondition=(Account Name|contains|*" + HttpUtility.UrlEncode(searchText) + "*)");

            return Json(result);
        }

        [HttpPost, ActionName("GetZohoContacts"), GSAAuthorizeAttribute()]
        public JsonResult GetZohoContacts(string accountId)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchContact(accountId);

            //https://crm.zoho.com/crm/private/xml/Contacts/getSearchRecordsByPDC?authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchColumn=accountid&searchValue=39087000007725037
            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecordsByPDC?fromIndex=0&toIndex=200&", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchColumn=accountid&searchValue=" + accountId + "");

            return Json(result);
        }

        [HttpPost, ActionName("SaveZohoClient"), GSAAuthorizeAttribute()]
        public JsonResult SaveZohoClient(string zohoAccountid, string zohoaccountName, string zohoContactId, string firstName, string lastName, string email, string zohoUserTz, int orderId)
        {
            UgsClientModel item = new UgsClientModel();
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            int clientid = 0;

            try
            {
                item = tytFacadeBiz.GetDetailClientInfo(item);

                item.ZohoAccountID = zohoAccountid;
                item.ZohoContactID = zohoContactId;
                item.ClientName = HttpUtility.HtmlDecode(zohoaccountName);
                item.ContactPerson = firstName + " " + lastName;
                item.EMail = email;
                //item.MapExpirationHours = 3120;
                //item.Retention = 35;
                item.DateCreated = DateTime.Now;
                //item.UsrQty = 3;
                //item.TYTVer = "6";
                //item.SubscriptionLevelId = 4;
                item.ZohoUserTz = zohoUserTz;
                item.OrderId = orderId;
                if (ModelState.IsValid)
                {
                    item.action = item.ClientID == 0 ? "I" : "U";
                    UserModel loggedInUseer = SessionVars.CurrentLoggedInUser;
                    item.SessionId = loggedInUseer.SessionId;
                    clientid = tytFacadeBiz.SaveClient(item);
                    //if (tytFacadeBiz.SaveClient(item) != -1)
                    //{
                    //    throw new Exception("save failed");
                    //}

                    UgsNewZohoContacts c = new UgsNewZohoContacts();
                    c.ZohoAccID = zohoAccountid;

                    tytFacadeBiz.getNewZohoContacts(c);
                    string newUsers = "", newUserComment = "";

                    foreach (newZohoUser Usr in c.newUserList)
                    {
                        newUsers += @"<p>
                                        User Name:<b>" + Usr.LogIn + @"</b><br/>
                                        Password:<b>" + Usr.Pin + @"</b><br/>
                                    </p>";
                        newUserComment += "User Name : " + Usr.LogIn + "\nPassword : " + Usr.Pin + "\n";
                    }
                    //string XML = "<?xml version='1.0' standalone='yes'?><Notes>" +
                    //        "<row no=\"1\">" +
                    //        "<FL val=\"entityId\">" + zohoAccountid + "</FL>" +
                    //        "<FL val=\"Note Title\">Generated UserNames and Passwords</FL>" +
                    //        "<FL val=\"Note Content\">" + newUserComment + "</FL>" +
                    //        "</row>" +
                    //        "</Notes>";

                    //ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/xml/Notes/insertRecords?",
                    //        "newFormat=1&authtoken=e892991b0debaa85e1b1e601d82b5798&scope=crmapi", XML);
                    _ZohoAccessManager = new ZohoAccessManager(
                        ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                        ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                        ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                        ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                        ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
                    );
                    _ZohoAPI = new ZohoAPI(_ZohoAccessManager);
                    _ZohoAPI.AddNote("Generated UserNames and Passwords", newUserComment, zohoAccountid);

                    string emailTemplateFilePath = Server.MapPath(Url.Content("~/EmailTemplates/ZohoClientCreation.htm"));
                    string ebody = System.IO.File.ReadAllText(emailTemplateFilePath);
                    ebody = ebody.Replace("{{client_name}}", firstName);
                    ebody = ebody.Replace("{{user_detail_html}}", newUsers);
                    ebody = ebody.Replace("{{copyright_year}}", DateTime.Now.Year.ToString());
                    //                    string ebody = firstName + @",<br/>  
                    //                                Login to NetTrack using the links below:
                    //                                <br/>
                    //                                <p> 
                    //                                    <b>Desktop:</b> <a href='www.trackyourtruck.com/nettrack'>www.trackyourtruck.com/nettrack</a><br/>
                    //                                    <b>Mobile site:</b> <a href='mo.tyt1.net'>mo.tyt1.net</a>
                    //                                </p>
                    //                                <br/>
                    //                                <p> 
                    //                                    <b>Recommended Internet Browsers:</b> <br/>
                    //                                    Mozilla Firefox<br/>
                    //                                    Google Chrome<br/>
                    //                                    * Internet Explorer 7,and 8 are not compatible.  IE 9,10 &11 are usable but not optimum.
                    //                                </p>
                    //                                <p>Your user names and passwords are below (not case-sensitive).</p>
                    //                                " + newUsers +@"
                    //                                <p>
                    //                                    A User Manual is available on the login page; click the link “NetTrack User Manual”.<br/>
                    //                                    Thank you for choosing Track Your Truck!
                    //                                </p>";
                    //string result = EmailSender.SendMailMessage(
                    //        ConfigurationManager.AppSettings["EmailFrom"],
                    //        email, "", "un-pw@trackyourtruck.com,iftekhar@globalsoftwarearchitects.net,munirul@mitraus.com", ConfigurationManager.AppSettings["ZohoEmailSubject"],
                    //    ebody, "");

                    string result = "";
                    string apiKey = ConfigurationManager.AppSettings["MailGunApiKey"];
                    string domain = ConfigurationManager.AppSettings["MailGunProdDomain"];
                    Contact sender = new Contact { Name = ConfigurationManager.AppSettings["ZohoClientNewUserSenderName"], MailAddress = ConfigurationManager.AppSettings["ZohoClientNewUserSenderAddress"].ToString() };
                    List<Contact> emailRecipents = email.Split(';').Select(i => new Contact { MailAddress = i }).ToList();
                    List<Contact> emailBcc = "iftekhar@globalsoftwarearchitects.net,munirul@mitraus.com,un-pw@trackyourtruck.com".Split(',').
                        Select(i => new Contact { MailAddress = i }).ToList();
                    try
                    {
                        MailManager manager = new MailManager
                        {
                            ApiKey = apiKey,
                            Domain = domain,
                            To = emailRecipents,
                            //CC = emailCC,
                            BCC = emailBcc,
                            From = sender,
                            MailType = MailType.TextAndHtml,
                            Subject = ConfigurationManager.AppSettings["ZohoEmailSubject"],
                            MessageText = HtmlToText.Convert(ebody),
                            MessageHTML = ebody
                        };
                        RestResponse<MailSendResponse> resp = manager.SendMessage() as RestResponse<MailSendResponse>;

                        if (resp.Data.message.Contains("Queued. Thank you."))
                        {
                            result = "Success";
                        }
                        else
                        {
                            result = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        result = "Fail";
                    }

                    QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
                    quoteOrderModel.QuoteOrderId = orderId;
                    quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);

                    quoteOrderModel.ClientId = clientid;
                    quoteOrderModel.NettrackClientStatusId = 1;
                    quoteOrderModel.Action = "U";
                    quoteOrderModel = tytFacadeBiz.SaveQuoteOrder(quoteOrderModel);
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors).ToList();
                    //return Json(new AjaxResponse { Message = "Save failed." });
                    return Json(new AjaxResponse { Message = "Save failed.", dataState = ModelState.ProcessToDataState() });
                }
            }
            catch (Exception e)
            {
                //Elmah.ErrorSignal.FromCurrentContext();
                return Json(new AjaxResponse { Message = "Save failed.\r\n\r\n" + e.Message, dataState = ModelState.ProcessToDataState() });
                //return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { PrimaryKey = clientid, Message = "Successfully Saved.", dataState = ModelState.ProcessToDataState() });
            //return Json(new AjaxResponse { PrimaryKey = clientid, Message = "Successfully Saved." });
        }
        #endregion
    }
}
