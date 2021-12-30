using Stencil.Native.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public class VisualConfig : IVisualConfig
    {
        public ThicknessInfo Padding { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
    }
}
