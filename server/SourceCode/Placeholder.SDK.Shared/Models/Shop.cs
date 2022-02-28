using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class Shop : SDKModel
    {	
        public Shop()
        {
				
        }

        public virtual Guid shop_id { get; set; }
        public virtual Guid tenant_id { get; set; }
        public virtual string shop_name { get; set; }
        public virtual string private_domain { get; set; }
        public virtual string public_domain { get; set; }
        
         /// <summary>
        /// Index Only
        /// </summary>
        public string partition_key { get { return this.shop_id.ToString().Substring(0, 5); } }
        
	}
}

