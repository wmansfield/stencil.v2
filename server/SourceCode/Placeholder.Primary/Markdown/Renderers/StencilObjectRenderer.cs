using Markdig.Renderers;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public abstract class StencilObjectRenderer<TSection, TObject> : MarkdownObjectRenderer<StencilRenderer<TSection>, TObject>
        where TSection : class
        where TObject : MarkdownObject
    {
    }
}
