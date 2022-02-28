using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class Account : DomainModel
    {	
        public Account()
        {
				
        }
    
        public Guid account_id { get; set; }
        public Guid? asset_id_avatar { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string account_display { get; set; }
        public string password { get; set; }
        public string password_salt { get; set; }
        public AccountStatus account_status { get; set; }
        public string api_key { get; set; }
        public string api_secret { get; set; }
        public string timezone { get; set; }
        public string email_verify_token { get; set; }
        public DateTime? email_verify_utc { get; set; }
        public string entitlements { get; set; }
        public DateTime? password_changed_utc { get; set; }
        public string password_reset_token { get; set; }
        public DateTime? password_reset_utc { get; set; }
        public string single_login_token { get; set; }
        public DateTime? single_login_token_expire_utc { get; set; }
        public DateTime? last_login_utc { get; set; }
        public string last_login_platform { get; set; }
        public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }
        public DateTime? deleted_utc { get; set; }
        public DateTime? sync_success_utc { get; set; }
        public DateTime? sync_invalid_utc { get; set; }
        public DateTime? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }
        public DerivedField<Asset> RelatedAvatar { get; set; }
        
	}
}

