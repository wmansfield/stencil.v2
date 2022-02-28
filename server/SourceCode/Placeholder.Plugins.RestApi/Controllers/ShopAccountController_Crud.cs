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
    [Route("api/shopaccounts")]
    public partial class ShopAccountController : HealthPlaceholderApiController
    {
        public ShopAccountController(IFoundation foundation)
            : base(foundation, "ShopAccount")
        {
        }

        [HttpGet("{shop_id}/{shop_account_id}")]
        public Task<IActionResult> GetById(Guid shop_id, Guid shop_account_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.ShopAccount result = await this.API.Store.ShopAccounts.GetDocumentAsync(shop_id, shop_account_id);
                
                if (result == null)
                {
                    return base.Http404("ShopAccount");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.ShopAccount>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        
        [HttpGet("for_shop/{shop_id}")]
        public Task<IActionResult> FindForShop(Guid shop_id, int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "", bool? enabled = true)
        {
            return base.ExecuteFunctionAsync<IActionResult>("FindForShop", async delegate()
            {
                Guid? shop_id_security = shop_id;
                this.Security.ValidateCanSearchShopAccount(this.GetCurrentAccount(), shop_id_security);
                
                ListResult<sdk.ShopAccount> result = await this.API.Store.ShopAccounts.FindForShopAsync(shop_id, skip, take, order_by, descending, keyword, enabled);
                
                result.success = true;
                return base.Http200(result);
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.ShopAccount shopaccount)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(shopaccount, "ShopAccount");

                dm.ShopAccount insert = shopaccount.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.ShopAccounts.Insert(insert);
                
                sdk.ShopAccount insertResult = await this.API.Store.ShopAccounts.GetDocumentAsync(insert.shop_id, insert.shop_account_id);

                return base.Http200(new ItemResult<sdk.ShopAccount>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{shop_account_id}")]
        public Task<IActionResult> Update(Guid shop_account_id, sdk.ShopAccount shopaccount)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(shopaccount, "ShopAccount");
                this.ValidateRouteMatch(shop_account_id, shopaccount.shop_account_id, "ShopAccount");

                dm.ShopAccount found = this.API.Direct.ShopAccounts.GetById(shop_account_id);
                this.ValidateNotNull(found, "ShopAccount");

                shopaccount.shop_account_id = shop_account_id;
                dm.ShopAccount update = shopaccount.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.ShopAccounts.Update(update);
                
                sdk.ShopAccount existing = await this.API.Store.ShopAccounts.GetDocumentAsync(update.shop_id, update.shop_account_id);
                
                return base.Http200(new ItemResult<sdk.ShopAccount>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{shop_account_id}")]
        public IActionResult Delete(Guid shop_account_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.ShopAccount delete = this.API.Direct.ShopAccounts.GetById(shop_account_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.ShopAccounts.Delete(shop_account_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = shop_account_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

