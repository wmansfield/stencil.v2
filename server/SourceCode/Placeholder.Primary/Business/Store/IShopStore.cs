using System;
using System.Threading.Tasks;
using Placeholder.SDK;
using Placeholder.SDK.Models;

namespace Placeholder.Primary.Business.Store
{
    public partial interface IShopStore
    {
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        Task<ListResult<Shop>> GetByShopifyUrlAsync(string shopify_url, int skip, int take, string order_by = "", bool descending = false);
    }
}
