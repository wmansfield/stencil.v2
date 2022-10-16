using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Maui
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
        public static void InitStencilResources(ResourceDictionary resources)
        {
            // standard stencil
            resources["PageBackground"] = Color.FromArgb(AppColors.Primary900);

            resources["fa_th_large"] = FontAwesome.fa_th_large;
            resources["fa_balance_scale"] = FontAwesome.fa_balance_scale;
            resources["fa_tasks"] = FontAwesome.fa_tasks;
            resources["fa_comments"] = FontAwesome.fa_comments;
            resources["fa_chart"] = FontAwesome.fa_chart;
            resources["fa_info_circle"] = FontAwesome.fa_info_circle;
            resources["fa_spinner"] = FontAwesome.fa_spinner;
            resources["fa_exclamation_triangle"] = FontAwesome.fa_exclamation_triangle;
            resources["fa_angle_left"] = FontAwesome.fa_angle_left;
            resources["fa_angle_right"] = FontAwesome.fa_angle_right;
            resources["fa_camera"] = FontAwesome.fa_camera;
            resources["fa_plus"] = FontAwesome.fa_plus;
            resources["fa_exchange"] = FontAwesome.fa_exchange;
            resources["fa_times"] = FontAwesome.fa_times;
            resources["fa_eye_open"] = FontAwesome.fa_eye_open;
            resources["fa_eye_slashed"] = FontAwesome.fa_eye_slashed;
            resources["fa_ellipsis_v"] = FontAwesome.fa_ellipsis_v;
            resources["fa_bars"] = FontAwesome.fa_bars;


            resources["PrimaryWhite"] = Color.FromArgb(AppColors.PrimaryWhite);
            resources["PrimaryBlack"] = Color.FromArgb(AppColors.PrimaryBlack);
            resources["PrimaryBlackMuted"] = Color.FromArgb(AppColors.PrimaryBlackMuted);

            resources["Primary50"] = Color.FromArgb(AppColors.Primary50);
            resources["Primary100"] = Color.FromArgb(AppColors.Primary100);
            resources["Primary200"] = Color.FromArgb(AppColors.Primary200);
            resources["Primary300"] = Color.FromArgb(AppColors.Primary300);
            resources["Primary400"] = Color.FromArgb(AppColors.Primary400);
            resources["Primary500"] = Color.FromArgb(AppColors.Primary500);
            resources["Primary600"] = Color.FromArgb(AppColors.Primary600);
            resources["Primary700"] = Color.FromArgb(AppColors.Primary700);
            resources["Primary800"] = Color.FromArgb(AppColors.Primary800);
            resources["Primary900"] = Color.FromArgb(AppColors.Primary900);

            resources["Accent50"] = Color.FromArgb(AppColors.Accent50);
            resources["Accent100"] = Color.FromArgb(AppColors.Accent100);
            resources["Accent200"] = Color.FromArgb(AppColors.Accent200);
            resources["Accent300"] = Color.FromArgb(AppColors.Accent300);
            resources["Accent400"] = Color.FromArgb(AppColors.Accent400);
            resources["Accent500"] = Color.FromArgb(AppColors.Accent500);
            resources["Accent600"] = Color.FromArgb(AppColors.Accent600);
            resources["Accent700"] = Color.FromArgb(AppColors.Accent700);
            resources["Accent800"] = Color.FromArgb(AppColors.Accent800);
            resources["Accent900"] = Color.FromArgb(AppColors.Accent900);

            resources["TextOverAccent"] = Color.FromArgb(AppColors.TextOverAccent);
            resources["TextOverBackground"] = Color.FromArgb(AppColors.TextOverBackground);
            resources["TextOverBackgroundMuted"] = Color.FromArgb(AppColors.TextOverBackgroundMuted);
            resources["TextOverPrimary"] = Color.FromArgb(AppColors.TextOverPrimary);

            resources["MenuBarBackground"] = Color.FromArgb(AppColors.MenuBarBackground);

            resources["MenuSelectedBackground"] = Color.FromArgb(AppColors.MenuBarBackground);
            resources["MenuSelectedText"] = Color.FromArgb(AppColors.MenuBarBackground);

            resources["MenuUnselectedBackground"] = Color.FromArgb(AppColors.MenuBarBackground);
            resources["MenuUnselectedText"] = Color.FromArgb(AppColors.MenuBarBackground);
        }
    }
}