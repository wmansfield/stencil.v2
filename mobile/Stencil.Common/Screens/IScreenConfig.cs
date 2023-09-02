using Stencil.Common.Views;
using System;
using System.Collections.Generic;

namespace Stencil.Common.Screens
{
    // Note: Persistence mapping is manual for this class, when adding fields, be sure to update
    public interface IScreenConfig
    {
        string ScreenName { get; }
        string ScreenParameter { get; }
        INavigationData ScreenNavigationData { get; }
        Lifetime Lifetime { get; }
        bool PreventExpired { get; }
        DateTimeOffset? DownloadedUTC { get; }
        DateTimeOffset? CacheUntilUTC { get; }
        DateTimeOffset? ExpireUTC { get; }
        DateTimeOffset? InvalidatedUTC { get; }
        bool AutomaticDownload { get; }
        bool IsMenuSupported { get; }
        bool DisableCellReuse { get; set;  }
        bool DisableCellSizeCaching { get; set; }
        IVisualConfig VisualConfig { get; }
        List<IViewConfig> HeaderConfigs { get; }
        List<IViewConfig> FooterConfigs { get; }
        List<IViewConfig> ViewConfigs { get; }
        List<IMenuConfig> MenuConfigs { get;  }
        List<ICommandConfig> BeforeShowCommands { get; }
        List<ICommandConfig> AfterShowCommands { get; }
        List<ICommandConfig> DownloadCommands { get; }
        List<string> Claims { get; }

    }
}