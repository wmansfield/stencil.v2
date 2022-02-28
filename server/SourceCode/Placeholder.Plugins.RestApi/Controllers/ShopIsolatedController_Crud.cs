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
    [Route("api/shopisolateds")]
    public partial class ShopIsolatedController : HealthPlaceholderApiController
    {
        public ShopIsolatedController(IFoundation foundation)
            : base(foundation, "ShopIsolated")
        {
        }

        [HttpGet("{shop_id}")]
        public Task<IActionResult> GetById(Guid shop_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.ShopIsolated result = await this.API.Store.ShopIsolateds.GetDocumentAsync(shop_id);
                
                if (result == null)
                {
                    return base.Http404("ShopIsolated");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.ShopIsolated>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.ShopIsolated shopisolated)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(shopisolated, "ShopIsolated");

                dm.ShopIsolated insert = shopisolated.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.ShopIsolateds.Insert(insert);
                
                sdk.ShopIsolated insertResult = await this.API.Store.ShopIsolateds.GetDocumentAsync(insert.shop_id);

                return base.Http200(new ItemResult<sdk.ShopIsolated>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{shop_id}")]
        public Task<IActionResult> Update(Guid shop_id, sdk.ShopIsolated shopisolated)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(shopisolated, "ShopIsolated");
                this.ValidateRouteMatch(shop_id, shopisolated.shop_id, "ShopIsolated");

                dm.ShopIsolated found = this.API.Direct.ShopIsolateds.GetById(shop_id);
                this.ValidateNotNull(found, "ShopIsolated");

                shopisolated.shop_id = shop_id;
                dm.ShopIsolated update = shopisolated.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.ShopIsolateds.Update(update);
                
                sdk.ShopIsolated existing = await this.API.Store.ShopIsolateds.GetDocumentAsync(update.shop_id);
                
                return base.Http200(new ItemResult<sdk.ShopIsolated>()
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
                dm.ShopIsolated delete = this.API.Direct.ShopIsolateds.GetById(shop_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.ShopIsolateds.Delete(shop_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = shop_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

