using System;
using Microsoft.Azure.Cosmos;

namespace Placeholder.Primary.Business.Store.Factory
{
    public interface IPlaceholderCosmosClientFactory
    {
        CosmosClient CreateSharedClient(string collectionName, Action<ContainerProperties> customizeContainerCreation = null);
        CosmosClient CreateIsolatedClient(string tenant_code, string collectionName, Action<ContainerProperties> customizeContainerCreation = null);
        CosmosClient CreateSharedBulkClient(string collectionName, Action<ContainerProperties> customizeContainerCreation = null);
        CosmosClient CreateIsolatedBulkClient(string tenant_code, string collectionName, Action<ContainerProperties> customizeContainerCreation = null);
        string GetSharedDatabaseId();
        string GetIsolatedDatabaseId(string tenant_code);
    }
}
