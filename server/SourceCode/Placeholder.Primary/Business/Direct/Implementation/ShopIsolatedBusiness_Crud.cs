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
    public partial class ShopIsolatedBusiness : BusinessBase, IShopIsolatedBusiness, INestedOperation<db.ShopIsolated, dm.ShopIsolated>
    {
        public ShopIsolatedBusiness(IFoundation foundation)
            : base(foundation, "ShopIsolated")
        {
        }
        
        public IShopIsolatedSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IShopIsolatedSynchronizer>();
            }
        }

        public ShopIsolated Insert(ShopIsolated insertShopIsolated)
        {
            return this.Insert(insertShopIsolated, Availability.Retrievable);
        }
        public ShopIsolated Insert(ShopIsolated insertShopIsolated, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(insertShopIsolated.shop_id))
                {
                    

                    this.PreProcess(insertShopIsolated, Crud.Insert);
                    this.Validate(insertShopIsolated, Crud.Insert);
                    var interception = this.Intercept(insertShopIsolated, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertShopIsolated.shop_id == Guid.Empty)
                    {
                        insertShopIsolated.shop_id = Guid.NewGuid();
                    }
                    if(insertShopIsolated.created_utc == default(DateTime))
                    {
                        insertShopIsolated.created_utc = DateTime.UtcNow;
                    }
                    if(insertShopIsolated.updated_utc == default(DateTime))
                    {
                        insertShopIsolated.updated_utc = insertShopIsolated.created_utc;
                    }

                    db.ShopIsolated dbModel = insertShopIsolated.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.ShopIsolateds.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertShopIsolated);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.shop_id, dbModel.shop_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertShopIsolated);
                    
                    this.DependencyCoordinator.ShopIsolatedInvalidated(Dependency.none, dbModel.shop_id);
                }
                return this.GetById(insertShopIsolated.shop_id);
            });
        }
        public ShopIsolated Update(ShopIsolated updateShopIsolated)
        {
            return this.Update(updateShopIsolated, Availability.Retrievable);
        }
        public ShopIsolated Update(ShopIsolated updateShopIsolated, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(updateShopIsolated.shop_id))
                {
                    this.PreProcess(updateShopIsolated, Crud.Update);
                    this.Validate(updateShopIsolated, Crud.Update);
                    var interception = this.Intercept(updateShopIsolated, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateShopIsolated.updated_utc = DateTime.UtcNow;
                    
                    db.ShopIsolated found = (from n in database.ShopIsolateds
                                    where n.shop_id == updateShopIsolated.shop_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        ShopIsolated previous = found.ToDomainModel();
                        
                        found = updateShopIsolated.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateShopIsolated, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_id, found.shop_id), Availability.Retrievable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopIsolatedInvalidated(Dependency.none, found.shop_id);
                    
                    }
                    
                    return this.GetById(updateShopIsolated.shop_id);
                }
            });
        }
        public void Delete(Guid shop_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopIsolated found = (from a in database.ShopIsolateds
                                    where a.shop_id == shop_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_id, found.shop_id), Availability.Retrievable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopIsolatedInvalidated(Dependency.none, found.shop_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.ShopIsolateds
                            .Where(x => (x.shop_id == shop_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.ShopIsolated()
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
                        db.ShopIsolateds
                            .Where(x => x.shop_id == shop_id && x.sync_success_utc == null)
                            .Update(x => new db.ShopIsolated()
                            {
                                sync_attempt_utc = DateTime.UtcNow,
                                sync_log = sync_log
                            });
                    }
                }
            });
        }
        
        public List<IdentityInfo> SynchronizationGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(tenant_code))
                {
                    if(string.IsNullOrWhiteSpace(sync_agent))
                    {
                        var data = (from a in db.ShopIsolateds
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.shop_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.ShopIsolateds
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.shop_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid shop_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.ShopIsolateds
                            .Where(x => x.shop_id == shop_id)
                            .Update(x => new db.ShopIsolated()
                            {
                                sync_hydrate_utc = sync_date_utc
                            });
                    }
                    else
                    {
                        db.ShopIsolateds
                            .Where(x => x.shop_id == shop_id)
                            .Update(x => new db.ShopIsolated()
                            {
                                sync_hydrate_utc = null
                            });
                    }
                }
            });
        }
        
        public List<IdentityInfo> SynchronizationHydrateGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(tenant_code))
                {
                    var data = (from a in db.ShopIsolateds
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.shop_id, route_id = a.shop_id});
                    return data.ToList();
                }
            });
        }
        
        public ShopIsolated GetById(Guid shop_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopIsolated result = (from n in db.ShopIsolateds
                                     where (n.shop_id == shop_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        
        public void Invalidate(Guid shop_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopIsolateds
                        .Where(x => x.shop_id == shop_id)
                        .Update(x => new db.ShopIsolated() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        


        public virtual void Validate(ShopIsolated shopisolated, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(shopisolated, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.ShopIsolated, dm.ShopIsolated> PrepareNestedInsert(PlaceholderContext database, dm.ShopIsolated insertShopIsolated)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertShopIsolated, Crud.Insert);
                this.Validate(insertShopIsolated, Crud.Insert);
                
                if (insertShopIsolated.shop_id == Guid.Empty)
                {
                    insertShopIsolated.shop_id = Guid.NewGuid();
                }
                insertShopIsolated.created_utc = DateTime.UtcNow;
                insertShopIsolated.updated_utc = insertShopIsolated.created_utc;

                db.ShopIsolated dbModel = insertShopIsolated.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.ShopIsolateds.Add(dbModel);
                
                
                return new NestedInsertInfo<db.ShopIsolated, dm.ShopIsolated>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertShopIsolated
                };
            });
        }
        public dm.ShopIsolated FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.ShopIsolated, dm.ShopIsolated> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.shop_id, insertInfo.DbModel.shop_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.ShopIsolatedInvalidated(Dependency.none, insertInfo.DbModel.shop_id);
            
                return this.GetById(insertInfo.InsertModel.shop_id);
            });
        }
        public NestedUpdateInfo<db.ShopIsolated, dm.ShopIsolated> PrepareNestedUpdate(PlaceholderContext database, dm.ShopIsolated updateShopIsolated)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateShopIsolated, Crud.Update);
                this.Validate(updateShopIsolated, Crud.Update);

                
                db.ShopIsolated found = (from n in database.ShopIsolateds
                                    where n.shop_id == updateShopIsolated.shop_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.ShopIsolated previous = found.ToDomainModel();

                    found = updateShopIsolated.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.ShopIsolated, dm.ShopIsolated>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateShopIsolated
                    };
                }
                return null;

            });
        }
        public dm.ShopIsolated FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.ShopIsolated, dm.ShopIsolated> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.shop_id, updateInfo.DbModel.shop_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.ShopIsolatedInvalidated(Dependency.none, updateInfo.DbModel.shop_id);

                return this.GetById(updateInfo.DbModel.shop_id);
            });
        }
        
        public InterceptArgs<ShopIsolated> Intercept(ShopIsolated shopisolated, Crud crud)
        {
            InterceptArgs<ShopIsolated> args = new InterceptArgs<ShopIsolated>()
            {
                Crud = crud,
                ReturnEntity = shopisolated
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(ShopIsolated shopisolated, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<ShopIsolated> args);
        partial void PreProcess(ShopIsolated shopisolated, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.ShopIsolated shopisolated, ShopIsolated insertShopIsolated);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.ShopIsolated shopisolated, ShopIsolated updateShopIsolated, ShopIsolated previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.ShopIsolated shopisolated);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.ShopIsolated shopisolated);
        partial void AfterInsertIndexed(PlaceholderContext database, db.ShopIsolated shopisolated, ShopIsolated insertShopIsolated);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.ShopIsolated shopisolated);
    }
}

