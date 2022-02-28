using Placeholder.SDK.Client.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Client
{
    public partial class PlaceholderSDK
    {
        // members for web ease
        public GlobalSettingEndpoint GlobalSetting;
        public TenantEndpoint Tenant;
        public AssetEndpoint Asset;
        public AccountEndpoint Account;
        public ShopEndpoint Shop;
        public ShopIsolatedEndpoint ShopIsolated;
        public ShopAccountEndpoint ShopAccount;
        public ShopSettingEndpoint ShopSetting;
        public CompanyEndpoint Company;
        

        protected virtual void ConstructCoreEndpoints()
        {
            this.GlobalSetting = new GlobalSettingEndpoint(this);
            this.Tenant = new TenantEndpoint(this);
            this.Asset = new AssetEndpoint(this);
            this.Account = new AccountEndpoint(this);
            this.Shop = new ShopEndpoint(this);
            this.ShopIsolated = new ShopIsolatedEndpoint(this);
            this.ShopAccount = new ShopAccountEndpoint(this);
            this.ShopSetting = new ShopSettingEndpoint(this);
            this.Company = new CompanyEndpoint(this);
            
        }   
    }
}

