using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class ShopAccount
    {
        public Guid shop_account_id { get; set; }
        public Guid shop_id { get; set; }
        public Guid account_id { get; set; }
        public int shop_role { get; set; }
        public bool enabled { get; set; }
        public DateTimeOffset created_utc { get; set; }
        public DateTimeOffset updated_utc { get; set; }
        public DateTimeOffset? deleted_utc { get; set; }
        public DateTimeOffset? sync_hydrate_utc { get; set; }
        public DateTimeOffset? sync_success_utc { get; set; }
        public DateTimeOffset? sync_invalid_utc { get; set; }
        public DateTimeOffset? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }

        public virtual Account Account { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
