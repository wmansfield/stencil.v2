using System;
using System.Collections.Generic;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
    public partial interface IShopBusiness
    {
        Shop GetByIdFromIsolated(Guid shop_id);
    }
}
