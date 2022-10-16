using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Stencil.Maui;
using Stencil.Maui.Droid.Controls;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;

namespace Stencil.Maui.Droid.Markdown
{
    public static class _MarkdownExtensions
    {
        public static void SetRangeFont(this SpannableString spannableString, int ixStart, int ixEnd, Typeface typeFace = null, float? typeFaceSize = null, TypefaceStyle? style = null)
        {
            CoreUtility.ExecuteMethod("SetRangeFont", delegate ()
            {
                if (style.HasValue)
                {
                    spannableString.SetSpan(new StyleSpan(style.Value), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                }
                if (typeFace != null)
                {
                    spannableString.SetSpan(new CustomTypefaceSpan(typeFace, typeFaceSize.GetValueOrDefault()), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                }
            });
        }
        public static void SetRangeColor(this SpannableString spannableString, int ixStart, int ixEnd, Color? backgroundColor = null, Color? textColor = null)
        {
            CoreUtility.ExecuteMethod("SetRangeStyle", delegate ()
            {
                if (textColor.HasValue && textColor.Value != Color.Transparent)
                {
                    spannableString.SetSpan(new ForegroundColorSpan(textColor.Value), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                }
                if (backgroundColor.HasValue && backgroundColor.Value != Color.Transparent)
                {
                    spannableString.SetSpan(new BackgroundColorSpan(backgroundColor.Value), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                }
            });
        }
        public static void SetRangeDecoration(this SpannableString spannableString, int ixStart, int ixEnd, bool? underline = false)
        {
            CoreUtility.ExecuteMethod("SetRangeDecoration", delegate ()
            {
                if (underline.GetValueOrDefault())
                {
                    spannableString.SetSpan(new UnderlineSpan(), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                }

            });
        }
        public static void SetRangeIndent(this SpannableString spannableString, int ixStart, int ixEnd, int firstLineIndent, int bodyIndent)
        {
            CoreUtility.ExecuteMethod("SetRangeIndent", delegate ()
            {
                spannableString.SetSpan(new LeadingMarginSpanStandard(firstLineIndent, bodyIndent), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
            });
        }
        public static void SetRangeAlignment(this SpannableString spannableString, int ixStart, int ixEnd, Layout.Alignment alignment)
        {
            CoreUtility.ExecuteMethod("SetRangeAlignment", delegate ()
            {
                spannableString.SetSpan(new AlignmentSpanStandard(alignment), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
            });
        }
        public static void SetRangeClickable(this SpannableString spannableString, int ixStart, int ixEnd, Action<CoreClickableSpan, View> onClick, string argument, Color textColor)
        {
            CoreUtility.ExecuteMethod("SetRangeClickable", delegate ()
            {
                CoreClickableSpan clickableSpan = new CoreClickableSpan(onClick, argument);
                clickableSpan.HideUnderline = true;
                spannableString.SetSpan(clickableSpan, ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                spannableString.SetSpan(new StyleSpan(TypefaceStyle.Bold), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
                spannableString.SetSpan(new ForegroundColorSpan(textColor), ixStart, ixEnd, SpanTypes.ExclusiveExclusive);
            });

        }
        public static void WireUpLinks(this SpannableString spannableString, string prefix, List<TextAnnotation> links, Color linkColor, Action<CoreClickableSpan, View> linkTapped)
        {
            CoreUtility.ExecuteMethod("WireUpLinks", delegate ()
            {
                if (spannableString != null && links != null && linkTapped != null)
                {
                    if (prefix == null)
                    {
                        prefix = string.Empty;
                    }
                    foreach (TextAnnotation link in links)
                    {
                        spannableString.SetRangeClickable(link.start + prefix.Length, link.end + prefix.Length, linkTapped, link.target, linkColor);
                    }
                }
            });
        }
    }
}