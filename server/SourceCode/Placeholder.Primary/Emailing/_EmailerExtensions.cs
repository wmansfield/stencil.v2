using System;
using System.Collections.Generic;
using Placeholder.Domain;
using Placeholder.Primary.Integration;
using Zero.Foundation;

namespace Placeholder.Primary.Emailing
{
    public static class _EmailerExtensions
    {
        public static void SendAccountRequestPasswordCompleted(this IEmailer emailer, Guid shop_id, Guid account_id, string recipientEmail, string recipientName)
        {
            try
            {
                if (emailer == null) { return; }

                Dictionary<string, string> values = new Dictionary<string, string>();
                values["account_id"] = account_id.ToString();
                values["recipient_name"] = recipientName;
                values["recipient_email"] = recipientEmail;
                emailer.SendEmailTemplate(shop_id, EmailTemplateKind.password_reset_completed, "PasswordResetCompleted", null, account_id, recipientEmail, values);
            }
            catch (Exception ex)
            {
                emailer.Foundation.LogError(ex, "SendAccountRequestPasswordCompleted");
            }
        }
        public static void SendAccountPasswordResetInitiated(this IEmailer emailer, Guid shop_id, Guid account_id, string recipientEmail, string recipientName, string resetToken)
        {
            try
            {
                if (emailer == null) { return; }

                Dictionary<string, string> values = new Dictionary<string, string>();
                values["token"] = resetToken;
                values["account_id"] = account_id.ToString();
                values["recipient_name"] = recipientName;
                values["recipient_email"] = recipientEmail;
                emailer.SendEmailTemplate(shop_id, EmailTemplateKind.password_reset_started, "PasswordResetInitiated", null, account_id, recipientEmail, values);
            }
            catch (Exception ex)
            {
                emailer.Foundation.LogError(ex, "SendAccountPasswordResetInitiated");
            }
        }

        public static void SendEmailChanged(this IEmailer emailer, Guid shop_id, Guid? account_id, string recipientEmail, string recipientName)
        {
            try
            {
                if (emailer == null) { return; }

                Dictionary<string, string> values = new Dictionary<string, string>();
                values["recipient_name"] = recipientName;
                values["recipient_email"] = recipientEmail;
                emailer.SendEmailTemplate(shop_id, EmailTemplateKind.email_changed, "EmailChanged", recipientEmail, account_id, recipientEmail, values);
            }
            catch (Exception ex)
            {
                emailer.Foundation.LogError(ex, "SendEmailChanged");
            }
        }

        public static void SendEmailVerify(this IEmailer emailer, Guid shop_id, Guid? account_id, string recipientEmail, string recipientName, string resetToken)
        {
            try
            {
                if (emailer == null) { return; }

                Dictionary<string, string> values = new Dictionary<string, string>();
                values["token"] = resetToken;
                values["recipient_name"] = recipientName;
                values["recipient_email"] = recipientEmail;
                emailer.SendEmailTemplate(shop_id, EmailTemplateKind.email_verify_started, "EmailVerifyInitiated", null, account_id, recipientEmail, values);
            }
            catch (Exception ex)
            {
                emailer.Foundation.LogError(ex, "SendEmailVerify");
            }
        }
    }
}
