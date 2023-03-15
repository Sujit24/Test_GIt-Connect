using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSS.Helper;
using NetTrackModel;
using NetTrackBiz;
using System.Web.Script.Serialization;
using GSA.Security;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class SettingsController : Controller
    {
        [GSAAuthorize()]
        public ActionResult Index()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            List<TSSSettings> settings = tytFacadeBiz.GetAllSettings();
            ViewBag.MaxHWQTY = settings.FirstOrDefault(f=>f.SettingsName == "MaxHWQTY").SettingsValue;

            ViewBag.Resend_1 = settings.FirstOrDefault(f => f.SettingsName == "Resend#1").SettingsValue;
            ViewBag.Resend_2 = settings.FirstOrDefault(f => f.SettingsName == "Resend#2").SettingsValue;
            ViewBag.Resend_3 = settings.FirstOrDefault(f => f.SettingsName == "Resend#3").SettingsValue;

            ViewBag.CanadaShipperPhone = settings.FirstOrDefault(f => f.SettingsName == "CanadaShipperPhone").SettingsValue;
            ViewBag.CanadaProductDescription = settings.FirstOrDefault(f => f.SettingsName == "CanadaProductDescription").SettingsValue;

            ViewBag.DirectDeliveryCharge = settings.FirstOrDefault(f => f.SettingsName == "DirectDeliveryCharge").SettingsValue;
            ViewBag.DevicesLessThan3 = settings.FirstOrDefault(f => f.SettingsName == "<3Devices").SettingsValue;
            ViewBag.Devices4To6 = settings.FirstOrDefault(f => f.SettingsName == "4-6Devices").SettingsValue;
            ViewBag.DevicesGreaterThan7 = settings.FirstOrDefault(f => f.SettingsName == ">7Devices").SettingsValue;

            ViewBag.ShipperAddressLine = settings.FirstOrDefault(f => f.SettingsName == "ShipperAddressLine").SettingsValue;
            ViewBag.ShipperCity = settings.FirstOrDefault(f => f.SettingsName == "ShipperCity").SettingsValue;
            ViewBag.ShipperState = settings.FirstOrDefault(f => f.SettingsName == "ShipperState").SettingsValue;
            ViewBag.ShipperZip = settings.FirstOrDefault(f => f.SettingsName == "ShipperZip").SettingsValue;
            ViewBag.ShipperNumberForShipping = settings.FirstOrDefault(f => f.SettingsName == "ShipperNumberForShipping").SettingsValue;

            return View();
        }

        [HttpPost, ActionName("Save"), GSAAuthorizeAttribute()]
        public ActionResult Save(TSSSettings settings)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                tytFacadeBiz.SetSettings(settings);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { Message = "Successfully Saved." });
        }
    }
}