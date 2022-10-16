using Foundation;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Stencil.Maui.iOS.Markdown
{
    public class MarkdownData
    {
        public MarkdownData()
        {
        }
        public MarkdownData(NSMutableAttributedString text, List<TextAnnotation> links)
        {
            this.Text = text;
            this.Links = links;
        }
        public NSMutableAttributedString Text { get; set; }
        public List<TextAnnotation> Links { get; set; }
    }
}