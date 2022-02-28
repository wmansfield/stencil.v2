using System;
using System.Collections.Generic;
using Nest;

namespace Placeholder.Primary.Business.Index
{
    public interface IPlaceholderElasticClientFactory
    {
        IElasticClient CreateReadClient();
        List<IElasticClient> CreateWriteClients();
        string IndexName { get; }
    }
}
