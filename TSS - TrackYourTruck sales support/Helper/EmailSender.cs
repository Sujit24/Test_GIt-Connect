using System;
using System.Collections.Generic;
using System.Configuration;
using MailGun;
using RestSharp;
using System.IO;
using System.Web;
using System.Net.Mail;
using System.Net;
using MimeTypes;
using System.Text;

namespace TSS.Helper
{
    public enum EmailResponseStatus
    {
        Success,
        Failed
    }

    public class EmailResponse
    {
        public string EmailId { get; set; }
        public EmailResponseStatus Status { get; set; }
    }

    public static class EmailSender
    {
        public static string SendMailMessage(
            string strFrom,
            string strTo,
            string strCC,
            string strBCC,
            string strSubject,
            string strBody,
            string strDisplayName)
        {
            string strSentStatus = "";
            try
            {
                CDO.Message msg = new CDO.Message();
                msg.From = strDisplayName + "(" + strFrom + ")";
                msg.Sender = strDisplayName + "(" + strFrom + ")";

                msg.To = strTo;
                msg.BCC = strBCC;
                msg.CC = strCC;
                msg.HTMLBody = strBody;

                //msg.AddAttachment(tempPdfPath, "", "");
                msg.Subject = strSubject;
                //TODO uncomment below line                
                msg.Send();

                strSentStatus = "Success";
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                strSentStatus = "Fail";
            }
            return strSentStatus;
        }

        public static string SendMailMessage(
            string strFrom,
            string strTo,
            string strCC,
            string strBCC,
            string strSubject,
            string strBody,
            string strDisplayName,
            List<MailGun.Attachment> attachment)
        {
            string strSentStatus = "";
            try
            {
                CDO.Message msg = new CDO.Message();
                msg.From = strDisplayName + "(" + strFrom + ")";
                msg.Sender = strDisplayName + "(" + strFrom + ")";

                msg.To = strTo;
                msg.BCC = strBCC;
                msg.CC = strCC;
                msg.HTMLBody = strBody;

                //msg.AddAttachment(tempPdfPath, "", "");
                string attachFileName = "";
                foreach (MailGun.Attachment attachItem in attachment)
                {
                    attachFileName = HttpContext.Current.Server.MapPath("~/PdfReports/" + CleanFileName(attachItem.FileName));
                    File.WriteAllBytes(attachFileName, attachItem.Contents);

                    msg.AddAttachment(attachFileName);
                }

                msg.Subject = strSubject;
                //TODO uncomment below line                
                msg.Send();

                strSentStatus = "Success";
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                strSentStatus = "Fail";

                throw ex;
            }

            return strSentStatus;
        }

        public static string SendMailMessageUsingRackspace(
            string strFrom,
            string strTo,
            string strCC,
            string strBCC,
            string strSubject,
            string strBody,
            string strDisplayName,
            List<MailGun.Attachment> attachment)
        {
            string strSentStatus = "";

            try
            {
                using (SmtpClient client = new SmtpClient("secure.emailsrvr.com", 587))
                {
                    client.Credentials = new NetworkCredential("TSS@trackyourtruck.com", "Cemtek%99");
                    client.EnableSsl = true;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(strFrom);

                        if (!string.IsNullOrWhiteSpace(strTo))
                        {
                            foreach (var to in strTo.Split(';'))
                            {
                                message.To.Add(to);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(strCC))
                        {
                            foreach (var cc in strCC.Split(';'))
                            {
                                message.CC.Add(cc);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(strBCC))
                        {
                            foreach (var bcc in strBCC.Split(';'))
                            {
                                message.Bcc.Add(bcc);
                            }
                        }

                        message.Body = strBody;
                        //message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.IsBodyHtml = true;
                        message.Subject = strSubject;

                        string attachFileName;
                        foreach (MailGun.Attachment attachItem in attachment)
                        {
                            attachItem.FileName = Encoding.UTF7.GetString(Encoding.Default.GetBytes(attachItem.FileName));
                            attachItem.FileName = CleanFileName(attachItem.FileName);

                            var attachFile = new System.Net.Mail.Attachment(new MemoryStream(attachItem.Contents), attachItem.FileName);
                            attachFile.ContentType = new System.Net.Mime.ContentType(MimeTypeMap.GetMimeType(Path.GetExtension(attachItem.FileName)));
                            attachFile.Name = attachItem.FileName;

                            /*attachFileName = HttpContext.Current.Server.MapPath("~/PdfReports/" + attachItem.FileName);
                            File.WriteAllBytes(attachFileName, attachItem.Contents);

                            var attachFile = new System.Net.Mail.Attachment(attachFileName);
                            attachFile.ContentType = new System.Net.Mime.ContentType(MimeTypeMap.GetMimeType(Path.GetExtension(attachItem.FileName)));
                            attachFile.Name = attachItem.FileName;*/

                            message.Attachments.Add(attachFile);
                        }

                        client.Send(message);
                        //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(string.IsNullOrWhiteSpace(strBody) ? "Null body" : strBody.Substring(0, 20)));
                    }
                }
                strSentStatus = "Success";
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                strSentStatus = "Fail";

                throw ex;
            }

            return strSentStatus;
        }

        public static string GetFromEmailAddr()
        {
            string fromEmailAddr = string.Empty;
            if (!(ConfigurationManager.AppSettings["EmailFrom"] == null && ConfigurationManager.AppSettings["EmailFrom"] == ""))
                fromEmailAddr = ConfigurationManager.AppSettings["EmailFrom"].ToString();

            return fromEmailAddr;
        }

        public static string GetToEmails()
        {
            string toEmails = string.Empty;
            if (!(ConfigurationManager.AppSettings["EmailTo"] == null && ConfigurationManager.AppSettings["EmailTo"] == ""))
                toEmails = ConfigurationManager.AppSettings["EmailTo"].ToString();

            return toEmails;
        }

        public static string GetCCAddr()
        {
            string ccAddr = String.Empty;
            if (!(ConfigurationManager.AppSettings["EmailCC"] == null && ConfigurationManager.AppSettings["EmailCC"] == ""))
                ccAddr = ConfigurationManager.AppSettings["EmailCC"].ToString();

            return ccAddr;
        }

        public static string GetBCCAddr()
        {
            string bccAddr = String.Empty;
            if (!(ConfigurationManager.AppSettings["EmailBCC"] == null && ConfigurationManager.AppSettings["EmailBCC"] == ""))
                bccAddr = ConfigurationManager.AppSettings["EmailBCC"].ToString();

            return bccAddr;
        }

        public static string SendMailMessageWithAttachment(string strSubject, string strBody, string attachPath)
        {
            string strSentStatus = "";
            try
            {
                CDO.Message msg = new CDO.Message();
                msg.From = GetFromEmailAddr();
                msg.Sender = GetFromEmailAddr();

                msg.To = GetToEmails();
                msg.BCC = GetBCCAddr();
                msg.CC = GetCCAddr();
                msg.HTMLBody = strBody;
                msg.AddAttachment(attachPath);
                msg.Subject = strSubject;
                //TODO uncomment below line                
                msg.Send();

                strSentStatus = "Success";
            }
            catch (Exception ex)
            {
                strSentStatus = "Fail";
            }
            return strSentStatus;
        }

        public static string SendMailUsingMailGun(bool isQuoteEmail, string mailTo, string subject, string body, string replyTo = "", string bcc = "")
        {
            string apiKey = ConfigurationManager.AppSettings["MailGunApiKey"];
            string domain = ConfigurationManager.AppSettings["MailGunProdDomain"];
            //Contact sender = new Contact { Name = "Track Your Truck", MailAddress = "postmaster@nettrackmail.trackyourtruck.com" }; //Don't change
            Contact sender = new Contact { Name = "Track Your Truck", MailAddress = (isQuoteEmail ? "quotes@nettrackmail.trackyourtruck.com" : "orders@nettrackmail.trackyourtruck.com") }; //Don't change

            List<Contact> reciepeients = new List<Contact>();
            foreach (string to in mailTo.Split(';'))
            {
                if (!string.IsNullOrWhiteSpace(to))
                {
                    reciepeients.Add(new Contact { MailAddress = to });
                }
            }

            List<Contact> contactBCC = new List<Contact>();
            foreach (string email in bcc.Split(';'))
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    contactBCC.Add(new Contact { MailAddress = email });
                }
            }

            string strSentStatus = "";
            try
            {
                MailManager mail = null;

                if (ConfigurationManager.AppSettings["IsDevPC"] == "1")
                {
                    reciepeients = new List<Contact>();
                    reciepeients.Add(new Contact { MailAddress = ConfigurationManager.AppSettings["DevEmail"] });
                    mail = new MailManager
                    {
                        ApiKey = apiKey,
                        Domain = domain,
                        From = sender,
                        To = reciepeients,
                        Subject = subject,
                        MailType = MailType.Html,
                        MessageHTML = body
                    };
                }
                else
                {
                    mail = new MailManager
                    {
                        ApiKey = apiKey,
                        Domain = domain,
                        From = sender,
                        To = reciepeients,
                        BCC = contactBCC,
                        Subject = subject,
                        MailType = MailType.TextAndHtml,
                        MessageHTML = body,
                        MessageText = HtmlToText.Convert(body),
                        ReplyTo = replyTo
                    };
                }

                RestResponse<MailSendResponse> resp = mail.SendMessage() as RestResponse<MailSendResponse>;

                if (resp.Data.message.Contains("Queued. Thank you."))
                {
                    strSentStatus = "Success";
                }
                else
                {
                    strSentStatus = "Fail";
                }
            }
            catch (Exception ex)
            {
                strSentStatus = "Fail";
            }

            return strSentStatus;
        }

        public static EmailResponse SendMailUsingMailGunWithCustomVars(bool isQuoteEmail, string mailTo, string subject, string body, string replyToName = "", string replyTo = "", string bcc = "", object customVars = null, List<MailGun.Attachment> attachment = null)
        {
            EmailResponse response = new EmailResponse();
            string apiKey = ConfigurationManager.AppSettings["MailGunApiKey"];
            string domain = ConfigurationManager.AppSettings["MailGunProdDomain"];
            //Contact sender = new Contact { Name = "Track Your Truck", MailAddress = "postmaster@nettrackmail.trackyourtruck.com" }; //Don't change
            //Contact sender = new Contact { Name = replyToName, MailAddress = replyTo };
            Contact sender = new Contact { Name = "Track Your Truck", MailAddress = (isQuoteEmail ? "quotes@nettrackmail.trackyourtruck.com" : "orders@nettrackmail.trackyourtruck.com") }; //Don't change

            List<Contact> reciepeients = new List<Contact>();
            foreach (string to in mailTo.Split(';'))
            {
                if (!string.IsNullOrWhiteSpace(to))
                {
                    reciepeients.Add(new Contact { MailAddress = to });
                }
            }

            List<Contact> contactBCC = new List<Contact>();
            foreach (string email in bcc.Split(';'))
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    contactBCC.Add(new Contact { MailAddress = email });
                }
            }

            string strSentStatus = "";
            try
            {
                MailManager mail = null;

                if (ConfigurationManager.AppSettings["IsDevPC"] == "1")
                {
                    reciepeients = new List<Contact>();
                    reciepeients.Add(new Contact { MailAddress = ConfigurationManager.AppSettings["DevEmail"] });
                    mail = new MailManager
                    {
                        ApiKey = apiKey,
                        Domain = domain,
                        From = sender,
                        To = reciepeients,
                        Subject = subject,
                        MailType = MailType.Html,
                        MessageHTML = body,
                        Attachements = attachment
                    };
                }
                else
                {
                    mail = new MailManager
                    {
                        ApiKey = apiKey,
                        Domain = domain,
                        From = sender,
                        To = reciepeients,
                        BCC = contactBCC,
                        Subject = subject,
                        MailType = MailType.TextAndHtml,
                        MessageHTML = body,
                        MessageText = HtmlToText.Convert(body),
                        ReplyTo = replyTo,
                        CustomVariables = customVars,
                        Attachements = attachment
                    };
                }

                RestResponse<MailSendResponse> resp = mail.SendMessage() as RestResponse<MailSendResponse>;

                if (resp.Data.message.Contains("Queued. Thank you."))
                {
                    response.Status = EmailResponseStatus.Success;
                    response.EmailId = resp.Data.id.Substring(1, resp.Data.id.Length - 2);
                }
                else
                {
                    response.Status = EmailResponseStatus.Failed;
                }
            }
            catch (Exception ex)
            {
                response.Status = EmailResponseStatus.Failed;
            }

            return response;
        }

        public static string CleanFileName(string fileName)
        {
            char[] illegalChar = new char[] {
                '#','%', '&', '{', '}', '\\', '<', '>', '*', '?', '/', '$', '!', '\'', '"', ':', '@', '+', '`', '|', '='
            };

            foreach (var charItem in illegalChar)
            {
                fileName = fileName.Replace(charItem, '_');
            }

            return fileName;
        }
    }
}