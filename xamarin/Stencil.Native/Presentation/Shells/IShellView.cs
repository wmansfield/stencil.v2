using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Native.Presentation.Shells
{
    public interface IShellView
    {
        View MenuContent { get; set; }
        View ViewContent { get; set; }
    }
}
