using Markdig.Syntax;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class ListRenderer<TSection> : StencilObjectRenderer<TSection, ListBlock>
        where TSection : class
    {
        protected override void Write(StencilRenderer<TSection> renderer, ListBlock listBlock)
        {
            List<AnnotatedTextItem> lineItems = new List<AnnotatedTextItem>();
            foreach (var item in listBlock)
            {
                ListItemBlock listItem = item as ListItemBlock;
                if(listItem != null)
                {
                    renderer.IsComposing = true;
                    renderer.StartTextCapture();
                    renderer.StartRawCapture();
                    renderer.WriteChildren(listItem);
                    string text = renderer.EndTextCapture().ToString();
                    string markdown = renderer.EndRawCapture().ToString();


                    SpanData[] spans = renderer.CollectSpans();
                    renderer.IsComposing = false;

                    if (!string.IsNullOrEmpty(text))
                    {
                        lineItems.Add(renderer.Generator.GenerateAnnotatedText(text, spans, markdown));
                    }
                }
            }

            if (lineItems.Count > 0)
            {
                if (listBlock.IsOrdered)
                {
                    renderer.Sections.Add(renderer.Generator.GenerateNumberedListSection(lineItems.ToArray()));
                }
                else
                {
                    renderer.Sections.Add(renderer.Generator.GenerateBulletedListSection(lineItems.ToArray()));
                }
            }
        }
    }

}
