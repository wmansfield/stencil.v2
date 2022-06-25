using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zero.Foundation;
using Placeholder.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Placeholder.Primary.Workers;
using Placeholder.Primary;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Web.Security
{
    public class ApiAccountAuthenticationHandler : AuthenticationHandler<ApiAccountAuthenticationSchemeOptions>
    {
        public ApiAccountAuthenticationHandler(IOptionsMonitor<ApiAccountAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IFoundation foundation)
            : base(options, logger, encoder, clock)
        {
            this.IFoundation = foundation;
        }

        public const string NAME_PREFIX = "a:";
        public const string SCHEME = "ApiAccount";

        public IFoundation IFoundation { get; set; }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                return Task.FromResult(this.ValidateRequest());
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex.Message));
            }
        }

        protected AuthenticateResult ValidateRequest()
        {
            string key = null;
            string signature = null;
            string shop = null;
            Guid? shop_id = null;

            if(this.Request?.Headers != null)
            {
                if(this.Request.Headers.ContainsKey(SecurityAssumptions.API_PARAM_KEY))
                {
                    key = this.Request.Headers[SecurityAssumptions.API_PARAM_KEY];
                }
                if(this.Request.Headers.ContainsKey(SecurityAssumptions.API_PARAM_KEY))
                {
                    signature = this.Request.Headers[SecurityAssumptions.API_PARAM_SIGNATURE];
                }
                if (this.Request.Headers.ContainsKey(SecurityAssumptions.PARAM_SHOP))
                {
                    shop = this.Request.Headers[SecurityAssumptions.PARAM_SHOP];
                }
            }

            if(string.IsNullOrWhiteSpace(key))
            {
                if(this.Request?.Query != null)
                {
                    if(this.Request.Query.ContainsKey(SecurityAssumptions.API_PARAM_KEY))
                    {
                        key = this.Request.Query[SecurityAssumptions.API_PARAM_KEY];
                    }
                    if(this.Request.Query.ContainsKey(SecurityAssumptions.API_PARAM_KEY))
                    {
                        signature = this.Request.Query[SecurityAssumptions.API_PARAM_SIGNATURE];
                    }
                    if (this.Request.Query.ContainsKey(SecurityAssumptions.PARAM_SHOP))
                    {
                        shop = this.Request.Headers[SecurityAssumptions.PARAM_SHOP];
                    }
                }
            }

            if(!string.IsNullOrWhiteSpace(shop))
            {
                if(Guid.TryParse(shop, out Guid parsed))
                {
                    shop_id = parsed;
                }
            }

            if(string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(signature))
            {
                return AuthenticateResult.NoResult();
            }

            PlaceholderHashedTimeSignatureAuthorizer authorizer = this.IFoundation.Resolve<PlaceholderHashedTimeSignatureAuthorizer>();
            Account account = authorizer.AuthorizeAccount(key, signature);

            if (account == null)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, NAME_PREFIX + account.account_id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ApiPrincipal principal = new ApiPrincipal(identity, account.account_id, string.Format("{0}", account.account_display), false);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

            string platform = this.GetPlatform(account);
            string language = this.GetLanguage(account, shop_id);

            if(this.Request.HttpContext != null)
            {
                if(this.Request?.HttpContext.Items != null)
                {
                    this.Request.HttpContext.Items[SecurityAssumptions.CURRENT_ACCOUNT_HTTP_CONTEXT_KEY] = account;
                    this.Request.HttpContext.Items[SecurityAssumptions.CURRENT_LANGUAGE_HTTP_CONTEXT_KEY] = language;
                }
            }

            AccountLoggedInWorker.EnqueueRequest(this.IFoundation, new LoggedInRequest()
            {
                account_id = account.account_id,
                platform = platform,
                login_utc = DateTime.UtcNow
            });

            return AuthenticateResult.Success(ticket);
        }

        protected string GetPlatform(Account account)
        {
            string result = string.Empty;
            try
            {
                if (this.Request.Headers.ContainsKey(SecurityAssumptions.PARAM_PLATFORM))
                {
                    string value = this.Request.Headers.GetCommaSeparatedValues(SecurityAssumptions.PARAM_PLATFORM).FirstOrDefault();
                    if (!string.IsNullOrEmpty(value))
                    {
                        result += value;
                    }
                }
                if (this.Request.Headers.ContainsKey(SecurityAssumptions.PARAM_VERSION))
                {
                    string value = this.Request.Headers.GetCommaSeparatedValues(SecurityAssumptions.PARAM_VERSION).FirstOrDefault();
                    if (!string.IsNullOrEmpty(value))
                    {
                        result += " - v" + value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.IFoundation.LogError(ex, "GetPlatform");
            }
            return result;
        }

        protected string GetLanguage(Account account, Guid? shop_id)
        {
            string result = string.Empty;
            try
            {
                // TRY HEADERS
                try
                {
                    if (this.Request.Headers.ContainsKey("accept-language"))
                    {
                        string value = this.Request.Headers.GetCommaSeparatedValues("accept-language").FirstOrDefault();
                        if (!string.IsNullOrEmpty(value))
                        {
                            result = value;
                        }
                    }
                    if (this.Request.Headers.ContainsKey(SecurityAssumptions.PARAM_LANGUAGE))
                    {
                        string value = this.Request.Headers.GetCommaSeparatedValues(SecurityAssumptions.PARAM_LANGUAGE).FirstOrDefault();
                        if (!string.IsNullOrEmpty(value))
                        {
                            result = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    CoreFoundation.Current.LogError(ex, "GetLanguage.Request");
                }

                string selectedLanguage = string.Empty;
                // see if it's a language we understand
                if (!string.IsNullOrEmpty(result) && shop_id.HasValue)
                {
                    /*
                    //TODO:SHOULD: Add language support and resolve here
                    PlaceholderAPI API = this.IFoundation.Resolve<PlaceholderAPI>();
                    sdk.Language match = API.Store.Languages.GetBestMatchCached(shop_id.Value, result);
                    if (match != null)
                    {
                        selectedLanguage = match.iso_long;
                    }
                    */
                }

                if (string.IsNullOrEmpty(selectedLanguage))
                {
                    result = "en-US";
                }

            }
            catch (Exception ex)
            {
                this.IFoundation.LogError(ex, "GetLanguage");
            }

            return result;
        }
    
    }
    
}
