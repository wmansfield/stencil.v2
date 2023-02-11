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
            : base(source)
        {
        }

        public MarkdownSectionKind kind { get; set; }
        public AnnotatedTextItem[] items { get; set; }
        public int level { get; set; }
        public AssetData asset { get; set; }


    }
}
