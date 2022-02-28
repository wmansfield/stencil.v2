using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class Asset
    {
        public Asset()
        {
            Accounts = new HashSet<Account>();
        }

        public Guid asset_id { get; set; }
        public int asset_kind { get; set; }
        public bool available { get; set; }
        public bool resize_required { get; set; }
        public bool encode_required { get; set; }
        public bool resize_processing { get; set; }
        public bool encode_processing { get; set; }
        public string thumb_small_dimensions { get; set; }
        public string thumb_medium_dimensions { get; set; }
        public string thumb_large_dimensions { get; set; }
        public string resize_status { get; set; }
        public int resize_attempts { get; set; }
        public DateTimeOffset? resize_attempt_utc { get; set; }
        public string encode_identifier { get; set; }
        public string encode_status { get; set; }
        public string relative_path { get; set; }
        public string raw_url { get; set; }
        public string public_url { get; set; }
        public string thumb_small_url { get; set; }
        public string thumb_medium_url { get; set; }
        public string thumb_large_url { get; set; }
        public string encode_log { get; set; }
        public string resize_log { get; set; }
        public int dependencies { get; set; }
        public int encode_attempts { get; set; }
        public DateTimeOffset? encode_attempt_utc { get; set; }
        public string resize_mode { get; set; }
        public DateTimeOffset created_utc { get; set; }
        public DateTimeOffset updated_utc { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
