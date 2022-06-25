using Stencil.Common;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class SlimEntry
        {
            public const string NAME = "slimEntry";
            public class Config : CommandFieldConfig
            {
                public string Input { get; set; }
                public string Placeholder { get; set; }
                public bool IsPassword { get; set; }
                public bool Borderless { get; set; }
                public string BackgroundColor { get; set; }
                public string InputBackgroundColor { get; set; }
                public string TextColor { get; set; }
                public string PlaceholderColor { get; set; }
                public ThicknessInfo Padding { get; set; }

            }
        }
    }
}
