using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class ShopSetting : DomainModel
    {	
        public ShopSetting()
        {
				
        }
    
        public Guid shop_setting_id { get; set; }
        public Guid shop_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string value_encrypted { get; set; }
        public bool encrypted { get; set; }
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

