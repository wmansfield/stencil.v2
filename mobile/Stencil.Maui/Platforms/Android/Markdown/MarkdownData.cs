using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Maui.Droid.Markdown
{
    public class MarkdownData
    {
        public MarkdownData()
        {
        }
        public MarkdownData(SpannableString text, List<TextAnnotation> links)
        {
            this.Text = text;
            this.Links = links;
        }
        public SpannableString Text { get; set; }
        public List<TextAnnotation> Links { get; set; }
    }
}