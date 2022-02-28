using System;

namespace Placeholder.Data.Sql
{
    public interface IPlaceholderContextFactory
    {
        PlaceholderContext CreateSharedContext();
        PlaceholderContext CreateIsolatedContext(string tenant_code);
    }
}
