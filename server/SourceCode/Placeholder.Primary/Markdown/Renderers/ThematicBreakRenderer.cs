using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class ThematicBreakRenderer<TSection> : StencilObjectRenderer<TSection, ThematicBreakBlock>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, ThematicBreakBlock thematicBreakBlock)
        {
            renderer.Sections.Add(renderer.Generator.GenerateDividerSection());

        }
    }
}
