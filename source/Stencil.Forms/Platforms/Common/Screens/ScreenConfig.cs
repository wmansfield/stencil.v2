using Stencil.Common.Screens;
using Stencil.Common.Views;
using Stencil.Forms.Data.Sync;
using Stencil.Forms.Views;
using System;
using System.Collections.Generic;

namespace Stencil.Forms.Screens
{
    // Note: Persistence mapping is manual for this class, when adding fields, be sure to update
    public class ScreenConfig : IScreenConfig
    {
        public static string FormatStorageKey(string screenName, string screenParameter)
        {
            return string.Format("{0}.{1}", screenName, screenParameter).Trim().Trim('.');
        }

        public ScreenConfig()
        {
            this.ViewConfigs = new List<IViewConfig>();
            this.HeaderConfigs = new List<IViewConfig>();
            this.FooterConfigs = new List<IViewConfig>();
            this.ShowCommands = new List<ICommandConfig>();
            this.MenuConfigs = new List<IMenuConfig>();
            this.DownloadCommands = new List<ICommandConfig>();
        }
        public string ScreenStorageKey { get; set; }

        public string ScreenName { get; set; }
        public string ScreenParameter { get; set; }
        public bool SuppressPersist { get; set; }
        public bool AutomaticDownload { get; set; }
        public bool IsMenuSupported { get; set; }

        public Lifetime Lifetime { get; set; }

        public DateTimeOffset? DownloadedUTC { get; set; }
        public DateTimeOffset? CacheUntilUTC { get; set; }
        public DateTimeOffset? ExpireUTC { get; set; }
        public DateTimeOffset? InvalidatedUTC { get; set; }

        public INavigationData ScreenNavigationData { get; set; }

        public IVisualConfig VisualConfig { get; set; }

        public List<IViewConfig> ViewConfigs { get; set; }
        public List<IViewConfig> HeaderConfigs { get; set; }
        public List<IViewConfig> FooterConfigs { get; set; }

        public List<ICommandConfig> ShowCommands { get; set; }
        public List<ICommandConfig> DownloadCommands { get; set; }

        public List<IMenuConfig> MenuConfigs { get; set; }

    }
}
