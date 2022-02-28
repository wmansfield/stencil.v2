using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class ShopIsolated : SDKModel
    {	
        public ShopIsolated()
        {
				
        }

        
        public static string GLOBAL_PARTITION = "ShopIsolated";
        public virtual Guid shop_id { get; set; }
        public virtual bool webhoooks_enabled { get; set; }
        public virtual bool fulfillment_enabled { get; set; }
        
         /// <summary>
        /// Index Only
        /// </summary>
        public string partition_key { get { return GLOBAL_PARTITION; } }
        
	}
}

