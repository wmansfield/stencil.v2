using System;
using Placeholder.Common.Data;
using db = Placeholder.Data.Sql.Models;
using dm = Placeholder.Domain;

namespace Placeholder.Primary
{
    public static partial class _DomainModelExtensions
    {
        public static dm.Account ToDomainModel(this Interim<db.Account, db.Asset> entity, dm.Account destination = null)
        {
            if (entity != null && entity.Item1 != null)
            {
                return entity.Item1.ToDomainModel(entity.Item2, destination);
            }
            return null;
        }
        public static dm.Account ToDomainModel(this db.Account entity, db.Asset asset, dm.Account destination = null)
        {
            if (entity != null)
            {
                dm.Account result = entity.ToDomainModel(destination);
                if (asset != null)
                {
                    result.RelatedAvatar = new dm.DerivedField<dm.Asset>(asset.ToDomainModel());
                }
                return result;
            }
            return null;
        }


    }
}
