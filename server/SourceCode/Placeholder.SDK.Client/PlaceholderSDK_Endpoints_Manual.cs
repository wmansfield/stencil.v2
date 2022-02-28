using Placeholder.SDK.Client.Endpoints;
using System;

namespace Placeholder.SDK.Client
{
    public partial class PlaceholderSDK
    {
        //don't use properties for js mapping ease

        public AuthEndpoint Auth;

        protected virtual void ConstructManualEndpoints()
        {
            this.Auth = new AuthEndpoint(this);
        }
    }
}
