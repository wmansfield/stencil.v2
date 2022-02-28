using System;
using Placeholder.Domain;
using Placeholder.Primary;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;

namespace Placeholder.Web.Security
{
    public class PlaceholderHashedTimeSignatureAuthorizer : ChokeableClass
    {
        public PlaceholderHashedTimeSignatureAuthorizer(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.TimedCache2 = new AspectCache("PlaceholderHashedTimeSignatureAuthorizer.TimedCache", iFoundation, new ExpireStaticLifetimeManager("PlaceholderHashedTimeSignatureAuthorizer.TimedCache.Static", TimeSpan.FromMinutes(2), false));
            this.API = iFoundation.Resolve<PlaceholderAPI>();
        }

        public virtual PlaceholderAPI API { get; set; }
        public virtual AspectCache TimedCache2 { get; set; }

        public virtual Account AuthorizeAccount(string api_key, string signature)
        {
            return base.ExecuteFunction(nameof(AuthorizeAccount), delegate ()
            {
                Account result = null;
                if (!string.IsNullOrEmpty(api_key))
                {
                    try
                    {
                        bool fromCache = true;
                        Account account = this.TimedCache2.PerLifetime("acc:" + api_key, delegate ()
                        {
                            fromCache = false;
                            return this.API.Direct.Accounts.GetByApiKey(api_key);
                        });

                        if (fromCache && (account != null) && (account.account_status != AccountStatus.enabled))
                        {
                            // try to get non-cached for enabled toggle
                            account = this.API.Direct.Accounts.GetByApiKey(api_key);
                        }

                        if ((account != null) && account.account_status == AccountStatus.enabled)
                        {
                            if (HashedTimeSignatureVerifier.ValidateSignature(account.api_key, account.api_secret, signature))
                            {
                                result = account;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.IFoundation.LogError(ex, "AuthorizeAccount");
                    }
                }
                return result;

            });
        }
    }
}
