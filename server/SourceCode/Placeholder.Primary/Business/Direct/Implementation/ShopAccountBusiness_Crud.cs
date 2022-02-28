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
    public partial class ShopAccountBusiness : BusinessBase, IShopAccountBusiness, INestedOperation<db.ShopAccount, dm.ShopAccount>
    {
        public ShopAccountBusiness(IFoundation foundation)
            : base(foundation, "ShopAccount")
        {
        }
        
        public IShopAccountSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IShopAccountSynchronizer>();
            }
        }

        public ShopAccount Insert(ShopAccount insertShopAccount)
        {
            return this.Insert(insertShopAccount, Availability.Retrievable);
        }
        public ShopAccount Insert(ShopAccount insertShopAccount, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    

                    this.PreProcess(insertShopAccount, Crud.Insert);
                    this.Validate(insertShopAccount, Crud.Insert);
                    var interception = this.Intercept(insertShopAccount, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertShopAccount.shop_account_id == Guid.Empty)
                    {
                        insertShopAccount.shop_account_id = Guid.NewGuid();
                    }
                    if(insertShopAccount.created_utc == default(DateTime))
                    {
                        insertShopAccount.created_utc = DateTime.UtcNow;
                    }
                    if(insertShopAccount.updated_utc == default(DateTime))
                    {
                        insertShopAccount.updated_utc = insertShopAccount.created_utc;
                    }

                    db.ShopAccount dbModel = insertShopAccount.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.ShopAccounts.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertShopAccount);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.shop_account_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertShopAccount);
                    
                    this.DependencyCoordinator.ShopAccountInvalidated(Dependency.none, dbModel.shop_account_id);
                }
                return this.GetById(insertShopAccount.shop_account_id);
            });
        }
        public ShopAccount Update(ShopAccount updateShopAccount)
        {
            return this.Update(updateShopAccount, Availability.Retrievable);
        }
        public ShopAccount Update(ShopAccount updateShopAccount, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateShopAccount, Crud.Update);
                    this.Validate(updateShopAccount, Crud.Update);
                    var interception = this.Intercept(updateShopAccount, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateShopAccount.updated_utc = DateTime.UtcNow;
                    
                    db.ShopAccount found = (from n in database.ShopAccounts
                                    where n.shop_account_id == updateShopAccount.shop_account_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        ShopAccount previous = found.ToDomainModel();
                        
                        found = updateShopAccount.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateShopAccount, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_account_id), Availability.Retrievable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopAccountInvalidated(Dependency.none, found.shop_account_id);
                    
                    }
                    
                    return this.GetById(updateShopAccount.shop_account_id);
                }
            });
        }
        public void Delete(Guid shop_account_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLSharedContext())
                {
                    db.ShopAccount found = (from a in database.ShopAccounts
                                    where a.shop_account_id == shop_account_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_account_id), Availability.Retrievable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopAccountInvalidated(Dependency.none, found.shop_account_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid shop_account_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    if (success)
                    {
                        db.ShopAccounts
                            .Where(x => (x.shop_account_id == shop_account_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.ShopAccount()
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
                        db.ShopAccounts
                            .Where(x => x.shop_account_id == shop_account_id && x.sync_success_utc == null)
                            .Update(x => new db.ShopAccount()
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
                        var data = (from a in db.ShopAccounts
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.shop_account_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.ShopAccounts
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.shop_account_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid shop_account_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
                {
                    using (var db = base.CreateSQLSharedContext())
                    {
                        if (success)
                        {
                            db.ShopAccounts
                                .Where(x => x.shop_account_id == shop_account_id)
                                .Update(x => new db.ShopAccount()
                                {
                                    sync_hydrate_utc = sync_date_utc
                                });
                        }
                        else
                        {
                            db.ShopAccounts
                                .Where(x => x.shop_account_id == shop_account_id)
                                .Update(x => new db.ShopAccount()
                                {
                                    sync_hydrate_utc = null
                                });
                        }
                    }
                });
            });
        }
        
        public List<IdentityInfo> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    var data = (from a in db.ShopAccounts
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.shop_account_id});
                    return data.ToList();
                }
            });
        }
        
        public ShopAccount GetById(Guid shop_account_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    db.ShopAccount result = (from n in db.ShopAccounts
                                     where (n.shop_account_id == shop_account_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<ShopAccount> GetByShop(Guid shop_id)
        {
            return base.ExecuteFunction("GetByShop", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    var result = (from n in db.ShopAccounts
                                     where (n.shop_id == shop_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        public List<ShopAccount> GetByAccount(Guid account_id)
        {
            return base.ExecuteFunction("GetByAccount", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    var result = (from n in db.ShopAccounts
                                     where (n.account_id == account_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid shop_account_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    db.ShopAccounts
                        .Where(x => x.shop_account_id == shop_account_id)
                        .Update(x => new db.ShopAccount() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        


        public virtual void Validate(ShopAccount shopaccount, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(shopaccount, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.ShopAccount, dm.ShopAccount> PrepareNestedInsert(PlaceholderContext database, dm.ShopAccount insertShopAccount)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertShopAccount, Crud.Insert);
                this.Validate(insertShopAccount, Crud.Insert);
                
                if (insertShopAccount.shop_account_id == Guid.Empty)
                {
                    insertShopAccount.shop_account_id = Guid.NewGuid();
                }
                insertShopAccount.created_utc = DateTime.UtcNow;
                insertShopAccount.updated_utc = insertShopAccount.created_utc;

                db.ShopAccount dbModel = insertShopAccount.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.ShopAccounts.Add(dbModel);
                
                
                return new NestedInsertInfo<db.ShopAccount, dm.ShopAccount>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertShopAccount
                };
            });
        }
        public dm.ShopAccount FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.ShopAccount, dm.ShopAccount> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.shop_account_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.ShopAccountInvalidated(Dependency.none, insertInfo.DbModel.shop_account_id);
            
                return this.GetById(insertInfo.InsertModel.shop_account_id);
            });
        }
        public NestedUpdateInfo<db.ShopAccount, dm.ShopAccount> PrepareNestedUpdate(PlaceholderContext database, dm.ShopAccount updateShopAccount)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateShopAccount, Crud.Update);
                this.Validate(updateShopAccount, Crud.Update);

                
                db.ShopAccount found = (from n in database.ShopAccounts
                                    where n.shop_account_id == updateShopAccount.shop_account_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.ShopAccount previous = found.ToDomainModel();

                    found = updateShopAccount.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.ShopAccount, dm.ShopAccount>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateShopAccount
                    };
                }
                return null;

            });
        }
        public dm.ShopAccount FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.ShopAccount, dm.ShopAccount> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.shop_account_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.ShopAccountInvalidated(Dependency.none, updateInfo.DbModel.shop_account_id);

                return this.GetById(updateInfo.DbModel.shop_account_id);
            });
        }
        
        public InterceptArgs<ShopAccount> Intercept(ShopAccount shopaccount, Crud crud)
        {
            InterceptArgs<ShopAccount> args = new InterceptArgs<ShopAccount>()
            {
                Crud = crud,
                ReturnEntity = shopaccount
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(ShopAccount shopaccount, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<ShopAccount> args);
        partial void PreProcess(ShopAccount shopaccount, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.ShopAccount shopaccount, ShopAccount insertShopAccount);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.ShopAccount shopaccount, ShopAccount updateShopAccount, ShopAccount previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.ShopAccount shopaccount);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.ShopAccount shopaccount);
        partial void AfterInsertIndexed(PlaceholderContext database, db.ShopAccount shopaccount, ShopAccount insertShopAccount);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.ShopAccount shopaccount);
    }
}

