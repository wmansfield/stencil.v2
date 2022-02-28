using Zero.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK;
using Placeholder.SDK.Models;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Business.Store
{
    public partial interface IShopSettingStore
    {
        
        Task<ShopSetting> GetDocumentAsync(Guid shop_id, Guid shop_setting_id);
        
        Task<bool> CreateDocumentAsync(ShopSetting model);
        Task<bool> DeleteDocumentAsync(ShopSetting model);
        
        
        Task<ListResult<ShopSetting>> FindForShopAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false, string keyword = "");
        Task DeleteForShopAsync(Guid shop_id);

        Task<PermissionInfo> GenerateReadPermissionForPartitionIsolatedAsync(Guid shop_id, Guid user_id, string partitionKey, string permissionId = "perm.standard.read");
    }
}
