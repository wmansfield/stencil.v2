using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class Tenant
    {
        public Tenant()
        {
            Shops = new HashSet<Shop>();
        }

        public Guid tenant_id { get; set; }
        public string tenant_name { get; set; }
        public string tenant_code { get; set; }
        public DateTimeOffset created_utc { get; set; }
        public DateTimeOffset updated_utc { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }
    }
}
