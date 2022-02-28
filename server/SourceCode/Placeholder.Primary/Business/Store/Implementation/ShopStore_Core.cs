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
    public partial class ShopStore : StoreBase<Shop>, IShopStore
    {
        #region Constructor
        
        public ShopStore(IFoundation foundation)
            : base(foundation, "ShopStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(Shop.shop_id), nameof(Shop.tenant_id)  };
        private static string[] INDEX_DATE_FIELDS = new string[] {  };
        private static string[] INDEX_STRING_FIELDS = new string[] { nameof(Shop.shop_name), nameof(Shop.private_domain), nameof(Shop.public_domain) };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.Shop;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(Shop.partition_key);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(Shop.shop_id);
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

        protected override string ExtractPartitionKey(Shop entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.partition_key;
            });
        }

        protected override string ExtractPrimaryKey(Shop entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.shop_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        public Task<Shop> GetDocumentAsync(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                
                string partitionKey = new Shop(){ shop_id = shop_id}.partition_key;

                Shop foundShop = await base.RetrieveByIdSharedAsync(partitionKey, shop_id.ToString());

                return foundShop;
            });
        }
        


        public Task<bool> CreateDocumentAsync(Shop model)
        {
            return base.ExecuteFunction(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<Shop> result = await base.UpsertSharedAsync(model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(Shop model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveSharedAsync(model);
            });
        }

        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        public Task<ListResult<Shop>> FindAsync(int skip, int take, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunction(nameof(FindAsync), async delegate ()
            {
                
                IQueryable<Shop> query = base.QuerySharedWithoutPartitionKey();
                
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => 
                        x.shop_name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.private_domain.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.public_domain.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    );
                }
                

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<Shop> result = await query.FetchAsSteppedListAsync(skip, take);
                
                return result;
            });
        }

        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Shop> ApplySafeSort(IQueryable<Shop> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Shop> ApplySafeSort(IQueryable<Shop> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<Shop> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            case nameof(Shop.shop_name):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.shop_name);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.shop_name);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.shop_name);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.shop_name);
                                    }
                                }
                                break;
                            case nameof(Shop.private_domain):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.private_domain);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.private_domain);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.private_domain);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.private_domain);
                                    }
                                }
                                break;
                            case nameof(Shop.public_domain):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.public_domain);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.public_domain);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.public_domain);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.public_domain);
                                    }
                                }
                                break;
                            
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


