using Microsoft.AspNetCore.Mvc;
using Placeholder.Domain;
using Placeholder.Primary;
using Placeholder.SDK;
using Placeholder.SDK.Models.Responses;
using Placeholder.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Plugins.RestAPI.Controllers
{
    public partial class AccountController
    {
        [HttpGet("self")]
        public Task<IActionResult> Self(bool persist = false)
        {
            return base.ExecuteFunctionAsync("Self", async delegate ()
            {
                Account currentAccount = this.GetCurrentAccount();

                sdk.Account account = await this.API.Store.Accounts.GetDocumentAsync(currentAccount.account_id);
                if(account == null)
                {
                    return Http401("Error locating resource.");
                }

                AccountInfo info = account.ToInfoModel();

#pragma warning disable 0618
                List<sdk.ShopAccount> shops = await this.API.Store.ShopAccounts.GetForAccountAsync(info.account_id, true);
                info.shops = shops.ToArray();
#pragma warning restore 0618

                if (persist)
                {
                    //TODO:Could: FormsAuthentication.SetAuthCookie(account.account_id.ToString(), true);
                }

                info.api_key = string.Empty; // security trim
                info.api_secret = string.Empty; // security trim

                ItemResult<AccountInfo> result = new ItemResult<AccountInfo>()
                {
                    success = true,
                    item = info
                };

                return Http200(result);
            });
        }
    }
}
