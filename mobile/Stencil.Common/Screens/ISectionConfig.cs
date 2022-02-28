using System.Collections.Generic;

namespace Stencil.Common.Screens
{
    public interface ISectionConfig
    {
        string configuration_json { get; set; }

        List<IViewConfig> ViewConfigs { get; }
    }
}
