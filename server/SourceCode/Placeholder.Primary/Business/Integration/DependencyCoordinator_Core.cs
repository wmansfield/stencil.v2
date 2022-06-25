using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Placeholder.Domain;
using Zero.Foundation.Aspect;
using Zero.Foundation;

namespace Placeholder.Primary.Business.Integration
{
    public partial class DependencyCoordinator_Core : ChokeableClass, IDependencyCoordinator
    {
        public DependencyCoordinator_Core(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.API = new PlaceholderAPI(iFoundation);
        }
        public virtual PlaceholderAPI API { get; set; }
        
        public virtual void GlobalSettingInvalidated(Dependency affectedDependencies, Guid global_setting_id)
        {
            base.ExecuteMethod("GlobalSettingInvalidated", delegate ()
            {
                DependencyWorker<GlobalSetting>.EnqueueRequest(this.IFoundation, affectedDependencies, global_setting_id, this.ProcessGlobalSettingInvalidation);
            });
        }
        protected virtual void ProcessGlobalSettingInvalidation(Dependency dependencies, Guid global_setting_id)
        {
            base.ExecuteMethod("ProcessGlobalSettingInvalidation", delegate ()
            {
                
            });
        }
        public virtual void TenantInvalidated(Dependency affectedDependencies, Guid tenant_id)
        {
            base.ExecuteMethod("TenantInvalidated", delegate ()
            {
                DependencyWorker<Tenant>.EnqueueRequest(this.IFoundation, affectedDependencies, tenant_id, this.ProcessTenantInvalidation);
            });
        }
        protected virtual void ProcessTenantInvalidation(Dependency dependencies, Guid tenant_id)
        {
            base.ExecuteMethod("ProcessTenantInvalidation", delegate ()
            {
                
            });
        }
        public virtual void AssetInvalidated(Dependency affectedDependencies, Guid asset_id)
        {
            base.ExecuteMethod("AssetInvalidated", delegate ()
            {
                DependencyWorker<Asset>.EnqueueRequest(this.IFoundation, affectedDependencies, asset_id, this.ProcessAssetInvalidation);
            });
        }
        protected virtual void ProcessAssetInvalidation(Dependency dependencies, Guid asset_id)
        {
            base.ExecuteMethod("ProcessAssetInvalidation", delegate ()
            {
                
            });
        }
        public virtual void AccountInvalidated(Dependency affectedDependencies, Guid account_id)
        {
            base.ExecuteMethod("AccountInvalidated", delegate ()
            {
                DependencyWorker<Account>.EnqueueRequest(this.IFoundation, affectedDependencies, account_id, this.ProcessAccountInvalidation);
            });
        }
        protected virtual void ProcessAccountInvalidation(Dependency dependencies, Guid account_id)
        {
            base.ExecuteMethod("ProcessAccountInvalidation", delegate ()
            {
                
            });
        }
        public virtual void ShopInvalidated(Dependency affectedDependencies, Guid shop_id)
        {
            base.ExecuteMethod("ShopInvalidated", delegate ()
            {
                DependencyWorker<Shop>.EnqueueRequest(this.IFoundation, affectedDependencies, shop_id, this.ProcessShopInvalidation);
            });
        }
        protected virtual void ProcessShopInvalidation(Dependency dependencies, Guid shop_id)
        {
            base.ExecuteMethod("ProcessShopInvalidation", delegate ()
            {
                
            });
        }
        public virtual void ShopIsolatedInvalidated(Dependency affectedDependencies, Guid shop_id)
        {
            base.ExecuteMethod("ShopIsolatedInvalidated", delegate ()
            {
                DependencyWorker<ShopIsolated>.EnqueueRequest(this.IFoundation, affectedDependencies, shop_id, this.ProcessShopIsolatedInvalidation);
            });
        }
        protected virtual void ProcessShopIsolatedInvalidation(Dependency dependencies, Guid shop_id)
        {
            base.ExecuteMethod("ProcessShopIsolatedInvalidation", delegate ()
            {
                
            });
        }
        public virtual void ShopAccountInvalidated(Dependency affectedDependencies, Guid shop_account_id)
        {
            base.ExecuteMethod("ShopAccountInvalidated", delegate ()
            {
                DependencyWorker<ShopAccount>.EnqueueRequest(this.IFoundation, affectedDependencies, shop_account_id, this.ProcessShopAccountInvalidation);
            });
        }
        protected virtual void ProcessShopAccountInvalidation(Dependency dependencies, Guid shop_account_id)
        {
            base.ExecuteMethod("ProcessShopAccountInvalidation", delegate ()
            {
                ShopAccount item = this.API.Direct.ShopAccounts.GetById(shop_account_id);
                
                if (item != null)
                {
                    this.API.Direct.Accounts.Invalidate(item.account_id, "ShopAccount changed");
                }
                
                this.API.Integration.Synchronization.AgitateSyncDaemon(null);
            });
        }
        public virtual void ShopSettingInvalidated(Dependency affectedDependencies, Guid shop_setting_id, Guid shop_id)
        {
            base.ExecuteMethod("ShopSettingInvalidated", delegate ()
            {
                DependencyWorker<ShopSetting>.EnqueueRequest(this.IFoundation, affectedDependencies, shop_setting_id, shop_id, this.ProcessShopSettingInvalidation);
            });
        }
        protected virtual void ProcessShopSettingInvalidation(Dependency dependencies, Guid shop_id, Guid shop_setting_id)
        {
            base.ExecuteMethod("ProcessShopSettingInvalidation", delegate ()
            {
                ShopSetting item = this.API.Direct.ShopSettings.GetById(shop_id, shop_setting_id);
                
                if (item != null)
                {
                    this.API.Direct.Shops.Invalidate(item.shop_id, "ShopSetting changed");
                }
                
                this.API.Integration.Synchronization.AgitateSyncDaemon(shop_id);
            });
        }
        public virtual void CompanyInvalidated(Dependency affectedDependencies, Guid company_id, Guid shop_id)
        {
            base.ExecuteMethod("CompanyInvalidated", delegate ()
            {
                DependencyWorker<Company>.EnqueueRequest(this.IFoundation, affectedDependencies, company_id, shop_id, this.ProcessCompanyInvalidation);
            });
        }
        protected virtual void ProcessCompanyInvalidation(Dependency dependencies, Guid shop_id, Guid company_id)
        {
            base.ExecuteMethod("ProcessCompanyInvalidation", delegate ()
            {
                
            });
        }
        public virtual void WidgetInvalidated(Dependency affectedDependencies, Guid widget_id, Guid shop_id)
        {
            base.ExecuteMethod("WidgetInvalidated", delegate ()
            {
                DependencyWorker<Widget>.EnqueueRequest(this.IFoundation, affectedDependencies, widget_id, shop_id, this.ProcessWidgetInvalidation);
            });
        }
        protected virtual void ProcessWidgetInvalidation(Dependency dependencies, Guid shop_id, Guid widget_id)
        {
            base.ExecuteMethod("ProcessWidgetInvalidation", delegate ()
            {
                
            });
        }
        
    }
}


