using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSA.Security;
using TSS.Helper;
using NetTrackBiz;
using TSS.Models.ViewModel;
using NetTrackModel;
using System.Web.Script.Serialization;
using System.Configuration;
using TYT.Helper;
using MailGun;
using DotNetShipping;
using DotNetShipping.ShippingProviders;
using System.IO;
using Zoho;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class QuoteTemplateController : Controller
    {
        private ZohoAccessManager _ZohoAccessManager;
        private ZohoAPI _ZohoAPI;

        [GSAAuthorizeAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        #region Zoho

        [HttpPost, ActionName("SearchLeadInZoho"), GSAAuthorizeAttribute()]
        public JsonResult SearchLeadInZoho(string searchText)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchLead(searchText);

            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Leads/getSearchRecords?", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchCondition=(Company|contains|*" + HttpUtility.UrlEncode(searchText) + "*)");

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
        public JsonResult SearchContactInZoho(string searchText)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchContact(searchText);

            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecords?", "authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(ACCOUNTID|equal|*" + searchText + "*)");
            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecordsByPDC?", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchColumn=accountid&searchValue=" + HttpUtility.UrlEncode(searchText) + "");

            /*if (result.Contains("nodata"))
            {
                result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Contacts/getSearchRecords?", "authtoken=ddacf63b0dcfcfd036dc5ebc1a7f3137&scope=crmapi&selectColumns=All&searchCondition=(Last Name|contains|*" + searchText + "*)");
            }*/

            return Json(result);
        }

        [HttpPost, ActionName("SearchAccountInZoho"), GSAAuthorizeAttribute()]
        public JsonResult SearchAccountInZoho(string searchText)
        {
            _ZohoAccessManager = new ZohoAccessManager(
                ConfigurationManager.AppSettings["ZohoAPIv2GrantKey"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientId"],
                ConfigurationManager.AppSettings["ZohoAPIv2ClientSecret"],
                ConfigurationManager.AppSettings["ZohoAPIv2RedirectUri"],
                ConfigurationManager.AppSettings["ZohoAPIv2RefreshToken"]
            );
            _ZohoAPI = new ZohoAPI(_ZohoAccessManager);

            var result = _ZohoAPI.SearchAccount(searchText);

            //string result = ZohoCrmApi.AccessCRM("https://crm.zoho.com/crm/private/json/Accounts/getSearchRecords?", "authtoken=8b458532b319987174d95f7e9bbf99f9&scope=crmapi&selectColumns=All&searchCondition=(Account Name|contains|*" + HttpUtility.UrlEncode(searchText) + "*)");

            return Json(result);
        }

        #endregion

        [GSAAuthorizeAttribute()]
        public ActionResult List()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            return View();
        }

        [HttpPost, ActionName("GetQuoteTemplateList"), GSAAuthorizeAttribute()]
        public ContentResult GetQuoteTemplateList(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<QuoteTemplateModel> quoteModelList = new List<QuoteTemplateModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                quoteModelList = tytFacadeBiz.GetQuoteTemplateList(SessionVars.CurrentLoggedInUser.SessionId);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return LargeJsonResult(quoteModelList);
        }

        public ContentResult LargeJsonResult(List<QuoteTemplateModel> quoteModelList)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here  
            ContentResult result = new ContentResult();
            result.Content = serializer.Serialize(quoteModelList);
            result.ContentType = "application/json";

            return result;
        }

        [HttpPost, ActionName("Save"), GSAAuthorizeAttribute()]
        public ActionResult SaveDataToDB(QuoteTemplateViewModel model)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                //if (ModelState.IsValid)
                {
                    model.QuoteTemplateModel.Action = model.QuoteTemplateModel.QuoteId == 0 ? "I" : "U";
                    model.QuoteTemplateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;

                    model.QuoteTemplateModel = tytFacadeBiz.SaveQuoteTemplate(model.QuoteTemplateModel);
                    if (model.QuoteTemplateModel.QuoteId <= 0)
                    {
                        throw new Exception("save failed");
                    }
                    else
                    {
                        int sortOrder = 1;
                        foreach (QuoteProductModel item in model.QuoteProductModelList)
                        {
                            item.Action = (item.QuoteProductId == 0 && !item.Deleted) ? "I" : (item.Deleted ? "D" : "U");
                            item.QuoteId = model.QuoteTemplateModel.QuoteId;
                            item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                            item.SortOrder = sortOrder++;

                            tytFacadeBiz.SaveQuoteTemplateProduct(item);
                        }
                    }
                }
                /*else
                {
                    return Json(new AjaxResponse { Message = "Save failed." });
                }*/
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { PrimaryKey = model.QuoteTemplateModel.QuoteId, Message = "Successfully Saved." });
        }

        [HttpPost, ActionName("GetSalesPerson"), GSAAuthorizeAttribute()]
        public JsonResult GetSalesPerson()
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            return Json(tytFacadeBiz.GetSalesPersonList(SessionVars.CurrentLoggedInUser.SessionId));
        }

        [HttpPost, ActionName("Delete"), GSAAuthorizeAttribute()]
        public JsonResult Delete(int PrimaryKey)
        {
            try
            {
                QuoteTemplateModel item = new QuoteTemplateModel();
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                item.QuoteId = PrimaryKey;

                if (item.QuoteId > 0)
                {
                    item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    item = tytFacadeBiz.GetQuoteTemplateInfo(item);

                    item.Action = "D";
                    item = tytFacadeBiz.SaveQuoteTemplate(item);

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
            ViewBag.Title = "View/Edit Quotes";
            ViewBag.QuoteId = id;

            return View("Index");
        }

        [ActionName("GetQuoteTemplate"), GSAAuthorizeAttribute()]
        public JsonResult GetQuoteTemplate(int id)
        {
            QuoteTemplateModel quoteTemplateModel = new QuoteTemplateModel();
            QuoteProductModel quoteProductModel = new QuoteProductModel();
            List<QuoteProductModel> quoteProductModelList = new List<QuoteProductModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                quoteTemplateModel.QuoteId = id;
                quoteTemplateModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteTemplateModel = tytFacadeBiz.GetQuoteTemplateInfo(quoteTemplateModel);

                quoteProductModel.QuoteId = id;
                quoteProductModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteProductModelList = tytFacadeBiz.GetQuoteProductTemplateList(quoteProductModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(new { Quote = quoteTemplateModel, QuoteProductList = quoteProductModelList });
        }

        [ActionName("UserView")]
        public ActionResult UserView(string id)
        {
            try
            {
                id = Cipher.Decrypt(id);
                ViewBag.Title = "Review Quotes";
                ViewBag.CurrentDateTime = DateTime.Now.ToString("MM-dd-yyyy hh:mm tt");

                if (Convert.ToDateTime(id.Split('|')[1]) >= DateTime.Now)
                {
                    ViewBag.QuoteId = id.Split('|')[0];

                    QuoteModel quoteModel = new QuoteModel();
                    TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                    quoteModel.QuoteId = Convert.ToInt32(ViewBag.QuoteId);
                    quoteModel.SessionId = 0;
                    quoteModel = tytFacadeBiz.GetQuoteInfo(quoteModel);

                    quoteModel.LastViewDate = DateTime.Now;
                    quoteModel.Action = "U";
                    tytFacadeBiz.SaveQuote(quoteModel);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View();
        }

        [ActionName("GetShippingAndHandling"), GSAAuthorizeAttribute()]
        public JsonResult GetShippingAndHandling(ShippingAndHandlingViewModel shippingAndHandlingModel)
        {
            decimal groundRate = 0, secondDayAirRate = 0, nextDayAirRate = 0;

            try
            {
                // Setup package and destination/origin addresses
                var packages = new List<Package>();
                packages.Add(new Package(0, 0, 0, (shippingAndHandlingModel.PackageWeight + (decimal)0.00000001), shippingAndHandlingModel.PackageInsuredValue));

                var origin = new Address("21754 S. Center Ave", "", "", "New Lenox", "IL", "60451", "US");
                var destination = new Address(shippingAndHandlingModel.ShipToAddress, "", "", shippingAndHandlingModel.ShipToCity, shippingAndHandlingModel.ShipToState, shippingAndHandlingModel.ShipToZip, "US");

                // Create RateManager
                var rateManager = new RateManager();
                rateManager.AddProvider(new UPSProvider("6CE2DAC4931936F5", "trackyourtruck", "phigam"));

                // (Optional) Add RateAdjusters
                rateManager.AddRateAdjuster(new PercentageRateAdjuster(.9M));

                // Call GetRates()
                Shipment shipment = rateManager.GetRates(origin, destination, packages);

                // Iterate through the rates returned
                foreach (Rate rate in shipment.Rates)
                {
                    if (rate.ProviderCode == "03")
                    {
                        groundRate = rate.TotalCharges;
                    }
                    else if (rate.ProviderCode == "02")
                    {
                        secondDayAirRate = rate.TotalCharges;
                    }
                    else if (rate.ProviderCode == "01")
                    {
                        nextDayAirRate = rate.TotalCharges;
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Error = ex.ToString(), GroundRate = groundRate, SecondDayAirRate = secondDayAirRate, NextDayAirRate = nextDayAirRate });
            }

            return Json(new { GroundRate = groundRate, SecondDayAirRate = secondDayAirRate, NextDayAirRate = nextDayAirRate });
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

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult SetActive(int quoteId, bool isActive)
        {
            try
            {
                QuoteTemplateModel item = new QuoteTemplateModel();
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                item.QuoteId = quoteId;
                item.IsActive = isActive;
                item = tytFacadeBiz.SetQuoteTemplateActive(item);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Failed" });
            }

            return Json(new AjaxResponse { Message = "Success" });
        }

        [HttpGet]
        public ActionResult GetQuoteTemplateGroupList()
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            List<DdlSourceModel> list = new List<DdlSourceModel>();

            try
            {
                list = new TytFacadeBiz().GetQuoteTemplateGroupList();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}