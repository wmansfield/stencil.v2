using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class AccountEndpoint : EndpointBase
    {
        public AccountEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Account>> GetAccountAsync(Guid account_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "accounts/{account_id}";
            request.AddUrlSegment("account_id", account_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Account>>(request);
        }

        public Task<ListResult<Account>> Find(int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "accounts";
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            
            return this.Sdk.ExecuteAsync<ListResult<Account>>(request);
        }

        public Task<ItemResult<Account>> CreateAccountAsync(Account account)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "accounts";
            request.AddJsonBody(account);
            return this.Sdk.ExecuteAsync<ItemResult<Account>>(request);
        }

        public Task<ItemResult<Account>> UpdateAccountAsync(Guid account_id, Account account)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "accounts/{account_id}";
            request.AddUrlSegment("account_id", account_id.ToString());
            request.AddJsonBody(account);
            return this.Sdk.ExecuteAsync<ItemResult<Account>>(request);
        }

        

        public Task<ActionResult> DeleteAccountAsync(Guid account_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "accounts/{account_id}";
            request.AddUrlSegment("account_id", account_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
