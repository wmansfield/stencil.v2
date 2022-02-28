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
    public partial interface IAccountStore
    {
        
        Task<Account> GetDocumentAsync(Guid account_id);
        
        Task<bool> CreateDocumentAsync(Account model);
        Task<bool> DeleteDocumentAsync(Account model);
        
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        Task<ListResult<Account>> FindAsync(int skip, int take, string order_by = "", bool descending = false, string keyword = "");
        

        Task<PermissionInfo> GenerateReadPermissionForPartitionSharedAsync(Guid user_id, string partitionKey, string permissionId = "perm.standard.read");
    }
}
