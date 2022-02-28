using System;
using sdk = Placeholder.SDK.Models;
using dm = Placeholder.Domain;
using System.Linq;
using System.Collections.Generic;
using Placeholder.Domain;
using AutoMapper;

namespace Placeholder.Primary
{
    public static partial class _SDKModelExtensions
    {
        public static bool IsSuperAdmin(this sdk.Account account)
        {
            return account.HasEntitlement(dm.WellKnownEntitlements.super_admin.ToString());
        }
        public static bool IsAdmin(this sdk.Account account)
        {
            return account.HasEntitlement(dm.WellKnownEntitlements.admin.ToString());
        }
        public static bool HasEntitlement(this sdk.Account account, string entitlement)
        {
            if (account != null && !string.IsNullOrWhiteSpace(account.entitlements) && !string.IsNullOrWhiteSpace(entitlement))
            {
                return account.entitlements.Split(',').Contains(entitlement);
            }
            return false;
        }

        public static List<sdk.IDPair> ToIDPair(this IEnumerable<TimeZoneInfo> items)
        {
            List<sdk.IDPair> result = new List<sdk.IDPair>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    result.Add(item.ToIDPair());
                }
            }
            return result;
        }
        public static sdk.IDPair ToIDPair(this TimeZoneInfo item)
        {
            if (item != null)
            {
                return new sdk.IDPair()
                {
                    id = item.StandardName,
                    name = item.DisplayName,
                    desc = ((int)item.GetUtcOffset(DateTime.UtcNow).TotalMinutes).ToString()
                };
            }

            return null;
        }
        public static sdk.Responses.AccountInfo ToInfoModel(this dm.Account entity)
        {
            if (entity != null)
            {
                sdk.Responses.AccountInfo response = Mapper.Map<dm.Account, sdk.Responses.AccountInfo>(entity);
                response.super_admin = entity.IsSuperAdmin();
                response.admin = entity.IsAdmin();
                return response;
            }
            return null;
        }
        public static sdk.Responses.AccountInfo ToInfoModel(this sdk.Account entity)
        {
            if (entity != null)
            {
                sdk.Responses.AccountInfo response = Mapper.Map<sdk.Account, sdk.Responses.AccountInfo>(entity);
                response.super_admin = entity.IsSuperAdmin();
                response.admin = entity.IsAdmin();
                return response;
            }
            return null;
        }

        public static sdk.AssetInfo ToRelatedModel(this DerivedField<dm.Asset> entity)
        {
            if (entity != null && entity.Value != null)
            {
                return entity.Value.ToRelatedModel();
            }
            return null;
        }
        public static sdk.AssetInfo ToRelatedModel(this dm.Asset entity)
        {
            if (entity != null)
            {
                sdk.AssetInfo result = new sdk.AssetInfo();
                result.url_large = entity.thumb_large_url;
                result.url_small = entity.thumb_small_url;
                result.asset_id = entity.asset_id;
                return result;
            }
            return null;
        }



    }
}
