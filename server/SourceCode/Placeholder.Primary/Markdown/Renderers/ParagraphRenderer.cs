using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class ParagraphRenderer<TSection> : StencilObjectRenderer<TSection, ParagraphBlock>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, ParagraphBlock paragraphBlock)
        {
            if (renderer.IsComposing)
            {
                // someone above us controls authoring
                renderer.WriteLeafInline(paragraphBlock);
            }
            else
            {
                // we control ourselves
                renderer.StartTextCapture();
                renderer.StartRawCapture();

                renderer.WriteLeafInline(paragraphBlock);

                string text = renderer.EndTextCapture().ToString();
                string markdown = renderer.EndRawCapture().ToString();


                SpanData[] spans = renderer.CollectSpans();
                if (!string.IsNullOrEmpty(text))
                {
                    // work around for double spaces : ¦
                    renderer.Sections.Add(renderer.Generator.GenerateTextSection(text.Replace("¦", ""), spans, markdown.Replace("¦", "")));
                }
                
            }
        }
    }
}
