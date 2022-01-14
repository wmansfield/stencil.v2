using Foundation;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Stencil.Forms.iOS.Markdown
{
    public static class _MarkdownExtensions
    {
        public static void WireUpLinks(this MarkDownLabel label, List<TextAnnotation> annotations, Action<string> clickAction)
        {
            if (annotations != null && label != null)
            {
                foreach (TextAnnotation item in annotations)
                {
                    try
                    {
                        label.AddLinkToURL(new NSUrl(item.target), new NSRange(item.start, item.end - item.start));
                    }
                    catch { }
                }
                label.DidSelectLinkWithUrlAction = (url) => { clickAction?.Invoke(url.ToString()); };
            }
        }
    }
}