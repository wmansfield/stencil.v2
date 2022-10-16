using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Globalization;

namespace Stencil.Maui.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = value?.ToString();
            if(color != null && color.Length > 0)
            {
                if(color.Length >= 6)
                {
                    return Color.FromArgb(color);
                }
                else if (color.Length >= 3)
                {
                    return Color.FromArgb("#FF" + color.Trim('#') + color.Trim('#')); // upscale
                }
                else
                {
                    return Color.FromArgb(color);// not sure.
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
