using AutoMapper;
using Zero.Foundation;
using System;
using System.Collections.Generic;
using Placeholder.Data.Sql;
using Placeholder.Domain;

namespace Placeholder.Primary
{
    public static partial class _SDKModelExtensions
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

        
        public static GlobalSetting ToDomainModel(this SDK.Models.GlobalSetting entity, GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.GlobalSetting(); }
                GlobalSetting result = Mapper.Map<SDK.Models.GlobalSetting, GlobalSetting>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.GlobalSetting ToSDKModel(this GlobalSetting entity, SDK.Models.GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.GlobalSetting(); }
                SDK.Models.GlobalSetting result = Mapper.Map<GlobalSetting, SDK.Models.GlobalSetting>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.GlobalSetting> ToSDKModel(this IEnumerable<GlobalSetting> entities)
        {
            List<SDK.Models.GlobalSetting> result = new List<SDK.Models.GlobalSetting>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Tenant ToDomainModel(this SDK.Models.Tenant entity, Tenant destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Tenant(); }
                Tenant result = Mapper.Map<SDK.Models.Tenant, Tenant>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Tenant ToSDKModel(this Tenant entity, SDK.Models.Tenant destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Tenant(); }
                SDK.Models.Tenant result = Mapper.Map<Tenant, SDK.Models.Tenant>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Tenant> ToSDKModel(this IEnumerable<Tenant> entities)
        {
            List<SDK.Models.Tenant> result = new List<SDK.Models.Tenant>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Asset ToDomainModel(this SDK.Models.Asset entity, Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Asset(); }
                Asset result = Mapper.Map<SDK.Models.Asset, Asset>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Asset ToSDKModel(this Asset entity, SDK.Models.Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Asset(); }
                SDK.Models.Asset result = Mapper.Map<Asset, SDK.Models.Asset>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Asset> ToSDKModel(this IEnumerable<Asset> entities)
        {
            List<SDK.Models.Asset> result = new List<SDK.Models.Asset>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Account ToDomainModel(this SDK.Models.Account entity, Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Account(); }
                Account result = Mapper.Map<SDK.Models.Account, Account>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Account ToSDKModel(this Account entity, SDK.Models.Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Account(); }
                SDK.Models.Account result = Mapper.Map<Account, SDK.Models.Account>(entity, destination);
                result.avatar = entity.RelatedAvatar.GetValueOrDefault().ToRelatedModel();
                
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Account> ToSDKModel(this IEnumerable<Account> entities)
        {
            List<SDK.Models.Account> result = new List<SDK.Models.Account>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Shop ToDomainModel(this SDK.Models.Shop entity, Shop destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Shop(); }
                Shop result = Mapper.Map<SDK.Models.Shop, Shop>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Shop ToSDKModel(this Shop entity, SDK.Models.Shop destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Shop(); }
                SDK.Models.Shop result = Mapper.Map<Shop, SDK.Models.Shop>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Shop> ToSDKModel(this IEnumerable<Shop> entities)
        {
            List<SDK.Models.Shop> result = new List<SDK.Models.Shop>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static ShopIsolated ToDomainModel(this SDK.Models.ShopIsolated entity, ShopIsolated destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.ShopIsolated(); }
                ShopIsolated result = Mapper.Map<SDK.Models.ShopIsolated, ShopIsolated>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.ShopIsolated ToSDKModel(this ShopIsolated entity, SDK.Models.ShopIsolated destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.ShopIsolated(); }
                SDK.Models.ShopIsolated result = Mapper.Map<ShopIsolated, SDK.Models.ShopIsolated>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.ShopIsolated> ToSDKModel(this IEnumerable<ShopIsolated> entities)
        {
            List<SDK.Models.ShopIsolated> result = new List<SDK.Models.ShopIsolated>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static ShopAccount ToDomainModel(this SDK.Models.ShopAccount entity, ShopAccount destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.ShopAccount(); }
                ShopAccount result = Mapper.Map<SDK.Models.ShopAccount, ShopAccount>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.ShopAccount ToSDKModel(this ShopAccount entity, SDK.Models.ShopAccount destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.ShopAccount(); }
                SDK.Models.ShopAccount result = Mapper.Map<ShopAccount, SDK.Models.ShopAccount>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.ShopAccount> ToSDKModel(this IEnumerable<ShopAccount> entities)
        {
            List<SDK.Models.ShopAccount> result = new List<SDK.Models.ShopAccount>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static ShopSetting ToDomainModel(this SDK.Models.ShopSetting entity, ShopSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.ShopSetting(); }
                ShopSetting result = Mapper.Map<SDK.Models.ShopSetting, ShopSetting>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.ShopSetting ToSDKModel(this ShopSetting entity, SDK.Models.ShopSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.ShopSetting(); }
                SDK.Models.ShopSetting result = Mapper.Map<ShopSetting, SDK.Models.ShopSetting>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.ShopSetting> ToSDKModel(this IEnumerable<ShopSetting> entities)
        {
            List<SDK.Models.ShopSetting> result = new List<SDK.Models.ShopSetting>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Company ToDomainModel(this SDK.Models.Company entity, Company destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Company(); }
                Company result = Mapper.Map<SDK.Models.Company, Company>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Company ToSDKModel(this Company entity, SDK.Models.Company destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Company(); }
                SDK.Models.Company result = Mapper.Map<Company, SDK.Models.Company>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Company> ToSDKModel(this IEnumerable<Company> entities)
        {
            List<SDK.Models.Company> result = new List<SDK.Models.Company>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
    }
}

