using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views.Standard
{
    public partial interface IPreparedBindingContext
    {
        CGSize? CachedSize { get; set; }
    }
}
