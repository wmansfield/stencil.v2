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
        DateTimeOffset? DownloadedUTC { get; }
        DateTimeOffset? CacheUntilUTC { get; }
        DateTimeOffset? ExpireUTC { get; }
        DateTimeOffset? InvalidatedUTC { get; }
        bool AutomaticDownload { get; }
        bool IsMenuSupported { get; }
        IVisualConfig VisualConfig { get; }
        List<IViewConfig> HeaderConfigs { get; }
        List<IViewConfig> FooterConfigs { get; }
        List<IViewConfig> ViewConfigs { get; }
        List<IMenuConfig> MenuConfigs { get;  }
        List<ICommandConfig> ShowCommands { get; }
        List<ICommandConfig> DownloadCommands { get; }

    }
}