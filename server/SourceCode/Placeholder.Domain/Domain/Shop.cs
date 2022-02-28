using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class Shop : DomainModel
    {	
        public Shop()
        {
				
        }
    
        public Guid shop_id { get; set; }
        public Guid tenant_id { get; set; }
        public string shop_name { get; set; }
        public string private_domain { get; set; }
        public string public_domain { get; set; }
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

