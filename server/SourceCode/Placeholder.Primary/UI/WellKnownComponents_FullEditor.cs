using Stencil.Common;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class FullEditor
        {
            public const string NAME = "fullEditor";
            public class Config : CommandFieldConfig
            {
                public string Label { get; set; }
                public string Input { get; set; }
                public string Placeholder { get; set; }
                public string BackgroundColor { get; set; }
                public string InputBackgroundColor { get; set; }
                public string TextColor { get; set; }
                public string PlaceholderColor { get; set; }
                public ThicknessInfo Padding { get; set; }
                public ThicknessInfo Margin { get; set; }

            }
        }
    }
}
