using Stencil.Native.Markdown;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Views.Markdown
{
    public class MarkdownLibrary : IMarkdownLibrary
    {
        public MarkdownLibrary()
        {
            _markdownElements = new Dictionary<string, IMarkdownElement>(StringComparer.OrdinalIgnoreCase);
        }

        public const string LIBRARY_NAME = "markdown";


        private Dictionary<string, IMarkdownElement> _markdownElements;

        public IMarkdownElement GetComponent(MarkdownSectionKind sectionKind)
        {
            switch (sectionKind)
            {
                case MarkdownSectionKind.text:
                    
                    break;
                case MarkdownSectionKind.bullet_text:
                    break;
                case MarkdownSectionKind.header:
                    break;
                case MarkdownSectionKind.image:
                    break;
                case MarkdownSectionKind.block_quote:
                    break;
                case MarkdownSectionKind.video:
                    break;
                case MarkdownSectionKind.bullet_number:
                    break;
                case MarkdownSectionKind.divider:
                    break;
                case MarkdownSectionKind.code_block:
                    break;
                default:
                    break;
            }
            if (_markdownElements.TryGetValue(sectionKind.ToString(), out IMarkdownElement markdownElement))
            {
                return markdownElement;
            }
            return null;
        }
    }
}
