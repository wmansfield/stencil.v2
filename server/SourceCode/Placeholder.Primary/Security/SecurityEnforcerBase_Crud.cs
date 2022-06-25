using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Common;
using Placeholder.Domain;
using Placeholder.Common.Exceptions;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Security
{
    // WARNING: THIS FILE IS GENERATED
    public partial class SecurityEnforcerBase : ISecurityEnforcer
    {
        
        #region GlobalSetting Methods

        public virtual void ValidateCanCreate(Account currentAccount, GlobalSetting globalsetting)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, GlobalSetting globalsetting)
        {
            
        }
        public virtual void ValidateCanSearchGlobalSetting(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListGlobalSetting(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, GlobalSetting globalsetting)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, GlobalSetting globalsetting)
        {
            
        }

        #endregion
        
        #region Tenant Methods

        public virtual void ValidateCanCreate(Account currentAccount, Tenant tenant)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Tenant tenant)
        {
            
        }
        public virtual void ValidateCanSearchTenant(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListTenant(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Tenant tenant)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Tenant tenant)
        {
            
        }

        #endregion
        
        #region Asset Methods

        public virtual void ValidateCanCreate(Account currentAccount, Asset asset)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Asset asset)
        {
            
        }
        public virtual void ValidateCanSearchAsset(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListAsset(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Asset asset)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Asset asset)
        {
            
        }

        #endregion
        
        #region Account Methods

        public virtual void ValidateCanCreate(Account currentAccount, Account account)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Account account)
        {
            
        }
        public virtual void ValidateCanSearchAccount(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListAccount(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Account account)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Account account)
        {
            
        }

        #endregion
        
        #region Shop Methods

        public virtual void ValidateCanCreate(Account currentAccount, Shop shop)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Shop shop)
        {
            
        }
        public virtual void ValidateCanSearchShop(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListShop(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Shop shop)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Shop shop)
        {
            
        }

        #endregion
        
        #region ShopIsolated Methods

        public virtual void ValidateCanCreate(Account currentAccount, ShopIsolated shopisolated)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, ShopIsolated shopisolated)
        {
            
        }
        public virtual void ValidateCanSearchShopIsolated(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListShopIsolated(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, ShopIsolated shopisolated)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, ShopIsolated shopisolated)
        {
            
        }

        #endregion
        
        #region ShopAccount Methods

        public virtual void ValidateCanCreate(Account currentAccount, ShopAccount shopaccount)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, ShopAccount shopaccount)
        {
            
        }
        public virtual void ValidateCanSearchShopAccount(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListShopAccount(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, ShopAccount shopaccount)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, ShopAccount shopaccount)
        {
            
        }

        #endregion
        
        #region ShopSetting Methods

        public virtual void ValidateCanCreate(Account currentAccount, ShopSetting shopsetting)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, ShopSetting shopsetting)
        {
            
        }
        public virtual void ValidateCanSearchShopSetting(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListShopSetting(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, ShopSetting shopsetting)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, ShopSetting shopsetting)
        {
            
        }

        #endregion
        
        #region Company Methods

        public virtual void ValidateCanCreate(Account currentAccount, Company company)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Company company)
        {
            
        }
        public virtual void ValidateCanSearchCompany(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListCompany(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Company company)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Company company)
        {
            
        }

        #endregion
        
        #region Widget Methods

        public virtual void ValidateCanCreate(Account currentAccount, Widget widget)
        {
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Widget widget)
        {
            
        }
        public virtual void ValidateCanSearchWidget(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanListWidget(Account currentAccount, Guid? shop_id = null)
        {
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Widget widget)
        {
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Widget widget)
        {
            
        }

        #endregion
        
    }
}

