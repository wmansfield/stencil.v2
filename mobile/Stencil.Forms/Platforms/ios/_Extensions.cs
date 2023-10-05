using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Stencil.Forms.iOS
{
    public static class _Extensions
    {
        public static string TrimSafe(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            return text.Trim();
        }

        #region Coloring

        private static Dictionary<string, UIColor> _colorCache = new Dictionary<string, UIColor>(StringComparer.OrdinalIgnoreCase);

        public static UIColor ConvertHexToColor(this string hexColor)
        {
            if (_colorCache.ContainsKey(hexColor))
            {
                return _colorCache[hexColor];
            }
            if (!string.IsNullOrEmpty(hexColor))
            {
                string hexaColor = hexColor.Replace("#", "");
                if (hexaColor.Length == 3)
                {
                    hexaColor = hexaColor + hexaColor;
                }
                if (hexaColor.Length == 6)
                {
                    hexaColor = "FF" + hexaColor;
                }
                hexaColor = hexaColor.ToUpper();
                if (hexaColor.Length == 8)
                {
                    if (_colorCache.ContainsKey(hexaColor))
                    {
                        return _colorCache[hexaColor];
                    }
                    UIColor result = UIColor.FromRGBA(
                        Convert.ToByte(hexaColor.Substring(2, 2), 16),
                        Convert.ToByte(hexaColor.Substring(4, 2), 16),
                        Convert.ToByte(hexaColor.Substring(6, 2), 16),
                        Convert.ToByte(hexaColor.Substring(0, 2), 16)
                    );

                    _colorCache[hexaColor] = result;
                    _colorCache[hexColor] = result;
                    return result;
                }
            }
            return UIColor.White;
        }

        public static UIColor ConvertHexToColor(this string hexColor, double alphaOf1)
        {
            if (_colorCache.ContainsKey(hexColor + alphaOf1.ToString()))
            {
                return _colorCache[hexColor + alphaOf1.ToString()];
            }
            if (!string.IsNullOrEmpty(hexColor))
            {
                string hexaColor = hexColor.Replace("#", "");
                if (hexaColor.Length == 3)
                {
                    hexaColor = hexaColor + hexaColor;
                }
                if (hexaColor.Length == 6)
                {
                    hexaColor = "FF" + hexaColor;
                }
                hexaColor = hexaColor.ToUpper();
                if (hexaColor.Length == 8)
                {
                    UIColor result = UIColor.FromRGBA(
                        Convert.ToByte(hexaColor.Substring(2, 2), 16),
                        Convert.ToByte(hexaColor.Substring(4, 2), 16),
                        Convert.ToByte(hexaColor.Substring(6, 2), 16),
                        Convert.ToByte((int)(255 * alphaOf1))
                    );
                    _colorCache[hexColor + alphaOf1.ToString()] = result;
                    return result;
                }
            }
            return UIColor.White;
        }

        #endregion


        public static int ClampToLineHeights(CGRect boudingRect, float fontSize)
        {
            int singleLineHeight = (int)Math.Ceiling(fontSize * 1.60d); //TODO: Magic number 1.55d (fontsize + ios font treatment, 16pt -> 25px)
            int rows = (int)(boudingRect.Size.Height / singleLineHeight);
            nfloat remainder = boudingRect.Size.Height % singleLineHeight;
            if (remainder / singleLineHeight > .1) // if more than 10% over, make a new line
            {
                rows++;
            }
            if (rows > 1)
            {
                return (rows * singleLineHeight) + rows - 1; // 1 extra for every row spacer to account for partial pixel renderings
            }
            else
            {
                return singleLineHeight;
            }
        }


        private static readonly Lazy<CGRect> _mainScreenBounds = new Lazy<CGRect>(() => UIScreen.MainScreen.Bounds);
        public static CGRect MainScreenBounds { get { return _mainScreenBounds.Value; } }
    }
}