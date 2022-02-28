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
    public partial interface IShopIsolatedStore
    {
        
        Task<ShopIsolated> GetDocumentAsync(Guid shop_id);
        
        Task<bool> CreateDocumentAsync(ShopIsolated model);
        Task<bool> DeleteDocumentAsync(ShopIsolated model);
        
        
        Task<ListResult<ShopIsolated>> FindAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false);
        

        Task<PermissionInfo> GenerateReadPermissionForPartitionIsolatedAsync(Guid shop_id, Guid user_id, string partitionKey, string permissionId = "perm.standard.read");
    }
}
