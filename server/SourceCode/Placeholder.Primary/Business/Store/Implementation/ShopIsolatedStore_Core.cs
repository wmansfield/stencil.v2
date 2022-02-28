using Zero.Foundation;
using Placeholder.Primary.Business.Store.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Placeholder.SDK;
using Placeholder.SDK.Models;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Business.Store.Implementation
{
    public partial class ShopIsolatedStore : StoreBase<ShopIsolated>, IShopIsolatedStore
    {
        #region Constructor
        
        public ShopIsolatedStore(IFoundation foundation)
            : base(foundation, "ShopIsolatedStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(ShopIsolated.shop_id)  };
        private static string[] INDEX_DATE_FIELDS = new string[] {  };
        private static string[] INDEX_STRING_FIELDS = new string[] {  };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.ShopIsolated;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(ShopIsolated.partition_key);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(ShopIsolated.shop_id);
            }
        }


        public override string[] IndexGuidFields
        {
            get
            {
                return INDEX_GUID_FIELDS;
            }
        }

        public override string[] IndexDateFields
        {
            get
            {
                return INDEX_DATE_FIELDS;
            }
        }

        public override string[] IndexStringFields
        {
            get
            {
                return INDEX_STRING_FIELDS;
            }
        }
        public override string[] IndexNumberFields
        {
            get
            {
                return INDEX_NUMBER_FIELDS;
            }
        }
        public override SortInfo[][] IndexComposites
        {
            get
            {
                return INDEX_COMPOSITES;
            }
        }

        #endregion

        #region Overrides

        protected override string ExtractPartitionKey(ShopIsolated entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.partition_key;
            });
        }

        protected override string ExtractPrimaryKey(ShopIsolated entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.shop_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        public Task<ShopIsolated> GetDocumentAsync(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                
                string partitionKey = new ShopIsolated(){ shop_id = shop_id}.partition_key;

                ShopIsolated foundShopIsolated = await base.RetrieveByIdIsolatedAsync(shop_id, partitionKey, shop_id.ToString());

                return foundShopIsolated;
            });
        }
        


        public Task<bool> CreateDocumentAsync(ShopIsolated model)
        {
            return base.ExecuteFunction(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<ShopIsolated> result = await base.UpsertIsolatedAsync(model.shop_id, model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(ShopIsolated model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveIsolatedAsync(model.shop_id, model);
            });
        }

        
        public Task<ListResult<ShopIsolated>> FindAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction(nameof(FindAsync), async delegate ()
            {
                
                IQueryable<ShopIsolated> query = base.QueryByPartitionIsolated(shop_id, ShopIsolated.GLOBAL_PARTITION);
                

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<ShopIsolated> result = await query.FetchAsSteppedListAsync(skip, take);
                
                return result;
            });
        }

        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<ShopIsolated> ApplySafeSort(IQueryable<ShopIsolated> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<ShopIsolated> ApplySafeSort(IQueryable<ShopIsolated> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<ShopIsolated> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            
                            default:
                                // nothing
                                break;
                        }
                    }
                }
                if(result == null)
                {
                    result = query.OrderBy(x => x.shop_id);
                }
                return result;
                
            });
        }

        

        #endregion
    }
}


