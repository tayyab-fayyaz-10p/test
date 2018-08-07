using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SSH.Common.Helper
{
    public class EmailHelper
    {
        public static void SendEmail(EmailConfiguration configuration)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            foreach (string address in configuration.ToAddress.Split(';'))
            {
                if (!string.IsNullOrEmpty(address))
                {
                    mail.To.Add(address);
                }
            }

            mail.Subject = configuration.EmailSubject;
            mail.Body = configuration.EmailBody;
            mail.IsBodyHtml = true;
            smtpClient.ServicePoint.MaxIdleTime = 2;
            smtpClient.Send(mail);
        }

        public static async Task SendGridEmail(EmailConfiguration configuration)
        {
            var toList = new List<EmailBodyObject.EmailAddress>();
            foreach (string address in configuration.ToAddress.Split(';'))
            {
                if (!string.IsNullOrEmpty(address))
                {
                    toList.Add(new EmailBodyObject.EmailAddress { Email = address });
                }
            }

            var emailBodyObject = new EmailBodyObject
            {
                Personalizations = new List<EmailBodyObject.ToEmail>
                {
                    new EmailBodyObject.ToEmail { To = toList }
                },
                From = new EmailBodyObject.EmailAddress { Email = configuration.FromAddress },
                Subject = configuration.EmailSubject,
                Content = new List<EmailBodyObject.EmailContent>
                {
                    new EmailBodyObject.EmailContent
                    {
                        Type = "text/html",
                        Value = configuration.EmailBody
                    }
                }
            };

            var jsonObject = JSONHelper.ConvertObjectToJSON(emailBodyObject);
            var sendEmail = await HttpRequestHelper.Execute("POST", configuration.Api, jsonObject, configuration.Token);
        }
    }

    public class EmailBodyObject
    {
        public List<ToEmail> Personalizations { get; set; }

        public EmailAddress From { get; set; }

        public string Subject { get; set; }

        public List<EmailContent> Content { get; set; }

        public class ToEmail
        {
            public List<EmailAddress> To { get; set; }
        }

        public class EmailAddress
        {
            public string Email { get; set; }
        }

        public class EmailContent
        {
            public string Type { get; set; }

            public string Value { get; set; }
        }
    }


    public class EmailConfiguration
    {
        public int Id { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        public int RetryCount { get; set; }

        public string Exception { get; set; }

        public bool IsEmailSend { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Token { get; set; }

        public string Api { get; set; }
    }
}
