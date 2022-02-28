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
    public partial interface IShopAccountStore
    {
        
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        Task<ShopAccount> GetDocumentAsync(Guid shop_account_id);
        
        Task<ShopAccount> GetDocumentAsync(Guid shop_id, Guid shop_account_id);
        
        Task<bool> CreateDocumentAsync(ShopAccount model);
        Task<bool> DeleteDocumentAsync(ShopAccount model);
        
        
        Task<ListResult<ShopAccount>> FindForShopAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false, string keyword = "", bool? enabled = true);
        Task DeleteForShopAsync(Guid shop_id);

        Task<PermissionInfo> GenerateReadPermissionForPartitionSharedAsync(Guid user_id, string partitionKey, string permissionId = "perm.standard.read");
    }
}
