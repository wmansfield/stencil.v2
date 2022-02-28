using Placeholder.SDK.Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class AccountEndpoint
    {
        public Task<ItemResult<AccountInfo>> GetSelfAsync()
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "accounts/self";
            return this.Sdk.ExecuteAsync<ItemResult<AccountInfo>>(request);
        }

    }
}
