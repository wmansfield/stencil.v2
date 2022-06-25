using Stencil.Common;
using Stencil.Common.Markdown;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class Markdown
        {
            public const string NAME = "markdown-container";
            public class Config
            {
                public Config()
                {
                    this.FontSize = 16;
                }
                public bool SuppressDivider { get; set; }
                public int FontSize { get; set; }
                public List<MarkdownSection> sections { get; set; }
            }
        }
    }
}
