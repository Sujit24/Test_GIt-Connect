using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSS.Helper;
using NetTrackModel;
using GSA.Security;
using NetTrackBiz;
using TSS.Models.ViewModel;
using System.Text.RegularExpressions;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class EmailTemplateController : Controller
    {
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

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetEmailTemplateList(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<EmailTemplateModel> emailTemplateModelList = new List<EmailTemplateModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                emailTemplateModelList = tytFacadeBiz.GetEmailTemplateList();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(emailTemplateModelList);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult Delete(int PrimaryKey)
        {
            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                if (PrimaryKey > 0)
                {
                    tytFacadeBiz.DeleteEmailTemplate(PrimaryKey);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Delete failed." });
            }

            return Json(new AjaxResponse { Message = "Successfully Deleted." });
        }

        [GSAAuthorizeAttribute()]
        public JsonResult Get(int id)
        {
            EmailTemplateModel emailTemplateModel = null;

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                emailTemplateModel = tytFacadeBiz.GetEmailTemplate(id);

                emailTemplateModel.Template = emailTemplateModel.Template.Replace("<span class=\"sceditor-selection sceditor-ignore\" style=\"line-height: 0; display: none;\" id=\"sceditor-end-marker\"> </span><span class=\"sceditor-selection sceditor-ignore\" style=\"line-height: 0; display: none;\" id=\"sceditor-start-marker\"> </span>", " ");
                emailTemplateModel.Template = emailTemplateModel.Template.Replace("#", "\\#");

                foreach (var match in Regex.Matches(emailTemplateModel.Template, @"\$[a-zA-Z]+\$"))
                {
                    var matchValue = match.ToString();

                    emailTemplateModel.Template = emailTemplateModel.Template.Replace(matchValue, String.Format("#= {0} #", matchValue.Replace("$", "")));
                }

                emailTemplateModel.Template = emailTemplateModel.Template.Trim();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(emailTemplateModel);
        }

        [GSAAuthorizeAttribute()]
        public ActionResult Edit(int id)
        {
            EmailTemplateViewModel emailTemplateViewModel = new EmailTemplateViewModel();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                EmailTemplateModel emailTemplateModel = tytFacadeBiz.GetEmailTemplate(id);

                PropertyCopier<EmailTemplateModel, EmailTemplateViewModel>.CopyPropertyValues(emailTemplateModel, emailTemplateViewModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View(emailTemplateViewModel);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult Update(EmailTemplateViewModel emailTemplate)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                EmailTemplateModel emailTemplateModel = tytFacadeBiz.GetEmailTemplate(emailTemplate.EmailTemplateId);
                emailTemplateModel.Title = emailTemplate.Title;
                emailTemplateModel.Template = emailTemplate.Template;
                emailTemplateModel.ModifiedBy = SessionVars.CurrentLoggedInUser.EmployeeId;

                tytFacadeBiz.UpdateEmailTemplate(emailTemplateModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Successfully Failed." }); ;
            }

            return Json(new AjaxResponse { Message = "Successfully Saved." }); ;
        }

        [GSAAuthorizeAttribute()]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult Save(EmailTemplateViewModel emailTemplate)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                EmailTemplateModel emailTemplateModel = new EmailTemplateModel();
                emailTemplateModel.Title = emailTemplate.Title;
                emailTemplateModel.Template = emailTemplate.Template;
                emailTemplateModel.CreatedBy = SessionVars.CurrentLoggedInUser.EmployeeId;

                tytFacadeBiz.SaveEmailTemplate(emailTemplateModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Successfully Failed." }); ;
            }

            return Json(new AjaxResponse { Message = "Successfully Saved." }); ;
        }
    }
}
