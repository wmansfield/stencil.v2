using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Markdown
{
    public enum MarkdownSectionKind
    {
        text = 0,
        bullet_text = 1,
        header = 2,
        image = 3,
        block_quote = 4,
        video = 5,
        bullet_number = 6,
        divider = 7,
        code_block = 8
    }
}
