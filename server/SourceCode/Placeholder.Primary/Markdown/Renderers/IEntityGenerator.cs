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
