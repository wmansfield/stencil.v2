using System;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
    public partial interface ITenantBusiness
    {
        Tenant GetByShop(Guid shop_id);
        Tenant CreateInitialTenant(Tenant insertIfEmpty);
    }
}
