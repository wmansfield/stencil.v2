using Zero.Foundation;
using Placeholder.Primary.Business.Direct;
using Placeholder.Primary.Business.Direct.Implementation;
using Placeholder.Primary.Business.Store;
using Placeholder.Primary.Business.Store.Implementation;
using Placeholder.Primary.Business.Synchronization;
using Placeholder.Primary.Business.Synchronization.Implementation;
using Unity;

namespace Placeholder.Primary.Foundation
{
    public partial class PlaceholderBootStrap
    {
        protected virtual void RegisterDataElements(IFoundation foundation)
        {
            foundation.Container.RegisterType<IGlobalSettingBusiness, GlobalSettingBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<ITenantBusiness, TenantBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IAssetBusiness, AssetBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IAccountBusiness, AccountBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopBusiness, ShopBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopIsolatedBusiness, ShopIsolatedBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopAccountBusiness, ShopAccountBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopSettingBusiness, ShopSettingBusiness>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<ICompanyBusiness, CompanyBusiness>(TypeLifetime.Scoped);
            
            
            //Indexes
            

            //Stores
            foundation.Container.RegisterType<IAccountStore, AccountStore>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopStore, ShopStore>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopIsolatedStore, ShopIsolatedStore>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopAccountStore, ShopAccountStore>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopSettingStore, ShopSettingStore>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<ICompanyStore, CompanyStore>(TypeLifetime.Scoped);
            
            
            //Synchronizers
            foundation.Container.RegisterType<IAccountSynchronizer, AccountSynchronizer>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopSynchronizer, ShopSynchronizer>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopIsolatedSynchronizer, ShopIsolatedSynchronizer>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopAccountSynchronizer, ShopAccountSynchronizer>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<IShopSettingSynchronizer, ShopSettingSynchronizer>(TypeLifetime.Scoped);
            foundation.Container.RegisterType<ICompanySynchronizer, CompanySynchronizer>(TypeLifetime.Scoped);
            
        }
    }
}

