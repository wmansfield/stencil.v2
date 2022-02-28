using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class TenantEndpoint : EndpointBase
    {
        public TenantEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Tenant>> GetTenantAsync(Guid tenant_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "tenants/{tenant_id}";
            request.AddUrlSegment("tenant_id", tenant_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Tenant>>(request);
        }

        public Task<ListResult<Tenant>> Find(int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "tenants";
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            
            return this.Sdk.ExecuteAsync<ListResult<Tenant>>(request);
        }

        public Task<ItemResult<Tenant>> CreateTenantAsync(Tenant tenant)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "tenants";
            request.AddJsonBody(tenant);
            return this.Sdk.ExecuteAsync<ItemResult<Tenant>>(request);
        }

        public Task<ItemResult<Tenant>> UpdateTenantAsync(Guid tenant_id, Tenant tenant)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "tenants/{tenant_id}";
            request.AddUrlSegment("tenant_id", tenant_id.ToString());
            request.AddJsonBody(tenant);
            return this.Sdk.ExecuteAsync<ItemResult<Tenant>>(request);
        }

        

        public Task<ActionResult> DeleteTenantAsync(Guid tenant_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "tenants/{tenant_id}";
            request.AddUrlSegment("tenant_id", tenant_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
