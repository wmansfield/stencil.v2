using Stencil.Native.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public interface IVisualConfig
    {
        ThicknessInfo Padding { get; }
        string BackgroundColor { get; }
        string BackgroundImage { get; }
    }
}
