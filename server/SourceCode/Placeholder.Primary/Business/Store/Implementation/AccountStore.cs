using System;
using System.Linq;
using System.Threading.Tasks;
using Placeholder.SDK;
using Placeholder.SDK.Models;

namespace Placeholder.Primary.Business.Store.Implementation
{
    public partial class AccountStore
    {
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        public Task<ListResult<Account>> FindByStatusAsync(AccountStatus? account_status, int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction(nameof(FindByStatusAsync), delegate ()
            {
                IQueryable<Account> query = base.QuerySharedWithoutPartitionKey();

                if (account_status.HasValue)
                {
                    query = query.Where(x => x.account_status == account_status.Value);
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => x.account_display.Contains(keyword)
                                          || x.email.Contains(keyword)
                                          || x.first_name.Contains(keyword)
                                          || x.last_name.Contains(keyword));
                }


                query = this.ApplySafeSort(query, order_by, descending);

                return query.FetchAsSteppedListAsync(skip, take);
            });
        }
    }
}
