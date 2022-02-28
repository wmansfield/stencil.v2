using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class Tenant : DomainModel
    {	
        public Tenant()
        {
				
        }
    
        public Guid tenant_id { get; set; }
        public string tenant_name { get; set; }
        public string tenant_code { get; set; }
        public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }
	}
}

