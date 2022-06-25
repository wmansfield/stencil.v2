using Zero.Foundation;
using Zero.Foundation.Aspect;
using Z.EntityFramework.Plus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dm = Placeholder.Domain;
using db = Placeholder.Data.Sql.Models;
using Placeholder.Common;
using Placeholder.Common.Exceptions;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;
using Placeholder.SDK;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    // WARNING: THIS FILE IS GENERATED
    public partial class AccountBusiness : BusinessBase, IAccountBusiness, INestedOperation<db.Account, dm.Account>
    {
        public AccountBusiness(IFoundation foundation)
            : base(foundation, "Account")
        {
        }
        
        public IAccountSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IAccountSynchronizer>();
            }
        }

        public Account Insert(Account insertAccount)
        {
            return this.Insert(insertAccount, Availability.Retrievable);
        }
        public Account Insert(Account insertAccount, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    

                    this.PreProcess(insertAccount, Crud.Insert);
                    this.Validate(insertAccount, Crud.Insert);
                    var interception = this.Intercept(insertAccount, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertAccount.account_id == Guid.Empty)
                    {
                        insertAccount.account_id = Guid.NewGuid();
                    }
                    if(insertAccount.created_utc == default(DateTime))
                    {
                        insertAccount.created_utc = DateTime.UtcNow;
                    }
                    if(insertAccount.updated_utc == default(DateTime))
                    {
                        insertAccount.updated_utc = insertAccount.created_utc;
                    }

                    db.Account dbModel = insertAccount.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.Accounts.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertAccount);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.account_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertAccount);
                    
                    this.DependencyCoordinator.AccountInvalidated(Dependency.none, dbModel.account_id);
                }
                return this.GetById(insertAccount.account_id);
            });
        }
        public Account Update(Account updateAccount)
        {
            return this.Update(updateAccount, Availability.Retrievable);
        }
        public Account Update(Account updateAccount, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateAccount, Crud.Update);
                    this.Validate(updateAccount, Crud.Update);
                    var interception = this.Intercept(updateAccount, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateAccount.updated_utc = DateTime.UtcNow;
                    
                    db.Account found = (from n in database.Accounts
                                    where n.account_id == updateAccount.account_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Account previous = found.ToDomainModel();
                        
                        found = updateAccount.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateAccount, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.account_id), Availability.Retrievable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.AccountInvalidated(Dependency.none, found.account_id);
                    
                    }
                    
                    return this.GetById(updateAccount.account_id);
                }
            });
        }
        public void Delete(Guid account_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Account found = (from a in database.Accounts
                                    where a.account_id == account_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.account_id), Availability.Retrievable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.AccountInvalidated(Dependency.none, found.account_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid account_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    if (success)
                    {
                        db.Accounts
                            .Where(x => (x.account_id == account_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.Account()
                            {
                                sync_success_utc = sync_date_utc,
                                sync_attempt_utc = null,
                                sync_invalid_utc = null,
                                sync_agent = string.Empty,
                                sync_log = sync_log
                            });
                    }
                    else
                    {
                        db.Accounts
                            .Where(x => x.account_id == account_id && x.sync_success_utc == null)
                            .Update(x => new db.Account()
                            {
                                sync_attempt_utc = DateTime.UtcNow,
                                sync_log = sync_log
                            });
                    }
                }
            });
        }
        
        public List<IdentityInfo> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    if(string.IsNullOrWhiteSpace(sync_agent))
                    {
                        var data = (from a in db.Accounts
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.account_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.Accounts
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.account_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid account_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    if (success)
                    {
                        db.Accounts
                            .Where(x => x.account_id == account_id)
                            .Update(x => new db.Account()
                            {
                                sync_hydrate_utc = sync_date_utc
                            });
                    }
                    else
                    {
                        db.Accounts
                            .Where(x => x.account_id == account_id)
                            .Update(x => new db.Account()
                            {
                                sync_hydrate_utc = null
                            });
                    }
                }
            });
        }
        
        public List<IdentityInfo> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    var data = (from a in db.Accounts
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.account_id});
                    return data.ToList();
                }
            });
        }
        
        public Account GetById(Guid account_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    db.Account result = (from n in db.Accounts
                                     where (n.account_id == account_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        
        public void Invalidate(Guid account_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    db.Accounts
                        .Where(x => x.account_id == account_id)
                        .Update(x => new db.Account() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        public List<Account> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.Accounts
                                where (keyword == "" 
                                    || p.email.Contains(keyword)
                                
                                    || p.first_name.Contains(keyword)
                                
                                    || p.last_name.Contains(keyword)
                                
                                    || p.account_display.Contains(keyword)
                                )
                                select p);

                    List<db.Account> result = new List<db.Account>();

                    switch (order_by)
                    {
                        case "email":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.email).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.email).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        case "first_name":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.first_name).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.first_name).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        case "last_name":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.last_name).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.last_name).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        case "account_display":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.account_display).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.account_display).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        case "last_login_platform":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.last_login_platform).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.last_login_platform).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        default:
                            result = data.OrderBy(s => s.account_id).Skip(skip).Take(take).ToList();
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        
        public int FindTotal(string keyword = "")
        {
            return base.ExecuteFunction("FindTotal", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }
                    var data = (from p in db.Accounts
                                where (keyword == "" 
                                    || p.email.Contains(keyword)
                                
                                    || p.first_name.Contains(keyword)
                                
                                    || p.last_name.Contains(keyword)
                                
                                    || p.account_display.Contains(keyword)
                                )
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(Account account, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            if(string.IsNullOrWhiteSpace(account.first_name))
            {
                errors["first_name"] = new LocalizableString(LocalizableString.SERVER, "Account.first_name.Required", "First Name is required");
            }
            else if(account.first_name != null && account.first_name.Length > 50)
            {
                errors["first_name"] = new LocalizableString(LocalizableString.SERVER, "Account.first_name.CharacterSize", "First Name must be 50 characters or less");
            }
            if(string.IsNullOrWhiteSpace(account.last_name))
            {
                errors["last_name"] = new LocalizableString(LocalizableString.SERVER, "Account.last_name.Required", "Last Name is required");
            }
            else if(account.last_name != null && account.last_name.Length > 50)
            {
                errors["last_name"] = new LocalizableString(LocalizableString.SERVER, "Account.last_name.CharacterSize", "Last Name must be 50 characters or less");
            }
            if(string.IsNullOrWhiteSpace(account.email))
            {
                errors["email"] = new LocalizableString(LocalizableString.SERVER, "Account.email.Required", "E-mail is required");
            }
            else
            {
                string parsed_email = null;
                if(!SDKUtility.IsValidEmail(account.email, out parsed_email))
                {
                    errors["email"] = new LocalizableString(LocalizableString.SERVER, "Account.email.EmailInvalid", "E-mail is not a valid e-mail address");
                }
                else
                {
                    account.email = parsed_email;
                }
            }
            

            this.ValidatePostProcess(account, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.Account, dm.Account> PrepareNestedInsert(PlaceholderContext database, dm.Account insertAccount)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertAccount, Crud.Insert);
                this.Validate(insertAccount, Crud.Insert);
                
                if (insertAccount.account_id == Guid.Empty)
                {
                    insertAccount.account_id = Guid.NewGuid();
                }
                insertAccount.created_utc = DateTime.UtcNow;
                insertAccount.updated_utc = insertAccount.created_utc;

                db.Account dbModel = insertAccount.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.Accounts.Add(dbModel);
                
                
                return new NestedInsertInfo<db.Account, dm.Account>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertAccount
                };
            });
        }
        public dm.Account FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.Account, dm.Account> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.account_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.AccountInvalidated(Dependency.none, insertInfo.DbModel.account_id);
            
                return this.GetById(insertInfo.InsertModel.account_id);
            });
        }
        public NestedUpdateInfo<db.Account, dm.Account> PrepareNestedUpdate(PlaceholderContext database, dm.Account updateAccount)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateAccount, Crud.Update);
                this.Validate(updateAccount, Crud.Update);

                
                db.Account found = (from n in database.Accounts
                                    where n.account_id == updateAccount.account_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.Account previous = found.ToDomainModel();

                    found = updateAccount.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.Account, dm.Account>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateAccount
                    };
                }
                return null;

            });
        }
        public dm.Account FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.Account, dm.Account> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.account_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.AccountInvalidated(Dependency.none, updateInfo.DbModel.account_id);

                return this.GetById(updateInfo.DbModel.account_id);
            });
        }
        
        public InterceptArgs<Account> Intercept(Account account, Crud crud)
        {
            InterceptArgs<Account> args = new InterceptArgs<Account>()
            {
                Crud = crud,
                ReturnEntity = account
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(Account account, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<Account> args);
        partial void PreProcess(Account account, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.Account account, Account insertAccount);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.Account account, Account updateAccount, Account previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.Account account);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.Account account);
        partial void AfterInsertIndexed(PlaceholderContext database, db.Account account, Account insertAccount);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.Account account);
    }
}

