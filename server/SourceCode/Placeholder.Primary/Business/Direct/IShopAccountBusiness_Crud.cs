using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IShopAccountBusiness
    {
        IShopAccountSynchronizer Synchronizer { get; }
        ShopAccount GetById(Guid shop_account_id);
        
        List<ShopAccount> GetByShop(Guid shop_id);
        
        List<ShopAccount> GetByAccount(Guid account_id);
        
        ShopAccount Insert(ShopAccount insertShopAccount);
        ShopAccount Insert(ShopAccount insertShopAccount, Availability availability);
        ShopAccount Update(ShopAccount updateShopAccount);
        ShopAccount Update(ShopAccount updateShopAccount, Availability availability);
        
        void Delete(Guid shop_account_id);
        
        void SynchronizationUpdate(Guid shop_account_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid shop_account_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid shop_account_id, string reason);
    }
}

