using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Common.Screens
{
    public class SectionConfigExchange : ISectionConfig
    {
        public string configuration_json { get; set; }

        public List<ViewConfigExchange> ViewConfigs { get; set; }

        List<IViewConfig> ISectionConfig.ViewConfigs
        {
            get
            {
                return this.ViewConfigs.AsEnumerable<IViewConfig>().ToList();
            }
        }
    }
}
