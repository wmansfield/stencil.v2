using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class CodeBlockRenderer<TSection> : StencilObjectRenderer<TSection, CodeBlock>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, CodeBlock codeBlock)
        {
            // we control ourselves
            renderer.StartTextCapture();
            renderer.StartRawCapture();

            renderer.WriteLeafRawLines(codeBlock, false);

            string text = renderer.EndTextCapture().ToString();
            string markdown = renderer.EndRawCapture().ToString();


            SpanData[] spans = renderer.CollectSpans();
            if (!string.IsNullOrEmpty(text))
            {
                renderer.Sections.Add(renderer.Generator.GenerateCodeBlockSection(text, markdown));
            }
        }
    }
}
