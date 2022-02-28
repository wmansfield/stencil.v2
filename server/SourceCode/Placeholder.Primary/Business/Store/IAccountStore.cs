using Placeholder.SDK;
using Placeholder.SDK.Models;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Store
{
    public partial interface IAccountStore
    {
        Task<ListResult<Account>> FindByStatusAsync(AccountStatus? account_status, int skip, int take, string keyword = "", string order_by = "", bool descending = false);
    }
}
