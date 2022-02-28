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
    public partial class ShopAccountStore : StoreBase<ShopAccount>, IShopAccountStore
    {
        #region Constructor
        
        public ShopAccountStore(IFoundation foundation)
            : base(foundation, "ShopAccountStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(ShopAccount.shop_account_id), nameof(ShopAccount.shop_id), nameof(ShopAccount.account_id)  };
        private static string[] INDEX_DATE_FIELDS = new string[] {  };
        private static string[] INDEX_STRING_FIELDS = new string[] { nameof(ShopAccount.shop_name) };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.ShopAccount;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(ShopAccount.shop_id);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(ShopAccount.shop_account_id);
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

        protected override string ExtractPartitionKey(ShopAccount entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.shop_id.ToString();;
            });
        }

        protected override string ExtractPrimaryKey(ShopAccount entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.shop_account_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        public Task<ShopAccount> GetDocumentAsync(Guid shop_account_id)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                ShopAccount foundShopAccount = await base.RetrieveByIdSharedWithoutPartitionKeyAsync(shop_account_id.ToString());

                return foundShopAccount;
                
            });
        }
        
        public Task<ShopAccount> GetDocumentAsync(Guid shop_id, Guid shop_account_id)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                ShopAccount found = await this.RetrieveByIdSharedAsync(shop_id.ToString(), shop_account_id.ToString());
                

                return found;
            });
        }
        


        public Task<bool> CreateDocumentAsync(ShopAccount model)
        {
            return base.ExecuteFunction(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<ShopAccount> result = await base.UpsertSharedAsync(model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(ShopAccount model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveSharedAsync(model);
            });
        }

        
        public Task<ListResult<ShopAccount>> FindForShopAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false, string keyword = "", bool? enabled = true)
        {
            return base.ExecuteFunction(nameof(FindForShopAsync), async delegate ()
            {
                IQueryable<ShopAccount> query = this.QueryByPartitionShared(shop_id.ToString());
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => 
                        x.shop_name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    );
                }
                
                if(enabled.HasValue)
                {
                    query = query.Where(x => x.enabled == enabled.Value);
                }
                

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<ShopAccount> result = await query.FetchAsSteppedListAsync(skip, take);
                
                return result;
            });
        }

        
        public Task DeleteForShopAsync(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(DeleteForShopAsync), async delegate ()
            {
                List<ShopAccount> deleteItems = await base.RetrieveByPartitionSharedAsync(shop_id.ToString());

                return base.BulkRemoveSharedAsync(deleteItems);
            });
        }
        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<ShopAccount> ApplySafeSort(IQueryable<ShopAccount> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<ShopAccount> ApplySafeSort(IQueryable<ShopAccount> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<ShopAccount> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            case nameof(ShopAccount.shop_role):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.shop_role);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.shop_role);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.shop_role);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.shop_role);
                                    }
                                }
                                break;
                            case nameof(ShopAccount.shop_name):
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
                            
                            default:
                                // nothing
                                break;
                        }
                    }
                }
                if(result == null)
                {
                    result = query.OrderBy(x => x.shop_account_id);
                }
                return result;
                
            });
        }

        

        #endregion
    }
}


