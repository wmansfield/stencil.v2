using Placeholder.SDK;
using Placeholder.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Store.Implementation
{
    public partial class ShopAccountStore
    {
        [Obsolete("Use caution, this is expensive", false)]
        public Task<List<ShopAccount>> GetForAccountAsync(Guid account_id, bool? enabled = true)
        {
            return base.ExecuteFunction(nameof(GetForAccountAsync), async delegate ()
            {
                IQueryable<ShopAccount> query = this.QuerySharedWithoutPartitionKey();
                query = query.Where(x => x.account_id == account_id);
                if (enabled.HasValue)
                {
                    query = query.Where(x => x.enabled == enabled.Value);
                }

                query = this.ApplySafeSort(query, nameof(ShopAccount.shop_name), false);

                List<ShopAccount> result = await query.FetchAsListAsync();

                return result;
            });
        }
    }
}
