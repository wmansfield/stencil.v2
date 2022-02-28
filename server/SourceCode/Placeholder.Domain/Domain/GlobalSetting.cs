using System;
using System.Collections.Generic;
using System.Text;


namespace Placeholder.Domain
{
    public partial class GlobalSetting : DomainModel
    {	
        public GlobalSetting()
        {
				
        }
    
        public Guid global_setting_id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string value_encrypted { get; set; }
        public bool encrypted { get; set; }
        
	}
}

