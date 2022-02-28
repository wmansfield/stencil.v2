using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class ShopAccount : SDKModel
    {	
        public ShopAccount()
        {
				
        }

        public virtual Guid shop_account_id { get; set; }
        public virtual Guid shop_id { get; set; }
        public virtual Guid account_id { get; set; }
        public virtual ShopRole shop_role { get; set; }
        public virtual bool enabled { get; set; }
        
        /// <summary>
        /// Index Only
        /// </summary>
        public string shop_name { get; set; }
        
	}
}

