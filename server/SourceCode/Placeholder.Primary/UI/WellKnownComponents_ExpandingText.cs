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
        public class ExpandingText
        {
            public const string NAME = "expandingText";
            public class Config
            {
                public Config()
                {
                }
                public string HeaderText { get; set; }
                public string DetailText { get; set; }
            }
        }
    }
}
