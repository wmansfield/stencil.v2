using System;
using System.Collections.Generic;
using Placeholder.Domain;
using Placeholder.SDK;
using Zero.Foundation;

namespace Placeholder.Primary.Integration
{
    public interface IEmailer
    {
        IFoundation Foundation { get; } 
        
        void SendAdminEmail(string subject, string message);

        ActionResult SendEmailTemplate(Guid shop_id, EmailTemplateKind template, string purpose, string purpose_identifier, Guid? account_id_target, string recipientEmail, Dictionary<string, string> tokenValues, string[] ccRecipients = null);
        ActionResult SendEmail(Guid shop_id, string purpose, string purpose_identifier, Guid? account_id_target, string toEmail, string toName, string fromEmail, string fromName, string ccList, string subjectTemplate, string bodyHtmlTemplate, Dictionary<string, string> tokenValues, DateTime? targetDeliveryDate = null, string watermark = null);

    }
}
