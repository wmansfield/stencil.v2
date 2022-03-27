using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers.Inlines
{
    public class LiteralInlineRenderer<TSection> : StencilObjectRenderer<TSection, LiteralInline>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, LiteralInline literalInline)
        {
            if (!literalInline.Content.IsEmpty)
            {
                renderer.WriteText(ref literalInline.Content);
            }
        }
    }
}
