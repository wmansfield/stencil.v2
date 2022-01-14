using Stencil.Common.Screens;
using System.Collections.Generic;

namespace Stencil.Forms.Screens
{
    public class SectionConfig : ISectionConfig
    {
        public SectionConfig()
        {
            this.ViewConfigs = new List<IViewConfig>();
        }
        public List<IViewConfig> ViewConfigs { get; set; }
        public string configuration_json { get; set; }

    }
}
