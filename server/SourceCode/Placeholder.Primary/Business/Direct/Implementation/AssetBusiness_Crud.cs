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
    public partial class AssetBusiness : BusinessBase, IAssetBusiness, INestedOperation<db.Asset, dm.Asset>
    {
        public AssetBusiness(IFoundation foundation)
            : base(foundation, "Asset")
        {
        }
        
        

        public Asset Insert(Asset insertAsset)
        {
            return this.Insert(insertAsset, Availability.Retrievable);
        }
        public Asset Insert(Asset insertAsset, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    

                    this.PreProcess(insertAsset, Crud.Insert);
                    this.Validate(insertAsset, Crud.Insert);
                    var interception = this.Intercept(insertAsset, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertAsset.asset_id == Guid.Empty)
                    {
                        insertAsset.asset_id = Guid.NewGuid();
                    }
                    if(insertAsset.created_utc == default(DateTime))
                    {
                        insertAsset.created_utc = DateTime.UtcNow;
                    }
                    if(insertAsset.updated_utc == default(DateTime))
                    {
                        insertAsset.updated_utc = insertAsset.created_utc;
                    }

                    db.Asset dbModel = insertAsset.ToDbModel();
                    
                    

                    database.Assets.Add(dbModel);
                    
                    
                    database.SaveChanges();

                    
                    
                    this.AfterInsertPersisted(database, dbModel, insertAsset);
                    
                    
                    this.DependencyCoordinator.AssetInvalidated(Dependency.none, dbModel.asset_id);
                }
                return this.GetById(insertAsset.asset_id);
            });
        }
        public Asset Update(Asset updateAsset)
        {
            return this.Update(updateAsset, Availability.Retrievable);
        }
        public Asset Update(Asset updateAsset, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateAsset, Crud.Update);
                    this.Validate(updateAsset, Crud.Update);
                    var interception = this.Intercept(updateAsset, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateAsset.updated_utc = DateTime.UtcNow;
                    
                    db.Asset found = (from n in database.Assets
                                    where n.asset_id == updateAsset.asset_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Asset previous = found.ToDomainModel();
                        
                        found = updateAsset.ToDbModel(found);
                        
                        database.SaveChanges();

                        

                        this.AfterUpdatePersisted(database, found, updateAsset, previous);
                        
                        
                        this.DependencyCoordinator.AssetInvalidated(Dependency.none, found.asset_id);
                    
                    }
                    
                    return this.GetById(updateAsset.asset_id);
                }
            });
        }
        public void Delete(Guid asset_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Asset found = (from a in database.Assets
                                    where a.asset_id == asset_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        database.Assets.Remove(found);
                        

                        database.SaveChanges();

                        
                        
                        this.AfterDeletePersisted(database, found);
                        
                        
                        this.DependencyCoordinator.AssetInvalidated(Dependency.none, found.asset_id);
                    }
                }
            });
        }
        
        public Asset GetById(Guid asset_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    db.Asset result = (from n in db.Assets
                                     where (n.asset_id == asset_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<Asset> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLSharedContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.Assets
                                where (keyword == "" 
                                    || p.public_url.Contains(keyword)
                                )
                                select p);

                    List<db.Asset> result = new List<db.Asset>();

                    switch (order_by)
                    {
                        default:
                            result = data.OrderBy(s => s.asset_id).Skip(skip).Take(take).ToList();
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
                    var data = (from p in db.Assets
                                where (keyword == "" 
                                    || p.public_url.Contains(keyword)
                                )
                                select p).Count();

                    
                    return data;
                }
            });
        }
        


        public virtual void Validate(Asset asset, Crud crud)
        {
            Dictionary<string, LocalizableString> errors = new Dictionary<string, LocalizableString>();
            

            this.ValidatePostProcess(asset, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo<db.Asset, dm.Asset> PrepareNestedInsert(PlaceholderContext database, dm.Asset insertAsset)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                

                this.PreProcess(insertAsset, Crud.Insert);
                this.Validate(insertAsset, Crud.Insert);
                
                if (insertAsset.asset_id == Guid.Empty)
                {
                    insertAsset.asset_id = Guid.NewGuid();
                }
                insertAsset.created_utc = DateTime.UtcNow;
                insertAsset.updated_utc = insertAsset.created_utc;

                db.Asset dbModel = insertAsset.ToDbModel();
                
                

                database.Assets.Add(dbModel);
                
                
                return new NestedInsertInfo<db.Asset, dm.Asset>()
                { 
                    DbModel = dbModel,  
                    InsertModel = insertAsset
                };
            });
        }
        public dm.Asset FinalizeNestedInsert(PlaceholderContext database, NestedInsertInfo<db.Asset, dm.Asset> insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                
                this.DependencyCoordinator.AssetInvalidated(Dependency.none, insertInfo.DbModel.asset_id);
            
                return this.GetById(insertInfo.InsertModel.asset_id);
            });
        }
        public NestedUpdateInfo<db.Asset, dm.Asset> PrepareNestedUpdate(PlaceholderContext database, dm.Asset updateAsset)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(updateAsset, Crud.Update);
                this.Validate(updateAsset, Crud.Update);

                updateAsset.updated_utc = DateTime.UtcNow;
                
                db.Asset found = (from n in database.Assets
                                    where n.asset_id == updateAsset.asset_id
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.Asset previous = found.ToDomainModel();

                    found = updateAsset.ToDbModel(found);
                    

                    return new NestedUpdateInfo<db.Asset, dm.Asset>()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = updateAsset
                    };
                }
                return null;

            });
        }
        public dm.Asset FinalizeNestedUpdate(PlaceholderContext database, NestedUpdateInfo<db.Asset, dm.Asset> updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                
                this.DependencyCoordinator.AssetInvalidated(Dependency.none, updateInfo.DbModel.asset_id);

                return this.GetById(updateInfo.DbModel.asset_id);
            });
        }
        
        public InterceptArgs<Asset> Intercept(Asset asset, Crud crud)
        {
            InterceptArgs<Asset> args = new InterceptArgs<Asset>()
            {
                Crud = crud,
                ReturnEntity = asset
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(Asset asset, Crud crud, Dictionary<string, LocalizableString> errors);
        partial void PerformIntercept(InterceptArgs<Asset> args);
        partial void PreProcess(Asset asset, Crud crud);
        partial void AfterInsertPersisted(PlaceholderContext database, db.Asset asset, Asset insertAsset);
        partial void AfterUpdatePersisted(PlaceholderContext database, db.Asset asset, Asset updateAsset, Asset previous);
        partial void AfterDeletePersisted(PlaceholderContext database, db.Asset asset);
        
    }
}

