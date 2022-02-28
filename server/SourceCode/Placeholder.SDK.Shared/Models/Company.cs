using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class Company : SDKModel
    {	
        public Company()
        {
				
        }

        public virtual Guid company_id { get; set; }
        public virtual Guid shop_id { get; set; }
        public virtual string company_name { get; set; }
        public virtual bool disabled { get; set; }
        
	}
}

