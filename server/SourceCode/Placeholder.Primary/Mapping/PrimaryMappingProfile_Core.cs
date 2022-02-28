using System;
using db = Placeholder.Data.Sql.Models;
using dm = Placeholder.Domain;

namespace Placeholder.Primary.Mapping
{
    public partial class PrimaryMappingProfile : AutoMapper.Profile
    {
        public PrimaryMappingProfile()
            : base("PrimaryMappingProfile")
        {
            this.Configure();
        }

        protected void Configure()
        {
            this.DbAndDomainMappings();
            this.DomainAndSDKMappings();
            
            this.DbAndDomainMappings_Manual();
            this.DomainAndSDKMappings_Manual();
        }
        
        partial void DbAndDomainMappings_Manual();
        partial void DomainAndSDKMappings_Manual();
        
        protected void DbAndDomainMappings()
        {
            this.CreateMap<DateTimeOffset?, DateTime?>()
                .ConvertUsing(x => x.HasValue ? x.Value.UtcDateTime : (DateTime?)null);

            this.CreateMap<DateTimeOffset, DateTime?>()
                .ConvertUsing(x => x.UtcDateTime);

            this.CreateMap<DateTimeOffset, DateTime>()
                .ConvertUsing(x => x.UtcDateTime);

            this.CreateMap<DateTime?, DateTimeOffset?>()
                .ConvertUsing(x => x.HasValue ? new DateTimeOffset(x.Value) : (DateTimeOffset?)null);
                
            this.CreateMap<db.GlobalSetting, dm.GlobalSetting>();
            this.CreateMap<dm.GlobalSetting, db.GlobalSetting>();
            this.CreateMap<db.Tenant, dm.Tenant>();
            this.CreateMap<dm.Tenant, db.Tenant>();
            this.CreateMap<db.Asset, dm.Asset>();
            this.CreateMap<dm.Asset, db.Asset>();
            this.CreateMap<db.Account, dm.Account>();
            this.CreateMap<dm.Account, db.Account>();
            this.CreateMap<db.Shop, dm.Shop>();
            this.CreateMap<dm.Shop, db.Shop>();
            this.CreateMap<db.ShopIsolated, dm.ShopIsolated>();
            this.CreateMap<dm.ShopIsolated, db.ShopIsolated>();
            this.CreateMap<db.ShopAccount, dm.ShopAccount>();
            this.CreateMap<dm.ShopAccount, db.ShopAccount>();
            this.CreateMap<db.ShopSetting, dm.ShopSetting>();
            this.CreateMap<dm.ShopSetting, db.ShopSetting>();
            this.CreateMap<db.Company, dm.Company>();
            this.CreateMap<dm.Company, db.Company>();
            
        }
        protected void DomainAndSDKMappings()
        {
            this.CreateMap<Domain.AccountStatus, SDK.Models.AccountStatus>().ConvertUsing(x => (SDK.Models.AccountStatus)(int)x);
            this.CreateMap<SDK.Models.AccountStatus, Domain.AccountStatus>().ConvertUsing(x => (Domain.AccountStatus)(int)x);
            this.CreateMap<Domain.AssetKind, SDK.Models.AssetKind>().ConvertUsing(x => (SDK.Models.AssetKind)(int)x);
            this.CreateMap<SDK.Models.AssetKind, Domain.AssetKind>().ConvertUsing(x => (Domain.AssetKind)(int)x);
            this.CreateMap<Domain.Dependency, SDK.Models.Dependency>().ConvertUsing(x => (SDK.Models.Dependency)(int)x);
            this.CreateMap<SDK.Models.Dependency, Domain.Dependency>().ConvertUsing(x => (Domain.Dependency)(int)x);
            this.CreateMap<Domain.EmailTemplateKind, SDK.Models.EmailTemplateKind>().ConvertUsing(x => (SDK.Models.EmailTemplateKind)(int)x);
            this.CreateMap<SDK.Models.EmailTemplateKind, Domain.EmailTemplateKind>().ConvertUsing(x => (Domain.EmailTemplateKind)(int)x);
            this.CreateMap<Domain.ShopRole, SDK.Models.ShopRole>().ConvertUsing(x => (SDK.Models.ShopRole)(int)x);
            this.CreateMap<SDK.Models.ShopRole, Domain.ShopRole>().ConvertUsing(x => (Domain.ShopRole)(int)x);
            
            this.CreateMap<Domain.GlobalSetting, SDK.Models.GlobalSetting>();
            this.CreateMap<SDK.Models.GlobalSetting, Domain.GlobalSetting>();
            
            this.CreateMap<Domain.Tenant, SDK.Models.Tenant>();
            this.CreateMap<SDK.Models.Tenant, Domain.Tenant>();
            
            this.CreateMap<Domain.Asset, SDK.Models.Asset>();
            this.CreateMap<SDK.Models.Asset, Domain.Asset>();
            
            this.CreateMap<Domain.Account, SDK.Models.Account>();
            this.CreateMap<SDK.Models.Account, Domain.Account>();
            
            this.CreateMap<Domain.Shop, SDK.Models.Shop>();
            this.CreateMap<SDK.Models.Shop, Domain.Shop>();
            
            this.CreateMap<Domain.ShopIsolated, SDK.Models.ShopIsolated>();
            this.CreateMap<SDK.Models.ShopIsolated, Domain.ShopIsolated>();
            
            this.CreateMap<Domain.ShopAccount, SDK.Models.ShopAccount>();
            this.CreateMap<SDK.Models.ShopAccount, Domain.ShopAccount>();
            
            this.CreateMap<Domain.ShopSetting, SDK.Models.ShopSetting>();
            this.CreateMap<SDK.Models.ShopSetting, Domain.ShopSetting>();
            
            this.CreateMap<Domain.Company, SDK.Models.Company>();
            this.CreateMap<SDK.Models.Company, Domain.Company>();
            
        }
    }
}

