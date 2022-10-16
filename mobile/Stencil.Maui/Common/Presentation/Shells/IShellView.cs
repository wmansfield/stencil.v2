using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Presentation.Shells
{
    public interface IShellView
    {
        View MenuContent { get; set; }
        View ViewContent { get; set; }
    }
}
