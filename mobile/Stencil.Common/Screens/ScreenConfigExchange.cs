using Newtonsoft.Json;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stencil.Common.Screens
{
    // Note: Persistence mapping is manual for this class, when adding fields, be sure to update

    /// <summary>
    /// Only used to support serialization of concrete objects to interfaces, not used internally beyond remote serializing
    /// </summary>
    public class ScreenConfigExchange : IScreenConfig
    {
        public static string FormatStorageKey(string screenName, string screenParameter)
        {
            return string.Format("{0}.{1}", screenName, screenParameter).Trim().Trim('.');
        }

        public ScreenConfigExchange()
        {
            this.ViewConfigs = new List<ViewConfig>();
            this.HeaderConfigs = new List<ViewConfig>();
            this.FooterConfigs = new List<ViewConfig>();
            this.BeforeShowCommands = new List<CommandConfig>();
            this.AfterShowCommands = new List<CommandConfig>();
            this.MenuConfigs = new List<MenuConfig>();
            this.DownloadCommands = new List<CommandConfig>();
            this.Claims = new List<string>();
        }
        public string ScreenStorageKey { get; set; }
        public string ScreenName { get; set; }
        public string ScreenParameter { get; set; }
        public NavigationData ScreenNavigationData { get; set; }

        public bool SuppressPersist { get; set; }
        public bool AutomaticDownload { get; set; }
        public bool IsMenuSupported { get; set; }
        public bool DisableCellReuse { get; set; }

        public Lifetime Lifetime { get; set; }
        public DateTimeOffset? DownloadedUTC { get; set; }
        public DateTimeOffset? CacheUntilUTC { get; set; }
        public DateTimeOffset? ExpireUTC { get; set; }
        public DateTimeOffset? InvalidatedUTC { get; set; }


        public VisualConfig VisualConfig { get; set; }

        public List<ViewConfig> ViewConfigs { get; set; }
        public List<ViewConfig> HeaderConfigs { get; set; }
        public List<ViewConfig> FooterConfigs { get; set; }
        public List<CommandConfig> BeforeShowCommands { get; set; }
        public List<CommandConfig> AfterShowCommands { get; set; }
        public List<CommandConfig> DownloadCommands { get; set; }
        public List<MenuConfig> MenuConfigs { get; set; }
        public List<string> Claims { get; set; }

        INavigationData IScreenConfig.ScreenNavigationData
        {
            get
            {
                return this.ScreenNavigationData;
            }
        }

        List<IViewConfig> IScreenConfig.ViewConfigs
        {
            get
            {
                return this.ViewConfigs.AsEnumerable<IViewConfig>().ToList();
            }
        }
        List<IViewConfig> IScreenConfig.HeaderConfigs
        {
            get
            {
                return this.HeaderConfigs.AsEnumerable<IViewConfig>().ToList();
            }
        }
        List<IViewConfig> IScreenConfig.FooterConfigs
        {
            get
            {
                return this.FooterConfigs.AsEnumerable<IViewConfig>().ToList();
            }
        }

        List<IMenuConfig> IScreenConfig.MenuConfigs
        {
            get
            {
                return this.MenuConfigs.AsEnumerable<IMenuConfig>().ToList();
            }
        }

        List<ICommandConfig> IScreenConfig.AfterShowCommands
        {
            get
            {
                return this.AfterShowCommands.AsEnumerable<ICommandConfig>().ToList();
            }
        }
        List<ICommandConfig> IScreenConfig.BeforeShowCommands
        {
            get
            {
                return this.BeforeShowCommands.AsEnumerable<ICommandConfig>().ToList();
            }
        }
        List<ICommandConfig> IScreenConfig.DownloadCommands
        {
            get
            {
                return this.DownloadCommands.AsEnumerable<ICommandConfig>().ToList();
            }
        }

        IVisualConfig IScreenConfig.VisualConfig
        {
            get
            {
                return this.VisualConfig as IVisualConfig;
            }
        }
    }
}
