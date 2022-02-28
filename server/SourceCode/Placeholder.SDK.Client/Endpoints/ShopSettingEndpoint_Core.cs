using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class ShopSettingEndpoint : EndpointBase
    {
        public ShopSettingEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<ShopSetting>> GetShopSettingAsync(Guid shop_id, Guid shop_setting_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shopsettings/{shop_id}/{shop_setting_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddUrlSegment("shop_setting_id", shop_setting_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<ShopSetting>>(request);
        }

        public Task<ListResult<ShopSetting>> FindForShop(Guid shop_id, int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shopsettings/for_shop/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            
            return this.Sdk.ExecuteAsync<ListResult<ShopSetting>>(request);
        }

        public Task<ItemResult<ShopSetting>> CreateShopSettingAsync(ShopSetting shopsetting)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "shopsettings";
            request.AddJsonBody(shopsetting);
            return this.Sdk.ExecuteAsync<ItemResult<ShopSetting>>(request);
        }

        public Task<ItemResult<ShopSetting>> UpdateShopSettingAsync(Guid shop_setting_id, ShopSetting shopsetting)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "shopsettings/{shop_setting_id}";
            request.AddUrlSegment("shop_setting_id", shop_setting_id.ToString());
            request.AddJsonBody(shopsetting);
            return this.Sdk.ExecuteAsync<ItemResult<ShopSetting>>(request);
        }

        

        public Task<ActionResult> DeleteShopSettingAsync(Guid shop_id, Guid shop_setting_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "shopsettings/{shop_id}/{shop_setting_id}";
            request.AddUrlSegment("shop_setting_id", shop_setting_id.ToString());
            request.AddUrlSegment("shop_id", shop_id.ToString());
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
