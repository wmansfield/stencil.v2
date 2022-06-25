using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class HeaderWithIcon
        {
            public const string NAME = "headerWithIcon";
            public class Config
            {
                public string Text { get; set; }
                public string TextColor { get; set; }
                public string BackgroundColor { get; set; }

                public string Icon { get; set; }
                public bool ShowIcon { get; set; }
                public int FontSize { get; set; }
                public ThicknessInfo Padding { get; set; }


            }
        }

    }
}
