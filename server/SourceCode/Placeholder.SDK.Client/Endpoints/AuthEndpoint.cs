using Placeholder.SDK.Models;
using Placeholder.SDK.Models.Requests;
using Placeholder.SDK.Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class AuthEndpoint : EndpointBase
    {
        public AuthEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }

        public Task<ItemResult<AccountInfo>> LoginAsync(AuthLoginInput input)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "auth/login";
            request.AddJsonBody(input);
            return this.Sdk.ExecuteAsync<ItemResult<AccountInfo>>(request);
        }
        public Task<ActionResult> LogoutAsync()
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "auth/logout";
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }

        public Task<ActionResult> PasswordResetStartAsync(string email)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "auth/password_reset/start";
            request.AddJsonBody(new PasswordResetInput() { email = email });
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
        public Task<ActionResult> PasswordResetCompleteAsync(string email, string token, string password)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "auth/password_reset/complete";
            request.AddJsonBody(new PasswordResetInput() { email = email, password = password, token = token });
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }

        public Task<ListResult<IDPair>> GetTimeZonesAllAsync()
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "auth/timezones/all";
            return this.Sdk.ExecuteAsync<ListResult<IDPair>>(request);
        }
        public Task<ListResult<IDPair>> GetTimeZonesUsaAsync()
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "auth/timezones/usa";
            return this.Sdk.ExecuteAsync<ListResult<IDPair>>(request);
        }
    }
}
