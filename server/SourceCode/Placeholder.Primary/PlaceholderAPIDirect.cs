using Zero.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Placeholder.Primary.Business.Direct;

namespace Placeholder.Primary
{
    public class PlaceholderAPIDirect : BaseClass
    {
        public PlaceholderAPIDirect(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        public IGlobalSettingBusiness GlobalSettings
        {
            get { return this.IFoundation.Resolve<IGlobalSettingBusiness>(); }
        }
        public ITenantBusiness Tenants
        {
            get { return this.IFoundation.Resolve<ITenantBusiness>(); }
        }
        public IAssetBusiness Assets
        {
            get { return this.IFoundation.Resolve<IAssetBusiness>(); }
        }
        public IAccountBusiness Accounts
        {
            get { return this.IFoundation.Resolve<IAccountBusiness>(); }
        }
        public IShopBusiness Shops
        {
            get { return this.IFoundation.Resolve<IShopBusiness>(); }
        }
        public IShopIsolatedBusiness ShopIsolateds
        {
            get { return this.IFoundation.Resolve<IShopIsolatedBusiness>(); }
        }
        public IShopAccountBusiness ShopAccounts
        {
            get { return this.IFoundation.Resolve<IShopAccountBusiness>(); }
        }
        public IShopSettingBusiness ShopSettings
        {
            get { return this.IFoundation.Resolve<IShopSettingBusiness>(); }
        }
        public ICompanyBusiness Companies
        {
            get { return this.IFoundation.Resolve<ICompanyBusiness>(); }
        }
        public IWidgetBusiness Widgets
        {
            get { return this.IFoundation.Resolve<IWidgetBusiness>(); }
        }
        
    }
}


