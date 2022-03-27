using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers.Inlines
{
    public class CodeInlineRenderer<TSection> : StencilObjectRenderer<TSection, CodeInline>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, CodeInline codeInline)
        {
            string type = "code";

            renderer.StartSpan(new SpanData() { type = type }, "`");
            renderer.WriteText(codeInline.Content, 0, codeInline.Content.Length);
            renderer.EndSpan(type, "`");

        }
    }
}
