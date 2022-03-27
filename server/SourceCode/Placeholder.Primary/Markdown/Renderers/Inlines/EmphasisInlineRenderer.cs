using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers.Inlines
{
    public class EmphasisInlineRenderer<TSection> : StencilObjectRenderer<TSection, EmphasisInline>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, EmphasisInline emphasisInline)
        {
            string type = string.Empty;
            string node = string.Empty;
            string wrap = emphasisInline.DelimiterCount == 2 ? new string(emphasisInline.DelimiterChar, 2) : emphasisInline.DelimiterChar.ToString();
            switch (emphasisInline.DelimiterChar)
            {
                case '*':
                case '_':
                    type = emphasisInline.DelimiterCount == 2 ? "bold" : "italic";
                    node = emphasisInline.DelimiterCount == 2 ? StencilTags.TAG_BOLD : StencilTags.TAG_ITALIC;
                    break;
                /* Not supported by renderer at all, never runs
                case '~':
                    type = emphasisInline.IsDouble ? "strike" : "subscript";
                    break;
                case '^':
                    type = "superscript";
                    break;
                */
                case '+':
                    type = "underline";
                    node = StencilTags.TAG_UNDERLINE;
                    break;
                case '=':
                    type = "highlight";
                    node = StencilTags.TAG_HIGHLIGHT;
                    break;
            }

            renderer.StartSpan(new SpanData() { type = type }, string.Format("<{0}>", node));
            renderer.WriteChildren(emphasisInline);
            renderer.EndSpan(type, string.Format("</{0}>", node));

        }
    }
}
