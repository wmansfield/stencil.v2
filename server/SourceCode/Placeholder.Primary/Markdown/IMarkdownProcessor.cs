using Placeholder.Primary.Markdown.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dm = Placeholder.Domain;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Markdown
{
    public interface IMarkdownProcessor
    {
        string MarkDownFromStencilHtml(string html);
        List<TSection> Parse<TSection>(IEntityGenerator<TSection> generator, string payload)
            where TSection : class;

        void ParseAndApply(dm.Widget source, sdk.Widget target);
    }
}
