using System;
using System.Linq;
using System.Threading.Tasks;
using Placeholder.SDK;
using Placeholder.SDK.Models;

namespace Placeholder.Primary.Business.Store.Implementation
{
    public partial class ShopStore
    {
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        public Task<ListResult<Shop>> GetByShopifyUrlAsync(string shopify_url, int skip, int take, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction(nameof(GetByShopifyUrlAsync), delegate ()
            {
                if (string.IsNullOrWhiteSpace(shopify_url))
                {
                    return Task.FromResult(new ListResult<Shop>());
                }

                IQueryable<Shop> query = base.QuerySharedWithoutPartitionKey();

                query = query.Where(x => x.private_domain.Contains(shopify_url, StringComparison.OrdinalIgnoreCase));

                query = this.ApplySafeSort(query, order_by, descending);

                return query.FetchAsSteppedListAsync(skip, take);
            });
        }
    }
}
