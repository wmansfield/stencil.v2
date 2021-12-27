using Stencil.Native.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public class VisualConfig : IVisualConfig
    {
        public ThicknessInfo Margin { get; set; }
        public string BackgroundColor { get; set; }
    }
}
