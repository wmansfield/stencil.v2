using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IShopBusiness
    {
        IShopSynchronizer Synchronizer { get; }
        Shop GetById(Guid shop_id);
        List<Shop> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false, Guid? tenant_id = null);
        int FindTotal(string keyword = "", Guid? tenant_id = null);
        
        List<Shop> GetByTenant(Guid tenant_id);
        
        Shop Insert(Shop insertShop);
        Shop Insert(Shop insertShop, Availability availability);
        Shop Update(Shop updateShop);
        Shop Update(Shop updateShop, Availability availability);
        
        void Delete(Guid shop_id);
        
        
        void SynchronizationUpdateIsolated(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalidIsolated(Guid shop_id, int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdateIsolated(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalidIsolated(Guid shop_id, int retryPriorityThreshold, string sync_agent);
        void SynchronizationUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid shop_id, string reason);
    }
}

