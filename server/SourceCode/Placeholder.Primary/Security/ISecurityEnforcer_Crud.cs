using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Security
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface ISecurityEnforcer
    {
        
        void ValidateCanCreate(Account currentAccount, Account account);
        void ValidateCanSearchAccount(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListAccount(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, Account account);
        void ValidateCanUpdate(Account currentAccount, Account account);
        void ValidateCanDelete(Account currentAccount, Account account);
        
        void ValidateCanCreate(Account currentAccount, Asset asset);
        void ValidateCanSearchAsset(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListAsset(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, Asset asset);
        void ValidateCanUpdate(Account currentAccount, Asset asset);
        void ValidateCanDelete(Account currentAccount, Asset asset);
        
        void ValidateCanCreate(Account currentAccount, Company company);
        void ValidateCanSearchCompany(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListCompany(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, Company company);
        void ValidateCanUpdate(Account currentAccount, Company company);
        void ValidateCanDelete(Account currentAccount, Company company);
        
        void ValidateCanCreate(Account currentAccount, GlobalSetting globalsetting);
        void ValidateCanSearchGlobalSetting(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListGlobalSetting(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, GlobalSetting globalsetting);
        void ValidateCanUpdate(Account currentAccount, GlobalSetting globalsetting);
        void ValidateCanDelete(Account currentAccount, GlobalSetting globalsetting);
        
        void ValidateCanCreate(Account currentAccount, Shop shop);
        void ValidateCanSearchShop(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListShop(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, Shop shop);
        void ValidateCanUpdate(Account currentAccount, Shop shop);
        void ValidateCanDelete(Account currentAccount, Shop shop);
        
        void ValidateCanCreate(Account currentAccount, ShopAccount shopaccount);
        void ValidateCanSearchShopAccount(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListShopAccount(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, ShopAccount shopaccount);
        void ValidateCanUpdate(Account currentAccount, ShopAccount shopaccount);
        void ValidateCanDelete(Account currentAccount, ShopAccount shopaccount);
        
        void ValidateCanCreate(Account currentAccount, ShopIsolated shopisolated);
        void ValidateCanSearchShopIsolated(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListShopIsolated(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, ShopIsolated shopisolated);
        void ValidateCanUpdate(Account currentAccount, ShopIsolated shopisolated);
        void ValidateCanDelete(Account currentAccount, ShopIsolated shopisolated);
        
        void ValidateCanCreate(Account currentAccount, ShopSetting shopsetting);
        void ValidateCanSearchShopSetting(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListShopSetting(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, ShopSetting shopsetting);
        void ValidateCanUpdate(Account currentAccount, ShopSetting shopsetting);
        void ValidateCanDelete(Account currentAccount, ShopSetting shopsetting);
        
        void ValidateCanCreate(Account currentAccount, Tenant tenant);
        void ValidateCanSearchTenant(Account currentAccount, Guid? shop_id = null);
        void ValidateCanListTenant(Account currentAccount, Guid? shop_id = null);
        void ValidateCanRetrieve(Account currentAccount, Tenant tenant);
        void ValidateCanUpdate(Account currentAccount, Tenant tenant);
        void ValidateCanDelete(Account currentAccount, Tenant tenant);
        
    }
}

