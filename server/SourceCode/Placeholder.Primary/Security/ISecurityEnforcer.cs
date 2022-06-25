using Placeholder.Domain;
using System;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Security
{
    public partial interface ISecurityEnforcer
    {
        T CachedExecute<T>(string entity, Guid identifier, Func<Guid, T> method);
        T CachedExecute<T>(string entity, Guid? route, Guid identifier, Func<Guid, Guid, T> method);

        bool IsShopRole(Account account, Guid shop_id, sdk.ShopRole? role);
        bool IsAnyShopRole(Account account, Guid shop_id);

    }
}
