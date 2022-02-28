using System;
using System.Collections.Generic;

namespace Placeholder.Data.Sql.Models
{
    public partial class GlobalSetting
    {
        public Guid global_setting_id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string value_encrypted { get; set; }
        public bool encrypted { get; set; }
    }
}
