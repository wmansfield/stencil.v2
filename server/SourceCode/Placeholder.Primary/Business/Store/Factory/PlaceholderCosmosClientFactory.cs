using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Placeholder.Common;
using Placeholder.Primary.Business.Store.Models;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Primary.Business.Store.Factory
{
    public class PlaceholderCosmosClientFactory : ChokeableClass, IPlaceholderCosmosClientFactory
    {
        public PlaceholderCosmosClientFactory(IFoundation foundation)
            : base(foundation)
        {
            this.API = foundation.Resolve<PlaceholderAPI>();
            this.Clients = new Dictionary<string, CosmosClient>();
            this.TenantCreation = new ConcurrentDictionary<string, TenantCreationData>(StringComparer.OrdinalIgnoreCase);
            this.ConnectionStringCache = new AspectCache("PlaceholderCosmosClientFactory", this.IFoundation);
        }

        public static object _SyncRoot = new object();

        protected AspectCache ConnectionStringCache { get; set; }


        public PlaceholderAPI API { get; set; }
        public Dictionary<string, CosmosClient> Clients { get; set; }

        public ConcurrentDictionary<string, TenantCreationData> TenantCreation { get; set; }

        
        protected string GetServerName(string tenant_code)
        {
            return base.ExecuteFunction(nameof(GetServerName), delegate ()
            {
                string key = string.Format(CommonAssumptions.APP_KEY_COSMOS_CACHE_SERVER_FORMAT, tenant_code);
                return this.ConnectionStringCache.PerFoundation("GetServerName" + key, delegate ()
                {
                    return this.API.Integration.Settings.GetSetting(key);
                });
            });
        }
        protected string GetAuthKey(string tenant_code)
        {
            return base.ExecuteFunction(nameof(GetAuthKey), delegate ()
            {
                string key = string.Format(CommonAssumptions.APP_KEY_COSMOS_CACHE_AUTH_KEY_FORMAT, tenant_code);
                return this.ConnectionStringCache.PerFoundation("GetAuthKey" + key, delegate ()
                {
                    return this.API.Integration.Settings.GetSetting(key);
                });
            });
        }

        public virtual string GetSharedDatabaseId()
        {
            return base.ExecuteFunction("GetSharedDatabaseId", delegate ()
            {
                return this.GetDatabaseId(CommonAssumptions.COSMOS_PRIMARY_CODE);
            });
        }
        public virtual string GetIsolatedDatabaseId(string tenant_code)
        {
            return base.ExecuteFunction("GetIsolatedDatabaseId", delegate ()
            {
                return this.GetDatabaseId(tenant_code);
            });
        }

        public virtual CosmosClient CreateSharedClient(string containerName, Action<ContainerProperties> customizeContainerCreation = null)
        {
            return base.ExecuteFunction("CreateSharedClient", delegate ()
            {
                return this.CreateIsolatedClient(CommonAssumptions.COSMOS_PRIMARY_CODE, containerName, customizeContainerCreation);
            });
        }
        public virtual CosmosClient CreateIsolatedClient(string tenant_code, string containerName, Action<ContainerProperties> customizeContainerCreation = null)
        {
            return base.ExecuteFunction("CreateIsolatedClient", delegate ()
            {
                CosmosClient client = null;
                this.Clients.TryGetValue(tenant_code, out client);

                string databaseName = this.GetDatabaseId(tenant_code);

                if (!this.TenantCreation.ContainsKey(tenant_code))
                {
                    lock(_SyncRoot)
                    {
                        if (!this.TenantCreation.ContainsKey(tenant_code))
                        {
                            this.TenantCreation[tenant_code] = new TenantCreationData();
                        }
                    }
                }

                TenantCreationData tenantData = this.TenantCreation[tenant_code];

                if (client == null)
                {
                    lock (_SyncRoot)
                    {
                        this.Clients.TryGetValue(tenant_code, out client);
                        if (client == null)
                        {
                            client = new CosmosClient(this.GetServerName(tenant_code), this.GetAuthKey(tenant_code), new CosmosClientOptions()
                            {
                                Serializer = new FilteredCosmosSerializer(new CosmosSerializationOptions()) 
                            });
                                                        
                            if (!tenantData.DatabasesEnsured.ContainsKey(databaseName))
                            {
                                client.CreateDatabaseIfNotExistsAsync(databaseName).Wait();
                                tenantData.DatabasesEnsured.TryAdd(databaseName, true);
                            }

                            this.Clients[tenant_code] = client;
                        }
                    }
                }

                if (!tenantData.ContainersEnsured.ContainsKey(containerName))
                {
                    lock (_SyncRoot)
                    {
                        if (!tenantData.ContainersEnsured.ContainsKey(containerName))
                        {
                            Database database = client.GetDatabase(databaseName);
                            ContainerProperties properties = new ContainerProperties();
                            properties.Id = containerName;
                            properties.DefaultTimeToLive = -1;
                            properties.IndexingPolicy = new IndexingPolicy();
                            properties.IndexingPolicy.Automatic = false;
                            properties.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath() { Path = "/" });
                            customizeContainerCreation?.Invoke(properties);


                            database.CreateContainerIfNotExistsAsync(properties).Wait();

                            tenantData.ContainersEnsured.TryAdd(containerName, true);
                        }
                    }
                }
                return client;
            });
        }
        public virtual CosmosClient CreateSharedBulkClient(string containerName, Action<ContainerProperties> customizeContainerCreation = null)
        {
            return base.ExecuteFunction("CreateSharedBulkClient", delegate ()
            {
                return this.CreateIsolatedBulkClient(CommonAssumptions.COSMOS_PRIMARY_CODE, containerName, customizeContainerCreation);
            });
        }
        public virtual CosmosClient CreateIsolatedBulkClient(string tenant_code, string containerName, Action<ContainerProperties> customizeContainerCreation = null)
        {
            return base.ExecuteFunction("CreateIsolatedBulkClient", delegate ()
            {
                CosmosClient client = new CosmosClient(this.GetServerName(tenant_code), this.GetAuthKey(tenant_code), new CosmosClientOptions()
                {
                    AllowBulkExecution = true,
                    ConnectionMode = ConnectionMode.Direct,
                });
                
                string databaseName = this.GetDatabaseId(tenant_code);

                if (!this.TenantCreation.ContainsKey(tenant_code))
                {
                    lock (_SyncRoot)
                    {
                        if (!this.TenantCreation.ContainsKey(tenant_code))
                        {
                            this.TenantCreation[tenant_code] = new TenantCreationData();
                        }
                    }
                }

                TenantCreationData tenantData = this.TenantCreation[tenant_code];


                if (!tenantData.DatabasesEnsured.ContainsKey(databaseName))
                {
                    lock (_SyncRoot)
                    {
                        if (!tenantData.DatabasesEnsured.ContainsKey(databaseName))
                        {
                            client.CreateDatabaseIfNotExistsAsync(databaseName).Wait();
                            tenantData.DatabasesEnsured.TryAdd(databaseName, true);
                        }
                    }
                }

                if (!tenantData.ContainersEnsured.ContainsKey(containerName))
                {
                    lock (_SyncRoot)
                    {
                        if (!tenantData.ContainersEnsured.ContainsKey(containerName))
                        {
                            ContainerProperties properties = new ContainerProperties();
                            properties.Id = containerName;
                            properties.DefaultTimeToLive = -1;
                            properties.IndexingPolicy = new IndexingPolicy();
                            properties.IndexingPolicy.Automatic = false;
                            properties.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath() { Path = "/" });
                            customizeContainerCreation?.Invoke(properties);

                            Database database = client.GetDatabase(databaseName);
                            database.CreateContainerIfNotExistsAsync(properties).Wait();

                            tenantData.ContainersEnsured.TryAdd(containerName, true);
                        }
                    }
                }

                return client;
            });
        }

        protected string GetDatabaseId(string tenant_code)
        {
            return base.ExecuteFunction(nameof(GetDatabaseId), delegate ()
            {
                string key = string.Format(CommonAssumptions.APP_KEY_COSMOS_CACHE_DATABASE_FORMAT, tenant_code);
                return this.ConnectionStringCache.PerFoundation("GetDatabaseId" + key, delegate ()
                {
                    return this.API.Integration.Settings.GetSetting(key);
                });
            });
        }

    }
}
