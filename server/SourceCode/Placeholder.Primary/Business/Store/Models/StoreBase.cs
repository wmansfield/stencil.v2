using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Placeholder.Primary.Business.Store.Factory;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;

namespace Placeholder.Primary.Business.Store
{
    public abstract class StoreBase<TEntity> : StoreBaseHealth
        where TEntity : class
    {
        public StoreBase(IFoundation foundation, string trackPrefix)
            : base(foundation, trackPrefix)
        {
            this.API = foundation.Resolve<PlaceholderAPI>();
            this.SharedCacheStatic15 = new AspectCache("StoreBase" + trackPrefix, foundation, new ExpireStaticLifetimeManager("StoreBase" + trackPrefix, TimeSpan.FromMinutes(15)));
            this.SharedCacheStatic2 = new AspectCache("StoreBase.2", foundation, new ExpireStaticLifetimeManager("StoreBase.2", TimeSpan.FromMinutes(2)));
        }


        public AspectCache SharedCacheStatic15 { get; set; }
        public AspectCache SharedCacheStatic2 { get; set; }
        public PlaceholderAPI API { get; set; }

        public virtual string[] IgnoredFields { get; }

        public abstract string ContainerName { get; }
        public abstract string PartitionKeyField { get; }
        public abstract string PrimaryKeyField { get; }
        public abstract string[] IndexGuidFields { get; }
        public abstract string[] IndexDateFields { get; }
        public abstract string[] IndexStringFields { get; }
        public abstract string[] IndexNumberFields { get; }
        public abstract SortInfo[][] IndexComposites { get; }


        protected virtual FilteredSerializerContractResolver<TEntity> ContractResolver { get; set; }

        protected virtual IPlaceholderCosmosClientFactory ClientFactory
        {
            get
            {
                return this.IFoundation.Resolve<IPlaceholderCosmosClientFactory>();
            }
        }

        protected abstract string ExtractPrimaryKey(TEntity entity);
        protected abstract string ExtractPartitionKey(TEntity entity);


        protected virtual Task<TEntity> RetrieveByIdSharedAsync(string partitionKey, string id)
        {
            return base.ExecuteFunctionInternal<Task<TEntity>>("RetrieveByIdSharedAsync", async delegate ()
            {
                try
                {
                    Container container = this.CreateSharedContainer();

                    ItemRequestOptions requestOptions = new ItemRequestOptions()
                    {
                        ConsistencyLevel = ConsistencyLevel.Strong,
                    };

                    ItemResponse<TEntity> result = await container.ReadItemAsync<TEntity>(id, new PartitionKey(partitionKey), requestOptions);

                    result.TrackRequestCharge();

                    return result;
                }
                catch (CosmosException cex)
                when (cex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            });
        }
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        protected virtual Task<TEntity> RetrieveByIdSharedWithoutPartitionKeyAsync(string id)
        {
            return base.ExecuteFunctionInternal<Task<TEntity>>("RetrieveByIdSharedWithoutPartitionKeyAsync", async delegate ()
            {
                try
                {
                    Container container = this.CreateSharedContainer();

                    QueryRequestOptions requestOptions = new QueryRequestOptions()
                    {
                        EnableScanInQuery = true,
                        MaxItemCount = 1,
                        ConsistencyLevel = ConsistencyLevel.Strong,
                    };

                    QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                                                .WithParameter("@id", id);

                    FeedIterator<TEntity> data = container.GetItemQueryIterator<TEntity>(query, null, requestOptions);
                    FeedResponse<TEntity> result = await data.ReadNextAsync();

                    result.TrackRequestCharge();

                    return result.FirstOrDefault();
                }
                catch (CosmosException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            });
        }


        protected virtual Task<TEntity> RetrieveByIdIsolatedAsync(Guid shard_id, string partitionKey, string id)
        {
            return base.ExecuteFunctionInternal<Task<TEntity>>("RetrieveByIdIsolatedAsync", async delegate ()
            {
                try
                {
                    Container container = this.CreateIsolatedContainer(shard_id);

                    ItemResponse<TEntity> result = await container.ReadItemAsync<TEntity>(id, new PartitionKey(partitionKey), new ItemRequestOptions() { ConsistencyLevel = ConsistencyLevel.Strong });

                    result.TrackRequestCharge();

                    return result;
                }
                catch (CosmosException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            });
        }
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        protected virtual Task<TEntity> RetrieveByIdIsolatedWithoutPartitionKeyAsync(Guid shard_id, string id)
        {
            return base.ExecuteFunctionInternal<Task<TEntity>>("RetrieveByIdIsolatedWithoutPartitionKeyAsync", async delegate ()
            {
                try
                {
                    Container container = this.CreateIsolatedContainer(shard_id);

                    QueryRequestOptions requestOptions = new QueryRequestOptions()
                    {
                        EnableScanInQuery = true,
                        MaxItemCount = 1,
                        ConsistencyLevel = ConsistencyLevel.Strong,
                    };

                    QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                                                .WithParameter("@id", id);

                    FeedIterator<TEntity> data = container.GetItemQueryIterator<TEntity>(query, null, requestOptions);
                    FeedResponse<TEntity> result = await data.ReadNextAsync();

                    result.TrackRequestCharge();

                    return result.FirstOrDefault();
                }
                catch (CosmosException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            });
        }


        protected virtual Task<List<TEntity>> RetrieveByPartitionSharedAsync(string partitionKey)
        {
            return base.ExecuteFunctionInternal<Task<List<TEntity>>>("RetrieveByPartitionSharedAsync", delegate ()
            {
                Container container = this.CreateSharedContainer();

                QueryRequestOptions requestOptions = new QueryRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong,
                    PartitionKey = new PartitionKey(partitionKey)
                };

                return container.GetItemLinqQueryable<TEntity>(true, null, requestOptions)
                                .FetchAsListAsync();
            });
        }
        protected virtual Task<List<TEntity>> RetrieveByPartitionIsolatedAsync(Guid shard_id, string partitionKey)
        {
            return base.ExecuteFunctionInternal("RetrieveByPartitionIsolatedAsync", delegate ()
            {
                Container container = this.CreateIsolatedContainer(shard_id);

                QueryRequestOptions requestOptions = new QueryRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong,
                    PartitionKey = new PartitionKey(partitionKey)
                };

                return container.GetItemLinqQueryable<TEntity>(true, null, requestOptions)
                                .FetchAsListAsync();
            });
        }


        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        protected virtual IOrderedQueryable<TEntity> QuerySharedWithoutPartitionKey()
        {
            return base.ExecuteFunctionInternal("QuerySharedWithoutPartitionKey", delegate ()
            {
                //TODO:SHOULD: add metric for DTU consumption here
                Container container = this.CreateSharedContainer();

                QueryRequestOptions requestOptions = new QueryRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong,
                };


                return container.GetItemLinqQueryable<TEntity>(true, null, requestOptions);
            });
        }
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        protected virtual IOrderedQueryable<TEntity> QueryIsolatedWithoutPartitionKey(Guid shard_id)
        {
            return base.ExecuteFunctionInternal("QueryIsolatedWithoutPartitionKey", delegate ()
            {
                Container container = this.CreateIsolatedContainer(shard_id);

                QueryRequestOptions requestOptions = new QueryRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong,
                };

                return container.GetItemLinqQueryable<TEntity>(true, null, requestOptions);
            });
        }


        protected virtual IOrderedQueryable<TEntity> QueryByPartitionShared(string partitionKey)
        {
            return base.ExecuteFunctionInternal("QueryByPartitionShared", delegate ()
            {
                Container container = this.CreateSharedContainer();

                QueryRequestOptions requestOptions = new QueryRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong,
                    PartitionKey = new PartitionKey(partitionKey),
                };

                return container.GetItemLinqQueryable<TEntity>(true, null, requestOptions);

            });
        }
        protected virtual IOrderedQueryable<TEntity> QueryByPartitionIsolated(Guid shard_id, string partitionKey)
        {
            return base.ExecuteFunctionInternal("QueryByPartitionIsolated", delegate ()
            {
                Container container = this.CreateIsolatedContainer(shard_id);

                QueryRequestOptions requestOptions = new QueryRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong,
                    PartitionKey = new PartitionKey(partitionKey)
                };

                return container.GetItemLinqQueryable<TEntity>(true, null, requestOptions);
            });
        }


        protected virtual Task<ItemResponse<TEntity>> UpsertSharedAsync(TEntity entity)
        {
            return base.ExecuteFunctionWriteInternal("UpsertSharedAsync", async delegate ()
            {
                Container container = this.CreateSharedContainer();

                string partitionKey = this.ExtractPartitionKey(entity);

                ItemRequestOptions requestOptions = new ItemRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong
                };

                ItemResponse<TEntity> result = await container.UpsertItemAsync<TEntity>(entity, new PartitionKey(partitionKey), requestOptions);

                result.TrackRequestCharge();

                return result;
            });
        }
        protected virtual Task<ItemResponse<TEntity>> UpsertIsolatedAsync(Guid shard_id, TEntity entity)
        {
            return base.ExecuteFunctionWriteInternal("UpsertIsolatedAsync", async delegate ()
            {
                Container container = this.CreateIsolatedContainer(shard_id);

                string partitionKey = this.ExtractPartitionKey(entity);

                ItemRequestOptions requestOptions = new ItemRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong
                };

                ItemResponse<TEntity> result = await container.UpsertItemAsync<TEntity>(entity, new PartitionKey(partitionKey), requestOptions);

                result.TrackRequestCharge();

                return result;
            });
        }


        protected virtual Task<bool> RemoveSharedAsync(TEntity entity)
        {
            return base.ExecuteFunctionWriteInternal("RemoveSharedAsync", async delegate ()
            {
                try
                {
                    Container container = this.CreateSharedContainer();

                    string primaryKey = this.ExtractPrimaryKey(entity);
                    string partitionKey = this.ExtractPartitionKey(entity);

                    ItemRequestOptions requestOptions = new ItemRequestOptions()
                    {
                        ConsistencyLevel = ConsistencyLevel.Strong
                    };


                    ItemResponse<TEntity> result = await container.DeleteItemAsync<TEntity>(primaryKey, new PartitionKey(partitionKey), requestOptions);

                    result.TrackRequestCharge();

                    return result.StatusCode.IsSuccessOrMissing();
                }
                catch (CosmosException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return true;
                }
            });
        }
        protected virtual Task<bool> RemoveIsolatedAsync(Guid shard_id, TEntity entity)
        {
            return base.ExecuteFunctionWriteInternal("RemoveIsolatedAsync", async delegate ()
            {
                try
                {
                    Container container = this.CreateIsolatedContainer(shard_id);

                    string primaryKey = this.ExtractPrimaryKey(entity);
                    string partitionKey = this.ExtractPartitionKey(entity);

                    ItemRequestOptions requestOptions = new ItemRequestOptions()
                    {
                        ConsistencyLevel = ConsistencyLevel.Strong
                    };


                    ItemResponse<TEntity> result = await container.DeleteItemAsync<TEntity>(primaryKey, new PartitionKey(partitionKey), requestOptions);

                    result.TrackRequestCharge();

                    return result.StatusCode.IsSuccessOrMissing();
                }
                catch (CosmosException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return true;
                }
            });
        }


        protected virtual Task<BulkOperationResponse<TEntity>> BulkUpsertSharedAsync(List<TEntity> entities)
        {
            return base.ExecuteFunctionWriteInternal("BulkUpsertSharedAsync", delegate ()
            {
                Container container = this.CreateSharedContainer();

                ItemRequestOptions requestOptions = new ItemRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong
                };

                BulkOperations<TEntity> bulkOperations = new BulkOperations<TEntity>(entities.Count);
                foreach (TEntity item in entities)
                {
                    bulkOperations.Tasks.Add(CaptureOperationResponse(container.UpsertItemAsync(item, new PartitionKey(this.ExtractPartitionKey(item)), requestOptions), item));
                }
                return bulkOperations.ExecuteAsync();
            });
        }
        protected virtual Task<BulkOperationResponse<TEntity>> BulkUpsertIsolatedAsync(Guid shard_id, List<TEntity> entities)
        {
            return base.ExecuteFunctionWriteInternal("BulkUpsertIsolatedAsync", delegate ()
            {
                Container container = this.CreateIsolatedContainer(shard_id);

                ItemRequestOptions requestOptions = new ItemRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong
                };

                BulkOperations<TEntity> bulkOperations = new BulkOperations<TEntity>(entities.Count);
                foreach (TEntity item in entities)
                {
                    bulkOperations.Tasks.Add(CaptureOperationResponse(container.UpsertItemAsync(item, new PartitionKey(this.ExtractPartitionKey(item)), requestOptions), item));
                }
                return bulkOperations.ExecuteAsync();
            });
        }


        protected virtual Task<BulkOperationResponse<TEntity>> BulkRemoveSharedAsync(List<TEntity> entities)
        {
            return base.ExecuteFunctionWriteInternal("BulkRemoveSharedAsync", delegate ()
            {

                Container container = this.CreateSharedContainer();

                ItemRequestOptions requestOptions = new ItemRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong
                };

                BulkOperations<TEntity> bulkOperations = new BulkOperations<TEntity>(entities.Count);
                foreach (TEntity item in entities)
                {
                    bulkOperations.Tasks.Add(CaptureOperationResponse(container.DeleteItemAsync<TEntity>(this.ExtractPrimaryKey(item), new PartitionKey(this.ExtractPartitionKey(item)), requestOptions), item));
                }
                return bulkOperations.ExecuteAsync();

            });
        }
        protected virtual Task<BulkOperationResponse<TEntity>> BulkRemoveIsolatedAsync(Guid shard_id, List<TEntity> entities)
        {
            return base.ExecuteFunctionWriteInternal("BulkRemoveIsolatedAsync", delegate ()
            {
                Container container = this.CreateIsolatedContainer(shard_id);

                ItemRequestOptions requestOptions = new ItemRequestOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Strong
                };

                BulkOperations<TEntity> bulkOperations = new BulkOperations<TEntity>(entities.Count);
                foreach (TEntity item in entities)
                {
                    bulkOperations.Tasks.Add(CaptureOperationResponse(container.DeleteItemAsync<TEntity>(this.ExtractPrimaryKey(item), new PartitionKey(this.ExtractPartitionKey(item)), requestOptions), item));
                }
                return bulkOperations.ExecuteAsync();

            });
        }

        private static async Task<OperationResponse<T>> CaptureOperationResponse<T>(Task<ItemResponse<T>> task, T item)
        {
            try
            {
                ItemResponse<T> response = await task;
                return new OperationResponse<T>()
                {
                    Item = item,
                    IsSuccessful = true,
                    RequestUnitsConsumed = task.Result.RequestCharge
                };
            }
            catch (Exception ex)
            {
                if (ex is CosmosException cosmosException)
                {
                    return new OperationResponse<T>()
                    {
                        Item = item,
                        RequestUnitsConsumed = cosmosException.RequestCharge,
                        IsSuccessful = false,
                        CosmosException = cosmosException
                    };
                }

                return new OperationResponse<T>()
                {
                    Item = item,
                    IsSuccessful = false,
                    CosmosException = ex
                };
            }
        }


        protected virtual void ConfigureContainer(ContainerProperties collection)
        {
            base.ExecuteMethodByPassHealth("ConfigureContainer", delegate ()
            {
                if (this.IndexGuidFields != null)
                {
                    foreach (string item in this.IndexGuidFields)
                    {
                        collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
                        {
                            Path = string.Format("/{0}/?", item.TrimStart('/'))
                        });
                    }
                }
                if (this.IndexDateFields != null)
                {
                    foreach (string item in this.IndexDateFields)
                    {
                        collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
                        {
                            Path = string.Format("/{0}/?", item.TrimStart('/')),
                        });
                    }
                }
                if (this.IndexStringFields != null)
                {
                    foreach (string item in this.IndexStringFields)
                    {
                        collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
                        {
                            Path = string.Format("/{0}/?", item.TrimStart('/'))
                        });
                    }
                }
                if (this.IndexNumberFields != null)
                {
                    foreach (string item in this.IndexNumberFields)
                    {
                        collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
                        {
                            Path = string.Format("/{0}/?", item.TrimStart('/'))
                        });
                    }
                }
                if (this.IndexComposites != null)
                {
                    foreach (SortInfo[] composite in this.IndexComposites)
                    {
                        Collection<CompositePath> composites = new Collection<CompositePath>();

                        foreach (SortInfo info in composite)
                        {
                            if (!string.IsNullOrWhiteSpace(info.field))
                            {
                                composites.Add(new CompositePath() { Path = info.field, Order = info.descending ? CompositePathSortOrder.Descending : CompositePathSortOrder.Ascending });
                            }
                        }

                        if (composites.Count > 0)
                        {
                            collection.IndexingPolicy.CompositeIndexes.Add(composites);
                        }
                    }
                }

                collection.PartitionKeyPath = string.Format("/{0}", this.PartitionKeyField.TrimStart('/'));
            });
        }

        protected virtual FilteredSerializerContractResolver<TEntity> GenerateSettings()
        {
            return base.ExecuteFunctionByPassHealth("GenerateSettings", delegate ()
            {
                FilteredSerializerContractResolver<TEntity> resolver = new FilteredSerializerContractResolver<TEntity>();
                resolver.RenameProperty(this.PrimaryKeyField, "id");
                if (this.IgnoredFields != null)
                {
                    foreach (string item in this.IgnoredFields)
                    {
                        resolver.IgnoreProperty(item);
                    }
                }
                return resolver;
            });
        }


        public Task<PermissionInfo> GenerateReadPermissionForPartitionSharedAsync(Guid user_id, string partitionKey, string permissionId = "perm.standard.read")
        {
            return base.ExecuteFunction("GenerateReadPermissionForPartitionSharedAsync", async delegate ()
            {
                CosmosClient client = this.CreateSharedClient();
                string databaseId = this.ClientFactory.GetSharedDatabaseId();

                Database database = client.GetDatabase(databaseId);
                Container container = database.GetContainer(this.ContainerName);

                PermissionInfo result = new PermissionInfo()
                {
                    user_id = user_id,
                    expires_utc = DateTime.UtcNow.AddHours(1) // default
                };

                User user = null;
                try
                {
                    user = await database.CreateUserAsync(user_id.ToString());
                }
                catch (CosmosException cex)
                when (cex.StatusCode == HttpStatusCode.Conflict)
                {
                    // allow
                    user = database.GetUser(user_id.ToString());
                }

                try
                {
                    PermissionProperties permissionProperties = new PermissionProperties(permissionId, PermissionMode.Read, container, new PartitionKey(partitionKey));

                    PermissionResponse response = await user.CreatePermissionAsync(permissionProperties);
                    if (response?.Resource?.Token != null)
                    {
                        result.token = response.Resource.Token;
                    }
                }
                catch (CosmosException cex)
                when (cex.StatusCode == HttpStatusCode.Conflict)
                {
                    // retrieve
                    PermissionResponse response = await user.GetPermission(permissionId).ReadAsync();
                    if (response?.Resource?.Token != null)
                    {
                        result.token = response.Resource.Token;
                    }
                }
                return result;
            });

        }
        public Task<PermissionInfo> GenerateReadPermissionForPartitionIsolatedAsync(Guid shard_id, Guid user_id, string partitionKey, string permissionId = "perm.standard.read")
        {
            return base.ExecuteFunction(nameof(GenerateReadPermissionForPartitionIsolatedAsync), async delegate ()
            {
                string tenant_code = this.ResolveTenant(shard_id);
                CosmosClient client = this.CreateIsolatedClient(tenant_code);
                string databaseId = this.ClientFactory.GetIsolatedDatabaseId(tenant_code);

                Database database = client.GetDatabase(databaseId);
                Container container = database.GetContainer(this.ContainerName);

                PermissionInfo result = new PermissionInfo()
                {
                    user_id = user_id,
                    expires_utc = DateTime.UtcNow.AddHours(1) // default
                };

                User user = null;
                try
                {
                    user = await database.CreateUserAsync(user_id.ToString());
                }
                catch (CosmosException cex)
                when (cex.StatusCode == HttpStatusCode.Conflict)
                {
                    // allow
                    user = database.GetUser(user_id.ToString());
                }

                try
                {
                    PermissionProperties permissionProperties = new PermissionProperties(permissionId, PermissionMode.Read, container, new PartitionKey(partitionKey));

                    PermissionResponse response = await user.CreatePermissionAsync(permissionProperties);
                    if (response?.Resource?.Token != null)
                    {
                        result.token = response.Resource.Token;
                    }
                }
                catch (CosmosException cex)
                when (cex.StatusCode == HttpStatusCode.Conflict)
                {
                    // retrieve
                    PermissionResponse response = await user.GetPermission(permissionId).ReadAsync();
                    if (response?.Resource?.Token != null)
                    {
                        result.token = response.Resource.Token;
                    }
                }
                return result;
            });
        }

        protected virtual string ResolveTenant(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(ResolveTenant), delegate ()
            {
                return this.SharedCacheStatic2.PerLifetime(string.Format("IsolatedShop{0}", shop_id), delegate ()
                {
                    Domain.Tenant tenant = this.API.Direct.Tenants.GetByShop(shop_id);
                    if (tenant != null)
                    {
                        return tenant.tenant_code;
                    }
                    return null;
                });
            });
        }

        protected virtual Container CreateSharedContainer()
        {
            return base.ExecuteFunctionByPassHealth(nameof(CreateSharedContainer), delegate ()
            {
                CosmosClient client = this.CreateSharedClient();
                string databaseId = this.ClientFactory.GetSharedDatabaseId();

                Database database = client.GetDatabase(databaseId);
                Container container = database.GetContainer(this.ContainerName);
                return container;
            });
        }
        protected virtual Container CreateIsolatedContainer(Guid shard_id)
        {
            return base.ExecuteFunctionByPassHealth(nameof(CreateIsolatedContainer), delegate ()
            {
                string tenant_code = this.ResolveTenant(shard_id);

                CosmosClient client = this.CreateIsolatedClient(tenant_code);
                string databaseId = this.ClientFactory.GetIsolatedDatabaseId(tenant_code);

                Database database = client.GetDatabase(databaseId);
                Container container = database.GetContainer(this.ContainerName);
                return container;
            });
        }

        protected virtual CosmosClient CreateSharedClient()
        {
            return base.ExecuteFunctionByPassHealth(nameof(CreateSharedClient), delegate ()
            {
                CosmosClient result = this.ClientFactory.CreateSharedClient(this.ContainerName, this.ConfigureContainer);
                FilteredCosmosSerializer serializer = result?.ClientOptions?.Serializer as FilteredCosmosSerializer;
                if (serializer != null)
                {
                    serializer.EnsureSerializer<TEntity>(this.GenerateSettings);
                }
                return result;
            });
        }
        protected virtual CosmosClient CreateIsolatedClient(string tenant_code)
        {
            return base.ExecuteFunctionByPassHealth(nameof(CreateIsolatedClient), delegate ()
            {
                CosmosClient result = this.ClientFactory.CreateIsolatedClient(tenant_code, this.ContainerName, this.ConfigureContainer);
                FilteredCosmosSerializer serializer = result?.ClientOptions?.Serializer as FilteredCosmosSerializer;
                if (serializer != null)
                {
                    serializer.EnsureSerializer<TEntity>(this.GenerateSettings);
                }
                return result;
            });
        }

    }
}
