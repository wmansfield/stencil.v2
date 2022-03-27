using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Markdown
{
    public class MarkdownSection : AnnotatedTextItem
    {
        public MarkdownSection()
        {

        }
        public MarkdownSection(string source)
        {
            _source = source;
        }

        public MarkdownSectionKind kind { get; set; }
        public AnnotatedTextItem[] items { get; set; }
        public int level { get; set; }
        public AssetData asset { get; set; }


        private string _source = string.Empty;
        public string GetSource()
        {
            return _source;
        }
        public void SetSource(string value)
        {
            _source = value;
        }
    }
}
