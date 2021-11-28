using Newtonsoft.Json;
using Stencil.Native.Screens;
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
        public static ScreenConfig ToUIModel(this db.ScreenConfig source)
        {
            if(source == null)
            {
                return null;
            }
            ScreenConfig result = new ScreenConfig()
            {
                id = source.id,
                SuppressPersist = source.suppress_persist,
                IsMenuSupported = source.is_menu_supported,
                BackgroundColor = string.IsNullOrWhiteSpace(source.background_color_hex) ? Color.Transparent : Color.FromHex(source.background_color_hex),
                ViewConfigs = new List<IViewConfig>()
            };
            if (!string.IsNullOrWhiteSpace(source.margin))
            {
                ThicknessInfo margin = JsonConvert.DeserializeObject<ThicknessInfo>(source.margin);
                result.Margin = margin.ToThickness();
            }
            if (!string.IsNullOrWhiteSpace(source.json))
            {
                List<ViewConfig> viewConfigs = JsonConvert.DeserializeObject<List<ViewConfig>>(source.json);
                foreach (ViewConfig item in viewConfigs)
                {
                    result.ViewConfigs.Add(item);
                }
            }
            return result;
        }

        public static db.ScreenConfig ToDbModel(this ScreenConfig source, db.ScreenConfig destination = null)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new db.ScreenConfig(); }

            destination.id = source.id.ToString();
            destination.suppress_persist = source.SuppressPersist;
            destination.is_menu_supported = source.IsMenuSupported;
            destination.background_color_hex = source.BackgroundColor.ToHex();
            if (source.Margin != 0)
            {
                destination.margin = JsonConvert.SerializeObject(source.Margin.ToThicknessInfo());
            }
            if (source.ViewConfigs != null && source.ViewConfigs.Count > 0)
            {
                destination.json = JsonConvert.SerializeObject(source.ViewConfigs);
            }

            return destination;
        }
    }
}
