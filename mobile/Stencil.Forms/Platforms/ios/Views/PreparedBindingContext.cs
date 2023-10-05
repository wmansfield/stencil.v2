using CoreGraphics;
using Newtonsoft.Json;
using Stencil.Forms.iOS.Markdown;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views.Standard
{
    public abstract partial class PreparedBindingContext<TAPI>
    {
        [JsonIgnore]
        public CGSize? CachedSize { get; set; }

        [JsonIgnore]
        public CacheModel UICache { get; set; }
    }
}
