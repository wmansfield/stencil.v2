using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Placeholder.Common;
using Placeholder.Domain;
using Placeholder.Primary.Integration;
using Placeholder.Primary.Business.Synchronization;
using Placeholder.SDK;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;

namespace Placeholder.Primary.Emailing
{
    public class QueuedEmailer : ChokeableClass, IEmailer
    {
        public QueuedEmailer(IFoundation foundation)
            : base(foundation)
        {
            this.API = new PlaceholderAPI(foundation);
            this.Cache5 = new AspectCache("QueuedEmailer", this.IFoundation, new ExpireStaticLifetimeManager("QueuedEmailer.Cache", TimeSpan.FromMinutes(5), false));
        }

        public virtual IFoundation Foundation
        {
            get
            {
                return base.IFoundation;
            }
        }

        public virtual AspectCache Cache5 { get; set; }
        public virtual PlaceholderAPI API { get; set; }



        public virtual void SendAdminEmail(string subject, string message)
        {
            base.ExecuteMethod("SendAdminEmail", delegate ()
            {
                try
                {
                    string recipientEmail = this.API.Direct.GlobalSettings.GetValueOrDefaultCached("EmailTarget_Admin", "wmansfield@foundationzero.com");
                    string fromEmail = this.API.Direct.GlobalSettings.GetValueOrDefaultCached("EmailFromEmail_System", "no-reply@foundationzero.com");
                    string fromName = this.API.Direct.GlobalSettings.GetValueOrDefaultCached("EmailFromName_System", "Placeholder");


                    this.SendEmail(PrimaryAssumptions.ADMIN_SHOP_ID, "admin", null, null, recipientEmail, string.Empty, fromEmail, string.Empty, string.Empty, subject, message, null);
                }
                catch (Exception ex)
                {
                    base.IFoundation.LogError(ex, "SendAdminEmail");
                }
            });
        }

        public ActionResult SendEmailTemplate(Guid shop_id, EmailTemplateKind template, string purpose, string purpose_identifier, Guid? account_id_target, string recipientEmail, Dictionary<string, string> tokenValues, string[] ccRecipients = null)
        {
            return base.ExecuteFunction("SendEmailTemplate", delegate ()
            {
                try
                {
                    string subject = this.API.Direct.GlobalSettings.GetValueOrDefaultCached(string.Format(CommonAssumptions.EMAILTEMPLATE_GLOBAL_SUBJECT_FORMAT, template.ToString().ToLower()), string.Empty);
                    string body = this.API.Direct.GlobalSettings.GetValueOrDefaultCached(string.Format(CommonAssumptions.EMAILTEMPLATE_GLOBAL_BODY_FORMAT, template.ToString().ToLower()), string.Empty);
                    string fromName = this.API.Direct.GlobalSettings.GetValueOrDefaultCached(CommonAssumptions.EMAILTEMPLATE_GLOBAL_FROM_NAME, "Placeholder");
                    string fromEmail = this.API.Direct.GlobalSettings.GetValueOrDefaultCached(CommonAssumptions.EMAILTEMPLATE_GLOBAL_FROM_EMAIL, "placeholder@foundationzero.com");


                    if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
                    {
                        this.SendAdminEmail("Flawed Configuration", "No Email Subject, Body Template found for: " + template.ToString());
                        return new ActionResult() 
                        { 
                            success = true,
                            message = "Missing template"
                        };
                    }
                   
                    if (string.IsNullOrWhiteSpace(purpose))
                    {
                        purpose = "unknown";
                    }
                    if (string.IsNullOrWhiteSpace(purpose_identifier))
                    {
                        purpose_identifier = Guid.NewGuid().ToString("N");
                    }

                    /*
                    //TODO:SHOULD:Email Queue Support
                    Email email = new Email()
                    {
                        email_id = Guid.NewGuid(),
                        shop_id = shop_id,
                        account_id_target = account_id_target,
                        purpose_identifier = purpose_identifier,
                        purpose = purpose,
                        dependency = Dependency.none,
                        dependency_identifier = null,
                        should_send = true,
                        send_attempts = 0,
                        target_delivery_utc = null,
                        delivery_mode = DeliveryMode.immediate,
                        from_email = fromEmail,
                        from_name = fromName,
                        to_email = recipientEmail,
                        to_name = string.Empty,
                        cc_list = string.Empty,
                        body_html = this.ProcessTemplate(body, recipientEmail, tokenValues),
                        subject = this.ProcessTemplate(subject, recipientEmail, tokenValues),
                        created_utc = DateTime.UtcNow,
                        updated_utc = DateTime.UtcNow,
                        transport_log = string.Empty,
                    };

                    if(ccRecipients != null)
                    {
                        email.cc_list = string.Join(";", ccRecipients);
                    }

                    this.API.Direct.Emails.Insert(email, Availability.Retrievable);
                    */
                    this.API.Integration.Synchronization.AgitateDaemon(shop_id, CommonAssumptions.DAEMON_EMAIL_FORMAT);

                    return new ActionResult() { success = true };
                }
                catch (Exception ex)
                {
                    return new ActionResult()
                    {
                        success = true,
                        message = ex.FirstNonAggregateException().Message
                    };
                }
            });
        }

        public ActionResult SendEmail(Guid shop_id, string purpose, string purpose_identifier, Guid? account_id_target, string toEmail, string toName, string fromEmail, string fromName, string ccList, string subjectTemplate, string bodyHtmlTemplate, Dictionary<string, string> tokenValues, DateTime? targetDeliveryDate = null, string watermark = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(purpose) && !string.IsNullOrWhiteSpace(purpose_identifier))
                {
                    /*
                    //TODO:SHOULD:Email Queue Support
                    Email existingEmail = this.API.Direct.Emails.GetByPurpose(shop_id, purpose, purpose_identifier);
                    if (existingEmail != null)
                    {
                        // already sent it
                        return new ActionResult() { success = true };
                    }
                    */
                }

                if (string.IsNullOrWhiteSpace(purpose))
                {
                    purpose = "unknown";
                }
                if (string.IsNullOrWhiteSpace(purpose_identifier))
                {
                    purpose_identifier = Guid.NewGuid().ToString("N");
                }

                /*
                //TODO:SHOULD:Email Queue Support
                Email email = new Email()
                {
                    email_id = Guid.NewGuid(),
                    shop_id = shop_id,
                    test_id = test_id_target,
                    account_id_target = account_id_target,
                    purpose_identifier = purpose_identifier,
                    purpose = purpose,
                    dependency = Dependency.none,
                    dependency_identifier = null,
                    should_send = true,
                    send_attempts = 0,
                    target_delivery_utc = targetDeliveryDate,
                    delivery_mode = targetDeliveryDate.HasValue ? DeliveryMode.scheduled : DeliveryMode.immediate,
                    from_email = fromEmail,
                    from_name = fromName,
                    to_email = toEmail,
                    to_name = toName,
                    cc_list = ccList,
                    body_html = this.ProcessTemplate(bodyHtmlTemplate, toEmail, tokenValues),
                    subject = this.ProcessTemplate(subjectTemplate, toEmail, tokenValues),
                    created_utc = DateTime.UtcNow,
                    updated_utc = DateTime.UtcNow,
                    transport_log = string.Empty,
                };

                this.API.Direct.Emails.Insert(email, Availability.Retrievable);
                */
                this.API.Integration.Synchronization.AgitateDaemon(shop_id, CommonAssumptions.DAEMON_EMAIL_FORMAT);

                return new ActionResult() { success = true };
            }
            catch (Exception ex)
            {
                return new ActionResult() 
                {
                    success = true,
                    message = ex.FirstNonAggregateException().Message
                };
            }
        }

        protected virtual string ProcessTemplate(string template, string recipientEmail, Dictionary<string, string> tokenValues)
        {
            return base.ExecuteFunction("ProcessTemplate", delegate ()
            {
                if(string.IsNullOrWhiteSpace(template))
                {
                    return string.Empty;
                }
                if (tokenValues != null)
                {
                    foreach (var item in tokenValues)
                    {
                        template = template.Replace("%" + item.Key.ToLower() + "%", item.Value);
                    }
                }
                // always after template
                template = template.Replace("%recipient%", recipientEmail);

                // remove any missing
                template = Regex.Replace(template, @"%[a-zA-Z_-]*?%", "", RegexOptions.IgnoreCase);

                return template;
            });
        }


    }
}
