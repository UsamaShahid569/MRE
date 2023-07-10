using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MRE.Presistence.Context;
using MRE.Presistence.IProvider;
using Model.Models;

namespace MRE.Presistence.Providers
{
    public class EmailProvider : IEmailProvider
    {
        private readonly IOptions<ConfigModel> _optionsAccessor;
        

        public EmailProvider(IOptions<ConfigModel> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
            
        }

        public void sendEmailToAdmin(string subject, string templateName, IDictionary<string, string> dynamicParams,
             List<Attachment> attachments)
        {
            List<EmailAddress> recipientEmails = new List<EmailAddress>
            {
                new EmailAddress(_optionsAccessor.Value.AdminEmail,"Administrator")
            };
            this.Execute(subject, templateName, dynamicParams, recipientEmails, attachments);
        }

        public void sendEmail(string subject, string templateName, IDictionary<string, string> dynamicParams,
            List<EmailAddress> recipientEmails, List<Attachment> attachments, EmailAddress sender)
        {
            this.Execute(subject, templateName, dynamicParams, recipientEmails, attachments, sender);
        }


        public void sendEmailBySystem(string subject, string templateName, IDictionary<string, string> dynamicParams,
            List<EmailAddress> recipientEmails, List<Attachment> attachments)
        {
            this.SendEmailHtml(recipientEmails.First().Email, subject, "test", attachments);
            this.Execute(subject, templateName, dynamicParams, recipientEmails, attachments);
        }

        public void SendEmailHtmlToAdmin(string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(subject))
            {
                subject = "Contact Form Submitted.";
            }
            var client = new SendGridClient(_optionsAccessor.Value.SendGridKey);
            var from = new EmailAddress
            {
                Email = _optionsAccessor.Value.SendGridEmail,
                Name = _optionsAccessor.Value.SendGridName
            };
            var to = new EmailAddress(_optionsAccessor.Value.AdminEmail,"Administrator");
            var htmlContent = htmlMessage;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = client.SendEmailAsync(msg).Result;

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var error = response.Body.ReadAsStringAsync().Result;
                throw new Exception($"SendGrid error {response.StatusCode}: {error}");
            }

        }

        public void SendEmailHtml(string email, string subject, string htmlMessage
            , List<Attachment> attachments)
        {

            var client = new SendGridClient(_optionsAccessor.Value.SendGridKey);
            var from = new EmailAddress
            {
                Email = _optionsAccessor.Value.SendGridEmail,
                Name = _optionsAccessor.Value.SendGridName
            };
            var to = new EmailAddress(email, "");
            var htmlContent = htmlMessage;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            if (attachments != null)
            {
                msg.AddAttachments(attachments);
            }
            var response = client.SendEmailAsync(msg).Result;

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var error = response.Body.ReadAsStringAsync().Result;
                throw new Exception($"SendGrid error {response.StatusCode}: {error}");
            }
           
        }


        private void Execute(string subject, string templateName, IDictionary<string, string> dynamicParams
            , List<EmailAddress> recipientEmails, List<Attachment> attachments, EmailAddress sender = null)
        {
            if (sender == null)
            {
                sender = new EmailAddress
                {
                    Email = _optionsAccessor.Value.SendGridEmail,
                    Name = _optionsAccessor.Value.SendGridName
                };
            }

            var client = new SendGridClient(_optionsAccessor.Value.SendGridKey);

            var sendGridMessage = new SendGridMessage();
            sendGridMessage.SetSubject(subject);
            sendGridMessage.SetFrom(sender.Email, sender.Name);
            sendGridMessage.SetTemplateId(templateName);
            if (attachments != null)
            {
                sendGridMessage.AddAttachments(attachments);
            }
            sendGridMessage.Personalizations = new List<Personalization>
            {

                new Personalization()
                {
                    Subject =subject,
                    Tos = recipientEmails ,
                    TemplateData = dynamicParams,
                }
            };

            var response = client.SendEmailAsync(sendGridMessage).Result;

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var error = response.Body.ReadAsStringAsync().Result;
                throw new Exception($"SendGrid error {response.StatusCode}: {error}");
            }

        }
    }
}
