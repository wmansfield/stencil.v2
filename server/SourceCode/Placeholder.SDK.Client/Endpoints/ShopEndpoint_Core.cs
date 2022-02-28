using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class ShopEndpoint : EndpointBase
    {
        public ShopEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Shop>> GetShopAsync(Guid shop_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shops/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Shop>>(request);
        }

        public Task<ListResult<Shop>> Find(int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false, Guid? tenant_id = null)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shops";
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            request.AddParameter("tenant_id", tenant_id);
            
            return this.Sdk.ExecuteAsync<ListResult<Shop>>(request);
        }
        public Task<ListResult<Shop>> GetShopByTenantAsync(Guid tenant_id, int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shops/by_tenant/{tenant_id}";
            request.AddUrlSegment("tenant_id", tenant_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            
            return this.Sdk.ExecuteAsync<ListResult<Shop>>(request);
        }
        

        public Task<ItemResult<Shop>> CreateShopAsync(Shop shop)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "shops";
            request.AddJsonBody(shop);
            return this.Sdk.ExecuteAsync<ItemResult<Shop>>(request);
        }

        public Task<ItemResult<Shop>> UpdateShopAsync(Guid shop_id, Shop shop)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "shops/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddJsonBody(shop);
            return this.Sdk.ExecuteAsync<ItemResult<Shop>>(request);
        }

        

        public Task<ActionResult> DeleteShopAsync(Guid shop_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "shops/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
