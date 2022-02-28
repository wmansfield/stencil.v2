using System;
using System.Linq;
using System.Collections.Generic;
using Placeholder.Domain;
using db = Placeholder.Data.Sql.Models;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    public partial class ShopBusiness
    {
        public Shop GetByIdFromIsolated(Guid shop_id)
        {
            return base.ExecuteFunction("GetByIdFromIsolated", delegate ()
            {
                using (var db = this.CreateSQLIsolatedContext(shop_id))
                {
                    db.Shop result = (from n in db.Shops
                                      where (n.shop_id == shop_id)
                                      select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
    }
}
