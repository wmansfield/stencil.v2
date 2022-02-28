using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class AssetEndpoint : EndpointBase
    {
        public AssetEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Asset>> GetAssetAsync(Guid asset_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "assets/{asset_id}";
            request.AddUrlSegment("asset_id", asset_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Asset>>(request);
        }

        public Task<ListResult<Asset>> Find(int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "assets";
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            
            return this.Sdk.ExecuteAsync<ListResult<Asset>>(request);
        }

        public Task<ItemResult<Asset>> CreateAssetAsync(Asset asset)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "assets";
            request.AddJsonBody(asset);
            return this.Sdk.ExecuteAsync<ItemResult<Asset>>(request);
        }

        public Task<ItemResult<Asset>> UpdateAssetAsync(Guid asset_id, Asset asset)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "assets/{asset_id}";
            request.AddUrlSegment("asset_id", asset_id.ToString());
            request.AddJsonBody(asset);
            return this.Sdk.ExecuteAsync<ItemResult<Asset>>(request);
        }

        

        public Task<ActionResult> DeleteAssetAsync(Guid asset_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "assets/{asset_id}";
            request.AddUrlSegment("asset_id", asset_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
