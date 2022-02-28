using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Forms
{
    public static class StencilPreferences
    {
        public static string FONT_REGULAR = string.Empty;
        public static string FONT_ITALIC = string.Empty;
        public static string FONT_BOLD = string.Empty;
        public static string FONT_BOLD_ITALIC = string.Empty;

        public static string COLOR_Markdown_Text_Foreground = "#000000";

        public static string COLOR_Markdown_Code_Foreground = "#008000";
        public static string COLOR_Markdown_Code_Background = "#f1f1f1";
        public static string COLOR_Markdown_Highlight_Background = "#ffecbe";
        public static string COLOR_Markdown_Highlight_Foreground = COLOR_Markdown_Text_Foreground;

        public static string COLOR_Markdown_Image_Background = "#000000";

        public static string COLOR_Markdown_Link_Foreground = "#007AFF";


        public static void InitFonts(string assetPathFontRegular, string assetPathFontItalic, string assetPathFontBold, string assetPathFontBoldItalic)
        {
            StencilPreferences.FONT_REGULAR = assetPathFontRegular;
            StencilPreferences.FONT_ITALIC = assetPathFontItalic;
            StencilPreferences.FONT_BOLD = assetPathFontBold;
            StencilPreferences.FONT_BOLD_ITALIC = assetPathFontBoldItalic;
        }
    }
}