using Newtonsoft.Json;
using Stencil.Common.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Screens
{
    public static class _Extensions
    {
        public static ScreenConfig ToScreenConfig(this ScreenConfigExchange source)
        {
            ScreenConfig result = null;
            if (source != null)
            {
                result = new ScreenConfig()
                {
                    ScreenStorageKey = source.ScreenStorageKey,
                    ScreenName = source.ScreenName,
                    ScreenParameter = source.ScreenParameter,
                    SuppressPersist = source.SuppressPersist,
                    IsMenuSupported = source.IsMenuSupported,
                    AutomaticDownload = source.AutomaticDownload,
                    CacheUntilUTC = source.CacheUntilUTC,
                    DownloadedUTC = source.DownloadedUTC,
                    ExpireUTC = source.ExpireUTC,
                    InvalidatedUTC = source.InvalidatedUTC,
                    Lifetime = source.Lifetime,
                    ScreenNavigationData = (source as IScreenConfig).ScreenNavigationData,
                    VisualConfig = (source as IScreenConfig).VisualConfig,
                    MenuConfigs = (source as IScreenConfig).MenuConfigs,
                    ViewConfigs = (source as IScreenConfig).ViewConfigs,
                    HeaderConfigs = (source as IScreenConfig).HeaderConfigs,
                    FooterConfigs = (source as IScreenConfig).FooterConfigs,
                    BeforeShowCommands = (source as IScreenConfig).BeforeShowCommands,
                    AfterShowCommands = (source as IScreenConfig).AfterShowCommands,
                    DownloadCommands = (source as IScreenConfig).DownloadCommands,
                    Claims = (source as IScreenConfig).Claims
                };
            }
            return result;
        }
    }
}
