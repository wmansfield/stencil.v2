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

namespace Placeholder.Primary.Markdown.Renderers
{
    public interface IEntityGenerator<TSection>
        where TSection : class
    {
        TSection GenerateVideoSection(string asset_identifier, string url = null, string dimensions = null, string cover_url = null);
        TSection GenerateImageSection(string asset_identifier, string url = null, string dimensions = null, string cover_url = null);
        TSection GenerateQuoteBlockSection(string text, SpanData[] emphasisData, string markdown);
        TSection GenerateCodeBlockSection(string text, string markdown);
        TSection GenerateTextSection(string text, SpanData[] emphasisData, string markdown);
        TSection GenerateHeaderSection(string text, int level, SpanData[] emphasisData, string markdown);
        TSection GenerateDividerSection();
        TSection GenerateBulletedListSection(AnnotatedTextItem[] textItems);
        TSection GenerateNumberedListSection(AnnotatedTextItem[] textItems);
        AnnotatedTextItem GenerateAnnotatedText(string text, SpanData[] emphasisData, string markdown);
    }
}
