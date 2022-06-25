using Stencil.Common;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class H1
        {
            public const string NAME = "h1";
            public class Config
            {
                public string Text { get; set; }
                public string TextColor { get; set; }
                public string BackgroundColor { get; set; }
                public ThicknessInfo Padding { get; set; }

            }
        }

        public class H2
        {
            public const string NAME = "h2";
            public class Config
            {
                public string Text { get; set; }
                public string TextColor { get; set; }
                public string BackgroundColor { get; set; }
                public ThicknessInfo Padding { get; set; }

            }
        }

        public class H3
        {
            public const string NAME = "h3";
            public class Config
            {
                public string Text { get; set; }
                public string TextColor { get; set; }
                public string BackgroundColor { get; set; }
                public ThicknessInfo Padding { get; set; }

            }
        }

    }
}
