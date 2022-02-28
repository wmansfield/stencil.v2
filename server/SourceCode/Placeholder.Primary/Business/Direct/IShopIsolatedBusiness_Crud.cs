using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IShopIsolatedBusiness
    {
        IShopIsolatedSynchronizer Synchronizer { get; }
        ShopIsolated GetById(Guid shop_id);
        
        ShopIsolated Insert(ShopIsolated insertShopIsolated);
        ShopIsolated Insert(ShopIsolated insertShopIsolated, Availability availability);
        ShopIsolated Update(ShopIsolated updateShopIsolated);
        ShopIsolated Update(ShopIsolated updateShopIsolated, Availability availability);
        
        void Delete(Guid shop_id);
        
        void SynchronizationUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid shop_id, string reason);
    }
}

