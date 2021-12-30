using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Markdown
{
    public class MarkdownSection : AnnotatedTextItem
    {
        public MarkdownSectionKind kind { get; set; }
        public AnnotatedTextItem[] items { get; set; }

        public AssetData asset { get; set; }

    }
}
