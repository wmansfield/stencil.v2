using System;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
    public partial interface IShopSettingBusiness
    {
        ShopSetting Upsert(Guid shop_id, string name, string value, bool encrypt = false);
        ShopSetting GetByName(Guid shop_id, string name);

        bool GetValueOrDefault(Guid shop_id, string name, bool defaultValue);
        decimal GetValueOrDefault(Guid shop_id, string name, decimal defaultValue);
        int GetValueOrDefault(Guid shop_id, string name, int defaultValue);
        string GetValueOrDefault(Guid shop_id, string name, string defaultValue);
        DateTime? GetValueOrDefault(Guid shop_id, string name, DateTime? defaultValue);
        long? GetValueOrDefault(Guid shop_id, string name, long? defaultValue);
        string GetValueOrDefaultCached(Guid shop_id, string name, string defaultValue);
        int GetValueOrDefaultCached(Guid shop_id, string name, int defaultValue);
    }
}
