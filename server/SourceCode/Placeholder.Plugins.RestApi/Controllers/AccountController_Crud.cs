using Zero.Foundation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActionResult = Placeholder.SDK.ActionResult;
using sdk = Placeholder.SDK.Models;
using dm = Placeholder.Domain;
using Placeholder.Primary;
using Placeholder.SDK;
using Placeholder.SDK.Client;
using Placeholder.Web.Controllers;

namespace Placeholder.Plugins.RestAPI.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    public partial class AccountController : HealthPlaceholderApiController
    {
        public AccountController(IFoundation foundation)
            : base(foundation, "Account")
        {
        }

        [HttpGet("{account_id}")]
        public Task<IActionResult> GetById(Guid account_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.Account result = await this.API.Store.Accounts.GetDocumentAsync(account_id);
                
                if (result == null)
                {
                    return base.Http404("Account");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.Account>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        [HttpGet("")]
        public Task<IActionResult> Find(int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunctionAsync<IActionResult>("Find", async delegate()
            {
                Guid? shop_id_security = null;
                this.Security.ValidateCanSearchAccount(this.GetCurrentAccount(), shop_id_security);
                
                ListResult<sdk.Account> result = await this.API.Store.Accounts.FindAsync(skip, take, order_by, descending, keyword);
                
                result.success = true;
                return base.Http200(result);
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.Account account)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(account, "Account");

                dm.Account insert = account.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.Accounts.Insert(insert);
                
                sdk.Account insertResult = await this.API.Store.Accounts.GetDocumentAsync(insert.account_id);

                return base.Http200(new ItemResult<sdk.Account>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{account_id}")]
        public Task<IActionResult> Update(Guid account_id, sdk.Account account)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(account, "Account");
                this.ValidateRouteMatch(account_id, account.account_id, "Account");

                dm.Account found = this.API.Direct.Accounts.GetById(account_id);
                this.ValidateNotNull(found, "Account");

                account.account_id = account_id;
                dm.Account update = account.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.Accounts.Update(update);
                
                sdk.Account existing = await this.API.Store.Accounts.GetDocumentAsync(update.account_id);
                
                return base.Http200(new ItemResult<sdk.Account>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{account_id}")]
        public IActionResult Delete(Guid account_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Account delete = this.API.Direct.Accounts.GetById(account_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.Accounts.Delete(account_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = account_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

