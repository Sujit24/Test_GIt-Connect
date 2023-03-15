using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RestSharp;

namespace MailGun
{
    public class MailManager
    {
        /// <summary>
        ///     Private api key set in MailGun 
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        ///     From which Sub Domain the Mail to be sent
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        ///     Sender Email Address
        /// </summary>
        /// <example>
        ///     Can be used in this Format : Excited User <me@samples.mailgun.org> 
        /// </example> 
        public Contact From { get; set; }

        /// <summary>
        ///     Recieients Email Address in list format
        /// </summary>
        /// <example>
        ///     Can be used in this Format : Excited User <me@samples.mailgun.org> 
        /// </example> 
        public List<Contact> To { get; set; }

        /// <summary>
        ///     Carbon Copy Recieients Email Address in list format
        /// </summary>
        /// <example>
        ///     Can be used in this Format : Excited User <me@samples.mailgun.org> 
        /// </example> 
        public List<Contact> CC { get; set; }

        /// <summary>
        ///     Blank Carbon Copy Recieients Email Address in list forma
        /// </summary>
        /// <example>
        ///     Can be used in this Format : Excited User <me@samples.mailgun.org> 
        /// </example> 
        public List<Contact> BCC { get; set; }

        /// <summary>
        ///     Email's Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     E-Mail Message Type
        /// </summary>
        public MailType MailType { get; set; }

        /// <summary>
        ///     Plain Text Message
        /// </summary>
        /// <remarks>
        ///     Will only be sent if Mail type is either of Text or Both
        /// </remarks>
        public string MessageText { get; set; }

        /// <summary>
        ///     HTML Message 
        /// </summary>
        /// <remarks>
        ///     Will only be sent if Mail type is either of Html or Both
        /// </remarks>
        public string MessageHTML { get; set; }

        /// <summary>
        ///     List of Attachments Added as byte Array
        /// </summary>
        public List<Attachment> Attachements { get; set; }

        /// <summary>
        ///     List of Tags for grouping up Email Types
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Custom Mail Specific Data
        /// </summary>
        public object CustomVariables { get; set; }

        /// <summary>
        /// Custom Mail Specific Data
        /// </summary>
        public string ReplyTo { get; set; }


        public MailManager() { }


        public MailManager(string apiKey, string domain)
        {
            ApiKey = apiKey; Domain = domain;
        }

        public IRestResponse<MailSendResponse> SendMessage()
        {
            #region Validation Checks

            if (ApiKey == null) throw new ArgumentNullException("ApiKey", "Api key can not be empty");
            if (Domain == null) throw new ArgumentNullException("Domain", "Domain Name can not be empty");
            if (From == null || string.IsNullOrEmpty(From.MailAddress)) throw new ArgumentNullException("From", "Sender Name can not be empty");

            if (To == null || To.Count == 0 || To.Any(e => string.IsNullOrEmpty(e.MailAddress))) throw new ArgumentNullException("To", "At least one reciepient is necessary");
            if (Subject == null) throw new ArgumentNullException("Subject", "Emails Subject is necessary");
            if (Tags != null && Tags.Count > 3) throw new ArgumentOutOfRangeException("Tags", "Maximum 3 Tags are allowed");
            if (MailType == MailGun.MailType.Text && string.IsNullOrEmpty(MessageText)) throw new ArgumentNullException("MessageText", "Missing Message Body Text");
            if (MailType == MailGun.MailType.Html && string.IsNullOrEmpty(MessageHTML)) throw new ArgumentNullException("MessageHTML", "Missing Message Body Html");

            #endregion Validation Checks

            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");

            client.Authenticator = new HttpBasicAuthenticator("api", ApiKey);

            RestRequest request = new RestRequest();

            request.AddParameter("domain", Domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";


            request.AddParameter("from", From.MailContact);
            foreach (Contact c in To)
                request.AddParameter("to", c.MailContact);
            if (CC != null && CC.Count > 0)
                foreach (Contact c in CC)
                    request.AddParameter("cc", c.MailContact);
            if (BCC != null && BCC.Count > 0)
                foreach (Contact c in BCC)
                    request.AddParameter("bcc", c.MailContact);

            request.AddParameter("subject", Subject);

            if (MailType == MailGun.MailType.Html || MailType == MailGun.MailType.TextAndHtml)
                request.AddParameter("html", MessageHTML);
            if (MailType == MailGun.MailType.Text || MailType == MailGun.MailType.TextAndHtml)
                request.AddParameter("text", MessageText);

            if (Attachements != null && Attachements.Count > 0)
            {
                foreach (Attachment a in Attachements)
                {
                    if (!string.IsNullOrEmpty(a.Path))
                        request.AddFile("attachment", a.Path);
                    else if (a.ContentType == null)
                        request.AddFile("attachment", a.Contents, a.FileName);
                    else
                        request.AddFile("attachment", a.Contents, a.FileName, a.ContentType);
                }
            }
            if (Tags != null && Tags.Count > 0)
                foreach (string tag in Tags)
                    request.AddParameter("o:tag", tag);
            if (CustomVariables != null)
                request.AddParameter("v:my-custom-data", SimpleJson.SerializeObject(CustomVariables));

            if (!string.IsNullOrWhiteSpace(ReplyTo))
            {
                request.AddParameter("h:Reply-To", ReplyTo);
                //request.AddParameter("sender", ReplyTo);
                //request.AddParameter("X-Sender", ReplyTo);
            }

            request.AddParameter("h:List-Unsubscribe", "<https://secure.trackyourtruck.com/tytquote/Quote/Unsubscribe/100>");

            request.Method = Method.POST;
            return client.Execute<MailSendResponse>(request);
        }
    }
    public enum MailType
    {
        Text = 1,
        Html,
        TextAndHtml
    }

    public class Contact
    {
        public string Name { get; set; }
        public string MailAddress { get; set; }
        public string MailContact
        {
            get
            {
                return (Name != null ? Name + " <" : "") + MailAddress + (Name != null ? ">" : "");
            }
        }
    }

    public class Attachment
    {
        //public string name { get; set; }
        public string Path { get; set; }
        public byte[] Contents { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Attachment() { }
        public Attachment(string path)
        {
            Path = path;
        }
        public Attachment(byte[] contents, string fileName, string contentType = null)
        {
            Contents = contents;
            FileName = fileName;
            ContentType = contentType;
        }

    }

    public class MailSendResponse
    {
        public string message { get; set; }
        public string id { get; set; }
        public override string ToString()
        {
            return "MailGun ID : " + id + ". Message : " + message;
        }
    }

    public static class MailGunExtensionMetods
    {
        public static string getAllMailContacts(this List<Contact> contactList)
        {
            return contactList == null ? "" : String.Join(";", contactList.Select(e => e.MailContact).ToArray<string>());
        }
    }
}
