using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IShopSettingBusiness
    {
        IShopSettingSynchronizer Synchronizer { get; }
        ShopSetting GetById(Guid shop_id, Guid shop_setting_id);
        List<ShopSetting> Find(Guid shop_id, int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        int FindTotal(Guid shop_id, string keyword = "");
        
        List<ShopSetting> GetByShop(Guid shop_id);
        
        ShopSetting Insert(ShopSetting insertShopSetting);
        ShopSetting Insert(ShopSetting insertShopSetting, Availability availability);
        ShopSetting Update(ShopSetting updateShopSetting);
        ShopSetting Update(ShopSetting updateShopSetting, Availability availability);
        
        void Delete(Guid shop_id, Guid shop_setting_id);
        
        void SynchronizationUpdate(Guid shop_id, Guid shop_setting_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid shop_id, Guid shop_setting_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid shop_id, Guid shop_setting_id, string reason);
    }
}

