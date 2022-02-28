using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class ShopSetting
    {
        public Guid shop_setting_id { get; set; }
        public Guid shop_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string value_encrypted { get; set; }
        public bool encrypted { get; set; }
        public DateTimeOffset created_utc { get; set; }
        public DateTimeOffset updated_utc { get; set; }
        public DateTimeOffset? deleted_utc { get; set; }
        public DateTimeOffset? sync_hydrate_utc { get; set; }
        public DateTimeOffset? sync_success_utc { get; set; }
        public DateTimeOffset? sync_invalid_utc { get; set; }
        public DateTimeOffset? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
