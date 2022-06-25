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
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers.Generators
{
    public class MarkdownEntityGenerator : IEntityGenerator<MarkdownSection>
    {
        public MarkdownSection GenerateDividerSection()
        {
            return new MarkdownSection()
            {
                kind = MarkdownSectionKind.divider
            };
        }
        public MarkdownSection GenerateImageSection(string asset_identifier, string url = null, string dimensions = null, string cover_url = null)
        {
            return new MarkdownSection()
            {
                kind = MarkdownSectionKind.image,
                asset = new AssetData()
                {
                    identifier = asset_identifier,
                    url = url,
                    dimensions = dimensions,
                    cover_url = cover_url
                }
            };
        }
        public MarkdownSection GenerateVideoSection(string asset_identifier, string url = null, string dimensions = null, string cover_url = null)
        {
            return new MarkdownSection()
            {
                kind = MarkdownSectionKind.video,
                asset = new AssetData()
                {
                    identifier = asset_identifier,
                    url = url,
                    dimensions = dimensions,
                    cover_url = cover_url
                }
            };
        }
        public MarkdownSection GenerateHeaderSection(string text, int level, SpanData[] emphasisData, string markdown)
        {
            return new MarkdownSection(markdown)
            {
                kind = MarkdownSectionKind.header,
                level = level,
                text = text,
                annotations = this.ToAnnotations(emphasisData)
            };
        }
        public MarkdownSection GenerateTextSection(string text, SpanData[] emphasisData, string markdown)
        {
            return new MarkdownSection(markdown)
            {
                kind = MarkdownSectionKind.text,
                text = text,
                annotations = this.ToAnnotations(emphasisData)
            };
        }
        public MarkdownSection GenerateCodeBlockSection(string text, string markdown)
        {
            return new MarkdownSection(markdown)
            {
                kind = MarkdownSectionKind.code_block,
                text = text,
            };
        }
        
        public MarkdownSection GenerateQuoteBlockSection(string text, SpanData[] emphasisData, string markdown)
        {
            return new MarkdownSection(markdown)
            {
                kind = MarkdownSectionKind.block_quote,
                text = text,
                annotations = this.ToAnnotations(emphasisData)
            };
        }
        public MarkdownSection GenerateBulletedListSection(AnnotatedTextItem[] textItems)
        {
            return new MarkdownSection()
            {
                kind = MarkdownSectionKind.bullet_text,
                items = textItems
            };
        }
        public MarkdownSection GenerateNumberedListSection(AnnotatedTextItem[] textItems)
        {
            return new MarkdownSection()
            {
                kind = MarkdownSectionKind.bullet_number,
                items = textItems
            };
        }
        public AnnotatedTextItem GenerateAnnotatedText(string text, SpanData[] emphasisData, string markdown)
        {
            return new AnnotatedTextItem(markdown)
            {
                text = text,
                annotations = ToAnnotations(emphasisData)
            };
        }

        public List<TextAnnotation> ToAnnotations(SpanData[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }
            List<TextAnnotation> result = new List<TextAnnotation>();
            for (int i = 0; i < data.Length; i++)
            {
                result.Add(this.ToAnnotation(data[i]));
            }
            return result;
        }
        public TextAnnotation ToAnnotation(SpanData data)
        {
            return new TextAnnotation()
            {
                end = data.end,
                start = data.start,
                type = data.type,
                target = data.target
            };
        }
    }
}
