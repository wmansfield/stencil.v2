using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Markdown
{
    public interface IMarkdownHost
    {
        void LinkTapped(string argument);
        void AnythingTapped();
        int FontSize { get; }
        bool SuppressDivider { get; }
    }
}
