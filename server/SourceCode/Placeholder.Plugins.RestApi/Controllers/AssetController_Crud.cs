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
    [Route("api/assets")]
    public partial class AssetController : HealthPlaceholderApiController
    {
        public AssetController(IFoundation foundation)
            : base(foundation, "Asset")
        {
        }

        [HttpGet("{asset_id}")]
        public IActionResult GetById(Guid asset_id)
        {
            return base.ExecuteFunction<IActionResult>("GetById", delegate()
            {
                dm.Asset result = this.API.Direct.Assets.GetById(asset_id);
                if (result == null)
                {
                    return Http404("Asset");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result);
                

                return base.Http200(new ItemResult<sdk.Asset>()
                {
                    success = true,
                    item = result.ToSDKModel()
                });
            });
        }
        
        [HttpGet("")]
        public IActionResult Find(int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunction<IActionResult>("Find", delegate()
            {

                this.Security.ValidateCanSearchAsset(this.GetCurrentAccount());

                int takePlus = take;
                if (take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }

                List<dm.Asset> result = this.API.Direct.Assets.Find(skip, takePlus, keyword, order_by, descending);
                return base.Http200(result.ToSteppedListResult(skip, take, result.Count));

            });
        }
        
        
        [HttpPost]
        public IActionResult Create(sdk.Asset asset)
        {
            return base.ExecuteFunction<IActionResult>("Create", delegate()
            {
                this.ValidateNotNull(asset, "Asset");

                dm.Asset insert = asset.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.Assets.Insert(insert);
                
                sdk.Asset insertResult = insert.ToSDKModel();

                return base.Http200(new ItemResult<sdk.Asset>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{asset_id}")]
        public IActionResult Update(Guid asset_id, sdk.Asset asset)
        {
            return base.ExecuteFunction<IActionResult>("Update", delegate()
            {
                this.ValidateNotNull(asset, "Asset");
                this.ValidateRouteMatch(asset_id, asset.asset_id, "Asset");

                dm.Asset found = this.API.Direct.Assets.GetById(asset_id);
                this.ValidateNotNull(found, "Asset");

                asset.asset_id = asset_id;
                dm.Asset update = asset.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.Assets.Update(update);
                
                sdk.Asset existing = this.API.Direct.Assets.GetById(update.asset_id).ToSDKModel();
                
                return base.Http200(new ItemResult<sdk.Asset>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{asset_id}")]
        public IActionResult Delete(Guid asset_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Asset delete = this.API.Direct.Assets.GetById(asset_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.Assets.Delete(asset_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = asset_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

