using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class Tenant : SDKModel
    {	
        public Tenant()
        {
				
        }

        public virtual Guid tenant_id { get; set; }
        public virtual string tenant_name { get; set; }
        public virtual string tenant_code { get; set; }
        
	}
}

