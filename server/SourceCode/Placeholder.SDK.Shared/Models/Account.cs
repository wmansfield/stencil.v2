using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class Account : SDKModel
    {	
        public Account()
        {
				
        }

        public virtual Guid account_id { get; set; }
        public virtual Guid? asset_id_avatar { get; set; }
        public virtual string email { get; set; }
        public virtual string first_name { get; set; }
        public virtual string last_name { get; set; }
        public virtual string account_display { get; set; }
        public virtual string password { get; set; }
        public virtual string password_salt { get; set; }
        public virtual AccountStatus account_status { get; set; }
        public virtual string api_key { get; set; }
        public virtual string api_secret { get; set; }
        public virtual string timezone { get; set; }
        public virtual string email_verify_token { get; set; }
        public virtual DateTime? email_verify_utc { get; set; }
        public virtual string entitlements { get; set; }
        public virtual DateTime? password_changed_utc { get; set; }
        public virtual string password_reset_token { get; set; }
        public virtual DateTime? password_reset_utc { get; set; }
        public virtual string single_login_token { get; set; }
        public virtual DateTime? single_login_token_expire_utc { get; set; }
        public virtual DateTime? last_login_utc { get; set; }
        public virtual string last_login_platform { get; set; }
        
         /// <summary>
        /// Index Only
        /// </summary>
        public string partition_key { get { return this.account_id.ToString().Substring(0, 5); } }
        
        /// <summary>
        /// Index Only
        /// </summary>
        public AssetInfo avatar { get; set; }
        
	}
}

