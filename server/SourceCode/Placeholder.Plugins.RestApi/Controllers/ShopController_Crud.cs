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
    [Route("api/shops")]
    public partial class ShopController : HealthPlaceholderApiController
    {
        public ShopController(IFoundation foundation)
            : base(foundation, "Shop")
        {
        }

        [HttpGet("{shop_id}")]
        public Task<IActionResult> GetById(Guid shop_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.Shop result = await this.API.Store.Shops.GetDocumentAsync(shop_id);
                
                if (result == null)
                {
                    return base.Http404("Shop");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.Shop>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        [HttpGet("")]
        public Task<IActionResult> Find(int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "", Guid? tenant_id = null)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Find", async delegate()
            {
                Guid? shop_id_security = null;
                this.Security.ValidateCanSearchShop(this.GetCurrentAccount(), shop_id_security);
                
                ListResult<sdk.Shop> result = await this.API.Store.Shops.FindAsync(skip, take, order_by, descending, keyword);
                
                result.success = true;
                return base.Http200(result);
            });
        }
        
        [HttpGet("by_tenant/{tenant_id}")]
        public IActionResult GetByTenant(Guid tenant_id, int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunction<IActionResult>("GetByTenant", delegate ()
            {
                

                Guid? shop_id_security = null;
                

                this.Security.ValidateCanListShop(this.GetCurrentAccount(), shop_id_security);
                ListResult<sdk.Shop> result = new ListResult<sdk.Shop>();
                result.items = this.API.Direct.Shops.GetByTenant(tenant_id).ToSDKModel();
                
                result.success = true;
                return base.Http200(result);
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.Shop shop)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(shop, "Shop");

                dm.Shop insert = shop.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.Shops.Insert(insert);
                
                sdk.Shop insertResult = await this.API.Store.Shops.GetDocumentAsync(insert.shop_id);

                return base.Http200(new ItemResult<sdk.Shop>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{shop_id}")]
        public Task<IActionResult> Update(Guid shop_id, sdk.Shop shop)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(shop, "Shop");
                this.ValidateRouteMatch(shop_id, shop.shop_id, "Shop");

                dm.Shop found = this.API.Direct.Shops.GetById(shop_id);
                this.ValidateNotNull(found, "Shop");

                shop.shop_id = shop_id;
                dm.Shop update = shop.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.Shops.Update(update);
                
                sdk.Shop existing = await this.API.Store.Shops.GetDocumentAsync(update.shop_id);
                
                return base.Http200(new ItemResult<sdk.Shop>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{shop_id}")]
        public IActionResult Delete(Guid shop_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Shop delete = this.API.Direct.Shops.GetById(shop_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.Shops.Delete(shop_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = shop_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

