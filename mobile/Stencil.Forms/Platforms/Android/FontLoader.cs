using Android.Content.Res;
using Android.Graphics;
using System;
using System.Collections.Generic;

namespace Stencil.Forms.Droid
{
    public static class FontLoader
    {
        private static Dictionary<string, Typeface> _FontCache = new Dictionary<string, Typeface>(StringComparer.OrdinalIgnoreCase);
        private static object _FontSyncRoot = new object();

        public static Typeface GetFont(AssetManager assets, string assetPath)
        {
            try
            {
                Typeface result = null;

                if (!string.IsNullOrEmpty(assetPath))
                {
                    if (!_FontCache.TryGetValue(assetPath, out result))
                    {
                        lock (_FontSyncRoot)
                        {
                            if (!_FontCache.TryGetValue(assetPath, out result))
                            {
                                result = Typeface.CreateFromAsset(assets, assetPath);
                                _FontCache[assetPath] = result;
                            }
                        }
                    }
                }
                if(result == null)
                {
                    result = Typeface.Default;
                }
                return result;
            }
            catch
            {
                return Typeface.Default;
            }
            
        }
    }
}