using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class HeadingRenderer<TSection> : StencilObjectRenderer<TSection, HeadingBlock>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, HeadingBlock headingBlock)
        {
            renderer.StartTextCapture();
            renderer.StartRawCapture();

            renderer.WriteLeafInline(headingBlock);

            string text = renderer.EndTextCapture().ToString();
            string markdown = renderer.EndRawCapture().ToString();

            SpanData[] spans = renderer.CollectSpans();
            if (!string.IsNullOrEmpty(text))
            {
                renderer.Sections.Add(renderer.Generator.GenerateHeaderSection(text, headingBlock.Level, spans, markdown));
            }
        }
    }
}
