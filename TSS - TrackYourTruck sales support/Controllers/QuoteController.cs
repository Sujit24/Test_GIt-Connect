using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNetShipping;
using DotNetShipping.ShippingProviders;
using GSA.Security;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TSS.Models.ViewModel;
using TYT.Helper;
using System.Linq;
using System.Web;
using ExcelLibrary.SpreadSheet;
using MailGun;
using Zoho;
using UPS.Models;
using UPS;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class QuoteController : Controller
    {
        private IFacadeBiz _TytFacadeBiz;
        private ZohoAccessManager _ZohoAccessManager;
        private ZohoAPI _ZohoAPI;

        public QuoteController(IFacadeBiz iFacadeBiz)
        {
            _TytFacadeBiz = iFacadeBiz;
        }

        #region Zoho

        [HttpPost, ActionName("SearchLeadInZoho"), GSAAuthorizeAttribute()]
        public JsonResult SearchLeadInZoho(string searchText, int fromIndex = 0, int toIndex = 200)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchLead(searchText, fromIndex, toIndex);

            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Leads/getSearchRecords?fromIndex=" + fromIndex.ToString() + "&toIndex=" + toIndex.ToString() + "&", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchCondition=(Company|contains|*" + HttpUtility.UrlEncode(searchText) + "*)");

            /*if (result.Contains("nodata"))
            {
                result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Leads/getSearchRecords?", "authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(First Name|contains|*" + searchText + "*)");
            }

            if (result.Contains("nodata"))
            {
                result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Leads/getSearchRecords?", "authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(Last Name|contains|*" + searchText + "*)");
            }*/

            return Json(result);
        }

        [HttpPost, ActionName("SearchContactInZoho"), GSAAuthorizeAttribute()]
        public JsonResult SearchContactInZoho(string searchText, int fromIndex = 0, int toIndex = 200)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchContact(searchText, fromIndex, toIndex);

            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecords?", "authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(ACCOUNTID|equal|*" + searchText + "*)");
            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecordsByPDC?fromIndex=" + fromIndex.ToString() + "&toIndex=" + toIndex.ToString() + "&", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchColumn=accountid&searchValue=" + HttpUtility.UrlEncode(searchText) + "");

            /*if (result.Contains("nodata"))
            {
                result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecords?", "authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(Last Name|contains|*" + searchText + "*)");
            }*/

            return Json(result);
        }

        [HttpPost, ActionName("SearchAccountInZoho"), GSAAuthorizeAttribute()]
        public JsonResult SearchAccountInZoho(string searchText, int fromIndex = 0, int toIndex = 200)
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

            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Accounts/getSearchRecords?fromIndex=" + fromIndex.ToString() + "&toIndex=" + toIndex.ToString() + "&", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchCondition=(Account Name|contains|*" + HttpUtility.UrlEncode(searchText) + "*)");

            return Json(result);
        }

        #endregion

        [GSAAuthorizeAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        [GSAAuthorizeAttribute()]
        public ActionResult List()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            BluePaySettingsModel bluePayModel = _TytFacadeBiz.GetBluePaySettingsInfo(ConfigurationManager.AppSettings["BluePayServer"]);

            ViewBag.BluePayMode = bluePayModel.CurrentMode;
            /*ViewBag.SearchDateFrom = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("MM/dd/yyyy");
            ViewBag.SearchDateTo = DateTime.Today.AddDays(7).AddSeconds(-1).ToString("MM/dd/yyyy");*/
            ViewBag.SearchDateFrom = DateTime.Today.AddDays(-30).ToString("MM/dd/yyyy");
            ViewBag.SearchDateTo = DateTime.Today.ToString("MM/dd/yyyy");

            CrystalReportPrefetch.Prefetch(Server);

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["QID"]))
            {
                ViewBag.QuoteId = Cipher.Decrypt(HttpContext.Request.QueryString["QID"]);
            }

            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            List<TSSSettings> settings = tytFacadeBiz.GetAllSettings();
            ViewBag.DirectDeliveryCharge = settings.FirstOrDefault(f => f.SettingsName == "DirectDeliveryCharge").SettingsValue;
            ViewBag.DevicesLessThan3 = settings.FirstOrDefault(f => f.SettingsName == "<3Devices").SettingsValue;
            ViewBag.Devices4To6 = settings.FirstOrDefault(f => f.SettingsName == "4-6Devices").SettingsValue;
            ViewBag.DevicesGreaterThan7 = settings.FirstOrDefault(f => f.SettingsName == ">7Devices").SettingsValue;


            return View();
        }

        [HttpPost, ActionName("GetQuoteList"), GSAAuthorizeAttribute()]
        public ContentResult GetQuoteList(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<QuoteModel> quoteModelList = new List<QuoteModel>();

            try
            {
                QuoteModel quoteModel = new QuoteModel();
                quoteModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteModel.SearchFromDate = DateTime.Parse(Request.QueryString["dateFrom"]);
                quoteModel.SearchToDate = DateTime.Parse(Request.QueryString["dateTo"]);

                if (SessionVars.CurrentLoggedInUser.IsTSSAdmin)
                {
                    quoteModelList = _TytFacadeBiz.GetQuoteList(quoteModel);
                }
                else
                {
                    SecurityGroupSalesPersonModel securityGroupSalesPerson = new SecurityGroupSalesPersonModel();
                    securityGroupSalesPerson.EmployeeId = SessionVars.CurrentLoggedInUser.EmployeeId;
                    var employeeList = _TytFacadeBiz.GetSalesPersonsByEmployeeId(securityGroupSalesPerson);

                    List<int> salesPersonList = new List<int>();
                    salesPersonList = (from e in employeeList
                                       select e.EmployeeId).ToList();

                    if (salesPersonList.Count() == 0)
                    {
                        salesPersonList.Add(SessionVars.CurrentLoggedInUser.EmployeeId);
                    }

                    quoteModelList = _TytFacadeBiz.GetQuoteList(quoteModel).Where(q => salesPersonList.Contains(q.SalesPersonId)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["searchText"]))
                {
                    if (Request.QueryString["searchSelect"] == "CompanyName")
                    {
                        quoteModelList = quoteModelList.Where(w => w.CompanyName.ToLower().Contains(Request.QueryString["searchText"].ToLower())).ToList();
                    }
                    else if (Request.QueryString["searchSelect"] == "SearchQuoteId")
                    {
                        quoteModelList = quoteModelList.Where(w => w.SearchQuoteId.Contains(Request.QueryString["searchText"])).ToList();
                    }
                    else if (Request.QueryString["searchSelect"] == "LastViewCount")
                    {
                        quoteModelList = quoteModelList.Where(w => w.LastViewCount == int.Parse(Request.QueryString["searchText"])).ToList();
                    }
                }

                string userViewUrl = ConfigurationManager.AppSettings["UserViewUrl"];
                foreach (QuoteModel item in quoteModelList)
                {
                    item.IsExpired = item.QuoteDate.AddDays(item.ValidUntil) < DateTime.Now;

                    item.Url = userViewUrl + "Quote/UserView/?id="
                        + Server.UrlEncode(
                            Cipher.Encrypt(item.QuoteId.ToString() + "|" + item.QuoteDate.AddDays(item.ValidUntil).ToString("dd-MMM-yyyy"))
                            );
                    //item.PdfToken = "" + Server.UrlEncode(Cipher.Encrypt(item.QuoteId.ToString())) + "";
                    item.PdfToken = item.QuoteId.ToString();
                    item.ShowApprove = SessionVars.CurrentLoggedInUser.IsTSSQuoteApproval;
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return LargeJsonResult(quoteModelList);
        }

        public ContentResult LargeJsonResult(List<QuoteModel> quoteModelList)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here  
            ContentResult result = new ContentResult();
            result.Content = serializer.Serialize(quoteModelList);
            result.ContentType = "application/json";

            return result;
        }

        [HttpPost, ActionName("Save"), GSAAuthorizeAttribute()]
        public ActionResult SaveDataToDB(QuoteViewModel model)
        {
            string message = "Successfully Saved.";

            try
            {
                int maxApprovedHW = 50;
                try
                {
                    maxApprovedHW = Convert.ToInt32(_TytFacadeBiz.GetSettings("MaxHWQTY").SettingsValue);
                }
                catch
                {
                    maxApprovedHW = Convert.ToInt32(ConfigurationManager.AppSettings["MaxApprovedHW"]);
                }


                int currentHWCount = 0;
                int currentPackageId = 0;
                foreach (QuoteProductModel item in model.QuoteProductModelList)
                {
                    if (item.ProductCategory == "Hardware" && item.Price > 0 && item.PackageId != currentPackageId && currentHWCount < item.Quantity)
                    {
                        currentHWCount = item.Quantity;
                        currentPackageId = item.PackageId;
                    }
                }

                //if (ModelState.IsValid)
                {
                    model.QuoteModel.Action = model.QuoteModel.QuoteId == 0 ? "I" : "U";
                    model.QuoteModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;

                    if (currentHWCount > maxApprovedHW)
                    {
                        message = "Successfully Saved.\nQuote exceeds maximum hardware quantity (" + maxApprovedHW + "), it needs to be approved.";
                        model.QuoteModel.IsApproved = "N";
                    }
                    else
                    {
                        model.QuoteModel.IsApproved = null;
                    }

                    model.QuoteModel = _TytFacadeBiz.SaveQuote(model.QuoteModel);
                    if (model.QuoteModel.QuoteId <= 0)
                    {
                        throw new Exception("save failed");
                    }
                    else
                    {
                        int sortOrder = 1;
                        foreach (QuoteProductModel item in model.QuoteProductModelList)
                        {
                            item.Action = (item.QuoteProductId == 0 && !item.Deleted) ? "I" : (item.Deleted ? "D" : "U");
                            item.QuoteId = model.QuoteModel.QuoteId;
                            item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                            item.SortOrder = sortOrder++;

                            _TytFacadeBiz.SaveQuoteProduct(item);
                        }
                    }

                    if (model.QuoteModel.IsApproved == "N")
                    {
                        List<UserModel> tssAdmin = _TytFacadeBiz.GetTSSAdminList();

                        foreach (UserModel user in tssAdmin)
                        {
                            MailViewModel mailViewModel = new MailViewModel();
                            mailViewModel.QuoteId = model.QuoteModel.QuoteId;
                            mailViewModel.CompanyName = model.QuoteModel.BillToCompanyName;
                            mailViewModel.CustomerName = user.FirstName + " " + user.LastName;
                            mailViewModel.Body = "Click the link above to approve the Quote.";
                            mailViewModel.ApproveUrl = ConfigurationManager.AppSettings["UserViewUrl"] + "Quote/List?QID=" + Server.UrlEncode(Cipher.Encrypt(model.QuoteModel.QuoteId.ToString()));

                            string body = Server.HtmlDecode(RenderPartialViewToString(this, "ApproveTemplate", mailViewModel));
                            EmailSender.SendMailUsingMailGun(true, user.Email, "TYT Quote Approval", body, "tssorders@trackyourtruck.com");
                        }
                    }

                    /*string body = "Click the link below to view your Track Your Truck Vehicle Tracking Quote.\n<a target='_blank' href='" + ConfigurationManager.AppSettings["UserViewUrl"]
                        + "Quote/UserView/?id="
                        + Server.UrlEncode(Cipher.Encrypt(
                            model.QuoteModel.QuoteId.ToString() + "|" + model.QuoteModel.QuoteDate.AddDays(model.QuoteModel.ValidUntil).ToString("MM/dd/yyyy"))
                        ) + "' >View your quote</a>";

                    SendUserUrlMail(
                        string.IsNullOrWhiteSpace(model.QuoteModel.BillToBillingEmail) ? "rhall@trackyourtruck.com" : model.QuoteModel.BillToBillingEmail.Trim(),
                        body,
                        model.QuoteModel.BillToCompanyName,
                        "",
                        ConfigurationManager.AppSettings["BCCForTest"]
                    );*/
                }
                /*else
                {
                    return Json(new AjaxResponse { Message = "Save failed." });
                }*/

                System.IO.File.AppendAllText(Server.MapPath("~/NoteLog.txt"),
                    string.Format("[{0}] {1}\n", model.QuoteModel.QuoteId, model.QuoteModel.Note));
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { PrimaryKey = model.QuoteModel.QuoteId, Message = message });
        }

        [HttpPost, ActionName("GetSalesPerson"), GSAAuthorizeAttribute()]
        public JsonResult GetSalesPerson()
        {
            return Json(_TytFacadeBiz.GetSalesPersonList(SessionVars.CurrentLoggedInUser.SessionId));
        }

        [HttpPost, ActionName("Delete"), GSAAuthorizeAttribute()]
        public JsonResult Delete(int PrimaryKey)
        {
            try
            {
                QuoteModel item = new QuoteModel();
                item.QuoteId = PrimaryKey;

                if (item.QuoteId > 0)
                {
                    item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    item = _TytFacadeBiz.GetQuoteInfo(item);

                    item.Action = "D";
                    item = _TytFacadeBiz.SaveQuote(item);

                    if (item.QuoteId <= 0)
                    {
                        throw new Exception("Delete failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Delete failed." });
            }

            return Json(new AjaxResponse { Message = "Successfully Deleted." });
        }

        [ActionName("Edit"), GSAAuthorizeAttribute()]
        public ActionResult EditQuote(long id)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            ViewBag.Title = "View/Edit Quotes";
            ViewBag.QuoteId = id;

            return View("Index");
        }

        [ActionName("GetQuote"), GSAAuthorizeAttribute()]
        public JsonResult GetQuote(int id)
        {
            QuoteModel quoteModel = new QuoteModel();
            QuoteProductModel quoteProductModel = new QuoteProductModel();
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            try
            {
                quoteModel.QuoteId = id;
                quoteModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteModel = _TytFacadeBiz.GetQuoteInfo(quoteModel);

                quoteProductModel.QuoteId = id;
                quoteProductModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteProductModelList = _TytFacadeBiz.GetQuoteProductList(quoteProductModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(new { Quote = quoteModel, QuoteProductList = quoteProductModelList });
        }

        [ActionName("UserView")]
        public ActionResult UserView(string id)
        {
            //Only for tracking for which server is calling
            //Elmah.ErrorSignal.FromCurrentContext().Raise(new NotImplementedException(HttpContext.Request.ToString()));

            try
            {
                Session["QuoteUserViewId"] = id;

                try
                {
                    id = Cipher.Decrypt(id);
                }
                catch
                {
                    QuoteLinkMappingModel quoteLinkMapping = _TytFacadeBiz.GetQuoteLinkMapping(Server.UrlEncode(id));
                    id = String.Format("{0}|", quoteLinkMapping.QuoteId);
                }

                QuoteModel quoteModel = new QuoteModel();
                quoteModel.QuoteId = int.Parse(id.Split('|')[0]);
                quoteModel = _TytFacadeBiz.GetQuoteInfo(quoteModel);

                if (quoteModel.QuoteStartDate > quoteModel.QuoteDate)
                {
                    if (quoteModel.QuoteStartDate.AddDays(quoteModel.ValidUntil) >= DateTime.Now)
                    {
                        ViewBag.QuoteId = id.Split('|')[0];
                        Session["QuoteId"] = ViewBag.QuoteId;
                    }
                }
                else
                {
                    if (quoteModel.QuoteDate.AddDays(quoteModel.ValidUntil) >= DateTime.Now)
                    {
                        ViewBag.QuoteId = id.Split('|')[0];
                        Session["QuoteId"] = ViewBag.QuoteId;
                    }
                }

                ViewBag.Title = "Review Quotes";
                ViewBag.CurrentDateTime = DateTime.Now.ToString("MM-dd-yyyy hh:mm tt");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View();
        }

        [ActionName("UserViewGetQuote")]
        public JsonResult UserViewGetQuote(int id)
        {
            QuoteModel quoteModel = new QuoteModel();
            QuoteProductModel quoteProductModel = new QuoteProductModel();
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            try
            {
                quoteModel.QuoteId = id;
                quoteModel.SessionId = 0;
                quoteModel = _TytFacadeBiz.GetQuoteInfo(quoteModel);

                if (!quoteModel.LastViewDate.HasValue)
                {
                    SendUserUrlMail(
                        quoteModel.SalesPersonEmail,
                        string.Format("{0}, {1} has viewed quote {2}.", quoteModel.BillToCompanyName, quoteModel.BillToBillingContact, quoteModel.QuoteId),
                        quoteModel.SalesPersonName
                        );
                }

                quoteModel.LastViewDate = DateTime.Now;
                quoteModel.Action = "U";
                _TytFacadeBiz.SaveQuote(quoteModel);

                quoteProductModel.QuoteId = id;
                quoteProductModel.SessionId = 0;
                quoteProductModelList = _TytFacadeBiz.GetQuoteProductList(quoteProductModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(new { Quote = quoteModel, QuoteProductList = quoteProductModelList });
        }

        [HttpPost, ActionName("UserViewSave")]
        public JsonResult UserViewSave(QuoteViewModel model)
        {
            QuoteModel quoteModel = new QuoteModel();
            BluePaySettingsModel bluePayModel = _TytFacadeBiz.GetBluePaySettingsInfo(ConfigurationManager.AppSettings["BluePayServer"]);

            /*string bluePayMode = bluePayModel.CurrentMode;
            string bluePayUrl = string.Format((bluePayMode.ToUpper() == "TEST" ? bluePayModel.TestUrl : bluePayModel.LiveUrl), model.TotalValue, model.QuoteOrderModel.QuoteId);*/
            
            string bluePayUrl = Url.Action("Index", "Payment");
            try
            {
                quoteModel.QuoteId = model.QuoteOrderModel.QuoteId;
                quoteModel.SessionId = 0;
                quoteModel = _TytFacadeBiz.GetQuoteInfo(quoteModel);

                model.QuoteOrderModel.TssOrderTypeId = quoteModel.TssOrderTypeId;

                quoteModel.BillToCompanyName = model.QuoteOrderModel.BillToCompanyName;
                quoteModel.BillToAddress1 = model.QuoteOrderModel.BillToAddress1;
                quoteModel.BillToAddress2 = model.QuoteOrderModel.BillToAddress2;
                quoteModel.BillToCity = model.QuoteOrderModel.BillToCity;
                quoteModel.BillToState = model.QuoteOrderModel.BillToState;
                quoteModel.BillToZip = model.QuoteOrderModel.BillToZip;
                quoteModel.BillToCountry = model.QuoteOrderModel.BillToCountry;
                quoteModel.BillToBillingContact = model.QuoteOrderModel.BillToBillingContact;
                quoteModel.BillToBillingEmail = model.QuoteOrderModel.BillToBillingEmail;
                quoteModel.BillToPhone = model.QuoteOrderModel.BillToPhone;
                quoteModel.ShipToCompanyName = model.QuoteOrderModel.ShipToCompanyName;
                quoteModel.ShipToAddress1 = model.QuoteOrderModel.ShipToAddress1;
                quoteModel.ShipToAddress2 = model.QuoteOrderModel.ShipToAddress2;
                quoteModel.ShipToCity = model.QuoteOrderModel.ShipToCity;
                quoteModel.ShipToState = model.QuoteOrderModel.ShipToState;
                quoteModel.ShipToZip = model.QuoteOrderModel.ShipToZip;
                quoteModel.ShipToCountry = model.QuoteOrderModel.ShipToCountry;
                quoteModel.ShipToBillingContact = model.QuoteOrderModel.ShipToBillingContact;
                quoteModel.ShipToBillingEmail = model.QuoteOrderModel.ShipToBillingEmail;
                quoteModel.ShipToPhone = model.QuoteOrderModel.ShipToPhone;
                quoteModel.ShippingAndHandlingType = model.QuoteOrderModel.ShippingAndHandlingType;
                quoteModel.ShippingAndHandling = model.QuoteOrderModel.ShippingAndHandling;
                quoteModel.SalesTax = model.QuoteOrderModel.SalesTax;
                quoteModel.Action = "U";
                _TytFacadeBiz.SaveQuote(quoteModel);


                /*bluePayUrl += "&ORDER_ID=" + model.QuoteOrderModel.QuoteId;

                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToBillingContact)) bluePayUrl += "&NAME=" + model.QuoteOrderModel.BillToBillingContact.Replace(" ", "%20");
                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToAddress1)) bluePayUrl += "&ADDR1=" + model.QuoteOrderModel.BillToAddress1.Replace(" ", "%20");
                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToState)) bluePayUrl += "&STATE=" + model.QuoteOrderModel.BillToState.Replace(" ", "%20");
                bluePayUrl += "&COUNTRY=US";
                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToPhone)) bluePayUrl += "&PHONE=" + model.QuoteOrderModel.BillToPhone.Replace(" ", "%20");

                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToCompanyName)) bluePayUrl += "&COMPANY_NAME=" + model.QuoteOrderModel.BillToCompanyName.Replace(" ", "%20");
                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToCity)) bluePayUrl += "&CITY=" + model.QuoteOrderModel.BillToCity.Replace(" ", "%20");
                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToZip)) bluePayUrl += "&ZIPCODE=" + model.QuoteOrderModel.BillToZip.Replace(" ", "%20");
                if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.BillToBillingEmail)) bluePayUrl += "&EMAIL=" + model.QuoteOrderModel.BillToBillingEmail.Replace(" ", "%20");*/

                Session["BillToBillingEmail"] = quoteModel.BillToBillingEmail;
                Session["CustomerName"] = quoteModel.BillToBillingContact;
                Session["BillToCompanyName"] = quoteModel.BillToCompanyName;
                Session["SalesPersonEmail"] = quoteModel.SalesPersonEmail;

                Session["QuoteViewModel"] = model;
                Session["QuoteId"] = model.QuoteOrderModel.QuoteId;

                //Session["BluePayUrl"] = bluePayUrl;

                //BluePay != 5 (Credit Card) != 7 ACH payment
                if (model.QuoteOrderModel.QuotePaymentMethodId != 5 && model.QuoteOrderModel.QuotePaymentMethodId != 7)
                {
                    bluePayUrl = "";

                    model.QuoteOrderModel.Action = model.QuoteOrderModel.QuoteOrderId == 0 ? "I" : "U";
                    model.QuoteOrderModel.PurchaseDate = DateTime.Now;
                    //model.QuoteOrderModel.QuoteDate = DateTime.Now;
                    model.QuoteOrderModel.SessionId = 0;
                    model.QuoteOrderModel.OrderStatusId = 4;

                    model.QuoteOrderModel = _TytFacadeBiz.SaveQuoteOrder(model.QuoteOrderModel);

                    if (model.QuoteOrderModel.QuoteOrderId <= 0)
                    {
                        throw new Exception("save failed");
                    }
                    else
                    {
                        _TytFacadeBiz.SaveOrderStatusHistory(model.QuoteOrderModel.QuoteOrderId, model.QuoteOrderModel.OrderStatusId);

                        int sortOrder = 1;
                        foreach (SalesOrderModel item in model.SalesOrderModelList)
                        {
                            item.Action = "I";
                            item.QuoteOrderId = model.QuoteOrderModel.QuoteOrderId;
                            item.SessionId = 0;
                            item.SortOrder = sortOrder++;

                            _TytFacadeBiz.SaveSalesOrder(item);
                        }
                    }

                    string body = "", commonBody = "";
                    commonBody += "Thank you for your order#" + model.QuoteOrderModel.QuoteId + " in the amount of $" + model.TotalValue.ToString() + "!<br />";
                    commonBody += "Our team will review your order and contact you promptly.<br /><br />";
                    if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.PaymentMethodComment))
                    {
                        commonBody += "Comments: " + model.QuoteOrderModel.PaymentMethodComment + "<br /><br />";
                    }
                    /*commonBody += "Click the link below to view your Track Your Truck Order Confirmation.<br /><a target='_blank' href='"
                        + ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrder/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(model.QuoteOrderModel.QuoteOrderId.ToString()))
                        + "' >View your Order</a><br />";*/

                    MailViewModel mailViewModel = new MailViewModel();
                    mailViewModel.QuoteId = quoteModel.QuoteId;
                    mailViewModel.CompanyName = quoteModel.BillToCompanyName;
                    mailViewModel.CustomerName = quoteModel.BillToBillingContact;
                    mailViewModel.Body = commonBody;
                    mailViewModel.OrderUrl = ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrder/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(model.QuoteOrderModel.QuoteOrderId.ToString()));

                    body = Server.HtmlDecode(RenderPartialViewToString(this, "ConvertToOrderEmailTemplate", mailViewModel));
                    EmailSender.SendMailUsingMailGun(false, Session["BillToBillingEmail"].ToString(), "Track Your Truck Order Confirmation", body, "tssorders@trackyourtruck.com");

                    //Mail to orders@trackyourtruck.com
                    //commonBody += "Company Name: " + Session["BillToCompanyName"].ToString();

                    body = Server.HtmlDecode(RenderPartialViewToString(this, "ConvertToOrderEmailTemplate", mailViewModel));
                    EmailSender.SendMailUsingMailGun(false, Session["SalesPersonEmail"].ToString(), "Track Your Truck Order Confirmation", body, "tssorders@trackyourtruck.com");
                    EmailSender.SendMailUsingMailGun(false, "tssorders@trackyourtruck.com", "Track Your Truck Order Confirmation", body);
                }

            }
            catch (Exception ex)
            {
                QuoteOrderModel item = new QuoteOrderModel();
                item.QuoteId = model.QuoteOrderModel.QuoteId;
                item.Action = "D";
                item.SessionId = 0;
                item = _TytFacadeBiz.SaveQuoteOrder(item);

                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                return Json(new AjaxResponse { PrimaryKey = 0, Message = "" });
            }

            /// if payment method is ach
            if (model.QuoteOrderModel.QuotePaymentMethodId == 7)
            {
                bluePayUrl = Url.Action("ach", "Payment");
            }
            return Json(new AjaxResponse { PrimaryKey = model.QuoteOrderModel.QuoteId, Message = bluePayUrl });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult SendUserUrl(MailViewModel mailViewModel)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            QuoteModel quoteModel = new QuoteModel();

            try
            {
                List<Attachment> attachment = new List<Attachment>();
                if (mailViewModel.EmailAttachment != null)
                {
                    foreach (HttpPostedFileBase file in mailViewModel.EmailAttachment)
                    {
                        if (file != null)
                        {
                            byte[] byteFile = new byte[file.InputStream.Length];
                            file.InputStream.Read(byteFile, 0, file.ContentLength);

                            attachment.Add(new MailGun.Attachment(byteFile, file.FileName));
                        }
                    }
                }

                quoteModel.QuoteId = mailViewModel.QuoteId;
                quoteModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteModel = _TytFacadeBiz.GetQuoteInfo(quoteModel);

                string userViewUrl = ConfigurationManager.AppSettings["UserViewUrl"];
                string orderNowUrl = userViewUrl + "Quote/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(quoteModel.QuoteId.ToString() + "|" + quoteModel.QuoteDate.AddDays(quoteModel.ValidUntil).ToString("dd-MMM-yyyy")));

                if (mailViewModel.IsAttachDoc)
                {
                    QuoteProductRptController quoteReport = new QuoteProductRptController(HttpContext);
                    Stream pdfStream = quoteReport.GetPDFStream(mailViewModel.QuoteId, orderNowUrl);
                    byte[] pdfByteFile = new byte[pdfStream.Length];
                    pdfStream.Read(pdfByteFile, 0, (int)pdfStream.Length);

                    if (mailViewModel.IsLocalMail)
                    {
                        attachment.Add(new MailGun.Attachment(pdfByteFile, string.Format("TYT Quote {0}.pdf", quoteModel.QuoteId)));
                    }
                    else
                    {
                        attachment.Add(new MailGun.Attachment(pdfByteFile, string.Format("TYT Quote {0} — {1}.pdf", quoteModel.QuoteId, quoteModel.BillToCompanyName)));
                    }
                }

                /*string quoteInfo = string.Format("<br><br>Company Name: {0}<br>Quote Number: {1}<br><br>", quoteModel.BillToCompanyName, quoteModel.QuoteId);
                mailViewModel.Body = mailViewModel.Body + quoteInfo;*/

                //SendUserUrlMail(quoteModel.BillToBillingEmail, url);

                EmailResponse emailResponse = new EmailResponse();
                string localEmailStatus = "";

                if (mailViewModel.IsLocalMail)
                {
                    localEmailStatus = EmailSender.SendMailMessageUsingRackspace(
                            string.IsNullOrWhiteSpace(quoteModel.SalesPersonEmail) ? SessionVars.CurrentLoggedInUser.Email : quoteModel.SalesPersonEmail,
                            mailViewModel.MailTo,
                            string.IsNullOrWhiteSpace(quoteModel.SalesPersonEmail) ? SessionVars.CurrentLoggedInUser.Email : quoteModel.SalesPersonEmail,
                            "",
                            "Track Your Truck Vehicle Tracking Quote",
                             mailViewModel.Body,
                            string.IsNullOrWhiteSpace(quoteModel.SalesPersonName) ? "" : quoteModel.SalesPersonName,
                            attachment
                        );

                    emailResponse.Status = localEmailStatus == "Fail" ? EmailResponseStatus.Failed : EmailResponseStatus.Success;
                }
                else
                {
                    emailResponse = SendUserUrlMailWithCustomVars(mailViewModel.MailTo, mailViewModel.Body,
                        string.IsNullOrWhiteSpace(quoteModel.SalesPersonName) ? "" : quoteModel.SalesPersonName,
                        string.IsNullOrWhiteSpace(quoteModel.SalesPersonEmail) ? SessionVars.CurrentLoggedInUser.Email : quoteModel.SalesPersonEmail,
                        string.IsNullOrWhiteSpace(quoteModel.SalesPersonEmail) ? SessionVars.CurrentLoggedInUser.Email : quoteModel.SalesPersonEmail,
                        new { TSSQuoteId = mailViewModel.QuoteId }, attachment);
                }

                if (!string.IsNullOrWhiteSpace(mailViewModel.MailTo))
                {
                    foreach (string recipent in mailViewModel.MailTo.Split(';'))
                    {
                        if (!string.IsNullOrWhiteSpace(recipent))
                        {
                            TSSSentEmailModel sentEmailLog = new TSSSentEmailModel();
                            sentEmailLog.QuoteId = mailViewModel.QuoteId;
                            sentEmailLog.EmailId = mailViewModel.IsLocalMail ? "Local" : emailResponse.EmailId;
                            sentEmailLog.Recipent = recipent;
                            sentEmailLog.MessageBody = mailViewModel.Body;
                            sentEmailLog.SentById = SessionVars.CurrentLoggedInUser.EmployeeId;
                            sentEmailLog.SentDate = DateTime.Now;
                            _TytFacadeBiz.SaveTSSSentEmail(sentEmailLog);
                        }
                    }

                    //Saving the EncryptValue and Quote ID mapping
                    try
                    {
                        QuoteLinkMappingModel quoteLinkMapping = new QuoteLinkMappingModel();
                        quoteLinkMapping.QuoteId = mailViewModel.QuoteId;
                        quoteLinkMapping.Recipient = mailViewModel.MailTo;

                        string originalValue = quoteModel.QuoteId.ToString() + "|" + quoteModel.QuoteDate.AddDays(quoteModel.ValidUntil).ToString("dd-MMM-yyyy");
                        string matchedUrl = System.Text.RegularExpressions.Regex.Match(mailViewModel.Body, "(\\?id=).*(\" t)").Value;
                        if (!String.IsNullOrWhiteSpace(matchedUrl))
                        {
                            quoteLinkMapping.EncryptValue = matchedUrl.Substring(4, matchedUrl.Length - 7);
                        }
                        try
                        {
                            quoteLinkMapping.DecryptValue = Cipher.Decrypt(Server.UrlDecode(quoteLinkMapping.EncryptValue));

                            if (quoteLinkMapping.DecryptValue != originalValue)
                            {
                                quoteLinkMapping.Error = String.Format("Value don't match. Original Value: {0}", originalValue);
                                quoteLinkMapping.HasError = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Saving the original values while there is an error
                            quoteLinkMapping.DecryptValue = originalValue;
                            quoteLinkMapping.Error = ex.Message;
                            quoteLinkMapping.HasError = true;
                        }

                        _TytFacadeBiz.SaveQuoteLinkMapping(quoteLinkMapping);
                    }
                    catch (Exception ex)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }


                if (emailResponse.Status == EmailResponseStatus.Failed)
                {
                    return Json(new AjaxResponse { Message = "Email Send failed." });
                }
                else
                {
                    quoteModel.UrlSendDate = DateTime.Now;
                    quoteModel.Action = "U";
                    _TytFacadeBiz.SaveQuote(quoteModel);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = ex.Message + "\nException has occurred. Email Send failed." });
            }

            return Json(new { UrlSendDate = quoteModel.UrlSendDate.Value.ToString("MM-dd-yyyy hh:mm tt"), Message = "Email Send Successfully." });
        }

        private string SendUserUrlMail(string mailTo, string body, string customerName, string salesPersonEmail = "", string bcc = "")
        {
            body = body.Replace("\n", "<br>");
            body = Server.HtmlDecode(RenderPartialViewToString(this, "EmailTemplate", new MailViewModel { CustomerName = customerName, Body = body }));

            return EmailSender.SendMailUsingMailGun(true, mailTo, "Track Your Truck Vehicle Tracking Quote", body, salesPersonEmail, bcc);
            //return EmailSender.SendMailUsingMailGun(ConfigurationManager.AppSettings["EmailFrom"], mailTo, "", "", "Track Your Truck Vehicle Tracking Quote", body, "TYT");
        }

        private EmailResponse SendUserUrlMailWithCustomVars(string mailTo, string body, string salesPersonName = "", string salesPersonEmail = "", string bcc = "", object customVars = null, List<Attachment> attachment = null)
        {
            /*body = body.Replace("\n", "<br>");
            body = Server.HtmlDecode(RenderPartialViewToString(this, "EmailTemplate", new MailViewModel { CustomerName = customerName, Body = body }));*/

            return EmailSender.SendMailUsingMailGunWithCustomVars(true, mailTo, "Track Your Truck Vehicle Tracking Quote", body, salesPersonName, salesPersonEmail, bcc, customVars, attachment);
            //return EmailSender.SendMailUsingMailGun(ConfigurationManager.AppSettings["EmailFrom"], mailTo, "", "", "Track Your Truck Vehicle Tracking Quote", body, "TYT");
        }

        [ActionName("GetShippingAndHandling")]
        public JsonResult GetShippingAndHandling(ShippingAndHandlingViewModel shippingAndHandlingModel)
        {
            decimal groundRate = 0, secondDayAirRate = 0, nextDayAirRate = 0;
            string errorDescription = "";

            try
            {
                shippingAndHandlingModel.PackageWeight = shippingAndHandlingModel.PackageWeight < 0 ? 1 : shippingAndHandlingModel.PackageWeight;

                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                List<TSSSettings> settings = tytFacadeBiz.GetAllSettings();
                var directDeliveryCharge = decimal.Parse(settings.FirstOrDefault(f => f.SettingsName == "DirectDeliveryCharge").SettingsValue);
                var devicesLessThan3 = settings.FirstOrDefault(f => f.SettingsName == "<3Devices").SettingsValue.Split('x');
                var devices4To6 = settings.FirstOrDefault(f => f.SettingsName == "4-6Devices").SettingsValue.Split('x');
                var devicesGreaterThan7 = settings.FirstOrDefault(f => f.SettingsName == ">7Devices").SettingsValue.Split('x');

                var shipperAddressLine = settings.FirstOrDefault(f => f.SettingsName == "ShipperAddressLine").SettingsValue;
                var shipperCity = settings.FirstOrDefault(f => f.SettingsName == "ShipperCity").SettingsValue;
                var shipperState = settings.FirstOrDefault(f => f.SettingsName == "ShipperState").SettingsValue;
                var shipperZip = settings.FirstOrDefault(f => f.SettingsName == "ShipperZip").SettingsValue;
                var shipperNumberForShipping = settings.FirstOrDefault(f => f.SettingsName == "ShipperNumberForShipping").SettingsValue;

                // Setup package and destination/origin addresses
                var packages = new List<DotNetShipping.Package>();
                if (shippingAndHandlingModel.TotalProduct < 3)
                {
                    packages.Add(new DotNetShipping.Package(
                        int.Parse(devicesLessThan3[0]),
                        int.Parse(devicesLessThan3[1]),
                        int.Parse(devicesLessThan3[2]),
                        (shippingAndHandlingModel.PackageWeight + (decimal)0.00000001), shippingAndHandlingModel.PackageInsuredValue));
                }
                else if (shippingAndHandlingModel.TotalProduct > 2 && shippingAndHandlingModel.TotalProduct < 7)
                {
                    packages.Add(new DotNetShipping.Package(
                        int.Parse(devices4To6[0]),
                        int.Parse(devices4To6[1]),
                        int.Parse(devices4To6[2]),
                        (shippingAndHandlingModel.PackageWeight + (decimal)0.00000001), shippingAndHandlingModel.PackageInsuredValue));
                }
                else
                {
                    packages.Add(new DotNetShipping.Package(
                        int.Parse(devicesGreaterThan7[0]),
                        int.Parse(devicesGreaterThan7[1]),
                        int.Parse(devicesGreaterThan7[2]),
                        (shippingAndHandlingModel.PackageWeight + (decimal)0.00000001), shippingAndHandlingModel.PackageInsuredValue));
                }
                //packages.Add(new DotNetShipping.Package(0, 0, 0, (shippingAndHandlingModel.PackageWeight + (decimal)0.00000001), shippingAndHandlingModel.PackageInsuredValue));

                var origin = new DotNetShipping.Address(
                                    shipperAddressLine,
                                    "",
                                    "",
                                    shipperCity,
                                    shipperState,
                                    shipperZip,
                                    "US"
                                );
                origin.ShipperNumber = shipperNumberForShipping;
                var destination = new DotNetShipping.Address(shippingAndHandlingModel.ShipToAddress, "", "", shippingAndHandlingModel.ShipToCity, shippingAndHandlingModel.ShipToState, shippingAndHandlingModel.ShipToZip, shippingAndHandlingModel.ShipToCountry);

                // Create RateManager
                var rateManager = new RateManager();
                //rateManager.AddProvider(new UPSProvider("6CE2DAC4931936F5", "trackyourtruck", "phigam"));
                rateManager.AddProvider(new UPSProvider("ED0A5D6A4EDC1E46", "trackyourtruckVA", "random%22"));

                // (Optional) Add RateAdjusters
                //rateManager.AddRateAdjuster(new PercentageRateAdjuster(.9M));

                // Call GetRates()
                DotNetShipping.Shipment shipment = rateManager.GetRates(origin, destination, packages);
                errorDescription = shipment.ErrorDescription;

                if (string.IsNullOrWhiteSpace(errorDescription))
                {
                    // Iterate through the rates returned
                    foreach (Rate rate in shipment.Rates)
                    {
                        if (shippingAndHandlingModel.ShipToCountry == "CA")
                        {
                            if (rate.ProviderCode == "11")
                            {
                                groundRate = rate.TotalCharges + directDeliveryCharge;
                            }
                            else if (rate.ProviderCode == "08")
                            {
                                secondDayAirRate = rate.TotalCharges + directDeliveryCharge;
                            }
                            else if (rate.ProviderCode == "65")
                            {
                                nextDayAirRate = rate.TotalCharges + directDeliveryCharge;
                            }
                        }
                        else
                        {
                            if (rate.ProviderCode == "03")
                            {
                                groundRate = rate.TotalCharges + directDeliveryCharge;
                            }
                            else if (rate.ProviderCode == "02")
                            {
                                secondDayAirRate = rate.TotalCharges + directDeliveryCharge;
                            }
                            else if (rate.ProviderCode == "01")
                            {
                                nextDayAirRate = rate.TotalCharges + directDeliveryCharge;
                            }
                        }
                    }
                }
                else
                {
                    new NetTrackDBContext.DBProduct().SaveUPSLog(
                        String.Format("{0} Requested address: address={1}, city={2}, state={3}, zip={4}, country={5}",
                        errorDescription,
                        shippingAndHandlingModel.ShipToAddress,
                        shippingAndHandlingModel.ShipToCity,
                        shippingAndHandlingModel.ShipToState,
                        shippingAndHandlingModel.ShipToZip,
                        shippingAndHandlingModel.ShipToCountry
                        ));

                    if (errorDescription.Contains("www.ups.com"))
                    {
                        errorDescription = "The remote name could not be resolved.";
                    }
                    else if (errorDescription.Contains("Exception: "))
                    {
                        errorDescription = "Service is down. Please try later.";
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Error = ex.Message, GroundRate = groundRate, SecondDayAirRate = secondDayAirRate, NextDayAirRate = nextDayAirRate });
            }

            return Json(new { Error = errorDescription, GroundRate = groundRate, SecondDayAirRate = secondDayAirRate, NextDayAirRate = nextDayAirRate });
        }

        public static string RenderPartialViewToString(Controller thisController, string viewName, object model)
        {
            // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
            thisController.ViewData.Model = model;

            // initialize a string builder
            using (StringWriter sw = new StringWriter())
            {
                // find and load the view or partial view, pass it through the controller factory
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

                // render it
                viewResult.View.Render(viewContext, sw);

                //return the razorized view/partial-view as a string
                return sw.ToString();
            }
        }

        [HttpPost, ActionName("GetNetTrackClientList"), GSAAuthorizeAttribute()]
        public JsonResult GetNetTrackClientList(string clientName)
        {
            ClientModel clientModel = new ClientModel();
            List<ClientModel> clientModelList = new List<ClientModel>();

            try
            {
                clientModel.SessionID = SessionVars.CurrentLoggedInUser.SessionId;
                clientModel.ClientName = clientName;
                clientModelList = _TytFacadeBiz.GetNetTrackClientList(clientModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(clientModelList);
        }

        [HttpPost, ActionName("SaveBluePayMode"), GSAAuthorizeAttribute()]
        public JsonResult SaveBluePayMode(string bluePayMode)
        {
            try
            {
                BluePaySettingsModel bluePayModel = new BluePaySettingsModel();

                bluePayModel.CurrentMode = bluePayMode;
                bluePayModel.ServerName = ConfigurationManager.AppSettings["BluePayServer"];
                _TytFacadeBiz.SaveBluePaySettings(bluePayModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, ActionName("GetQuotePaymentMethodList")]
        public JsonResult GetQuotePaymentMethodList()
        {
            return Json(_TytFacadeBiz.GetQuotePaymentMethodList());
        }

        [HttpPost, ActionName("ConvertToOrder"), GSAAuthorizeAttribute()]
        public JsonResult ConvertToOrder(int quoteId, string quotePaymentMethod, int quotePaymentMethodId, string comment)
        {
            QuoteModel quoteModel = new QuoteModel();
            QuoteProductModel quoteProductModel = new QuoteProductModel();
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            try
            {
                quoteModel.QuoteId = quoteId;
                quoteModel.SessionId = 0;
                quoteModel = _TytFacadeBiz.GetQuoteInfo(quoteModel);

                quoteProductModel.QuoteId = quoteId;
                quoteProductModel.SessionId = 0;
                quoteProductModelList = _TytFacadeBiz.GetQuoteProductList(quoteProductModel);


                PropertyCopier<QuoteModel, QuoteOrderModel>.CopyPropertyValues(quoteModel, quoteOrderModel);
                foreach (QuoteProductModel item in quoteProductModelList)
                {
                    SalesOrderModel salesOrderModel = new SalesOrderModel();
                    PropertyCopier<QuoteProductModel, SalesOrderModel>.CopyPropertyValues(item, salesOrderModel);

                    salesOrderModelList.Add(salesOrderModel);
                }

                quoteOrderModel.SessionId = 0;
                quoteOrderModel.Action = "I";
                quoteOrderModel.PurchaseDate = DateTime.Now;
                //quoteOrderModel.QuoteDate = DateTime.Now;
                quoteOrderModel.IsAccepted = true;
                quoteOrderModel.AcceptanceName = SessionVars.CurrentLoggedInUser.EmployeeName;
                quoteOrderModel.QuotePaymentMethodId = quotePaymentMethodId;
                quoteOrderModel.PaymentMethodComment = comment;
                quoteOrderModel.OrderStatusId = 1;
                quoteOrderModel = _TytFacadeBiz.SaveQuoteOrder(quoteOrderModel);

                if (quoteOrderModel.QuoteOrderId <= 0)
                {
                    throw new Exception("Failed");
                }
                else
                {
                    _TytFacadeBiz.SaveOrderStatusHistory(quoteOrderModel.QuoteOrderId, quoteOrderModel.OrderStatusId);

                    int sortOrder = 1;
                    foreach (SalesOrderModel item in salesOrderModelList)
                    {
                        item.Action = "I";
                        item.QuoteOrderId = quoteOrderModel.QuoteOrderId;
                        item.SessionId = 0;
                        item.SortOrder = sortOrder++;

                        _TytFacadeBiz.SaveSalesOrder(item);
                    }
                }

                string body = "", commonBody = "";
                commonBody += "The quote has been converted to order.<br />";
                //commonBody += "Order: " + quoteModel.QuoteId.ToString() + "<br />";
                commonBody += "Method of payment: " + quotePaymentMethod + "<br />";
                if (!string.IsNullOrWhiteSpace(comment))
                {
                    commonBody += "Comments: " + comment + "<br /><br />";
                }

                /*commonBody += "Click the link below to view your Track Your Truck Order Confirmation.<br /><a target='_blank' href='"
                    + ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrder/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(quoteOrderModel.QuoteOrderId.ToString()))
                    + "' >View your Order</a><br />";*/

                //commonBody += "Company Name: " + Session["BillToCompanyName"].ToString();

                MailViewModel mailViewModel = new MailViewModel();
                mailViewModel.QuoteId = quoteModel.QuoteId;
                mailViewModel.CompanyName = quoteModel.BillToCompanyName;
                mailViewModel.CustomerName = quoteModel.BillToBillingContact;
                mailViewModel.Body = commonBody;
                mailViewModel.OrderUrl = ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrder/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(quoteOrderModel.QuoteOrderId.ToString()));

                body = Server.HtmlDecode(RenderPartialViewToString(this, "ConvertToOrderEmailTemplate", mailViewModel));
                EmailSender.SendMailUsingMailGun(false, quoteModel.SalesPersonEmail.ToString(), "Track Your Truck Order Confirmation", body, "tssorders@trackyourtruck.com");
                EmailSender.SendMailUsingMailGun(false, "tssorders@trackyourtruck.com", "Track Your Truck Order Confirmation", body);
            }
            catch (Exception ex)
            {
                QuoteOrderModel item = new QuoteOrderModel();
                item.QuoteId = quoteId;
                item.Action = "D";
                item.SessionId = 0;
                item = _TytFacadeBiz.SaveQuoteOrder(item);

                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetQuoteLastViewList(int id)
        {
            List<QuoteLastViewModel> quoteLastViewModelList = new List<QuoteLastViewModel>();

            try
            {
                quoteLastViewModelList = _TytFacadeBiz.GetQuoteLastViewList(id);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(quoteLastViewModelList);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetSentEmailList(int id)
        {
            List<TSSSentEmailModel> tssSentEmailModelList = new List<TSSSentEmailModel>();

            try
            {
                tssSentEmailModelList = _TytFacadeBiz.GetSentEmailList(id);

                Session["GetSentEmailList_tssSentEmailModelList"] = tssSentEmailModelList;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(tssSentEmailModelList.Select(s => new
            {
                SentDateFormated = s.SentDateFormated,
                Recipent = s.Recipent,
                SenderName = s.SenderName,
                EmailId = s.EmailId,
                EmailStatus = s.EmailStatus,
                EmailStatusTimeFormated = s.EmailStatusTimeFormated,
                TSSSentEmailId = s.TSSSentEmailId,
                IsResendAvailable = !string.IsNullOrWhiteSpace(s.MessageBody)
            }));
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetSentEmail(int id)
        {
            TSSSentEmailModel tssSentEmailModel = new TSSSentEmailModel();
            List<TSSSentEmailModel> tssSentEmailModelList = new List<TSSSentEmailModel>();

            try
            {
                if (Session["GetSentEmailList_tssSentEmailModelList"] != null)
                {
                    tssSentEmailModelList = Session["GetSentEmailList_tssSentEmailModelList"] as List<TSSSentEmailModel>;

                    tssSentEmailModel = tssSentEmailModelList.FirstOrDefault(s => s.TSSSentEmailId == id);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(tssSentEmailModel);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetSentEmailReportList(TSSSentEmailModel sentEmailModel)
        {
            List<TSSSentEmailModel> tssSentEmailModelList = new List<TSSSentEmailModel>();

            try
            {
                tssSentEmailModelList = _TytFacadeBiz.GetSentEmailReportList(sentEmailModel);

                if (!SessionVars.CurrentLoggedInUser.IsTSSAdmin)
                {
                    tssSentEmailModelList = tssSentEmailModelList.Where(e => e.SentById == SessionVars.CurrentLoggedInUser.EmployeeId).ToList();
                }

                Session["GetSentEmailReportList_tssSentEmailModelList"] = tssSentEmailModelList;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(tssSentEmailModelList);
        }

        public ActionResult EmailHistoryExportToExcel()
        {
            MemoryStream excelStream = new MemoryStream();
            Workbook workbook = GetSentEmailExportData();
            workbook.SaveToStream(excelStream);

            string attachment = "attachment; filename=SalesOrder.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            excelStream.WriteTo(Response.OutputStream);
            Response.End();

            return View();
        }

        public Workbook GetSentEmailExportData()
        {
            try
            {
                string[] columnList = new string[] { "Quote Id", "Customer Name", "Sent Date (CST)", "Recipient", "Status", "Received Date (CST)" };
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Sheet1");
                CellFormat cellFormat = new CellFormat();
                cellFormat.HasBorder = true;

                //Build table header
                int courrColIndex = 0;
                for (int i = 0; i < columnList.Length; i++)
                {
                    worksheet.Cells[0, courrColIndex] = new Cell(columnList[i].ToString(), cellFormat);

                    //Auto space
                    SetExcelAutoSpace(worksheet.Cells, 0, courrColIndex);
                    courrColIndex++;
                }

                //Build table body
                int rowIndex = 1;
                List<TSSSentEmailModel> tssSentEmailModelList = Session["GetSentEmailReportList_tssSentEmailModelList"] as List<TSSSentEmailModel>;
                foreach (TSSSentEmailModel tssSentEmailModel in tssSentEmailModelList)
                {
                    courrColIndex = 0;
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(tssSentEmailModel.QuoteId, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(tssSentEmailModel.CustomerName, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(tssSentEmailModel.SentDateFormated, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(tssSentEmailModel.Recipent, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(tssSentEmailModel.EmailStatus, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(tssSentEmailModel.EmailStatusTimeFormated, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);

                    rowIndex++;
                }

                #region Excel Fix

                //Column wise
                for (int i = 0; i < rowIndex; i++)
                {
                    for (int j = columnList.Length; j < 26; j++)
                    {
                        worksheet.Cells[i, j] = new Cell(" ");
                    }
                }

                //Row wise
                for (int i = rowIndex; i < 11; i++)
                {
                    for (int j = 0; j < 26; j++)
                    {
                        worksheet.Cells[i, j] = new Cell(" ");
                    }
                }

                #endregion

                workbook.Worksheets.Add(worksheet);

                return workbook;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        private void SetExcelAutoSpace(CellCollection cells, int row, int col)
        {
            if (cells[row, col].Value != null && ((cells[row, col].Value.ToString().Length * 256) + 512) > cells.ColumnWidth[(ushort)col])
            {
                cells.ColumnWidth[(ushort)col] = (ushort)((cells[row, col].Value.ToString().Length * 256) + 512);
            }
        }

        /*
        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetLastView(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<QuoteModel> quoteModelList = new List<QuoteModel>();
            List<object> lastViewModel = new List<object>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

                QuoteModel quoteModel = new QuoteModel();
                quoteModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteModel.SearchFromDate = DateTime.Parse(Request.QueryString["dateFrom"]);
                quoteModel.SearchToDate = DateTime.Parse(Request.QueryString["dateTo"]);

                if (SessionVars.CurrentLoggedInUser.IsTSSAdmin)
                {
                    quoteModelList = tytFacadeBiz.GetQuoteList(quoteModel);
                }
                else
                {
                    quoteModelList = tytFacadeBiz.GetQuoteList(quoteModel).Where(q => q.SalesPersonId == SessionVars.CurrentLoggedInUser.EmployeeId).ToList();
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["searchText"]))
                {
                    if (Request.QueryString["searchSelect"] == "CustomerName")
                    {
                        quoteModelList = quoteModelList.Where(w => w.CustomerName.ToLower().Contains(Request.QueryString["searchText"].ToLower())).ToList();
                    }
                    else if (Request.QueryString["searchSelect"] == "SearchQuoteId")
                    {
                        quoteModelList = quoteModelList.Where(w => w.SearchQuoteId.Contains(Request.QueryString["searchText"])).ToList();
                    }
                }

                foreach (QuoteModel item in quoteModelList)
                {
                    lastViewModel.Add(new { QuoteId = item.QuoteId, LastViewDateFormated = item.LastViewDateFormated });
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(lastViewModel);
        }
        */

        public ActionResult Unsubscribe(string id)
        {
            try
            {
                Session["Unsubscribe_QuoteId"] = int.Parse(id);
            }
            catch
            {
                ViewBag.Message = "Invalid Url";
            }

            return View();
        }

        public JsonResult EmailUnsubscribe(string id)
        {
            try
            {
                QuoteEmailUnsubscribeModel quoteEmailUnsubscribeModel = new QuoteEmailUnsubscribeModel();
                quoteEmailUnsubscribeModel.QuoteId = Convert.ToInt32(Session["Unsubscribe_QuoteId"]);
                quoteEmailUnsubscribeModel.Email = id;

                _TytFacadeBiz.SaveQuoteEmailUnsubscribe(quoteEmailUnsubscribeModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetUnsubscribeList(int id)
        {
            List<QuoteEmailUnsubscribeModel> quoteEmailUnsubscribeList = new List<QuoteEmailUnsubscribeModel>();

            try
            {
                quoteEmailUnsubscribeList = _TytFacadeBiz.GetQuoteEmailUnsubscribeList(id);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(quoteEmailUnsubscribeList);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult ResetQuoteStartDate(int id)
        {
            try
            {
                _TytFacadeBiz.ResetQuoteStartDate(id);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult ApproveQuote(int id)
        {
            try
            {
                QuoteModel quote = new QuoteModel();
                quote.QuoteId = id;
                quote.IsApproved = "Y";
                quote.ApprovedBy = SessionVars.CurrentLoggedInUser.EmployeeId;
                quote.ApprovedDate = DateTime.Now;

                _TytFacadeBiz.ApproveQuote(quote);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpGet]
        public JsonResult ResendEmail()
        {
            try
            {
                List<TSSSentEmailModel> resendEmailList = new List<TSSSentEmailModel>();
                resendEmailList = _TytFacadeBiz.GetResendEmailList();

                foreach (var item in resendEmailList)
                {
                    try
                    {
                        EmailResponse emailResponse = new EmailResponse();

                        emailResponse = SendUserUrlMailWithCustomVars(
                            item.Recipent,
                            item.MessageBody,
                            string.IsNullOrWhiteSpace(item.SalesPersonName) ? "" : item.SalesPersonName,
                            item.SalesPersonEmail,
                            item.SalesPersonEmail,
                            new { TSSQuoteId = item.QuoteId }
                        );

                        TSSSentEmailModel sentEmailLog = new TSSSentEmailModel();
                        sentEmailLog.QuoteId = item.QuoteId;
                        sentEmailLog.EmailId = emailResponse.EmailId;
                        sentEmailLog.Recipent = item.Recipent;
                        sentEmailLog.MessageBody = item.MessageBody;
                        sentEmailLog.SentById = -1;
                        sentEmailLog.SentDate = DateTime.Now;
                        sentEmailLog.Resend = item.Resend + 1;

                        _TytFacadeBiz.SaveTSSSentEmail(sentEmailLog);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}