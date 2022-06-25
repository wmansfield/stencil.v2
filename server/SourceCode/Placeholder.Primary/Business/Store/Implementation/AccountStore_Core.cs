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
    public partial class AccountStore : StoreBase<Account>, IAccountStore
    {
        #region Constructor
        
        public AccountStore(IFoundation foundation)
            : base(foundation, "AccountStore")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { nameof(Account.account_id), nameof(Account.asset_id_avatar)  };
        private static string[] INDEX_DATE_FIELDS = new string[] { nameof(Account.email_verify_utc), nameof(Account.password_changed_utc), nameof(Account.password_reset_utc), nameof(Account.single_login_token_expire_utc), nameof(Account.last_login_utc) };
        private static string[] INDEX_STRING_FIELDS = new string[] { nameof(Account.email), nameof(Account.first_name), nameof(Account.last_name), nameof(Account.account_display), nameof(Account.last_login_platform) };
        private static string[] INDEX_NUMBER_FIELDS = new string[] {  };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.Account;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(Account.partition_key);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(Account.account_id);
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

        protected override string ExtractPartitionKey(Account entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.partition_key;
            });
        }

        protected override string ExtractPrimaryKey(Account entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.account_id.ToString();
            });
        }

        #endregion

        #region Public Methods

        
        public Task<Account> GetDocumentAsync(Guid account_id)
        {
            return base.ExecuteFunctionAsync(nameof(GetDocumentAsync), async delegate ()
            {
                
                string partitionKey = new Account(){ account_id = account_id}.partition_key;

                Account foundAccount = await base.RetrieveByIdSharedAsync(partitionKey, account_id.ToString());

                this.PostProcessSensitive(foundAccount);

                return foundAccount;
            });
        }
        


        public Task<bool> CreateDocumentAsync(Account model)
        {
            return base.ExecuteFunctionAsync(nameof(CreateDocumentAsync), async delegate ()
            {
                ItemResponse<Account> result = await base.UpsertSharedAsync(model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task<bool> DeleteDocumentAsync(Account model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.RemoveSharedAsync(model);
            });
        }

        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        public Task<ListResult<Account>> FindAsync(int skip, int take, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunctionAsync(nameof(FindAsync), async delegate ()
            {
                
                IQueryable<Account> query = base.QuerySharedWithoutPartitionKey();
                
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => 
                        x.email.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.first_name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.last_name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.account_display.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    );
                }
                

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult<Account> result = await query.FetchAsSteppedListAsync(skip, take);
                
                this.PostProcessSensitive(result.items);
                
                return result;
            });
        }

        

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Account> ApplySafeSort(IQueryable<Account> query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List<SortInfo>() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable<Account> ApplySafeSort(IQueryable<Account> query, List<SortInfo> sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable<Account> result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            case nameof(Account.email):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.email);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.email);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.email);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.email);
                                    }
                                }
                                break;
                            case nameof(Account.first_name):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.first_name);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.first_name);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.first_name);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.first_name);
                                    }
                                }
                                break;
                            case nameof(Account.last_name):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.last_name);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.last_name);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.last_name);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.last_name);
                                    }
                                }
                                break;
                            case nameof(Account.account_display):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.account_display);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.account_display);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.account_display);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.account_display);
                                    }
                                }
                                break;
                            case nameof(Account.account_status):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.account_status);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.account_status);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.account_status);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.account_status);
                                    }
                                }
                                break;
                            case nameof(Account.email_verify_utc):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.email_verify_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.email_verify_utc);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.email_verify_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.email_verify_utc);
                                    }
                                }
                                break;
                            case nameof(Account.password_changed_utc):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.password_changed_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.password_changed_utc);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.password_changed_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.password_changed_utc);
                                    }
                                }
                                break;
                            case nameof(Account.password_reset_utc):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.password_reset_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.password_reset_utc);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.password_reset_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.password_reset_utc);
                                    }
                                }
                                break;
                            case nameof(Account.single_login_token_expire_utc):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.single_login_token_expire_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.single_login_token_expire_utc);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.single_login_token_expire_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.single_login_token_expire_utc);
                                    }
                                }
                                break;
                            case nameof(Account.last_login_utc):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.last_login_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.last_login_utc);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.last_login_utc);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.last_login_utc);
                                    }
                                }
                                break;
                            case nameof(Account.last_login_platform):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x => x.last_login_platform);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x => x.last_login_platform);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x => x.last_login_platform);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x => x.last_login_platform);
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
                    result = query.OrderBy(x => x.account_id);
                }
                return result;
                
            });
        }

        
        protected void PostProcessSensitive<TModel>(List<TModel> items)
        {
            if(items != null)
            {
                foreach(var item in items)
                {
                    this.PostProcessSensitive(item);
                }
            }
        }
        protected void PostProcessSensitive<TModel>(TModel item)
        {
            if(item is Account)
            {
               
               (item as Account).api_key = default(string);
               (item as Account).api_secret = default(string);
            }
        }
        

        #endregion
    }
}


