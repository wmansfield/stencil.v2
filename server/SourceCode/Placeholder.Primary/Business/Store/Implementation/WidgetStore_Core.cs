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
    public partial class WidgetStore : StoreBase<Widget>, IWidgetStore
    {
        #region Constructor
        
        public WidgetStore(IFoundation foundation)
            : base(foundation, "WidgetStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(Widget.widget_id), nameof(Widget.shop_id)  };
        private static string[] INDEX_DATE_FIELDS = new string[] { nameof(Widget.stamp_utc) };
        private static string[] INDEX_STRING_FIELDS = new string[] {  };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.Widget;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(Widget.shop_id);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(Widget.widget_id);
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

        protected override string ExtractPartitionKey(Widget entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.shop_id.ToString();;
            });
        }

        protected override string ExtractPrimaryKey(Widget entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.widget_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        public Task<Widget> GetDocumentAsync(Guid shop_id, Guid widget_id)
        {
            return base.ExecuteFunctionAsync(nameof(GetDocumentAsync), async delegate ()
            {
                Widget found = await this.RetrieveByIdIsolatedAsync(shop_id, shop_id.ToString(), widget_id.ToString());
                

                return found;
            });
        }
        


        public Task<bool> CreateDocumentAsync(Widget model)
        {
            return base.ExecuteFunctionAsync(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<Widget> result = await base.UpsertIsolatedAsync(model.shop_id, model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(Widget model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveIsolatedAsync(model.shop_id, model);
            });
        }

        
        public Task<ListResult<Widget>> FindForShopAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunctionAsync(nameof(FindForShopAsync), async delegate ()
            {
                IQueryable<Widget> query = this.QueryByPartitionIsolated(shop_id, shop_id.ToString());

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<Widget> result = await query.FetchAsSteppedListAsync(skip, take);
                
                return result;
            });
        }

        
        public Task DeleteForShopAsync(Guid shop_id)
        {
            return base.ExecuteMethodAsync(nameof(DeleteForShopAsync), async delegate ()
            {
                List<Widget> deleteItems = await base.RetrieveByPartitionIsolatedAsync(shop_id, shop_id.ToString());

                await base.BulkRemoveIsolatedAsync(shop_id, deleteItems);
            });
        }
        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Widget> ApplySafeSort(IQueryable<Widget> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Widget> ApplySafeSort(IQueryable<Widget> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<Widget> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            case nameof(Widget.stamp_utc):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.stamp_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.stamp_utc);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.stamp_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.stamp_utc);
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
                    result = query.OrderBy(x => x.widget_id);
                }
                return result;
                
            });
        }

        

        #endregion
    }
}


