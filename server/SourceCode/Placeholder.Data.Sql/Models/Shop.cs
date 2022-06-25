using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class Shop
    {
        public Shop()
        {
            Companies = new HashSet<Company>();
            ShopAccounts = new HashSet<ShopAccount>();
            ShopSettings = new HashSet<ShopSetting>();
            Widgets = new HashSet<Widget>();
        }

        public Guid shop_id { get; set; }
        public Guid tenant_id { get; set; }
        public string shop_name { get; set; }
        public string private_domain { get; set; }
        public string public_domain { get; set; }
        public DateTimeOffset created_utc { get; set; }
        public DateTimeOffset updated_utc { get; set; }
        public DateTimeOffset? deleted_utc { get; set; }
        public DateTimeOffset? sync_hydrate_utc { get; set; }
        public DateTimeOffset? sync_success_utc { get; set; }
        public DateTimeOffset? sync_invalid_utc { get; set; }
        public DateTimeOffset? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual ShopIsolated ShopIsolated { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<ShopAccount> ShopAccounts { get; set; }
        public virtual ICollection<ShopSetting> ShopSettings { get; set; }
        public virtual ICollection<Widget> Widgets { get; set; }
    }
}
