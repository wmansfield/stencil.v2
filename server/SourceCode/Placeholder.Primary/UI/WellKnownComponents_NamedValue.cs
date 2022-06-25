using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class NamedValue
        {
            public const string NAME = "namedValue";
            public class Config
            {
                public string NameText { get; set; }
                public string NameTextColor { get; set; }
                public string ValueText { get; set; }
                public string ValueTextColor { get; set; }
                public string BackgroundColor { get; set; }

                public int FontSize { get; set; }
                public ThicknessInfo Padding { get; set; }
            }
        }

    }
}
