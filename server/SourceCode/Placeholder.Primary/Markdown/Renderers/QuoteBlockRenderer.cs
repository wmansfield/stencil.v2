using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class QuoteBlockRenderer<TSection> : StencilObjectRenderer<TSection, QuoteBlock>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, QuoteBlock quoteBlock)
        {
            renderer.IsComposing = true;
            renderer.StartTextCapture();
            renderer.StartRawCapture();

            renderer.WriteChildren(quoteBlock);

            string text = renderer.EndTextCapture().ToString();
            string markdown = renderer.EndRawCapture().ToString();

            SpanData[] spans = renderer.CollectSpans();
            renderer.IsComposing = false;
            if (!string.IsNullOrEmpty(text))
            {
                renderer.Sections.Add(renderer.Generator.GenerateQuoteBlockSection(text, spans, markdown));
            }
        }
    }
}
