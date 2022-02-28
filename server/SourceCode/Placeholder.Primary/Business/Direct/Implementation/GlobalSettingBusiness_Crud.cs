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
    public partial class GlobalSettingBusiness : BusinessBase, IGlobalSettingBusiness, INestedOperation<db.GlobalSetting, dm.GlobalSetting>
    {
        public GlobalSettingBusiness(IFoundation foundation)
            : base(foundation, "GlobalSetting")
        {
        }
        
        

        public GlobalSetting Insert(GlobalSetting insertGlobalSetting)
        {
            return this.Insert(insertGlobalSetting, Availability.Searchable);
        }
        public GlobalSetting Insert(GlobalSetting insertGlobalSetting, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    

                    this.PreProcess(insertGlobalSetting, Crud.Insert);
                    this.Validate(insertGlobalSetting, Crud.Insert);
                    var interception = this.Intercept(insertGlobalSetting, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertGlobalSetting.global_setting_id == Guid.Empty)
                    {
                        insertGlobalSetting.global_setting_id = Guid.NewGuid();
                    }
                    

                    db.GlobalSetting dbModel = insertGlobalSetting.ToDbModel();
                    
                    

                    database.GlobalSettings.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertGlobalSetting);
                    
                    
                    this.DependencyCoordinator.GlobalSettingInvalidated(Dependency.none, dbModel.global_setting_id);
                }
                return this.GetById(insertGlobalSetting.global_setting_id);
            });
        }
        public GlobalSetting Update(GlobalSetting updateGlobalSetting)
        {
            return this.Update(updateGlobalSetting, Availability.Searchable);
        }
        public GlobalSetting Update(GlobalSetting updateGlobalSetting, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateGlobalSetting, Crud.Update);
                    this.Validate(updateGlobalSetting, Crud.Update);
                    var interception = this.Intercept(updateGlobalSetting, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    
                    
                    db.GlobalSetting found = (from n in database.GlobalSettings
                                    where n.global_setting_id == updateGlobalSetting.global_setting_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        GlobalSetting previous = found.ToDomainModel();
                        
                        found = updateGlobalSetting.ToDbModel(found);
                        
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateGlobalSetting, previous);
                        
                        
                        this.DependencyCoordinator.GlobalSettingInvalidated(Dependency.none, found.global_setting_id);
                    
                    }
                    
                    return this.GetById(updateGlobalSetting.global_setting_id);
                }
            });
        }
        public void Delete(Guid global_setting_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLSharedContext())
                {
                    db.GlobalSetting found = (from a in database.GlobalSettings
                                    where a.global_setting_id == global_setting_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        database.GlobalSettings.Remove(found);
                        

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        
                        this.DependencyCoordinator.GlobalSettingInvalidated(Dependency.none, found.global_setting_id);
                    }
                }
            });
        }
        
        public GlobalSetting GetById(Guid global_setting_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    db.GlobalSetting result = (from n in db.GlobalSettings
                                     where (n.global_setting_id == global_setting_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<GlobalSetting> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.GlobalSettings
                                where (keyword == "" 
                                    || p.name.Contains(keyword)
                                )
                                select p);

                    List<db.GlobalSetting> result = new List<db.GlobalSetting>();

                    switch (order_by)
                    {
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
                    var data = (from p in db.GlobalSettings
                                where (keyword == "" 
                                    || p.name.Contains(keyword)
                                )
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(GlobalSetting globalsetting, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(globalsetting, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.GlobalSetting, dm.GlobalSetting> PrepareNestedInsert(PlaceholderContext database, dm.GlobalSetting insertGlobalSetting)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertGlobalSetting, Crud.Insert);
                this.Validate(insertGlobalSetting, Crud.Insert);
                
                if (insertGlobalSetting.global_setting_id == Guid.Empty)
                {
                    insertGlobalSetting.global_setting_id = Guid.NewGuid();
                }
                

                db.GlobalSetting dbModel = insertGlobalSetting.ToDbModel();
                
                

                database.GlobalSettings.Add(dbModel);
                
                
                return new NestedInsertInfo<db.GlobalSetting, dm.GlobalSetting>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertGlobalSetting
                };
            });
        }
        public dm.GlobalSetting FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.GlobalSetting, dm.GlobalSetting> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                
                this.DependencyCoordinator.GlobalSettingInvalidated(Dependency.none, insertInfo.DbModel.global_setting_id);
            
                return this.GetById(insertInfo.InsertModel.global_setting_id);
            });
        }
        public NestedUpdateInfo<db.GlobalSetting, dm.GlobalSetting> PrepareNestedUpdate(PlaceholderContext database, dm.GlobalSetting updateGlobalSetting)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateGlobalSetting, Crud.Update);
                this.Validate(updateGlobalSetting, Crud.Update);

                
                db.GlobalSetting found = (from n in database.GlobalSettings
                                    where n.global_setting_id == updateGlobalSetting.global_setting_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.GlobalSetting previous = found.ToDomainModel();

                    found = updateGlobalSetting.ToDbModel(found);
                    

                    return new NestedUpdateInfo<db.GlobalSetting, dm.GlobalSetting>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateGlobalSetting
                    };
                }
                return null;

            });
        }
        public dm.GlobalSetting FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.GlobalSetting, dm.GlobalSetting> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                
                this.DependencyCoordinator.GlobalSettingInvalidated(Dependency.none, updateInfo.DbModel.global_setting_id);

                return this.GetById(updateInfo.DbModel.global_setting_id);
            });
        }
        
        public InterceptArgs<GlobalSetting> Intercept(GlobalSetting globalsetting, Crud crud)
        {
            InterceptArgs<GlobalSetting> args = new InterceptArgs<GlobalSetting>()
            {
                Crud = crud,
                ReturnEntity = globalsetting
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(GlobalSetting globalsetting, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<GlobalSetting> args);
        partial void PreProcess(GlobalSetting globalsetting, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.GlobalSetting globalsetting, GlobalSetting insertGlobalSetting);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.GlobalSetting globalsetting, GlobalSetting updateGlobalSetting, GlobalSetting previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.GlobalSetting globalsetting);
        
    }
}

