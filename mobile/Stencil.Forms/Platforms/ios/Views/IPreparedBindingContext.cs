using CoreGraphics;
using Newtonsoft.Json;
using Stencil.Forms.iOS.Markdown;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views.Standard
{
    public partial interface IPreparedBindingContext
    {
        CGSize? CachedSize { get; set; }
        CacheModel UICache { get; set; }
    }
}
