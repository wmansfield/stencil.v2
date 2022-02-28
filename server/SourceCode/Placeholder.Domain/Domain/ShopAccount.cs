using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class ShopAccount : DomainModel
    {	
        public ShopAccount()
        {
				
        }
    
        public Guid shop_account_id { get; set; }
        public Guid shop_id { get; set; }
        public Guid account_id { get; set; }
        public ShopRole shop_role { get; set; }
        public bool enabled { get; set; }
        public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }
        public DateTime? deleted_utc { get; set; }
        public DateTime? sync_success_utc { get; set; }
        public DateTime? sync_invalid_utc { get; set; }
        public DateTime? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }
	}
}

