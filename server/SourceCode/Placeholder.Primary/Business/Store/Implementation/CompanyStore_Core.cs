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
    public partial class CompanyStore : StoreBase<Company>, ICompanyStore
    {
        #region Constructor
        
        public CompanyStore(IFoundation foundation)
            : base(foundation, "CompanyStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(Company.company_id), nameof(Company.shop_id)  };
        private static string[] INDEX_DATE_FIELDS = new string[] {  };
        private static string[] INDEX_STRING_FIELDS = new string[] { nameof(Company.company_name) };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.Company;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(Company.shop_id);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(Company.company_id);
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

        protected override string ExtractPartitionKey(Company entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.shop_id.ToString();;
            });
        }

        protected override string ExtractPrimaryKey(Company entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.company_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        public Task<Company> GetDocumentAsync(Guid shop_id, Guid company_id)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                Company found = await this.RetrieveByIdIsolatedAsync(shop_id, shop_id.ToString(), company_id.ToString());
                

                return found;
            });
        }
        


        public Task<bool> CreateDocumentAsync(Company model)
        {
            return base.ExecuteFunction(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<Company> result = await base.UpsertIsolatedAsync(model.shop_id, model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(Company model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveIsolatedAsync(model.shop_id, model);
            });
        }

        
        public Task<ListResult<Company>> FindForShopAsync(Guid shop_id, int skip, int take, string order_by = "", bool descending = false, string keyword = "", bool? disabled = null)
        {
            return base.ExecuteFunction(nameof(FindForShopAsync), async delegate ()
            {
                IQueryable<Company> query = this.QueryByPartitionIsolated(shop_id, shop_id.ToString());
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => 
                        x.company_name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    );
                }
                
                if(disabled.HasValue)
                {
                    query = query.Where(x => x.disabled == disabled.Value);
                }
                

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<Company> result = await query.FetchAsSteppedListAsync(skip, take);
                
                return result;
            });
        }

        
        public Task DeleteForShopAsync(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(DeleteForShopAsync), async delegate ()
            {
                List<Company> deleteItems = await base.RetrieveByPartitionIsolatedAsync(shop_id, shop_id.ToString());

                return base.BulkRemoveIsolatedAsync(shop_id, deleteItems);
            });
        }
        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Company> ApplySafeSort(IQueryable<Company> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Company> ApplySafeSort(IQueryable<Company> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<Company> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            case nameof(Company.company_name):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.company_name);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.company_name);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.company_name);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.company_name);
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
                    result = query.OrderBy(x => x.company_id);
                }
                return result;
                
            });
        }

        

        #endregion
    }
}


