using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;

namespace MRE.Presistence.IProvider
{
    public interface IEmailProvider
    {
        void SendEmailHtmlToAdmin(string subject, string htmlMessage);
        void SendEmailHtml(string email, string subject, string htmlMessage
            , List<Attachment> attachments);
        void sendEmail(string subject, string templateName, IDictionary<string, string> substitutionParams,
          List<EmailAddress> recipientEmails, List<Attachment> attachments, EmailAddress sender);

        void sendEmailBySystem(string subject, string templateName, IDictionary<string, string> substitutionParams,
           List<EmailAddress> recipientEmails, List<Attachment> attachments);

        void sendEmailToAdmin(string subject, string templateName, IDictionary<string, string> dynamicParams,
            List<Attachment> attachments);
    }
}
