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
    public partial class TenantBusiness : BusinessBase, ITenantBusiness, INestedOperation<db.Tenant, dm.Tenant>
    {
        public TenantBusiness(IFoundation foundation)
            : base(foundation, "Tenant")
        {
        }
        
        

        public Tenant Insert(Tenant insertTenant)
        {
            return this.Insert(insertTenant, Availability.Retrievable);
        }
        public Tenant Insert(Tenant insertTenant, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    

                    this.PreProcess(insertTenant, Crud.Insert);
                    this.Validate(insertTenant, Crud.Insert);
                    var interception = this.Intercept(insertTenant, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertTenant.tenant_id == Guid.Empty)
                    {
                        insertTenant.tenant_id = Guid.NewGuid();
                    }
                    if(insertTenant.created_utc == default(DateTime))
                    {
                        insertTenant.created_utc = DateTime.UtcNow;
                    }
                    if(insertTenant.updated_utc == default(DateTime))
                    {
                        insertTenant.updated_utc = insertTenant.created_utc;
                    }

                    db.Tenant dbModel = insertTenant.ToDbModel();
                    
                    

                    database.Tenants.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertTenant);
                    
                    
                    this.DependencyCoordinator.TenantInvalidated(Dependency.none, dbModel.tenant_id);
                }
                return this.GetById(insertTenant.tenant_id);
            });
        }
        public Tenant Update(Tenant updateTenant)
        {
            return this.Update(updateTenant, Availability.Retrievable);
        }
        public Tenant Update(Tenant updateTenant, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateTenant, Crud.Update);
                    this.Validate(updateTenant, Crud.Update);
                    var interception = this.Intercept(updateTenant, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateTenant.updated_utc = DateTime.UtcNow;
                    
                    db.Tenant found = (from n in database.Tenants
                                    where n.tenant_id == updateTenant.tenant_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Tenant previous = found.ToDomainModel();
                        
                        found = updateTenant.ToDbModel(found);
                        
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateTenant, previous);
                        
                        
                        this.DependencyCoordinator.TenantInvalidated(Dependency.none, found.tenant_id);
                    
                    }
                    
                    return this.GetById(updateTenant.tenant_id);
                }
            });
        }
        public void Delete(Guid tenant_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Tenant found = (from a in database.Tenants
                                    where a.tenant_id == tenant_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        database.Tenants.Remove(found);
                        

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        
                        this.DependencyCoordinator.TenantInvalidated(Dependency.none, found.tenant_id);
                    }
                }
            });
        }
        
        public Tenant GetById(Guid tenant_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    db.Tenant result = (from n in db.Tenants
                                     where (n.tenant_id == tenant_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<Tenant> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.Tenants
                                where (keyword == "" 
                                    || p.tenant_name.Contains(keyword)
                                
                                    || p.tenant_code.Contains(keyword)
                                )
                                select p);

                    List<db.Tenant> result = new List<db.Tenant>();

                    switch (order_by)
                    {
                        default:
                            result = data.OrderBy(s => s.tenant_id).Skip(skip).Take(take).ToList();
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
                    var data = (from p in db.Tenants
                                where (keyword == "" 
                                    || p.tenant_name.Contains(keyword)
                                
                                    || p.tenant_code.Contains(keyword)
                                )
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(Tenant tenant, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(tenant, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.Tenant, dm.Tenant> PrepareNestedInsert(PlaceholderContext database, dm.Tenant insertTenant)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertTenant, Crud.Insert);
                this.Validate(insertTenant, Crud.Insert);
                
                if (insertTenant.tenant_id == Guid.Empty)
                {
                    insertTenant.tenant_id = Guid.NewGuid();
                }
                insertTenant.created_utc = DateTime.UtcNow;
                insertTenant.updated_utc = insertTenant.created_utc;

                db.Tenant dbModel = insertTenant.ToDbModel();
                
                

                database.Tenants.Add(dbModel);
                
                
                return new NestedInsertInfo<db.Tenant, dm.Tenant>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertTenant
                };
            });
        }
        public dm.Tenant FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.Tenant, dm.Tenant> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                
                this.DependencyCoordinator.TenantInvalidated(Dependency.none, insertInfo.DbModel.tenant_id);
            
                return this.GetById(insertInfo.InsertModel.tenant_id);
            });
        }
        public NestedUpdateInfo<db.Tenant, dm.Tenant> PrepareNestedUpdate(PlaceholderContext database, dm.Tenant updateTenant)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateTenant, Crud.Update);
                this.Validate(updateTenant, Crud.Update);

                updateTenant.updated_utc = DateTime.UtcNow;
                
                db.Tenant found = (from n in database.Tenants
                                    where n.tenant_id == updateTenant.tenant_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.Tenant previous = found.ToDomainModel();

                    found = updateTenant.ToDbModel(found);
                    

                    return new NestedUpdateInfo<db.Tenant, dm.Tenant>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateTenant
                    };
                }
                return null;

            });
        }
        public dm.Tenant FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.Tenant, dm.Tenant> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                
                this.DependencyCoordinator.TenantInvalidated(Dependency.none, updateInfo.DbModel.tenant_id);

                return this.GetById(updateInfo.DbModel.tenant_id);
            });
        }
        
        public InterceptArgs<Tenant> Intercept(Tenant tenant, Crud crud)
        {
            InterceptArgs<Tenant> args = new InterceptArgs<Tenant>()
            {
                Crud = crud,
                ReturnEntity = tenant
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(Tenant tenant, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<Tenant> args);
        partial void PreProcess(Tenant tenant, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.Tenant tenant, Tenant insertTenant);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.Tenant tenant, Tenant updateTenant, Tenant previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.Tenant tenant);
        
    }
}

