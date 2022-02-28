using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class ShopAccountEndpoint : EndpointBase
    {
        public ShopAccountEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<ShopAccount>> GetShopAccountAsync(Guid shop_id, Guid shop_account_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shopaccounts/{shop_id}/{shop_account_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddUrlSegment("shop_account_id", shop_account_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<ShopAccount>>(request);
        }

        public Task<ListResult<ShopAccount>> FindForShop(Guid shop_id, int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false,  bool? enabled = true)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "shopaccounts/for_shop/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            request.AddParameter("enabled", enabled);
            request.AddParameter("enabled", enabled);
            
            return this.Sdk.ExecuteAsync<ListResult<ShopAccount>>(request);
        }

        public Task<ItemResult<ShopAccount>> CreateShopAccountAsync(ShopAccount shopaccount)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "shopaccounts";
            request.AddJsonBody(shopaccount);
            return this.Sdk.ExecuteAsync<ItemResult<ShopAccount>>(request);
        }

        public Task<ItemResult<ShopAccount>> UpdateShopAccountAsync(Guid shop_account_id, ShopAccount shopaccount)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "shopaccounts/{shop_account_id}";
            request.AddUrlSegment("shop_account_id", shop_account_id.ToString());
            request.AddJsonBody(shopaccount);
            return this.Sdk.ExecuteAsync<ItemResult<ShopAccount>>(request);
        }

        

        public Task<ActionResult> DeleteShopAccountAsync(Guid shop_account_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "shopaccounts/{shop_account_id}";
            request.AddUrlSegment("shop_account_id", shop_account_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
