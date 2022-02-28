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
    public partial class ShopSettingBusiness : BusinessBase, IShopSettingBusiness, INestedOperation<db.ShopSetting, dm.ShopSetting>
    {
        public ShopSettingBusiness(IFoundation foundation)
            : base(foundation, "ShopSetting")
        {
        }
        
        public IShopSettingSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IShopSettingSynchronizer>();
            }
        }

        public ShopSetting Insert(ShopSetting insertShopSetting)
        {
            return this.Insert(insertShopSetting, Availability.Searchable);
        }
        public ShopSetting Insert(ShopSetting insertShopSetting, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(insertShopSetting.shop_id))
                {
                    

                    this.PreProcess(insertShopSetting, Crud.Insert);
                    this.Validate(insertShopSetting, Crud.Insert);
                    var interception = this.Intercept(insertShopSetting, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertShopSetting.shop_setting_id == Guid.Empty)
                    {
                        insertShopSetting.shop_setting_id = Guid.NewGuid();
                    }
                    if(insertShopSetting.created_utc == default(DateTime))
                    {
                        insertShopSetting.created_utc = DateTime.UtcNow;
                    }
                    if(insertShopSetting.updated_utc == default(DateTime))
                    {
                        insertShopSetting.updated_utc = insertShopSetting.created_utc;
                    }

                    db.ShopSetting dbModel = insertShopSetting.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.ShopSettings.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertShopSetting);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.shop_setting_id, dbModel.shop_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertShopSetting);
                    
                    this.DependencyCoordinator.ShopSettingInvalidated(Dependency.none, dbModel.shop_setting_id, dbModel.shop_id);
                }
                return this.GetById(insertShopSetting.shop_id, insertShopSetting.shop_setting_id);
            });
        }
        public ShopSetting Update(ShopSetting updateShopSetting)
        {
            return this.Update(updateShopSetting, Availability.Searchable);
        }
        public ShopSetting Update(ShopSetting updateShopSetting, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(updateShopSetting.shop_id))
                {
                    this.PreProcess(updateShopSetting, Crud.Update);
                    this.Validate(updateShopSetting, Crud.Update);
                    var interception = this.Intercept(updateShopSetting, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateShopSetting.updated_utc = DateTime.UtcNow;
                    
                    db.ShopSetting found = (from n in database.ShopSettings
                                    where n.shop_setting_id == updateShopSetting.shop_setting_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        ShopSetting previous = found.ToDomainModel();
                        
                        found = updateShopSetting.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateShopSetting, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_setting_id, found.shop_id), Availability.Searchable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopSettingInvalidated(Dependency.none, found.shop_setting_id, found.shop_id);
                    
                    }
                    
                    return this.GetById(updateShopSetting.shop_id, updateShopSetting.shop_setting_id);
                }
            });
        }
        public void Delete(Guid shop_id, Guid shop_setting_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopSetting found = (from a in database.ShopSettings
                                    where a.shop_setting_id == shop_setting_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_setting_id, found.shop_id), Availability.Searchable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.ShopSettingInvalidated(Dependency.none, found.shop_setting_id, found.shop_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid shop_id, Guid shop_setting_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.ShopSettings
                            .Where(x => (x.shop_setting_id == shop_setting_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.ShopSetting()
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
                        db.ShopSettings
                            .Where(x => x.shop_setting_id == shop_setting_id && x.sync_success_utc == null)
                            .Update(x => new db.ShopSetting()
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
                        var data = (from a in db.ShopSettings
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.shop_setting_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.ShopSettings
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.shop_setting_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid shop_id, Guid shop_setting_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
                {
                    using (var db = base.CreateSQLIsolatedContext(shop_id))
                    {
                        if (success)
                        {
                            db.ShopSettings
                                .Where(x => x.shop_setting_id == shop_setting_id)
                                .Update(x => new db.ShopSetting()
                                {
                                    sync_hydrate_utc = sync_date_utc
                                });
                        }
                        else
                        {
                            db.ShopSettings
                                .Where(x => x.shop_setting_id == shop_setting_id)
                                .Update(x => new db.ShopSetting()
                                {
                                    sync_hydrate_utc = null
                                });
                        }
                    }
                });
            });
        }
        
        public List<IdentityInfo> SynchronizationHydrateGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(tenant_code))
                {
                    var data = (from a in db.ShopSettings
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.shop_setting_id, route_id = a.shop_id});
                    return data.ToList();
                }
            });
        }
        
        public ShopSetting GetById(Guid shop_id, Guid shop_setting_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopSetting result = (from n in db.ShopSettings
                                     where (n.shop_setting_id == shop_setting_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<ShopSetting> GetByShop(Guid shop_id)
        {
            return base.ExecuteFunction("GetByShop", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    var result = (from n in db.ShopSettings
                                     where (n.shop_id == shop_id)
                                     orderby n.name
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid shop_id, Guid shop_setting_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopSettings
                        .Where(x => x.shop_setting_id == shop_setting_id)
                        .Update(x => new db.ShopSetting() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        public List<ShopSetting> Find(Guid shop_id, int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.ShopSettings
                                where (keyword == "" 
                                    || p.name.Contains(keyword)
                                
                                    || p.value.Contains(keyword)
                                )
                                && (p.shop_id == shop_id)
                                
                                select p);

                    List<db.ShopSetting> result = new List<db.ShopSetting>();

                    switch (order_by)
                    {
                        case "name":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.name).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.name).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        default:
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.name).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.name).Skip(skip).Take(take).ToList();
                            }
                            
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        
        public int FindTotal(Guid shop_id, string keyword = "")
        {
            return base.ExecuteFunction("FindTotal", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }
                    var data = (from p in db.ShopSettings
                                where (keyword == "" 
                                    || p.name.Contains(keyword)
                                
                                    || p.value.Contains(keyword)
                                )
                                && (p.shop_id == shop_id)
                                
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(ShopSetting shopsetting, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(shopsetting, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.ShopSetting, dm.ShopSetting> PrepareNestedInsert(PlaceholderContext database, dm.ShopSetting insertShopSetting)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertShopSetting, Crud.Insert);
                this.Validate(insertShopSetting, Crud.Insert);
                
                if (insertShopSetting.shop_setting_id == Guid.Empty)
                {
                    insertShopSetting.shop_setting_id = Guid.NewGuid();
                }
                insertShopSetting.created_utc = DateTime.UtcNow;
                insertShopSetting.updated_utc = insertShopSetting.created_utc;

                db.ShopSetting dbModel = insertShopSetting.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.ShopSettings.Add(dbModel);
                
                
                return new NestedInsertInfo<db.ShopSetting, dm.ShopSetting>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertShopSetting
                };
            });
        }
        public dm.ShopSetting FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.ShopSetting, dm.ShopSetting> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.shop_setting_id, insertInfo.DbModel.shop_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.ShopSettingInvalidated(Dependency.none, insertInfo.DbModel.shop_setting_id, insertInfo.DbModel.shop_id);
            
                return this.GetById(insertInfo.InsertModel.shop_id, insertInfo.InsertModel.shop_setting_id);
            });
        }
        public NestedUpdateInfo<db.ShopSetting, dm.ShopSetting> PrepareNestedUpdate(PlaceholderContext database, dm.ShopSetting updateShopSetting)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateShopSetting, Crud.Update);
                this.Validate(updateShopSetting, Crud.Update);

                
                db.ShopSetting found = (from n in database.ShopSettings
                                    where n.shop_setting_id == updateShopSetting.shop_setting_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.ShopSetting previous = found.ToDomainModel();

                    found = updateShopSetting.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.ShopSetting, dm.ShopSetting>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateShopSetting
                    };
                }
                return null;

            });
        }
        public dm.ShopSetting FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.ShopSetting, dm.ShopSetting> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.shop_setting_id, updateInfo.DbModel.shop_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.ShopSettingInvalidated(Dependency.none, updateInfo.DbModel.shop_setting_id, updateInfo.DbModel.shop_id);

                return this.GetById(updateInfo.UpdateModel.shop_id, updateInfo.DbModel.shop_setting_id);
            });
        }
        
        public InterceptArgs<ShopSetting> Intercept(ShopSetting shopsetting, Crud crud)
        {
            InterceptArgs<ShopSetting> args = new InterceptArgs<ShopSetting>()
            {
                Crud = crud,
                ReturnEntity = shopsetting
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(ShopSetting shopsetting, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<ShopSetting> args);
        partial void PreProcess(ShopSetting shopsetting, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.ShopSetting shopsetting, ShopSetting insertShopSetting);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.ShopSetting shopsetting, ShopSetting updateShopSetting, ShopSetting previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.ShopSetting shopsetting);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.ShopSetting shopsetting);
        partial void AfterInsertIndexed(PlaceholderContext database, db.ShopSetting shopsetting, ShopSetting insertShopSetting);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.ShopSetting shopsetting);
    }
}

