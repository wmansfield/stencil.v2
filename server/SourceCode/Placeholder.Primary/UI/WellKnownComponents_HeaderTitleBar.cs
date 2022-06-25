using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class HeaderTitleBar
        {
            public const string NAME = "headerTitleBar";
            public class Config
            {
                public string TextColor { get; set; }
                public string BackgroundColor { get; set; }
                public string LeftIcon { get; set; }
                public string RightIcon { get; set; }
                public string Title { get; set; }
                public string RightCommandName { get; set; }
                public string RightCommandParameter { get; set; }

                public string LeftCommandName { get; set; }
                public string LeftCommandParameter { get; set; }
            }
        }
    }
}
