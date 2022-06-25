using Stencil.Common;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class DropDown
        {
            public const string NAME = "dropDown";
            public class Config : CommandFieldConfig
            {
                public string Label { get; set; }
                public DisplayPair SelectedItem { get; set; }
                public List<DisplayPair> AvailableValues { get; set; }
                public string BackgroundColor { get; set; }
                public string TextColor { get; set; }
                public string ButtonBackgroundColor { get; set; }
                public string DropDownCancelText { get; set; }
                public ThicknessInfo Padding { get; set; }

            }
        }
    }
}
