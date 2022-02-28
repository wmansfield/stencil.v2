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
    public partial class ShopBusiness : BusinessBase, IShopBusiness, INestedOperation<db.Shop, dm.Shop>
    {
        public ShopBusiness(IFoundation foundation)
            : base(foundation, "Shop")
        {
        }
        
        public IShopSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IShopSynchronizer>();
            }
        }

        public Shop Insert(Shop insertShop)
        {
            return this.Insert(insertShop, Availability.Retrievable);
        }
        public Shop Insert(Shop insertShop, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    

                    this.PreProcess(insertShop, Crud.Insert);
                    this.Validate(insertShop, Crud.Insert);
                    var interception = this.Intercept(insertShop, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertShop.shop_id == Guid.Empty)
                    {
                        insertShop.shop_id = Guid.NewGuid();
                    }
                    if(insertShop.created_utc == default(DateTime))
                    {
                        insertShop.created_utc = DateTime.UtcNow;
                    }
                    if(insertShop.updated_utc == default(DateTime))
                    {
                        insertShop.updated_utc = insertShop.created_utc;
                    }

                    db.Shop dbModel = insertShop.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.Shops.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    // Insert Isolate
                    using (var dbIsolated = base.CreateSQLIsolatedContext(dbModel.shop_id))
                    {
                        db.Shop match = (from n in dbIsolated.Shops
                                where n.shop_id == dbModel.shop_id
                                select n).FirstOrDefault();

                        if (match != null)
                        {
                            match = insertShop.ToDbModel(match);
                            dbIsolated.SaveChanges();
                        }
                    }
                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertShop);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.shop_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertShop);
                    
                    this.DependencyCoordinator.ShopInvalidated(Dependency.none, dbModel.shop_id);
                }
                return this.GetById(insertShop.shop_id);
            });
        }
        public Shop Update(Shop updateShop)
        {
            return this.Update(updateShop, Availability.Retrievable);
        }
        public Shop Update(Shop updateShop, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateShop, Crud.Update);
                    this.Validate(updateShop, Crud.Update);
                    var interception = this.Intercept(updateShop, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateShop.updated_utc = DateTime.UtcNow;
                    
                    db.Shop found = (from n in database.Shops
                                    where n.shop_id == updateShop.shop_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Shop previous = found.ToDomainModel();
                        
                        found = updateShop.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        
                        // Update Isolate
                        using (var dbIsolated = base.CreateSQLIsolatedContext(found.shop_id))
                        {
                            db.Shop match = (from n in dbIsolated.Shops
                                    where n.shop_id == found.shop_id
                                    select n).FirstOrDefault();

                            if (match != null)
                            {
                                match = updateShop.ToDbModel(match);
                                dbIsolated.SaveChanges();
                            }
                        }
                        

                        this.AfterUpdatePersisted(database, found, updateShop, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_id), Availability.Retrievable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopInvalidated(Dependency.none, found.shop_id);
                    
                    }
                    
                    return this.GetById(updateShop.shop_id);
                }
            });
        }
        public void Delete(Guid shop_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Shop found = (from a in database.Shops
                                    where a.shop_id == shop_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        // Delete From Isolate
                        using (var dbIsolated = base.CreateSQLIsolatedContext(found.shop_id))
                        {
                            db.Shop match = (from a in dbIsolated.Shops
                                    where a.shop_id == shop_id
                                    select a).FirstOrDefault();

                            if (match != null)
                            {
                                match.deleted_utc = DateTime.UtcNow;
                                match.InvalidateSync(this.DefaultAgent, "deleted");
                                dbIsolated.SaveChanges();
                            }
                        }
                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_id), Availability.Retrievable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopInvalidated(Dependency.none, found.shop_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    if (success)
                    {
                        db.Shops
                            .Where(x => (x.shop_id == shop_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.Shop()
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
                        db.Shops
                            .Where(x => x.shop_id == shop_id && x.sync_success_utc == null)
                            .Update(x => new db.Shop()
                            {
                                sync_attempt_utc = DateTime.UtcNow,
                                sync_log = sync_log
                            });
                    }
                }
            });
        }
        
        public void SynchronizationUpdateIsolated(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdateIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.Shops
                            .Where(x => (x.shop_id == shop_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.Shop()
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
                        db.Shops
                            .Where(x => x.shop_id == shop_id && x.sync_success_utc == null)
                            .Update(x => new db.Shop()
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
                        var data = (from a in db.Shops
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.shop_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.Shops
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.shop_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public List<IdentityInfo> SynchronizationGetInvalidIsolated(Guid shop_id, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalidIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if(string.IsNullOrWhiteSpace(sync_agent))
                    {
                        var data = (from a in db.Shops
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.shop_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.Shops
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.shop_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
                {
                    using (var db = base.CreateSQLSharedContext())
                    {
                        if (success)
                        {
                            db.Shops
                                .Where(x => x.shop_id == shop_id)
                                .Update(x => new db.Shop()
                                {
                                    sync_hydrate_utc = sync_date_utc
                                });
                        }
                        else
                        {
                            db.Shops
                                .Where(x => x.shop_id == shop_id)
                                .Update(x => new db.Shop()
                                {
                                    sync_hydrate_utc = null
                                });
                        }
                    }
                });
            });
        }
        
        public void SynchronizationHydrateUpdateIsolated(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdateIsolated", delegate ()
            {
                base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
                {
                    using (var db = base.CreateSQLIsolatedContext(shop_id))
                    {
                        if (success)
                        {
                            db.Shops
                                .Where(x => x.shop_id == shop_id)
                                .Update(x => new db.Shop()
                                {
                                    sync_hydrate_utc = sync_date_utc
                                });
                        }
                        else
                        {
                            db.Shops
                                .Where(x => x.shop_id == shop_id)
                                .Update(x => new db.Shop()
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
                    var data = (from a in db.Shops
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.shop_id});
                    return data.ToList();
                }
            });
        }
        
        public List<IdentityInfo> SynchronizationHydrateGetInvalidIsolated(Guid shop_id, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalidIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    var data = (from a in db.Shops
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.shop_id});
                    return data.ToList();
                }
            });
        }
        
        public Shop GetById(Guid shop_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    db.Shop result = (from n in db.Shops
                                     where (n.shop_id == shop_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<Shop> GetByTenant(Guid tenant_id)
        {
            return base.ExecuteFunction("GetByTenant", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    var result = (from n in db.Shops
                                     where (n.tenant_id == tenant_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid shop_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLSharedContext())
                {
                    db.Shops
                        .Where(x => x.shop_id == shop_id)
                        .Update(x => new db.Shop() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        public void InvalidateIsolated(Guid shop_id, string reason)
        {
            base.ExecuteMethod("InvalidateIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.Shops
                        .Where(x => x.shop_id == shop_id)
                        .Update(x => new db.Shop() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        public List<Shop> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false, Guid? tenant_id = null)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.Shops
                                where (keyword == "" 
                                    || p.shop_name.Contains(keyword)
                                
                                    || p.private_domain.Contains(keyword)
                                
                                    || p.public_domain.Contains(keyword)
                                )
                                && (tenant_id == null || p.tenant_id == tenant_id)
                                
                                select p);

                    List<db.Shop> result = new List<db.Shop>();

                    switch (order_by)
                    {
                        default:
                            result = data.OrderBy(s => s.shop_id).Skip(skip).Take(take).ToList();
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        
        public int FindTotal(string keyword = "", Guid? tenant_id = null)
        {
            return base.ExecuteFunction("FindTotal", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }
                    var data = (from p in db.Shops
                                where (keyword == "" 
                                    || p.shop_name.Contains(keyword)
                                
                                    || p.private_domain.Contains(keyword)
                                
                                    || p.public_domain.Contains(keyword)
                                )
                                && (tenant_id == null || p.tenant_id == tenant_id)
                                
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(Shop shop, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(shop, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.Shop, dm.Shop> PrepareNestedInsert(PlaceholderContext database, dm.Shop insertShop)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertShop, Crud.Insert);
                this.Validate(insertShop, Crud.Insert);
                
                if (insertShop.shop_id == Guid.Empty)
                {
                    insertShop.shop_id = Guid.NewGuid();
                }
                insertShop.created_utc = DateTime.UtcNow;
                insertShop.updated_utc = insertShop.created_utc;

                db.Shop dbModel = insertShop.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.Shops.Add(dbModel);
                
                
                return new NestedInsertInfo<db.Shop, dm.Shop>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertShop
                };
            });
        }
        public dm.Shop FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.Shop, dm.Shop> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.shop_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.ShopInvalidated(Dependency.none, insertInfo.DbModel.shop_id);
            
                return this.GetById(insertInfo.InsertModel.shop_id);
            });
        }
        public NestedUpdateInfo<db.Shop, dm.Shop> PrepareNestedUpdate(PlaceholderContext database, dm.Shop updateShop)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateShop, Crud.Update);
                this.Validate(updateShop, Crud.Update);

                
                db.Shop found = (from n in database.Shops
                                    where n.shop_id == updateShop.shop_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.Shop previous = found.ToDomainModel();

                    found = updateShop.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.Shop, dm.Shop>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateShop
                    };
                }
                return null;

            });
        }
        public dm.Shop FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.Shop, dm.Shop> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.shop_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.ShopInvalidated(Dependency.none, updateInfo.DbModel.shop_id);

                return this.GetById(updateInfo.DbModel.shop_id);
            });
        }
        
        public InterceptArgs<Shop> Intercept(Shop shop, Crud crud)
        {
            InterceptArgs<Shop> args = new InterceptArgs<Shop>()
            {
                Crud = crud,
                ReturnEntity = shop
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(Shop shop, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<Shop> args);
        partial void PreProcess(Shop shop, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.Shop shop, Shop insertShop);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.Shop shop, Shop updateShop, Shop previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.Shop shop);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.Shop shop);
        partial void AfterInsertIndexed(PlaceholderContext database, db.Shop shop, Shop insertShop);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.Shop shop);
    }
}

