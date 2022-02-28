using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class CompanyEndpoint : EndpointBase
    {
        public CompanyEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Company>> GetCompanyAsync(Guid shop_id, Guid company_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "companys/{shop_id}/{company_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddUrlSegment("company_id", company_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Company>>(request);
        }

        public Task<ListResult<Company>> FindForShop(Guid shop_id, int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false,  bool? disabled = null)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "companys/for_shop/{shop_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            request.AddParameter("disabled", disabled);
            request.AddParameter("disabled", disabled);
            
            return this.Sdk.ExecuteAsync<ListResult<Company>>(request);
        }

        public Task<ItemResult<Company>> CreateCompanyAsync(Company company)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "companys";
            request.AddJsonBody(company);
            return this.Sdk.ExecuteAsync<ItemResult<Company>>(request);
        }

        public Task<ItemResult<Company>> UpdateCompanyAsync(Guid company_id, Company company)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "companys/{company_id}";
            request.AddUrlSegment("company_id", company_id.ToString());
            request.AddJsonBody(company);
            return this.Sdk.ExecuteAsync<ItemResult<Company>>(request);
        }

        

        public Task<ActionResult> DeleteCompanyAsync(Guid shop_id, Guid company_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "companys/{shop_id}/{company_id}";
            request.AddUrlSegment("company_id", company_id.ToString());
            request.AddUrlSegment("shop_id", shop_id.ToString());
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
