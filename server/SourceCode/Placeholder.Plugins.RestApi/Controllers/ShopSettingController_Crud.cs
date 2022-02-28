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
    [Route("api/shopsettings")]
    public partial class ShopSettingController : HealthPlaceholderApiController
    {
        public ShopSettingController(IFoundation foundation)
            : base(foundation, "ShopSetting")
        {
        }

        [HttpGet("{shop_id}/{shop_setting_id}")]
        public Task<IActionResult> GetById(Guid shop_id, Guid shop_setting_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.ShopSetting result = await this.API.Store.ShopSettings.GetDocumentAsync(shop_id, shop_setting_id);
                
                if (result == null)
                {
                    return base.Http404("ShopSetting");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.ShopSetting>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        
        [HttpGet("for_shop/{shop_id}")]
        public Task<IActionResult> FindForShop(Guid shop_id, int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunctionAsync<IActionResult>("FindForShop", async delegate()
            {
                Guid? shop_id_security = shop_id;
                this.Security.ValidateCanSearchShopSetting(this.GetCurrentAccount(), shop_id_security);
                
                ListResult<sdk.ShopSetting> result = await this.API.Store.ShopSettings.FindForShopAsync(shop_id, skip, take, order_by, descending, keyword);
                
                result.success = true;
                return base.Http200(result);
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.ShopSetting shopsetting)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(shopsetting, "ShopSetting");

                dm.ShopSetting insert = shopsetting.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.ShopSettings.Insert(insert);
                
                sdk.ShopSetting insertResult = await this.API.Store.ShopSettings.GetDocumentAsync(insert.shop_id, insert.shop_setting_id);

                return base.Http200(new ItemResult<sdk.ShopSetting>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{shop_setting_id}")]
        public Task<IActionResult> Update(Guid shop_setting_id, sdk.ShopSetting shopsetting)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(shopsetting, "ShopSetting");
                this.ValidateRouteMatch(shop_setting_id, shopsetting.shop_setting_id, "ShopSetting");

                dm.ShopSetting found = this.API.Direct.ShopSettings.GetById(shopsetting.shop_id, shop_setting_id);
                this.ValidateNotNull(found, "ShopSetting");

                shopsetting.shop_setting_id = shop_setting_id;
                dm.ShopSetting update = shopsetting.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.ShopSettings.Update(update);
                
                sdk.ShopSetting existing = await this.API.Store.ShopSettings.GetDocumentAsync(update.shop_id, update.shop_setting_id);
                
                return base.Http200(new ItemResult<sdk.ShopSetting>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{shop_id}/{shop_setting_id}")]
        public IActionResult Delete(Guid shop_id, Guid shop_setting_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.ShopSetting delete = this.API.Direct.ShopSettings.GetById(shop_id, shop_setting_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.ShopSettings.Delete(shop_id, shop_setting_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = shop_setting_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

