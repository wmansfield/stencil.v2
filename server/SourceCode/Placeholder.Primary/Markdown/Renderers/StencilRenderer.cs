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
using Placeholder.Primary.Markdown.Renderers.Inlines;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers
{
    public class StencilRenderer<TSection> : RendererBase
        where TSection : class
    {
        public StencilRenderer(IEntityGenerator<TSection> entityGenerator)
        {
            this.Generator = entityGenerator;
            this.TextBuilders = new Stack<StringBuilder>();
            this.RawBuilders = new Stack<StringBuilder>();
            this.Sections = new List<TSection>();

            // Default block renderers
            this.ObjectRenderers.Add(new CodeBlockRenderer<TSection>());
            this.ObjectRenderers.Add(new ListRenderer<TSection>());
            this.ObjectRenderers.Add(new HeadingRenderer<TSection>());
            this.ObjectRenderers.Add(new ParagraphRenderer<TSection>());
            this.ObjectRenderers.Add(new QuoteBlockRenderer<TSection>());
            this.ObjectRenderers.Add(new ThematicBreakRenderer<TSection>());

            // Default inline renderers
            this.ObjectRenderers.Add(new CodeInlineRenderer<TSection>());
            this.ObjectRenderers.Add(new EmphasisInlineRenderer<TSection>());
            this.ObjectRenderers.Add(new LinkOrAssetInlineRenderer<TSection>());
            this.ObjectRenderers.Add(new LiteralInlineRenderer<TSection>());

            // Purposefully not supported
            //this.ObjectRenderers.Add(new DelimiterInlineRenderer());
            //this.ObjectRenderers.Add(new TableRenderer());
            //this.ObjectRenderers.Add(new TaskListRenderer());
        }

        

        public List<TSection> Sections { get; private set; }
        public Stack<StringBuilder> TextBuilders { get; private set; }
        public Stack<StringBuilder> RawBuilders { get; private set; }
        public IEntityGenerator<TSection> Generator { get; private set; }
        /// <summary>
        /// True if a parent container wants to own the composition of the current render
        /// </summary>
        public bool IsComposing { get; set; }
        private List<SpanData> _spans = new List<SpanData>();


        private Dictionary<string, Stack<SpanData>> _spanStack = new Dictionary<string, Stack<SpanData>>();

        private char[] _buffer = new char[1024];


        public override object Render(MarkdownObject markdownObject)
        {
            this.Write(markdownObject);
            return this.Sections;
        }

        public void StartTextCapture()
        {
            this.TextBuilders.Push(new StringBuilder());
        }
        public StringBuilder EndTextCapture()
        {
            return this.TextBuilders.Pop();
        }

        public void StartRawCapture()
        {
            this.RawBuilders.Push(new StringBuilder());
        }
        public StringBuilder EndRawCapture()
        {
            return this.RawBuilders.Pop();
        }

        public void StartSpan(SpanData span, string prefix)
        {
            if (!_spanStack.ContainsKey(span.type))
            {
                _spanStack[span.type] = new Stack<SpanData>();
            }
            span.start = this.TextBuilders.Peek().Length;
            _spanStack[span.type].Push(span);

            this.RawBuilders.Peek().Append(prefix);
        }
        public void EndSpan(string type, string suffix, bool collect = true)
        {
            if (_spanStack.ContainsKey(type))
            {
                SpanData result = _spanStack[type].Pop();
                result.end = this.TextBuilders.Peek().Length;
                if (collect)
                {
                    _spans.Add(result);
                }
                this.RawBuilders.Peek().Append(suffix);
            }
        }
        public SpanData[] CollectSpans()
        {
            SpanData[] result = _spans.ToArray();
            _spans.Clear();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteLeafInline(LeafBlock leafBlock)
        {
            Inline inline = leafBlock.Inline as Inline;
            while (inline != null)
            {
                this.Write(inline);
                inline = inline.NextSibling;
            }
        }

        public StencilRenderer<TSection> WriteLeafRawLines(LeafBlock leafBlock, bool stripNewLines = true)
        {
            if (leafBlock.Lines.Lines != null)
            {
                StringLineGroup lines = leafBlock.Lines;
                StringLine[] slices = lines.Lines;
                for (var i = 0; i < lines.Count; i++)
                {
                    if (!stripNewLines && i > 0)
                    {
                        this.WriteText("\n", 0 ,1);
                    }
                    this.WriteText(ref slices[i].Slice);
                }
            }
            return this;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StencilRenderer<TSection> WriteText(ref StringSlice slice)
        {
            if (slice.Start > slice.End)
            {
                return this;
            }
            return this.WriteText(slice.Text, slice.Start, slice.Length);
        }
        public StencilRenderer<TSection> WriteText(string content, int offset, int length)
        {
            if (content != null)
            {
                string text = null;
                if (offset == 0 && content.Length == length)
                {
                    text = content;
                }
                else
                {
                    if (length > _buffer.Length)
                    {
                        _buffer = content.ToCharArray();
                        text = new string(_buffer, offset, length);
                    }
                    else
                    {
                        content.CopyTo(offset, _buffer, 0, length);
                        text = new string(_buffer, 0, length);
                    }
                }

                string clean = text.Replace("\t", "   ");
                this.TextBuilders.Peek().Append(clean);
                this.RawBuilders.Peek().Append(clean);
            }
            return this;
        }
       
    }
}
