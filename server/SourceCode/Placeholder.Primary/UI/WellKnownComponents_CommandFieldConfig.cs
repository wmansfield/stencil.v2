using Stencil.Common;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class CommandFieldConfig
        {
            public bool IsRequired { get; set; }
            public string GroupName { get; set; }
            public string FieldName { get; set; }

        }
    }
}
