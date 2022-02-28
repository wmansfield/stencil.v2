using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Forms.Droid
{
    public static class _Extensions
    {
        public static void SetCustomFont(this TextView textView, string assetPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(assetPath))
                {
                    Typeface face = FontLoader.GetFont(textView.Context.Assets, assetPath);
                    if (face != null)
                    {
                        TypefaceStyle style = TypefaceStyle.Normal;
                        textView.SetTypeface(face, style);
                    }
                }
            }
            catch (Exception ex)
            {
                CoreUtility.Logger.LogError("SetCustomFont", ex);
            }
        }

        public static void EnsureEmojis(this SpannableString spannableString, Context context, int textSize)
        {
            //TODO: Emoji Support
            //EmojiconHandler.addEmojis(context, spannableString, textSize, (int)SpanAlign.Baseline, textSize, 0, -1);
        }
        public static float ToSp(this float value, Context context)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Sp, value, context.Resources.DisplayMetrics);
        }
        public static float ToDip(this float value, Context context)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, value, context.Resources.DisplayMetrics);
        }
        public static int ToDip(this int value, Context context)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, value, context.Resources.DisplayMetrics);
        }
        public static string TrimSafe(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            return text.Trim();
        }


        private static float? _screenWidth;
        public static float GetScreenWidth(this Context context)
        {
            if (!_screenWidth.HasValue)
            {
                DisplayMetrics displayMetrics = context.Resources.DisplayMetrics;
                _screenWidth = displayMetrics.WidthPixels / displayMetrics.Density;
            }
            return _screenWidth.GetValueOrDefault();
        }

        private static float? _screenHeight;

        public static float GetScreenHeight(this Context context)
        {
            if (!_screenHeight.HasValue)
            {
                DisplayMetrics displayMetrics = context.Resources.DisplayMetrics;
                _screenHeight = displayMetrics.HeightPixels / displayMetrics.Density;
            }
            return _screenHeight.GetValueOrDefault();
        }
        private static Dictionary<string, Color> _colorCache = new Dictionary<string, Color>();

        public static Color ConvertHexToColor(this string hexaColor)
        {
            if (string.IsNullOrEmpty(hexaColor))
            {
                return Color.Transparent;
            }
            hexaColor = hexaColor.Replace("#", "");
            hexaColor = hexaColor.ToUpper();
            if (_colorCache.ContainsKey(hexaColor))
            {
                return _colorCache[hexaColor];
            }
            Color result = Color.ParseColor("#" + hexaColor);
            _colorCache[hexaColor] = result;

            return result;
        }

        private static Dictionary<string, ColorDrawable> _drawableCache = new Dictionary<string, ColorDrawable>();
        public static ColorDrawable ConvertHexToDrawable(this string hexaColor)
        {
            if (_drawableCache.ContainsKey(hexaColor))
            {
                return _drawableCache[hexaColor];
            }
            ColorDrawable result = new ColorDrawable(hexaColor.ConvertHexToColor());
            _drawableCache[hexaColor] = result;
            return result;
        }

        

    }
}