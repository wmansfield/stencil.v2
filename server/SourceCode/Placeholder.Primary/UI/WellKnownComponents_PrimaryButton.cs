using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class PrimaryButton
        {
            public const string NAME = "primaryButton";
            public class Config
            {
                public string Text { get; set; }
                public string CommandName { get; set; }
                public string CommandParameter { get; set; }
                public string BackgroundColor { get; set; }
                public string Icon { get; set; }
                public bool ShowIcon { get; set; }
                public ThicknessInfo Padding { get; set; }
                public ThicknessInfo Margin { get; set; }
            }
        }
    }
}
