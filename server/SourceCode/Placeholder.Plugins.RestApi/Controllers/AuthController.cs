using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using sdk = Placeholder.SDK.Models;
using dm = Placeholder.Domain;
using Placeholder.SDK.Models.Requests;
using Placeholder.Web.Controllers;
using Zero.Foundation;
using ActionResult = Placeholder.SDK.ActionResult;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Common;
using Placeholder.SDK.Models.Responses;
using Placeholder.Primary;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Placeholder.SDK;

namespace Placeholder.Plugins.RestAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    public partial class AuthController : HealthPlaceholderApiController
    {
        public AuthController(IFoundation foundation)
            : base(foundation, "Auth")
        {
        }


        [HttpPost("login")]
        public Task<IActionResult> Login(AuthLoginInput input)
        {
            return base.ExecuteFunctionAsync("Login", async delegate ()
            {
                dm.Account account = this.API.Direct.Accounts.GetForValidPassword(input.user, input.password);
                if (account == null)
                {
                    return Http200(new ActionResult()
                    {
                        success = false,
                        message = this.Localize(new LocalizableString(LocalizableString.SERVER, "auth.invalidUserPass", "Invalid password/user combination"))
                    });
                }

                AccountInfo data = account.ToInfoModel();
#pragma warning disable 0618
                List<sdk.ShopAccount> shops = await this.API.Store.ShopAccounts.GetForAccountAsync(data.account_id, true);
                data.shops = shops.ToArray();
#pragma warning restore 0618

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, data.account_id.ToString()),
                    new Claim(nameof(data.account_display), string.Format("{0}", data.account_display))
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(16),
                    IsPersistent = true,
                };

                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                ItemResult<AccountInfo> result = new ItemResult<AccountInfo>()
                {
                    item = data,
                    success = true
                };
                return base.Http200(result);
            });
        }

        [HttpPost("logout")]
        public Task<IActionResult> Logout()
        {
            return base.ExecuteFunctionAsync("Logout", async delegate ()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return base.Http200(new ActionResult() { success = true });
            });
        }


        [HttpPost("password_reset/start")]
        public IActionResult PasswordResetStart(PasswordResetInput input)
        {
            return base.ExecuteFunction<IActionResult>("PasswordResetStart", delegate ()
            {
                dm.Account account = this.API.Direct.Accounts.GetByEmail(input.email);
                if (account != null)
                {
                    this.API.Direct.Accounts.PasswordResetStart(account.account_id);
                }

                return base.Http200(new ActionResult() { success = true });
            });
        }

        [HttpPost("password_reset/complete")]
        public IActionResult PasswordResetComplete(PasswordResetInput input)
        {
            return base.ExecuteFunction<IActionResult>("PasswordResetComplete", delegate ()
            {
                bool success = false;
                dm.Account account = this.API.Direct.Accounts.GetByEmail(input.email);
                if (account != null)
                {
                    success = this.API.Direct.Accounts.PasswordResetComplete(account.account_id, input.token, input.password);
                }

                return base.Http200(new ActionResult() { success = success });
            });
        }


        [HttpGet("timezones/all")]
        public IActionResult GetTimeZonesALL()
        {
            return base.ExecuteFunction<IActionResult>("GetTimeZonesALL", delegate ()
            {
                ListResult<sdk.IDPair> result = new ListResult<sdk.IDPair>();
                result.success = true;
                result.items =this.API.Direct.GlobalSettings.TimeZonesGetAll().Values.OrderBy(x => x.BaseUtcOffset).ThenBy(x => x.DisplayName).Select(x => new sdk.IDPair()
                {
                    id = x.StandardName,
                    name = x.DisplayName
                }).ToList();

                return base.Http200(result);
            });
        }

        [HttpGet("timezones/usa")]
        public IActionResult GetTimeZonesUSA()
        {
            return base.ExecuteFunction<IActionResult>("GetTimeZonesUSA", delegate ()
            {
                ListResult<sdk.IDPair> result = new ListResult<sdk.IDPair>();
                result.success = true;
                result.items = this.API.Direct.GlobalSettings.TimeZonesGetUsa().Values.OrderBy(x => x.BaseUtcOffset).ThenBy(x => x.DisplayName).Select(x => new sdk.IDPair()
                {
                    id = x.StandardName,
                    name = x.DisplayName
                }).ToList();

                return base.Http200(result);
            });
        }
    }
}
