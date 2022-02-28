using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class ShopIsolatedEndpoint : EndpointBase
    {
        public ShopIsolatedEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<ShopIsolated>> GetShopIsolatedAsync(Guid shop_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shopisolateds/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<ShopIsolated>>(request);
        }

        

        public Task<ItemResult<ShopIsolated>> CreateShopIsolatedAsync(ShopIsolated shopisolated)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "shopisolateds";
            request.AddJsonBody(shopisolated);
            return this.Sdk.ExecuteAsync<ItemResult<ShopIsolated>>(request);
        }

        public Task<ItemResult<ShopIsolated>> UpdateShopIsolatedAsync(Guid shop_id, ShopIsolated shopisolated)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "shopisolateds/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddJsonBody(shopisolated);
            return this.Sdk.ExecuteAsync<ItemResult<ShopIsolated>>(request);
        }

        

        public Task<ActionResult> DeleteShopIsolatedAsync(Guid shop_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "shopisolateds/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
