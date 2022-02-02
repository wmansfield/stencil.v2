using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Screens
{
    public interface IVisualConfig
    {
        ThicknessInfo Padding { get; }
        ThicknessInfo Margin { get; }

        string BackgroundColor { get; }
        string BackgroundImage { get; }
    }
}
