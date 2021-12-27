using Stencil.Native.Markdown;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Views.Markdown
{
    public interface IMarkdownLibrary
    {
        IMarkdownElement GetComponent(MarkdownSectionKind sectionKind);
    }
}
