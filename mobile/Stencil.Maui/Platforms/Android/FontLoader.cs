using Android.Content.Res;
using Android.Graphics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Platform;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Stencil.Maui.Droid
{
    public static class FontLoader
    {
        private static ConcurrentDictionary<string, Typeface> _FontCache = new ConcurrentDictionary<string, Typeface>(StringComparer.OrdinalIgnoreCase);

        public static Typeface GetFont(AssetManager assets, string fontName)
        {
            try
            {
                Typeface result = null;
                if(_FontCache.TryGetValue(fontName, out result) && result != null)
                {
                    return result;
                };

                IFontManager fontManager = StencilAPI.Instance.MauiServiceProvider.GetService<IFontManager>();
                result = fontManager.GetTypeface(Font.OfSize(fontName, 14));
                if (result == null)
                {
                    result = Typeface.Default;
                }
                
                _FontCache.TryAdd(fontName, result);

                return result;
            }
            catch
            {
                _FontCache.TryAdd(fontName, Typeface.Default);
                return Typeface.Default;
            }
            
        }
    }
}