using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTrackRepository;
using NetTrackModel;

namespace NetTrackBiz
{
    public class EmailTemplateBiz
    {
        private EmailTemplateRepository _EmailTemplateRepository;
             
        public EmailTemplateBiz()
        {
            _EmailTemplateRepository = new EmailTemplateRepository();
        }

        public List<EmailTemplateModel> GetEmailTemplateList()
        {
            return _EmailTemplateRepository.GetEmailTemplateList();
        }

        public EmailTemplateModel GetEmailTemplate(int emailTemplateId)
        {
            return _EmailTemplateRepository.GetEmailTemplate(emailTemplateId);
        }

        public EmailTemplateModel SaveEmailTemplate(EmailTemplateModel model)
        {
            return _EmailTemplateRepository.SaveEmailTemplate(model);
        }

        public void UpdateEmailTemplate(EmailTemplateModel model)
        {
            _EmailTemplateRepository.UpdateEmailTemplate(model);
        }

        public void DeleteEmailTemplate(int emailTemplateId)
        {
            _EmailTemplateRepository.DeleteEmailTemplate(emailTemplateId);
        }
    }
}
