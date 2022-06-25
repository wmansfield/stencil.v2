/*
The MIT License
Copyright (c) 2016 Social Haven

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
'Software'), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
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
