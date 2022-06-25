using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class Spacer
        {
            public const string NAME = "spacer";
            public class Config
            {
                public int Height { get; set; }
                public string BackgroundColor { get; set; }
            }
        }
    }
}
