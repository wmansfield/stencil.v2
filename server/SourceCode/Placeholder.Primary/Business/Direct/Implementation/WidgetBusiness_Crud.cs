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
    public partial class WidgetBusiness : BusinessBase, IWidgetBusiness, INestedOperation<db.Widget, dm.Widget>
    {
        public WidgetBusiness(IFoundation foundation)
            : base(foundation, "Widget")
        {
        }
        
        public IWidgetSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IWidgetSynchronizer>();
            }
        }

        public Widget Insert(Widget insertWidget)
        {
            return this.Insert(insertWidget, Availability.Searchable);
        }
        public Widget Insert(Widget insertWidget, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(insertWidget.shop_id))
                {
                    

                    this.PreProcess(insertWidget, Crud.Insert);
                    this.Validate(insertWidget, Crud.Insert);
                    var interception = this.Intercept(insertWidget, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertWidget.widget_id == Guid.Empty)
                    {
                        insertWidget.widget_id = Guid.NewGuid();
                    }
                    if(insertWidget.created_utc == default(DateTime))
                    {
                        insertWidget.created_utc = DateTime.UtcNow;
                    }
                    if(insertWidget.updated_utc == default(DateTime))
                    {
                        insertWidget.updated_utc = insertWidget.created_utc;
                    }

                    db.Widget dbModel = insertWidget.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.Widgets.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertWidget);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.widget_id, dbModel.shop_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertWidget);
                    
                    this.DependencyCoordinator.WidgetInvalidated(Dependency.none, dbModel.widget_id, dbModel.shop_id);
                }
                return this.GetById(insertWidget.shop_id, insertWidget.widget_id);
            });
        }
        public Widget Update(Widget updateWidget)
        {
            return this.Update(updateWidget, Availability.Searchable);
        }
        public Widget Update(Widget updateWidget, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(updateWidget.shop_id))
                {
                    this.PreProcess(updateWidget, Crud.Update);
                    this.Validate(updateWidget, Crud.Update);
                    var interception = this.Intercept(updateWidget, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateWidget.updated_utc = DateTime.UtcNow;
                    
                    db.Widget found = (from n in database.Widgets
                                    where n.widget_id == updateWidget.widget_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Widget previous = found.ToDomainModel();
                        
                        found = updateWidget.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateWidget, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.widget_id, found.shop_id), Availability.Searchable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.WidgetInvalidated(Dependency.none, found.widget_id, found.shop_id);
                    
                    }
                    
                    return this.GetById(updateWidget.shop_id, updateWidget.widget_id);
                }
            });
        }
        public void Delete(Guid shop_id, Guid widget_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.Widget found = (from a in database.Widgets
                                    where a.widget_id == widget_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.widget_id, found.shop_id), Availability.Searchable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.WidgetInvalidated(Dependency.none, found.widget_id, found.shop_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid shop_id, Guid widget_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.Widgets
                            .Where(x => (x.widget_id == widget_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.Widget()
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
                        db.Widgets
                            .Where(x => x.widget_id == widget_id && x.sync_success_utc == null)
                            .Update(x => new db.Widget()
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
                        var data = (from a in db.Widgets
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.widget_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.Widgets
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.widget_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid shop_id, Guid widget_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.Widgets
                            .Where(x => x.widget_id == widget_id)
                            .Update(x => new db.Widget()
                            {
                                sync_hydrate_utc = sync_date_utc
                            });
                    }
                    else
                    {
                        db.Widgets
                            .Where(x => x.widget_id == widget_id)
                            .Update(x => new db.Widget()
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
                    var data = (from a in db.Widgets
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.widget_id, route_id = a.shop_id});
                    return data.ToList();
                }
            });
        }
        
        public Widget GetById(Guid shop_id, Guid widget_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    db.Widget result = (from n in db.Widgets
                                     where (n.widget_id == widget_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<Widget> GetByShop(Guid shop_id)
        {
            return base.ExecuteFunction("GetByShop", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    var result = (from n in db.Widgets
                                     where (n.shop_id == shop_id)
                                     orderby n.stamp_utc
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid shop_id, Guid widget_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.Widgets
                        .Where(x => x.widget_id == widget_id)
                        .Update(x => new db.Widget() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        


        public virtual void Validate(Widget widget, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(widget, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.Widget, dm.Widget> PrepareNestedInsert(PlaceholderContext database, dm.Widget insertWidget)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertWidget, Crud.Insert);
                this.Validate(insertWidget, Crud.Insert);
                
                if (insertWidget.widget_id == Guid.Empty)
                {
                    insertWidget.widget_id = Guid.NewGuid();
                }
                insertWidget.created_utc = DateTime.UtcNow;
                insertWidget.updated_utc = insertWidget.created_utc;

                db.Widget dbModel = insertWidget.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.Widgets.Add(dbModel);
                
                
                return new NestedInsertInfo<db.Widget, dm.Widget>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertWidget
                };
            });
        }
        public dm.Widget FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.Widget, dm.Widget> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.widget_id, insertInfo.DbModel.shop_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.WidgetInvalidated(Dependency.none, insertInfo.DbModel.widget_id, insertInfo.DbModel.shop_id);
            
                return this.GetById(insertInfo.InsertModel.shop_id, insertInfo.InsertModel.widget_id);
            });
        }
        public NestedUpdateInfo<db.Widget, dm.Widget> PrepareNestedUpdate(PlaceholderContext database, dm.Widget updateWidget)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateWidget, Crud.Update);
                this.Validate(updateWidget, Crud.Update);

                
                db.Widget found = (from n in database.Widgets
                                    where n.widget_id == updateWidget.widget_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.Widget previous = found.ToDomainModel();

                    found = updateWidget.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.Widget, dm.Widget>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateWidget
                    };
                }
                return null;

            });
        }
        public dm.Widget FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.Widget, dm.Widget> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.widget_id, updateInfo.DbModel.shop_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.WidgetInvalidated(Dependency.none, updateInfo.DbModel.widget_id, updateInfo.DbModel.shop_id);

                return this.GetById(updateInfo.UpdateModel.shop_id, updateInfo.DbModel.widget_id);
            });
        }
        
        public InterceptArgs<Widget> Intercept(Widget widget, Crud crud)
        {
            InterceptArgs<Widget> args = new InterceptArgs<Widget>()
            {
                Crud = crud,
                ReturnEntity = widget
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(Widget widget, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<Widget> args);
        partial void PreProcess(Widget widget, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.Widget widget, Widget insertWidget);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.Widget widget, Widget updateWidget, Widget previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.Widget widget);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.Widget widget);
        partial void AfterInsertIndexed(PlaceholderContext database, db.Widget widget, Widget insertWidget);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.Widget widget);
    }
}

