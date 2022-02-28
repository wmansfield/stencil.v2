using System;
using System.Linq;
using dm = Placeholder.Domain;
using db = Placeholder.Data.Sql.Models;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    public partial class TenantBusiness
    {
        public dm.Tenant CreateInitialTenant(dm.Tenant insertIfEmpty)
        {
            return base.ExecuteFunction("CreateInitialTenant", delegate ()
            {
                dm.Tenant result = null;
                db.Tenant found = null;
                using (var database = base.CreateSQLSharedContext())
                {
                    found = (from a in database.Tenants
                             select a).FirstOrDefault();
                }
                if (found == null)
                {
                    result = this.Insert(insertIfEmpty);
                }
                return result;
            });
        }
        public dm.Tenant GetByShop(Guid shop_id)
        {
            return base.ExecuteFunction("GetByShop", delegate ()
            {
                using (var database = this.CreateSQLSharedContext())
                {
                    db.Tenant result = (from n in database.Shops
                                       where n.shop_id == shop_id
                                       select n.Tenant).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
    }
}
