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
    public partial class ShopSettingStore : StoreBase<ShopSetting>, IShopSettingStore
    {
        #region Constructor
        
        public ShopSettingStore(IFoundation foundation)
            : base(foundation, "ShopSettingStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(ShopSetting.shop_setting_id), nameof(ShopSetting.shop_id)  };
        private static string[] INDEX_DATE_FIELDS = new string[] {  };
        private static string[] INDEX_STRING_FIELDS = new string[] { nameof(ShopSetting.name), nameof(ShopSetting.value) };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.ShopSetting;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(ShopSetting.shop_id);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(ShopSetting.shop_setting_id);
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

        protected override string ExtractPartitionKey(ShopSetting entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.shop_id.ToString();;
            });
        }

        protected override string ExtractPrimaryKey(ShopSetting entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.shop_setting_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        public Task<ShopSetting> GetDocumentAsync(Guid shop_id, Guid shop_setting_id)
        {
            return base.ExecuteFunctionAsync(nameof(GetDocumentAsync), async delegate ()
            {
                ShopSetting found = await this.RetrieveByIdIsolatedAsync(shop_id, shop_id.ToString(), shop_setting_id.ToString());
                

                return found;
            });
        }
        


        public Task<bool> CreateDocumentAsync(ShopSetting model)
        {
            return base.ExecuteFunctionAsync(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<ShopSetting> result = await base.UpsertIsolatedAsync(model.shop_id, model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(ShopSetting model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveIsolatedAsync(model.shop_id, model);
            });
        }

        
        public Task<ListResult<ShopSetting>> FindForShopAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunctionAsync(nameof(FindForShopAsync), async delegate ()
            {
                IQueryable<ShopSetting> query = this.QueryByPartitionIsolated(shop_id, shop_id.ToString());
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => 
                        x.name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.value.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    );
                }
                

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<ShopSetting> result = await query.FetchAsSteppedListAsync(skip, take);
                
                return result;
            });
        }

        
        public Task DeleteForShopAsync(Guid shop_id)
        {
            return base.ExecuteMethodAsync(nameof(DeleteForShopAsync), async delegate ()
            {
                List<ShopSetting> deleteItems = await base.RetrieveByPartitionIsolatedAsync(shop_id, shop_id.ToString());

                await base.BulkRemoveIsolatedAsync(shop_id, deleteItems);
            });
        }
        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<ShopSetting> ApplySafeSort(IQueryable<ShopSetting> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<ShopSetting> ApplySafeSort(IQueryable<ShopSetting> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<ShopSetting> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            case nameof(ShopSetting.name):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.name);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.name);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.name);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.name);
                                    }
                                }
                                break;
                            case nameof(ShopSetting.value):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.value);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.value);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.value);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.value);
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
                    result = query.OrderBy(x => x.shop_setting_id);
                }
                return result;
                
            });
        }

        

        #endregion
    }
}


