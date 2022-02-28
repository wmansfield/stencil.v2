using Placeholder.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Store
{
    public partial interface IShopAccountStore
    {
        [Obsolete("Use caution, this is expensive", false)]
        Task<List<ShopAccount>> GetForAccountAsync(Guid account_id, bool? enabled = true);
    }
}
