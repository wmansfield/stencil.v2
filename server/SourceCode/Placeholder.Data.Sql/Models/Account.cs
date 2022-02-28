using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class Account
    {
        public Account()
        {
            ShopAccounts = new HashSet<ShopAccount>();
        }

        public Guid account_id { get; set; }
        public Guid? asset_id_avatar { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string account_display { get; set; }
        public string password { get; set; }
        public string password_salt { get; set; }
        public int account_status { get; set; }
        public string api_key { get; set; }
        public string api_secret { get; set; }
        public string timezone { get; set; }
        public string email_verify_token { get; set; }
        public DateTimeOffset? email_verify_utc { get; set; }
        public string entitlements { get; set; }
        public DateTimeOffset? password_changed_utc { get; set; }
        public string password_reset_token { get; set; }
        public DateTimeOffset? password_reset_utc { get; set; }
        public string single_login_token { get; set; }
        public DateTimeOffset? single_login_token_expire_utc { get; set; }
        public DateTimeOffset? last_login_utc { get; set; }
        public string last_login_platform { get; set; }
        public DateTimeOffset created_utc { get; set; }
        public DateTimeOffset updated_utc { get; set; }
        public DateTimeOffset? deleted_utc { get; set; }
        public DateTimeOffset? sync_hydrate_utc { get; set; }
        public DateTimeOffset? sync_success_utc { get; set; }
        public DateTimeOffset? sync_invalid_utc { get; set; }
        public DateTimeOffset? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual ICollection<ShopAccount> ShopAccounts { get; set; }
    }
}
