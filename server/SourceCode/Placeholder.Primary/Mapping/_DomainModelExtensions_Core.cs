using AutoMapper;
using Zero.Foundation;
using System;
using System.Collections.Generic;
using db = Placeholder.Data.Sql.Models;
using dm = Placeholder.Domain;

namespace Placeholder.Primary
{
    public static partial class _DomainModelExtensions
    {
        private static Lazy<IMapper> _Mapper = new Lazy<IMapper>(() => 
        {
            // just a touch of anti-pattern
            IMapper mapper = CoreFoundation.Current.ResolveWithFallback<IMapper>();
            return mapper;
        });
        public static IMapper Mapper
        {
            get
            {
                return _Mapper.Value;
            }
        }
        
        public static db.GlobalSetting ToDbModel(this dm.GlobalSetting entity, db.GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.GlobalSetting(); }
                return Mapper.Map<dm.GlobalSetting, db.GlobalSetting>(entity, destination);
            }
            return null;
        }
        public static dm.GlobalSetting ToDomainModel(this db.GlobalSetting entity, dm.GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.GlobalSetting(); }
                return Mapper.Map<db.GlobalSetting, dm.GlobalSetting>(entity, destination);
            }
            return null;
        }
        public static List<dm.GlobalSetting> ToDomainModel(this IEnumerable<db.GlobalSetting> entities)
        {
            List<dm.GlobalSetting> result = new List<dm.GlobalSetting>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.Tenant ToDbModel(this dm.Tenant entity, db.Tenant destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.Tenant(); }
                return Mapper.Map<dm.Tenant, db.Tenant>(entity, destination);
            }
            return null;
        }
        public static dm.Tenant ToDomainModel(this db.Tenant entity, dm.Tenant destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.Tenant(); }
                return Mapper.Map<db.Tenant, dm.Tenant>(entity, destination);
            }
            return null;
        }
        public static List<dm.Tenant> ToDomainModel(this IEnumerable<db.Tenant> entities)
        {
            List<dm.Tenant> result = new List<dm.Tenant>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.Asset ToDbModel(this dm.Asset entity, db.Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.Asset(); }
                return Mapper.Map<dm.Asset, db.Asset>(entity, destination);
            }
            return null;
        }
        public static dm.Asset ToDomainModel(this db.Asset entity, dm.Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.Asset(); }
                return Mapper.Map<db.Asset, dm.Asset>(entity, destination);
            }
            return null;
        }
        public static List<dm.Asset> ToDomainModel(this IEnumerable<db.Asset> entities)
        {
            List<dm.Asset> result = new List<dm.Asset>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.Account ToDbModel(this dm.Account entity, db.Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.Account(); }
                return Mapper.Map<dm.Account, db.Account>(entity, destination);
            }
            return null;
        }
        public static dm.Account ToDomainModel(this db.Account entity, dm.Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.Account(); }
                return Mapper.Map<db.Account, dm.Account>(entity, destination);
            }
            return null;
        }
        public static List<dm.Account> ToDomainModel(this IEnumerable<db.Account> entities)
        {
            List<dm.Account> result = new List<dm.Account>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.Shop ToDbModel(this dm.Shop entity, db.Shop destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.Shop(); }
                return Mapper.Map<dm.Shop, db.Shop>(entity, destination);
            }
            return null;
        }
        public static dm.Shop ToDomainModel(this db.Shop entity, dm.Shop destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.Shop(); }
                return Mapper.Map<db.Shop, dm.Shop>(entity, destination);
            }
            return null;
        }
        public static List<dm.Shop> ToDomainModel(this IEnumerable<db.Shop> entities)
        {
            List<dm.Shop> result = new List<dm.Shop>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.ShopIsolated ToDbModel(this dm.ShopIsolated entity, db.ShopIsolated destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.ShopIsolated(); }
                return Mapper.Map<dm.ShopIsolated, db.ShopIsolated>(entity, destination);
            }
            return null;
        }
        public static dm.ShopIsolated ToDomainModel(this db.ShopIsolated entity, dm.ShopIsolated destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.ShopIsolated(); }
                return Mapper.Map<db.ShopIsolated, dm.ShopIsolated>(entity, destination);
            }
            return null;
        }
        public static List<dm.ShopIsolated> ToDomainModel(this IEnumerable<db.ShopIsolated> entities)
        {
            List<dm.ShopIsolated> result = new List<dm.ShopIsolated>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.ShopAccount ToDbModel(this dm.ShopAccount entity, db.ShopAccount destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.ShopAccount(); }
                return Mapper.Map<dm.ShopAccount, db.ShopAccount>(entity, destination);
            }
            return null;
        }
        public static dm.ShopAccount ToDomainModel(this db.ShopAccount entity, dm.ShopAccount destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.ShopAccount(); }
                return Mapper.Map<db.ShopAccount, dm.ShopAccount>(entity, destination);
            }
            return null;
        }
        public static List<dm.ShopAccount> ToDomainModel(this IEnumerable<db.ShopAccount> entities)
        {
            List<dm.ShopAccount> result = new List<dm.ShopAccount>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.ShopSetting ToDbModel(this dm.ShopSetting entity, db.ShopSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.ShopSetting(); }
                return Mapper.Map<dm.ShopSetting, db.ShopSetting>(entity, destination);
            }
            return null;
        }
        public static dm.ShopSetting ToDomainModel(this db.ShopSetting entity, dm.ShopSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.ShopSetting(); }
                return Mapper.Map<db.ShopSetting, dm.ShopSetting>(entity, destination);
            }
            return null;
        }
        public static List<dm.ShopSetting> ToDomainModel(this IEnumerable<db.ShopSetting> entities)
        {
            List<dm.ShopSetting> result = new List<dm.ShopSetting>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static db.Company ToDbModel(this dm.Company entity, db.Company destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.Company(); }
                return Mapper.Map<dm.Company, db.Company>(entity, destination);
            }
            return null;
        }
        public static dm.Company ToDomainModel(this db.Company entity, dm.Company destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.Company(); }
                return Mapper.Map<db.Company, dm.Company>(entity, destination);
            }
            return null;
        }
        public static List<dm.Company> ToDomainModel(this IEnumerable<db.Company> entities)
        {
            List<dm.Company> result = new List<dm.Company>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
    }
}

