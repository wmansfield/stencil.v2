using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.SDK.Models
{
    public partial class GlobalSetting : SDKModel
    {	
        public GlobalSetting()
        {
				
        }

        public virtual Guid global_setting_id { get; set; }
        public virtual string name { get; set; }
        public virtual string value { get; set; }
        public virtual string value_encrypted { get; set; }
        public virtual bool encrypted { get; set; }
        
	}
}

