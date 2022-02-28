using System;

namespace Placeholder.SDK.Client.Endpoints
{
    public abstract class EndpointBase
    {
        public EndpointBase(PlaceholderSDK sdk)
        {
            this.Sdk = sdk;
        }

        protected virtual PlaceholderSDK Sdk { get; set; }

    }
}
