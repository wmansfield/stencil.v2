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
