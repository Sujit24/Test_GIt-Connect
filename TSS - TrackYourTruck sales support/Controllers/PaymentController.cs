using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BPCSharp;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TSS.Models.ViewModel;
using TYT.Helper;
using System.Collections.Specialized;
using System.Web;
using BluePayLibrary;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class PaymentController : Controller
    {
        [RequireHttpsCustomAttribute]
        public ActionResult Index()
        {
            BluePayTransactionModel bluePayTransaction = new BluePayTransactionModel();

            try
            {
                QuoteViewModel quoteViewModel = Session["QuoteViewModel"] as QuoteViewModel;
                if (quoteViewModel != null)
                {
                    if (quoteViewModel.QuoteOrderModel.IsDemo)
                    {
                        quoteViewModel.TotalValue = 0;
                    }

                    bluePayTransaction.OrderId = quoteViewModel.QuoteOrderModel.QuoteId;
                    bluePayTransaction.Amount = quoteViewModel.TotalValue;

                    bluePayTransaction.CompanyName = quoteViewModel.QuoteOrderModel.BillToCompanyName;
                    bluePayTransaction.Email = quoteViewModel.QuoteOrderModel.BillToBillingEmail;
                    bluePayTransaction.Phone = quoteViewModel.QuoteOrderModel.BillToPhone;

                    int clientId = quoteViewModel.QuoteOrderModel.ClientId;
                    if (false)
                    {
                        TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                        MyAccountModel myAccount = tytFacadeBiz.GetMyAccountInfo(clientId);

                        try
                        {
                            bluePayTransaction.CardNumber = Cipher.Decrypt(myAccount.CardNumber);
                        }
                        catch { }
                        try
                        {
                            bluePayTransaction.CVV2 = Cipher.Decrypt(myAccount.CVV2);
                        }
                        catch { }
                        bluePayTransaction.CardExpireMonth = myAccount.CardExpireMonth;
                        bluePayTransaction.CardExpireYear = myAccount.CardExpireYear;
                        bluePayTransaction.CardHolderFirstName = myAccount.CardHolderFirstName;
                        bluePayTransaction.CardHolderLastName = myAccount.CardHolderLastName;
                        bluePayTransaction.Address = myAccount.Address;
                        bluePayTransaction.City = myAccount.City;
                        bluePayTransaction.State = myAccount.State;
                        bluePayTransaction.ZipCode = myAccount.ZipCode;
                        bluePayTransaction.Country = myAccount.Country;
                    }
                    else
                    {
                        /*string[] name = quoteViewModel.QuoteOrderModel.BillToBillingContact.Split(' ');
                        if (name.Length == 2)
                        {
                            bluePayTransaction.CardHolderFirstName = name[0];
                            bluePayTransaction.CardHolderLastName = name[1];
                        }
                        else if (name.Length > 2)
                        {
                            bluePayTransaction.CardHolderFirstName = name[0] + name[1];
                            for (int i = 2; i < name.Length; i++)
                            {
                                bluePayTransaction.CardHolderLastName += (i == 2 ? name[i] : " " + name[i]);
                            }
                        }
                        else
                        {
                            bluePayTransaction.CardHolderFirstName = quoteViewModel.QuoteOrderModel.BillToBillingContact;
                        }*/
                        bluePayTransaction.CardHolderName = quoteViewModel.QuoteOrderModel.BillToBillingContact;
                        bluePayTransaction.Address = quoteViewModel.QuoteOrderModel.BillToAddress1;
                        bluePayTransaction.City = quoteViewModel.QuoteOrderModel.BillToCity;
                        bluePayTransaction.State = quoteViewModel.QuoteOrderModel.BillToState;
                        bluePayTransaction.ZipCode = quoteViewModel.QuoteOrderModel.BillToZip;
                        bluePayTransaction.Country = quoteViewModel.QuoteOrderModel.BillToCountry;
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View(bluePayTransaction);
        }

        public ActionResult Process(BluePayTransactionModel bluePay)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            BluePayResponse response = new BluePayResponse();
            QuoteViewModel quoteViewModel = Session["QuoteViewModel"] as QuoteViewModel;

            try
            {
                BluePaySettingsModel bluePayModel = tytFacadeBiz.GetBluePaySettingsInfo(ConfigurationManager.AppSettings["BluePayServer"]);
                string accountID = bluePayModel.AccountId;
                string secretKey = bluePayModel.SecretKey;
                string mode = bluePayModel.CurrentMode.ToUpper();

                BluePay payment = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                payment.SetCustomerInformationV2
                (
                    /*firstName: bluePay.CardHolderFirstName,
                    lastName: bluePay.CardHolderLastName,*/
                    name: bluePay.CardHolderName,
                    address1: bluePay.Address,
                    city: bluePay.City,
                    state: bluePay.State,
                    zip: bluePay.ZipCode,
                    country: bluePay.Country,
                    phone: bluePay.Phone,
                    email: bluePay.Email,
                    companyName: bluePay.CompanyName
                );

                payment.SetCCInformation
                (
                    ccNumber: bluePay.CardNumber,
                    ccExpiration: string.Format("{0}{1}", bluePay.CardExpireMonth, bluePay.CardExpireYear),
                    cvv2: bluePay.CVV2
                );

                payment.SetOrderID(bluePay.OrderId.ToString());

                // Validate Credit Card
                //payment.Auth("0.0");

                // Makes the API Request with BluePay
                //payment.Process();

                bool isValidCreditCard = true;

                // If transaction was successful reads the responses from BluePay
                /*if (payment.IsSuccessfulTransaction())
                {
                    string cvv2Message = GetCVV2Message(payment.GetCVV2());
                    if (!string.IsNullOrWhiteSpace(cvv2Message))
                    {
                        isValidCreditCard = false;
                        response.Status = BluePayStatus.Failed;
                        response.Message += cvv2Message + "<br />";
                    }

                    string avsMessage = GetAVSMessage(payment.GetAVS());
                    if (!string.IsNullOrWhiteSpace(avsMessage))
                    {
                        isValidCreditCard = false;
                        response.Status = BluePayStatus.Failed;
                        response.Message += avsMessage + "<br />";
                    }
                }
                else
                {
                    isValidCreditCard = false;

                    response.Status = BluePayStatus.Failed;
                    response.Message = FormatMessage(payment.GetMessage());
                }*/

                if (isValidCreditCard)
                {
                    if (bluePay.Amount == quoteViewModel.TotalValue || quoteViewModel.QuoteOrderModel.IsDemo)
                    {
                        if (quoteViewModel.QuoteOrderModel.IsDemo)
                        {
                            payment.Auth("0.0");
                        }
                        else
                        {
                            payment.Sale(bluePay.Amount.ToString());
                        }

                        payment.Process();

                        if (payment.IsSuccessfulTransaction())
                        {
                            try
                            {
                                bluePay.TransactionId = long.Parse(payment.GetTransID());
                                bluePay.CardNumber = Cipher.Encrypt(bluePay.CardNumber);
                                bluePay.CVV2 = Cipher.Encrypt(bluePay.CVV2);
                                tytFacadeBiz.SaveBluePayTrans(bluePay);


                                //QuoteViewModel model = (QuoteViewModel)Session["QuoteViewModel"];
                                quoteViewModel.QuoteOrderModel.Action = quoteViewModel.QuoteOrderModel.QuoteOrderId == 0 ? "I" : "U";
                                //quoteViewModel.QuoteOrderModel.QuoteDate = DateTime.Now;
                                quoteViewModel.QuoteOrderModel.TransactionId = payment.GetTransID();
                                quoteViewModel.QuoteOrderModel.SessionId = 0;
                                quoteViewModel.QuoteOrderModel.OrderStatusId = 1;

                                quoteViewModel.QuoteOrderModel = tytFacadeBiz.SaveQuoteOrder(quoteViewModel.QuoteOrderModel);

                                if (quoteViewModel.QuoteOrderModel.QuoteOrderId <= 0)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    tytFacadeBiz.SaveOrderStatusHistory(quoteViewModel.QuoteOrderModel.QuoteOrderId, quoteViewModel.QuoteOrderModel.OrderStatusId);

                                    int sortOrder = 1;
                                    foreach (SalesOrderModel item in quoteViewModel.SalesOrderModelList)
                                    {
                                        item.Action = "I";
                                        item.QuoteOrderId = quoteViewModel.QuoteOrderModel.QuoteOrderId;
                                        item.SessionId = 0;
                                        item.SortOrder = sortOrder++;

                                        tytFacadeBiz.SaveSalesOrder(item);
                                    }
                                }

                                SendEmail(quoteViewModel);


                                Session["ConfirmationMessage"] = "Thank you for your order#" + quoteViewModel.QuoteOrderModel.QuoteId + " in the amount of $" + quoteViewModel.TotalValue + "!<br />"
                                    + "Our team will review your order and contact you promptly.";

                                response.Status = BluePayStatus.Success;
                            }
                            catch (Exception ex)
                            {
                                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                response.Message = "Internal server error.";

                                QuoteOrderModel item = new QuoteOrderModel();
                                item.QuoteId = quoteViewModel.QuoteOrderModel.QuoteId;
                                item.SessionId = 0;
                                item.Action = "D";
                                tytFacadeBiz.SaveQuoteOrder(item);
                            }
                        }
                        else
                        {
                            response.Status = BluePayStatus.Failed;
                            response.Message += FormatMessage(payment.GetMessage()) + "<br />";

                            string cvv2Message = GetCVV2Message(payment.GetCVV2());
                            if (!string.IsNullOrWhiteSpace(cvv2Message))
                            {
                                isValidCreditCard = false;
                                response.Message += cvv2Message + "<br />";
                            }

                            string avsMessage = GetAVSMessage(payment.GetAVS());
                            if (!string.IsNullOrWhiteSpace(avsMessage))
                            {
                                isValidCreditCard = false;
                                response.Message += avsMessage + "<br />";
                            }
                        }
                    }
                    else
                    {
                        response.Status = BluePayStatus.Failed;
                        response.Message = "Invalid amount.";
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Status = BluePayStatus.Failed;
                response.Message = "Internal server error.";
            }

            return Json(response);
        }

        public ActionResult Processed()
        {
            ViewBag.Message = Session["ConfirmationMessage"] as String;

            return View();
        }


        public ActionResult ach()
        {
            BluePayACHTransactionModel bluePayACHTransaction = new BluePayACHTransactionModel();

            try
            {
                QuoteViewModel quoteViewModel = Session["QuoteViewModel"] as QuoteViewModel;
                if (quoteViewModel != null)
                {
                    if (quoteViewModel.QuoteOrderModel.IsDemo)
                    {
                        quoteViewModel.TotalValue = 0;
                    }

                    bluePayACHTransaction.OrderId = quoteViewModel.QuoteOrderModel.QuoteId;
                    bluePayACHTransaction.Amount = quoteViewModel.TotalValue;

                    bluePayACHTransaction.CompanyName = quoteViewModel.QuoteOrderModel.BillToCompanyName;
                    bluePayACHTransaction.Email = quoteViewModel.QuoteOrderModel.BillToBillingEmail;
                    bluePayACHTransaction.Phone = quoteViewModel.QuoteOrderModel.BillToPhone;

                    int clientId = quoteViewModel.QuoteOrderModel.ClientId;
                    
                    bluePayACHTransaction.Name = quoteViewModel.QuoteOrderModel.BillToBillingContact;
                    bluePayACHTransaction.Address = quoteViewModel.QuoteOrderModel.BillToAddress1;
                    bluePayACHTransaction.City = quoteViewModel.QuoteOrderModel.BillToCity;
                    bluePayACHTransaction.State = quoteViewModel.QuoteOrderModel.BillToState;
                    bluePayACHTransaction.ZipCode = quoteViewModel.QuoteOrderModel.BillToZip;
                    bluePayACHTransaction.Country = quoteViewModel.QuoteOrderModel.BillToCountry;
                    
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View(bluePayACHTransaction);
        }


        public ActionResult achprocess(BluePayACHTransactionModel bluePay)

        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            BluePayResponse response = new BluePayResponse();
            QuoteViewModel quoteViewModel = Session["QuoteViewModel"] as QuoteViewModel;

            try
            {
                BluePaySettingsModel bluePayModel = tytFacadeBiz.GetBluePaySettingsInfo(ConfigurationManager.AppSettings["BluePayServer"]);
                string accountID = bluePayModel.AccountId;
                string secretKey = bluePayModel.SecretKey;
                string mode = bluePayModel.CurrentMode.ToUpper();

                BluePay payment = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                payment.SetCustomerInformationV2
                (
                    /*firstName: bluePay.CardHolderFirstName,
                    lastName: bluePay.CardHolderLastName,*/
                    name: bluePay.Name,
                    address1: bluePay.Address,
                    city: bluePay.City,
                    state: bluePay.State,
                    zip: bluePay.ZipCode,
                    country: bluePay.Country,
                    phone: bluePay.Phone,
                    email: bluePay.Email,
                    companyName: bluePay.CompanyName
                );

                payment.SetACHInformation
                (
                     routingNum: bluePay.routingNum,
                     accountNum: bluePay.accountNum,
                     accountType: bluePay.accountType,
                     docType: "WEB"
                );

                payment.SetOrderID(bluePay.OrderId.ToString());

                bool isValid = true;

                if (isValid)
                {
                    if (bluePay.Amount == quoteViewModel.TotalValue || quoteViewModel.QuoteOrderModel.IsDemo)
                    {
                        if (quoteViewModel.QuoteOrderModel.IsDemo)
                        {
                            payment.Auth("0.0");
                        }
                        else
                        {
                            payment.Sale(bluePay.Amount.ToString());
                        }

                        payment.Process();/// check how to set "api" field

                        if (payment.IsSuccessfulTransaction())
                        {
                            try
                            {
                                bluePay.TransactionId = long.Parse(payment.GetTransID());
                                bluePay.routingNum = Cipher.Encrypt(bluePay.routingNum);
                                bluePay.accountNum = Cipher.Encrypt(bluePay.accountNum);
                                bluePay.PaymentDate = DateTime.Now;
                                bluePay.IsFundTransferred = false;
                                
                                tytFacadeBiz.SaveBluePayACHTrans(bluePay);


                                //QuoteViewModel model = (QuoteViewModel)Session["QuoteViewModel"];
                                quoteViewModel.QuoteOrderModel.Action = quoteViewModel.QuoteOrderModel.QuoteOrderId == 0 ? "I" : "U";
                                //quoteViewModel.QuoteOrderModel.QuoteDate = DateTime.Now;
                                quoteViewModel.QuoteOrderModel.TransactionId = payment.GetTransID();
                                quoteViewModel.QuoteOrderModel.SessionId = 0;
                                quoteViewModel.QuoteOrderModel.OrderStatusId = 4;

                                quoteViewModel.QuoteOrderModel = tytFacadeBiz.SaveQuoteOrder(quoteViewModel.QuoteOrderModel);

                                if (quoteViewModel.QuoteOrderModel.QuoteOrderId <= 0)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    tytFacadeBiz.SaveOrderStatusHistory(quoteViewModel.QuoteOrderModel.QuoteOrderId, quoteViewModel.QuoteOrderModel.OrderStatusId);

                                    int sortOrder = 1;
                                    foreach (SalesOrderModel item in quoteViewModel.SalesOrderModelList)
                                    {
                                        item.Action = "I";
                                        item.QuoteOrderId = quoteViewModel.QuoteOrderModel.QuoteOrderId;
                                        item.SessionId = 0;
                                        item.SortOrder = sortOrder++;

                                        tytFacadeBiz.SaveSalesOrder(item);
                                    }
                                }

                                SendEmail(quoteViewModel);


                                Session["ConfirmationMessage"] = "Thank you for your order#" + quoteViewModel.QuoteOrderModel.QuoteId + " in the amount of $" + quoteViewModel.TotalValue + "!<br />"
                                    + "Our team will review your order and contact you promptly.";

                                response.Status = BluePayStatus.Success;
                            }
                            catch (Exception ex)
                            {
                                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                response.Message = "1. Internal server error." + ex.StackTrace; 

                                QuoteOrderModel item = new QuoteOrderModel();
                                item.QuoteId = quoteViewModel.QuoteOrderModel.QuoteId;
                                item.SessionId = 0;
                                item.Action = "D";
                                tytFacadeBiz.SaveQuoteOrder(item);
                            }
                        }
                        else
                        {
                            response.Status = BluePayStatus.Failed;
                            response.Message += FormatMessage(payment.GetMessage()) + "<br />";

                            string status = payment.GetStatus();
                            if (!string.IsNullOrWhiteSpace(status))
                            {
                                isValid = false;
                                response.Message += status + "<br />";
                            }
                        }
                    }
                    else
                    {
                        response.Status = BluePayStatus.Failed;
                        response.Message = "Invalid amount.";
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Status = BluePayStatus.Failed;
                response.Message = "Internal server error."+ex.StackTrace;
            }

            return Json(response);
        }

        /*public ActionResult Processed(BluePayTransactionModel bluePay)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                bool isPurchaseDone = false;
                string quoteId = Session["QuoteId"] == null ? null : Session["QuoteId"].ToString();

                BluePayLogModel bluePayLog = new BluePayLogModel();
                bluePayLog.QuoteId = string.IsNullOrWhiteSpace(quoteId) ? 0 : int.Parse(quoteId);
                bluePayLog.SubmittedUrl = Session["BluePayUrl"] == null ? "" : Session["BluePayUrl"].ToString();
                bluePayLog.ReceivedUrl = Request.Url.ToString();
                tytFacadeBiz.SaveBluePayLog(bluePayLog);

                if (quoteId != null)
                {
                    string transactionId = Request.QueryString["INVOICE_ID"];
                    string postMessage = Request.QueryString["MESSAGE"];
                    postMessage = postMessage[0].ToString().ToUpper() + postMessage.Substring(1).ToLower();

                    ViewBag.Message = "Purchase failed. Message - " + postMessage + ".<br /><a href='" + Session["BluePayUrl"].ToString() + "'>Please try again.</a>";

                    BluePaySettingsModel bluePayModel = tytFacadeBiz.GetBluePaySettingsInfo(ConfigurationManager.AppSettings["BluePayServer"]);
                    string accountID = bluePayModel.AccountId;
                    string secretKey = bluePayModel.SecretKey;
                    string mode = bluePayModel.CurrentMode.ToUpper();

                    // Merchant's Account ID
                    // Merchant's Secret Key
                    // Transaction Mode: TEST (can also be LIVE)
                    BluePayPayment report = new BluePayPayment(
                        accountID,
                        secretKey,
                        mode);

                    report.getSingleTransQuery(
                        DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                        DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                        "1");

                    report.queryByTransactionID(transactionId);
                    report.Process();


                    NameValueCollection transResult = HttpUtility.ParseQueryString(report.response);
                    if (transResult != null && transResult["status"] == "1" && !string.IsNullOrWhiteSpace(transResult["amount"]))
                    {
                        try
                        {
                            QuoteViewModel model = (QuoteViewModel)Session["QuoteViewModel"];
                            model.QuoteOrderModel.Action = model.QuoteOrderModel.QuoteOrderId == 0 ? "I" : "U";
                            model.QuoteOrderModel.QuoteDate = DateTime.Now;
                            model.QuoteOrderModel.SessionId = 0;
                            model.QuoteOrderModel.OrderStatusId = 1;

                            model.QuoteOrderModel = tytFacadeBiz.SaveQuoteOrder(model.QuoteOrderModel);

                            if (model.QuoteOrderModel.QuoteOrderId <= 0)
                            {
                                throw new Exception("save failed");
                            }
                            else
                            {
                                foreach (SalesOrderModel item in model.SalesOrderModelList)
                                {
                                    item.Action = "I";
                                    item.QuoteOrderId = model.QuoteOrderModel.QuoteOrderId;
                                    item.SessionId = 0;

                                    tytFacadeBiz.SaveSalesOrder(item);
                                }
                            }

                            SendEmail(model);

                            isPurchaseDone = true;
                        }
                        catch (Exception ex)
                        {
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                            isPurchaseDone = false;
                            ViewBag.Message = "<span style='color:red;'>Please contact with TYT Administrator.</span>";
                        }
                    }

                    if (!isPurchaseDone)
                    {
                        QuoteOrderModel item = new QuoteOrderModel();
                        item.QuoteId = int.Parse(quoteId);

                        item.Action = "D";
                        item.SessionId = 0;
                        item = tytFacadeBiz.SaveQuoteOrder(item);
                    }

                }
                else
                {
                    ViewBag.Message = "<span style='color:red;'>Session Timeout. Please use the same Browser for Order processing...</span>";
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ViewBag.Message = "<span style='color:red;'>Session Timeout. Please use the same browser for Order processing...</span>";
            }

            return View();
        }*/

        private void SendEmail(QuoteViewModel model)
        {
            string body = "", commonBody = "";
            commonBody += "Thank you for your order#" + model.QuoteOrderModel.QuoteId + " in the amount of $" + model.TotalValue + "!<br />";
            commonBody += "Our team will review your order and contact you promptly.<br /><br />";
            if (!string.IsNullOrWhiteSpace(model.QuoteOrderModel.PaymentMethodComment))
            {
                commonBody += "Comments: " + model.QuoteOrderModel.PaymentMethodComment + "<br /><br />";
            }
            /*commonBody += "Click the link below to view your Track Your Truck Order Confirmation.<br /><a target='_blank' href='"
                + ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrder/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(model.QuoteOrderModel.QuoteOrderId.ToString()))
                + "' >View your Order</a><br />";*/

            MailViewModel mailViewModel = new MailViewModel();
            mailViewModel.QuoteId = model.QuoteOrderModel.QuoteId;
            mailViewModel.CompanyName = model.QuoteOrderModel.BillToCompanyName;
            mailViewModel.CustomerName = model.QuoteOrderModel.BillToBillingContact;
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

        private string GetAVSMessage(string avsCode)
        {
            try
            {
                avsCode = avsCode.Trim();

                switch (avsCode)
                {
                    case "X":
                    case "Y":
                    case "D":
                    case "M":
                        return "";
                    case "A":
                    case "B":
                        return "Invalid zip code.";
                    case "W":
                    case "Z":
                    case "P":
                        return "Invalid street address.";
                    case "N":
                        return "Invalid street address and zip code.";
                    case "U":
                        return "Address information unavailable.";
                    case "R":
                        return "Retry - Issuer's System Unavailable or Timed Out.";
                    case "E":
                        return "AVS data is invalid.";
                    case "S":
                        return "U.S. issuing bank does not support AVS.";
                    case "C":
                    case "I":
                        return "Address information not verified.";
                    case "G":
                        return "Non-US. Issuer does not participate.";
                }
            }
            catch { }

            return "";
        }

        private string GetCVV2Message(string cvv2Code)
        {
            try
            {
                cvv2Code = cvv2Code.Trim();

                switch (cvv2Code)
                {
                    case "M":
                        return "";
                    case "":
                    case "N":
                        return "Invalid CVV2.";
                    case "P":
                        return "Not Processed.";
                    case "S":
                        return "Issuer indicates that CVV2 data should be present on the card, but the merchant has indicated data is not present on the card.";
                    case "U":
                        return "Issuer has not certified for CVV2 or Issuer has not provided Visa with the CVV2 encryption keys.";

                }
            }
            catch { }

            return "";
        }

        private string FormatMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                return message[0].ToString() + message.ToLower().Substring(1);
            }

            return "";
        }
    }
}
