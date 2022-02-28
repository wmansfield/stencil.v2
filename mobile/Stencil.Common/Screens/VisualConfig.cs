using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Screens
{
    public class VisualConfig : IVisualConfig
    {
        public ThicknessInfo Padding { get; set; }
        public ThicknessInfo Margin { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public GradientBrushInfo BackgroundBrush { get; set; }
    }
}
