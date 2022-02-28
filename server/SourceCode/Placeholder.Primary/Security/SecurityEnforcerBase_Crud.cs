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
        
        #region Account Methods

        public virtual void ValidateCanCreate(Account currentAccount, Account account)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Account account)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            if(!allowed)
            {
                // Try Self
                allowed = (currentAccount.account_id == account.account_id);
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchAccount(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListAccount(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Account account)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Account account)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region Asset Methods

        public virtual void ValidateCanCreate(Account currentAccount, Asset asset)
        {
            bool allowed = false;
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Asset asset)
        {
            bool allowed = false;
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchAsset(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListAsset(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Asset asset)
        {
            bool allowed = false;
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Asset asset)
        {
            bool allowed = false;
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region Company Methods

        public virtual void ValidateCanCreate(Account currentAccount, Company company)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Company company)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchCompany(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListCompany(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Company company)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Company company)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region GlobalSetting Methods

        public virtual void ValidateCanCreate(Account currentAccount, GlobalSetting globalsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, GlobalSetting globalsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchGlobalSetting(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListGlobalSetting(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, GlobalSetting globalsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, GlobalSetting globalsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region Shop Methods

        public virtual void ValidateCanCreate(Account currentAccount, Shop shop)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Shop shop)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchShop(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListShop(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Shop shop)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Shop shop)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region ShopAccount Methods

        public virtual void ValidateCanCreate(Account currentAccount, ShopAccount shopaccount)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, ShopAccount shopaccount)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchShopAccount(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListShopAccount(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, ShopAccount shopaccount)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, ShopAccount shopaccount)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region ShopIsolated Methods

        public virtual void ValidateCanCreate(Account currentAccount, ShopIsolated shopisolated)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, ShopIsolated shopisolated)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchShopIsolated(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListShopIsolated(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, ShopIsolated shopisolated)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, ShopIsolated shopisolated)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region ShopSetting Methods

        public virtual void ValidateCanCreate(Account currentAccount, ShopSetting shopsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, ShopSetting shopsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchShopSetting(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListShopSetting(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, ShopSetting shopsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, ShopSetting shopsetting)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
        #region Tenant Methods

        public virtual void ValidateCanCreate(Account currentAccount, Tenant tenant)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, Tenant tenant)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            
        }
        public virtual void ValidateCanSearchTenant(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            
        }
        public virtual void ValidateCanListTenant(Account currentAccount, Guid? shop_id = null)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            
        }
        public virtual void ValidateCanUpdate(Account currentAccount, Tenant tenant)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            
        }
        public virtual void ValidateCanDelete(Account currentAccount, Tenant tenant)
        {
            bool allowed = false;
            
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            
        }

        #endregion
        
    }
}

