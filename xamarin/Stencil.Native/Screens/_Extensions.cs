﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
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
                    ShowCommands = (source as IScreenConfig).ShowCommands,
                    DownloadCommands = (source as IScreenConfig).DownloadCommands
                };
            }
            return result;
        }
    }
}
