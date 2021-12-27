using Stencil.Native.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stencil.Native.Screens
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
            this.ShowCommands = new List<CommandConfig>();
            this.MenuConfigs = new List<MenuConfig>();
        }
        public string ScreenStorageKey { get; set; }
        public string ScreenName { get; set; }
        public string ScreenParameter { get; set; }
        public bool SuppressPersist { get; set; }
        public bool AutomaticDownload { get; set; }
        public bool IsMenuSupported { get; set; }

        public DateTimeOffset? DownloadedUTC { get; set; }
        public DateTimeOffset? CacheUntilUTC { get; set; }
        public DateTimeOffset? ExpireUTC { get; set; }
        public DateTimeOffset? InvalidatedUTC { get; set; }


        public VisualConfig VisualConfig { get; set; }

        public List<ViewConfig> ViewConfigs { get; set; }
        public List<CommandConfig> ShowCommands { get; set; }
        public List<MenuConfig> MenuConfigs { get; set; }

        List<IViewConfig> IScreenConfig.ViewConfigs
        {
            get
            {
                return this.ViewConfigs.AsEnumerable<IViewConfig>().ToList();
            }
        }

        List<IMenuConfig> IScreenConfig.MenuConfigs
        {
            get
            {
                return this.MenuConfigs.AsEnumerable<IMenuConfig>().ToList();
            }
        }

        List<ICommandConfig> IScreenConfig.ShowCommands
        {
            get
            {
                return this.ShowCommands.AsEnumerable<ICommandConfig>().ToList();
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
