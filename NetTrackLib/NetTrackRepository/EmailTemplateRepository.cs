using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections;

namespace NetTrackRepository
{
    public class EmailTemplateRepository
    {
        private EmailTemplateModel _EmailTemplateModel;
        private DBEmailTemplate _DBEmailTemplate;

        public EmailTemplateRepository()
        {
            this._DBEmailTemplate = new DBEmailTemplate();
        }

        public List<EmailTemplateModel> GetEmailTemplateList()
        {
            EmailTemplateModel emailTemplateModel = null;
            List<EmailTemplateModel> emailTemplateModelList = new List<EmailTemplateModel>();

            DataTable dtEmailTemplate = _DBEmailTemplate.GetEmailTemplateList();
            foreach (DataRow dr in dtEmailTemplate.Rows)
            {
                emailTemplateModel = new EmailTemplateModel();
                emailTemplateModel.EmailTemplateId = Convert.ToInt32(dr["EmailTemplateId"]);
                emailTemplateModel.Title = Convert.ToString(dr["Title"]);
                emailTemplateModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                emailTemplateModel.CreatedDateFormatted = emailTemplateModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");

                emailTemplateModelList.Add(emailTemplateModel);
            }

            return emailTemplateModelList;
        }

        public EmailTemplateModel GetEmailTemplate(int emailTemplateId)
        {
            DataTable dtEmailTemplate = _DBEmailTemplate.GetEmailTemplate(emailTemplateId);
            foreach (DataRow dr in dtEmailTemplate.Rows)
            {
                _EmailTemplateModel = new EmailTemplateModel();
                _EmailTemplateModel.EmailTemplateId = Convert.ToInt32(dr["EmailTemplateId"]);
                _EmailTemplateModel.Title = Convert.ToString(dr["Title"]);
                _EmailTemplateModel.Template = Convert.ToString(dr["Template"]);
                _EmailTemplateModel.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
                _EmailTemplateModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                _EmailTemplateModel.CreatedDateFormatted = _EmailTemplateModel.CreatedDate.ToString("MM-dd-yyyy hh:mm tt");
                _EmailTemplateModel.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]);
                _EmailTemplateModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                _EmailTemplateModel.ModifiedDateFormatted = _EmailTemplateModel.ModifiedDate.ToString("MM-dd-yyyy hh:mm tt");
            }

            return _EmailTemplateModel;
        }

        public EmailTemplateModel SaveEmailTemplate(EmailTemplateModel model)
        {
            model.EmailTemplateId = 0;
            model.CreatedDate = DateTime.Now;
            model.ModifiedBy = model.CreatedBy;
            model.ModifiedDate = model.CreatedDate;

            return _DBEmailTemplate.SaveEmailTemplate(model);
        }

        public void UpdateEmailTemplate(EmailTemplateModel model)
        {
            model.ModifiedDate = DateTime.Now;

            _DBEmailTemplate.SaveEmailTemplate(model);
        }

        public void DeleteEmailTemplate(int emailTemplateId)
        {
            _EmailTemplateModel = new EmailTemplateModel();
            _EmailTemplateModel.EmailTemplateId = emailTemplateId;
            _EmailTemplateModel.CreatedDate = DateTime.Now;
            _EmailTemplateModel.ModifiedDate = DateTime.Now;

            _DBEmailTemplate.SaveEmailTemplate(_EmailTemplateModel);
        }
    }
}
