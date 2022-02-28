using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Integration
{
    public interface IDependencyCoordinator
    {
        void GlobalSettingInvalidated(Dependency affectedDependencies, Guid global_setting_id);
        void TenantInvalidated(Dependency affectedDependencies, Guid tenant_id);
        void AssetInvalidated(Dependency affectedDependencies, Guid asset_id);
        void AccountInvalidated(Dependency affectedDependencies, Guid account_id);
        void ShopInvalidated(Dependency affectedDependencies, Guid shop_id);
        void ShopIsolatedInvalidated(Dependency affectedDependencies, Guid shop_id);
        void ShopAccountInvalidated(Dependency affectedDependencies, Guid shop_account_id);
        void ShopSettingInvalidated(Dependency affectedDependencies, Guid shop_setting_id, Guid shop_id);
        void CompanyInvalidated(Dependency affectedDependencies, Guid company_id, Guid shop_id);
        
    }
}


