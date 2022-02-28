using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class Asset : DomainModel
    {	
        public Asset()
        {
				
        }
    
        public Guid asset_id { get; set; }
        public AssetKind asset_kind { get; set; }
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
        public DateTime? resize_attempt_utc { get; set; }
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
        public Dependency dependencies { get; set; }
        public int encode_attempts { get; set; }
        public DateTime? encode_attempt_utc { get; set; }
        public string resize_mode { get; set; }
        public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }
	}
}

