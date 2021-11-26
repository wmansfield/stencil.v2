using System.Collections.Generic;

namespace Stencil.Native.Screens
{
    public interface ISectionConfig
    {
        string configuration_json { get; set; }

        List<IViewConfig> ViewConfigs { get; }
    }
}
