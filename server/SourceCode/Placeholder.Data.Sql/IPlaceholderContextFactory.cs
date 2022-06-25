using System;
using Placeholder.Data.Sql.Models;

namespace Placeholder.Data.Sql
{
    public interface IPlaceholderContextFactory
    {
        PlaceholderContext CreateSharedContext();
        PlaceholderContext CreateIsolatedContext(string tenant_code);
    }
}
