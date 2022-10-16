using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Forms
{
    public static class StencilPreferences
    {
        public static string FONT_REGULAR = "LibreFranklin-Regular";
        public static string FONT_ITALIC = "LibreFranklin-Italic";
        public static string FONT_BOLD = "LibreFranklin-Bold";
        public static string FONT_BOLD_ITALIC = "LibreFranklin-BoldItalic";

        public static string COLOR_Markdown_Text_Foreground = "#000000";

        public static string COLOR_Markdown_Code_Foreground = "#008000";
        public static string COLOR_Markdown_Code_Background = "#f1f1f1";
        public static string COLOR_Markdown_Highlight_Background = "#ffecbe";
        public static string COLOR_Markdown_Highlight_Foreground = COLOR_Markdown_Text_Foreground;

        public static string COLOR_Markdown_Image_Background = "#000000";

        public static string COLOR_Markdown_Link_Foreground = "#007AFF";


        public static void InitFonts(string fontNameRegular, string fontNameItalic, string fontNameBold, string fontNameBoldItalic)
        {
            StencilPreferences.FONT_REGULAR = fontNameRegular;
            StencilPreferences.FONT_ITALIC = fontNameItalic;
            StencilPreferences.FONT_BOLD = fontNameBold;
            StencilPreferences.FONT_BOLD_ITALIC = fontNameBoldItalic;
        }
    }
}