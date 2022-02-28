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
    public partial class CompanyBusiness : BusinessBase, ICompanyBusiness, INestedOperation<db.Company, dm.Company>
    {
        public CompanyBusiness(IFoundation foundation)
            : base(foundation, "Company")
        {
        }
        
        public ICompanySynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<ICompanySynchronizer>();
            }
        }

        public Company Insert(Company insertCompany)
        {
            return this.Insert(insertCompany, Availability.Searchable);
        }
        public Company Insert(Company insertCompany, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(insertCompany.shop_id))
                {
                    

                    this.PreProcess(insertCompany, Crud.Insert);
                    this.Validate(insertCompany, Crud.Insert);
                    var interception = this.Intercept(insertCompany, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertCompany.company_id == Guid.Empty)
                    {
                        insertCompany.company_id = Guid.NewGuid();
                    }
                    if(insertCompany.created_utc == default(DateTime))
                    {
                        insertCompany.created_utc = DateTime.UtcNow;
                    }
                    if(insertCompany.updated_utc == default(DateTime))
                    {
                        insertCompany.updated_utc = insertCompany.created_utc;
                    }

                    db.Company dbModel = insertCompany.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    database.Companies.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertCompany);
                    
                    this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.company_id, dbModel.shop_id), availability);
                    this.AfterInsertIndexed(database, dbModel, insertCompany);
                    
                    this.DependencyCoordinator.CompanyInvalidated(Dependency.none, dbModel.company_id, dbModel.shop_id);
                }
                return this.GetById(insertCompany.shop_id, insertCompany.company_id);
            });
        }
        public Company Update(Company updateCompany)
        {
            return this.Update(updateCompany, Availability.Searchable);
        }
        public Company Update(Company updateCompany, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLIsolatedContext(updateCompany.shop_id))
                {
                    this.PreProcess(updateCompany, Crud.Update);
                    this.Validate(updateCompany, Crud.Update);
                    var interception = this.Intercept(updateCompany, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateCompany.updated_utc = DateTime.UtcNow;
                    
                    db.Company found = (from n in database.Companies
                                    where n.company_id == updateCompany.company_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Company previous = found.ToDomainModel();
                        
                        found = updateCompany.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateCompany, previous);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.company_id, found.shop_id), Availability.Searchable);
                        this.AfterUpdateIndexed(database, found);
                        
                        this.DependencyCoordinator.CompanyInvalidated(Dependency.none, found.company_id, found.shop_id);
                    
                    }
                    
                    return this.GetById(updateCompany.shop_id, updateCompany.company_id);
                }
            });
        }
        public void Delete(Guid shop_id, Guid company_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.Company found = (from a in database.Companies
                                    where a.company_id == company_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.company_id, found.shop_id), Availability.Searchable);
                        this.AfterDeleteIndexed(database, found);
                        
                        this.DependencyCoordinator.CompanyInvalidated(Dependency.none, found.company_id, found.shop_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid shop_id, Guid company_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    if (success)
                    {
                        db.Companies
                            .Where(x => (x.company_id == company_id)
                                    && ((x.sync_invalid_utc == null) || (x.sync_invalid_utc <= sync_date_utc)))
                            .Update(x => new db.Company()
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
                        db.Companies
                            .Where(x => x.company_id == company_id && x.sync_success_utc == null)
                            .Update(x => new db.Company()
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
                        var data = (from a in db.Companies
                                    where a.sync_success_utc == null
                                    && ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.company_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.Companies
                                    where a.sync_success_utc == null
                                    && (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.company_id, route_id = a.shop_id});
                        return data.ToList();
                    }
                }
            });
        }
        
        public void SynchronizationHydrateUpdate(Guid shop_id, Guid company_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
                {
                    using (var db = base.CreateSQLIsolatedContext(shop_id))
                    {
                        if (success)
                        {
                            db.Companies
                                .Where(x => x.company_id == company_id)
                                .Update(x => new db.Company()
                                {
                                    sync_hydrate_utc = sync_date_utc
                                });
                        }
                        else
                        {
                            db.Companies
                                .Where(x => x.company_id == company_id)
                                .Update(x => new db.Company()
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
                    var data = (from a in db.Companies
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.company_id, route_id = a.shop_id});
                    return data.ToList();
                }
            });
        }
        
        public Company GetById(Guid shop_id, Guid company_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    db.Company result = (from n in db.Companies
                                     where (n.company_id == company_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<Company> GetByShop(Guid shop_id)
        {
            return base.ExecuteFunction("GetByShop", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    var result = (from n in db.Companies
                                     where (n.shop_id == shop_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid shop_id, Guid company_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.Companies
                        .Where(x => x.company_id == company_id)
                        .Update(x => new db.Company() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        public List<Company> Find(Guid shop_id, int skip, int take, string keyword = "", string order_by = "", bool descending = false, bool? disabled = null)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.Companies
                                where (keyword == "" 
                                    || p.company_name.Contains(keyword)
                                )
                                && (p.shop_id == shop_id)
                                
                                && (disabled == null || p.disabled == disabled)
                                
                                select p);

                    List<db.Company> result = new List<db.Company>();

                    switch (order_by)
                    {
                        case "company_name":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.company_name).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.company_name).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        default:
                            result = data.OrderBy(s => s.company_id).Skip(skip).Take(take).ToList();
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        
        public int FindTotal(Guid shop_id, string keyword = "", bool? disabled = null)
        {
            return base.ExecuteFunction("FindTotal", delegate()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }
                    var data = (from p in db.Companies
                                where (keyword == "" 
                                    || p.company_name.Contains(keyword)
                                )
                                && (p.shop_id == shop_id)
                                
                                && (disabled == null || p.disabled == disabled)
                                
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(Company company, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(company, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.Company, dm.Company> PrepareNestedInsert(PlaceholderContext database, dm.Company insertCompany)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertCompany, Crud.Insert);
                this.Validate(insertCompany, Crud.Insert);
                
                if (insertCompany.company_id == Guid.Empty)
                {
                    insertCompany.company_id = Guid.NewGuid();
                }
                insertCompany.created_utc = DateTime.UtcNow;
                insertCompany.updated_utc = insertCompany.created_utc;

                db.Company dbModel = insertCompany.ToDbModel();
                
                dbModel.InvalidateSync(this.DefaultAgent, "insert");

                database.Companies.Add(dbModel);
                
                
                return new NestedInsertInfo<db.Company, dm.Company>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertCompany
                };
            });
        }
        public dm.Company FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.Company, dm.Company> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.company_id, insertInfo.DbModel.shop_id), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                this.DependencyCoordinator.CompanyInvalidated(Dependency.none, insertInfo.DbModel.company_id, insertInfo.DbModel.shop_id);
            
                return this.GetById(insertInfo.InsertModel.shop_id, insertInfo.InsertModel.company_id);
            });
        }
        public NestedUpdateInfo<db.Company, dm.Company> PrepareNestedUpdate(PlaceholderContext database, dm.Company updateCompany)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateCompany, Crud.Update);
                this.Validate(updateCompany, Crud.Update);

                
                db.Company found = (from n in database.Companies
                                    where n.company_id == updateCompany.company_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.Company previous = found.ToDomainModel();

                    found = updateCompany.ToDbModel(found);
                    found.InvalidateSync(this.DefaultAgent, "updated");

                    return new NestedUpdateInfo<db.Company, dm.Company>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateCompany
                    };
                }
                return null;

            });
        }
        public dm.Company FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.Company, dm.Company> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.company_id, updateInfo.DbModel.shop_id), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                
                this.DependencyCoordinator.CompanyInvalidated(Dependency.none, updateInfo.DbModel.company_id, updateInfo.DbModel.shop_id);

                return this.GetById(updateInfo.UpdateModel.shop_id, updateInfo.DbModel.company_id);
            });
        }
        
        public InterceptArgs<Company> Intercept(Company company, Crud crud)
        {
            InterceptArgs<Company> args = new InterceptArgs<Company>()
            {
                Crud = crud,
                ReturnEntity = company
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(Company company, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<Company> args);
        partial void PreProcess(Company company, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.Company company, Company insertCompany);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.Company company, Company updateCompany, Company previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.Company company);
        partial void AfterUpdateIndexed(PlaceholderContext database, db.Company company);
        partial void AfterInsertIndexed(PlaceholderContext database, db.Company company, Company insertCompany);
        partial void AfterDeleteIndexed(PlaceholderContext database, db.Company company);
    }
}

