﻿using Newtonsoft.Json;
using Stencil.Native.Screens;
using Stencil.Native.Views;
using System.Collections.Generic;
using Xamarin.Forms;
using db = Stencil.Native.Data.Models;


namespace Stencil.Native.Data
{
    public static class _DatabaseMapping
    {
        #region Generic Mapping

        public static TDestination ToDbModel<TSource, TDestination>(this TSource source, TDestination destination = null)
            where TDestination : class, IDatabaseModel, new()
            where TSource : class, IUIModel
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new TDestination(); }

            destination.id = source.id.ToString();
            destination.json = JsonConvert.SerializeObject(source);

            return destination;
        }

        public static TDestination ToUIModel<TSource, TDestination>(this TSource source)
            where TSource : class, IDatabaseModel
            where TDestination : class, IUIModel, new()
        {
            if (source == null) { return null; }

            return JsonConvert.DeserializeObject<TDestination>(source.json);
        }

        public static List<TDestination> ToUIModel<TSource, TDestination>(this IEnumerable<TSource> items)
            where TSource : class, IDatabaseModel
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
            if(source != null)
            {
                return new Thickness(source.left, source.top, source.right, source.bottom);
            }
            return new Thickness();
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
                IsMenuSupported = source.is_menu_supported,
                DownloadedUTC = source.download_utc,
                CacheUntilUTC = source.cache_until_utc,
                ExpireUTC = source.expire_utc,
                InvalidatedUTC = source.invalidated_utc,
                AutomaticDownload = source.automatic_download,
                ViewConfigs = new List<IViewConfig>(),
                MenuConfigs = new List<Views.IMenuConfig>(),
                ShowCommands = new List<ICommandConfig>()
            };
            

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
            if (!string.IsNullOrWhiteSpace(source.json_menu))
            {
                List<MenuConfig> menuConfigs = JsonConvert.DeserializeObject<List<MenuConfig>>(source.json_menu);
                foreach (MenuConfig item in menuConfigs)
                {
                    result.MenuConfigs.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(source.json_show_commands))
            {
                List<CommandConfig> showCommands = JsonConvert.DeserializeObject<List<CommandConfig>>(source.json_show_commands);
                foreach (CommandConfig item in showCommands)
                {
                    result.ShowCommands.Add(item);
                }
            }
            return result;
        }

        public static db.ScreenConfig ToDbModel(this ScreenConfig source, db.ScreenConfig destination = null)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new db.ScreenConfig(); }

            destination.id = source.ScreenStorageKey;
            destination.suppress_persist = source.SuppressPersist;
            destination.is_menu_supported = source.IsMenuSupported;

            destination.screen_name = source.ScreenName;
            destination.screen_parameter = source.ScreenParameter;

            destination.download_utc = source.DownloadedUTC;
            destination.cache_until_utc = source.CacheUntilUTC;
            destination.expire_utc = source.ExpireUTC;
            destination.invalidated_utc = source.InvalidatedUTC;
            destination.automatic_download = source.AutomaticDownload;

            if (source.VisualConfig != null)
            {
                destination.json_visual_config = JsonConvert.SerializeObject(source.VisualConfig);
            }
            if (source.ViewConfigs != null && source.ViewConfigs.Count > 0)
            {
                destination.json = JsonConvert.SerializeObject(source.ViewConfigs);
            }
            if (source.ShowCommands != null && source.ShowCommands.Count > 0)
            {
                destination.json_show_commands = JsonConvert.SerializeObject(source.ShowCommands);
            }
            if (source.MenuConfigs != null && source.MenuConfigs.Count > 0)
            {
                destination.json_menu = JsonConvert.SerializeObject(source.MenuConfigs);
            }
            return destination;
        }
    }
}
