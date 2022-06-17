using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Common.Views;
using Stencil.Forms.Platforms.Common.Data.Sync;
using Stencil.Forms.Screens;
using Stencil.Forms.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using db = Stencil.Forms.Data.Models;


namespace Stencil.Forms.Data
{
    public static class _DatabaseMapping
    {
        #region Generic Mapping

        public static TDestination ToDbModel<TSource, TDestination>(this TSource source, TDestination destination = null)
            where TDestination : class, IPersistedModel, new()
            where TSource : class, IUIModel
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new TDestination(); }

            destination.id = source.id.ToString();
            destination.json = JsonConvert.SerializeObject(source);

            return destination;
        }

        public static TDestination ToUIModel<TSource, TDestination>(this TSource source)
            where TSource : class, IPersistedModel
            where TDestination : class, IUIModel, new()
        {
            if (source == null) { return null; }

            return JsonConvert.DeserializeObject<TDestination>(source.json);
        }

        public static List<TDestination> ToUIModel<TSource, TDestination>(this IEnumerable<TSource> items)
            where TSource : class, IPersistedModel
            where TDestination : class, IUIModel, new()
        {
            List<TDestination> result = new List<TDestination>();
            if (items != null)
            {
                foreach (TSource item in items)
                {
                    result.Add(item.ToUIModel<TSource, TDestination>());
                }
            }
            return result;
        }

        #endregion


        public static Brush ToBrush(this GradientBrushInfo source)
        {
            LinearGradientBrush result = null;
            if(source != null)
            {
                result = new LinearGradientBrush();
                result.StartPoint = source.Start.ToPoint();
                result.EndPoint = source.End.ToPoint();
                if(source.Stops.Length > 0)
                {
                    foreach (GradientStopInfo item in source.Stops)
                    {
                        result.GradientStops.Add(new GradientStop()
                        {
                            Offset = item.Offset,
                            Color = Color.FromHex(item.Color)
                        });
                    }
                }
            }
            return result;
        }

        public static ThicknessInfo ToThicknessInfo(this Thickness source)
        {
            return new ThicknessInfo()
            {
                top = source.Top,
                bottom = source.Bottom,
                left = source.Left,
                right = source.Right
            };
        }
        public static Thickness ToThickness(this ThicknessInfo source)
        {
            return new Thickness(source.left, source.top, source.right, source.bottom);
        }


        public static Point ToPoint(this PointInfo source)
        {
            return new Point(source.x, source.y);
        }

        public static List<ScreenConfig> ToUIModel(this IEnumerable<db.ScreenConfig> items)
        {
            List<ScreenConfig> result = new List<ScreenConfig>();
            if (items != null)
            {
                foreach (db.ScreenConfig item in items)
                {
                    result.Add(item.ToUIModel());
                }
            }
            return result;
        }

        public static ScreenConfig ToUIModel(this db.ScreenConfig source)
        {
            if(source == null)
            {
                return null;
            }
            ScreenConfig result = new ScreenConfig()
            {
                ScreenStorageKey = source.id,
                ScreenName = source.screen_name,
                ScreenParameter = source.screen_parameter,
                SuppressPersist = source.suppress_persist,
                AutomaticDownload = source.automatic_download,
                IsMenuSupported = source.is_menu_supported,
                Lifetime = (Lifetime)source.lifetime,
                DownloadedUTC = source.download_utc,
                CacheUntilUTC = source.cache_until_utc,
                ExpireUTC = source.expire_utc,
                InvalidatedUTC = source.invalidated_utc,
                ViewConfigs = new List<IViewConfig>(),
                HeaderConfigs = new List<IViewConfig>(),
                FooterConfigs = new List<IViewConfig>(),
                MenuConfigs = new List<IMenuConfig>(),
                BeforeShowCommands = new List<ICommandConfig>(),
                AfterShowCommands = new List<ICommandConfig>()
            };

            if (!string.IsNullOrWhiteSpace(source.screen_navigation_data))
            {
                result.ScreenNavigationData = JsonConvert.DeserializeObject<NavigationData>(source.screen_navigation_data);
            }
            if (!string.IsNullOrWhiteSpace(source.json_visual_config))
            {
                result.VisualConfig = JsonConvert.DeserializeObject<VisualConfig>(source.json_visual_config);
            }
            if (!string.IsNullOrWhiteSpace(source.json))
            {
                List<ViewConfig> viewConfigs = JsonConvert.DeserializeObject<List<ViewConfig>>(source.json);
                foreach (ViewConfig item in viewConfigs)
                {
                    result.ViewConfigs.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(source.json_header_configs))
            {
                List<ViewConfig> headerConfigs = JsonConvert.DeserializeObject<List<ViewConfig>>(source.json_header_configs);
                foreach (ViewConfig item in headerConfigs)
                {
                    result.HeaderConfigs.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(source.json_footer_configs))
            {
                List<ViewConfig> footerConfigs = JsonConvert.DeserializeObject<List<ViewConfig>>(source.json_footer_configs);
                foreach (ViewConfig item in footerConfigs)
                {
                    result.FooterConfigs.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(source.json_before_show_commands))
            {
                List<CommandConfig> showCommands = JsonConvert.DeserializeObject<List<CommandConfig>>(source.json_before_show_commands);
                foreach (CommandConfig item in showCommands)
                {
                    result.BeforeShowCommands.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(source.json_after_show_commands))
            {
                List<CommandConfig> showCommands = JsonConvert.DeserializeObject<List<CommandConfig>>(source.json_after_show_commands);
                foreach (CommandConfig item in showCommands)
                {
                    result.AfterShowCommands.Add(item);
                }
            }
            // download commands ignored, ephemeral

            if (!string.IsNullOrWhiteSpace(source.json_menu))
            {
                List<MenuConfig> menuConfigs = JsonConvert.DeserializeObject<List<MenuConfig>>(source.json_menu);
                foreach (MenuConfig item in menuConfigs)
                {
                    result.MenuConfigs.Add(item);
                }
            }
            return result;
        }

        public static db.ScreenConfig ToDbModel(this ScreenConfig source, db.ScreenConfig destination = null)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new db.ScreenConfig(); }

            destination.id = source.ScreenStorageKey;
            
            destination.screen_name = source.ScreenName;
            destination.screen_parameter = source.ScreenParameter;
            destination.suppress_persist = source.SuppressPersist;
            destination.automatic_download = source.AutomaticDownload;
            destination.is_menu_supported = source.IsMenuSupported;

            destination.lifetime = (int)source.Lifetime;

            destination.download_utc = source.DownloadedUTC;
            destination.cache_until_utc = source.CacheUntilUTC;
            destination.expire_utc = source.ExpireUTC;
            destination.invalidated_utc = source.InvalidatedUTC;


            if (source.ScreenNavigationData != null)
            {
                destination.screen_navigation_data = JsonConvert.SerializeObject(source.ScreenNavigationData);
            }
            if (source.VisualConfig != null)
            {
                destination.json_visual_config = JsonConvert.SerializeObject(source.VisualConfig);
            }
            if (source.ViewConfigs != null && source.ViewConfigs.Count > 0)
            {
                destination.json = JsonConvert.SerializeObject(source.ViewConfigs);
            }
            if (source.HeaderConfigs != null && source.HeaderConfigs.Count > 0)
            {
                destination.json_header_configs = JsonConvert.SerializeObject(source.HeaderConfigs);
            }
            if (source.FooterConfigs != null && source.FooterConfigs.Count > 0)
            {
                destination.json_footer_configs = JsonConvert.SerializeObject(source.FooterConfigs);
            }
            if (source.AfterShowCommands != null && source.AfterShowCommands.Count > 0)
            {
                destination.json_after_show_commands = JsonConvert.SerializeObject(source.AfterShowCommands);
            }
            if (source.BeforeShowCommands != null && source.BeforeShowCommands.Count > 0)
            {
                destination.json_before_show_commands = JsonConvert.SerializeObject(source.BeforeShowCommands);
            }
            // download commands ignored, ephemeral

            if (source.MenuConfigs != null && source.MenuConfigs.Count > 0)
            {
                destination.json_menu = JsonConvert.SerializeObject(source.MenuConfigs);
            }
            return destination;
        }

        public static TrackedDownloadInfo ToUIModel(this db.TrackedDownloadInfo source)
        {
            if (source == null || string.IsNullOrWhiteSpace(source.json) ) { return null; }

            return JsonConvert.DeserializeObject<TrackedDownloadInfo>(source.json);
        }

        public static db.TrackedDownloadInfo ToDbModel(this TrackedDownloadInfo source, db.TrackedDownloadInfo destination = null)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new db.TrackedDownloadInfo(); }

            destination.id = TrackedDownloadInfo.FormatStorageKey(source.EntityName, source.EntityIdentifier);
            destination.json = JsonConvert.SerializeObject(source);

            return destination;
        }
    }
}
