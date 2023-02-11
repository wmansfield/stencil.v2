using Starter.SDK.Models;
using Stencil.Common.Screens;
using Stencil.Maui;

namespace Starter.App.Cloud
{
    public partial class StarterCloud : StarterTrackedClass
    {
        #region Constructor

        public StarterCloud()
            : base(nameof(StarterCloud))
        {

        }

        #endregion

        #region Properties

        protected MockSDKClient StarterSDKAnonymous { get; set; }

        #endregion

        #region SDK Methods

        protected MockSDKClient GenerateSDK()
        {
            return base.ExecuteFunction(nameof(GenerateSDK), delegate ()
            {
                MockSDKClient result = this.StarterSDKAnonymous;

                if (result == null)
                {
                    result = new MockSDKClient(StarterAssumptions.API_URL);
                    this.StarterSDKAnonymous = result;
                }

                int ix = result.CustomHeaders.FindIndex(x => x.Key == "accept-language");
                if (ix >= 0)
                {
                    result.CustomHeaders.RemoveAt(ix);
                }

                if (!string.IsNullOrEmpty(this.API.Application.CurrentCulture))
                {
                    result.CustomHeaders.Add(new KeyValuePair<string, string>("accept-language", this.API.Application.CurrentCulture));
                }
                return result;
            });

        }

        protected virtual Task<MockEnvelope> ExecuteSDK(string methodName, Func<MockSDKClient, Task<MockEnvelope>> action)
        {
            return base.ExecuteFunctionAsync(methodName, async delegate ()
            {
                MockSDKClient sdk = this.GenerateSDK();
                try
                {
                    return await action(sdk);
                }
                catch (Exception ex)
                {
                    this.LogError(ex, methodName);
                    return new MockEnvelope()
                    {
                        success = false,
                        message = ex.FirstNonAggregateException().Message
                    };
                }
            });
        }
        protected virtual Task<MockEnvelope<T>> ExecuteSDK<T>(string methodName, Func<MockSDKClient, Task<MockEnvelope<T>>> action)
        {
            return base.ExecuteFunctionAsync(methodName, async delegate ()
            {
                MockSDKClient sdk = this.GenerateSDK();
                try
                {
                    return await action(sdk);
                }
                catch (Exception ex)
                {
                    this.LogError(ex, methodName);
                    return new MockEnvelope<T>()
                    {
                        success = false,
                        message = ex.FirstNonAggregateException().Message
                    };
                }
            });
        }

        #endregion

        #region Cloud Methods

        public Task<MockEnvelope<ScreenConfigExchange>> RemoteScreenGetAsync(MockInput input)
        {
            return this.ExecuteSDK(nameof(RemoteScreenGetAsync), sdk =>
            {
                return sdk.ScreenConfigGetAsync(input);
            });
        }


        #endregion

    }

    public class MockSDKClient
    {
        public MockSDKClient(string baseUrl)
        {
            this.BaseUrl = baseUrl;
            this.CustomHeaders = new List<KeyValuePair<string, string>>();
        }

        public string BaseUrl { get; set; }
        public List<KeyValuePair<string, string>> CustomHeaders { get; set; }

        public Task<MockEnvelope<ScreenConfigExchange>> ScreenConfigGetAsync(MockInput input)
        {
            return Task.FromResult(new MockEnvelope<ScreenConfigExchange>()
            {
                item = new ScreenConfigExchange()
                {
                    ScreenName = input.navigation_data.screen_name,
                    ScreenStorageKey = input.navigation_data.screen_name,
                    SuppressPersist = true,
                }
            });
        }
    }

    public class MockEnvelope
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
    public class MockEnvelope<T> : MockEnvelope
    {
        public T item { get; set; }
    }

}

namespace Starter.SDK.Models
{
    public class MockInput
    {
        public NavigationData navigation_data { get; set; }
    }
}