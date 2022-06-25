using Markdig;
using Markdig.Syntax;
using Placeholder.Primary.Markdown.Renderers;
using Placeholder.Primary.Markdown.Renderers.Generators;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using dm = Placeholder.Domain;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Markdown
{
    public class MarkdownProcessor : ChokeableClass, IMarkdownProcessor
    {
        public MarkdownProcessor(IFoundation foundation)
            : base(foundation)
        {

        }

        public string MarkDownFromStencilHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            return html
                // Extra Spaces
                .Replace(string.Format("<{0}> ", StencilTags.TAG_BOLD), "**")
                .Replace(string.Format(" </{0}>", StencilTags.TAG_BOLD), "**")
                .Replace(string.Format("<{0}> ", StencilTags.TAG_ITALIC), "*")
                .Replace(string.Format(" </{0}>", StencilTags.TAG_ITALIC), "*")
                .Replace(string.Format("<{0}> ", StencilTags.TAG_UNDERLINE), "++")
                .Replace(string.Format(" </{0}>", StencilTags.TAG_UNDERLINE), "++")
                .Replace(string.Format("<{0}> ", StencilTags.TAG_HIGHLIGHT), "==")
                .Replace(string.Format(" </{0}>", StencilTags.TAG_HIGHLIGHT), "==")
                .Replace(string.Format("<{0}> ", StencilTags.TAG_ALINK), "(")
                .Replace(string.Format(" </{0}>", StencilTags.TAG_ALINK), ")")
                .Replace(string.Format("<{0}> ", StencilTags.TAG_ATITLE), "[")
                .Replace(string.Format(" </{0}>", StencilTags.TAG_ATITLE), "]")

                // Normal
                .Replace(string.Format("<{0}>", StencilTags.TAG_BOLD), "**")
                .Replace(string.Format("</{0}>", StencilTags.TAG_BOLD), "**")
                .Replace(string.Format("<{0}>", StencilTags.TAG_ITALIC), "*")
                .Replace(string.Format("</{0}>", StencilTags.TAG_ITALIC), "*")
                .Replace(string.Format("<{0}>", StencilTags.TAG_UNDERLINE), "")
                .Replace(string.Format("</{0}>", StencilTags.TAG_UNDERLINE), "++")
                .Replace(string.Format("<{0}>", StencilTags.TAG_HIGHLIGHT), "==")
                .Replace(string.Format("</{0}>", StencilTags.TAG_HIGHLIGHT), "==")
                .Replace(string.Format("<{0}>", StencilTags.TAG_ALINK), "(")
                .Replace(string.Format("</{0}>", StencilTags.TAG_ALINK), ")")
                .Replace(string.Format("<{0}>", StencilTags.TAG_ATITLE), "[")
                .Replace(string.Format("</{0}>", StencilTags.TAG_ATITLE), "]");

        }

        public List<TSection> Parse<TSection>(IEntityGenerator<TSection> generator, string payload)
            where TSection : class
        {
            if (string.IsNullOrEmpty(payload))
            {
                return new List<TSection>();
            }

            // convert soft-return to hard return, expand to double
            payload = payload
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");

            // reduce to max newlines
            payload = Regex.Replace(payload, @"(\n){2,}", "\n\n");

            // work around for extra line space
            payload = Regex.Replace(payload, "\n\n", "\n\n¦\n\n");

            // add leading/trailing space for easy parsing
            payload = string.Format(" {0} ", payload);

            StencilRenderer<TSection> renderer = new StencilRenderer<TSection>(generator);
            MarkdownPipeline pipeline = new MarkdownPipelineBuilder().Build();
            pipeline.Setup(renderer); // override the renderer with our own writer
            MarkdownDocument document = Markdig.Markdown.Parse(payload, pipeline);
            return renderer.Render(document) as List<TSection>;
        }

        public void ParseAndApply(dm.Widget source, sdk.Widget target)
        {
            base.ExecuteMethod("ParseAndApply", delegate ()
            {
                target.sections = null;
                target.text = string.Empty;

                if (!string.IsNullOrEmpty(source.payload))
                {
                    // Translate Markdown to DSL
                    target.sections = this.Parse<MarkdownSection>(new MarkdownEntityGenerator(), source.payload).ToArray();

                    // Prepare for Search and some backwards compat
                    foreach (MarkdownSection section in target.sections)
                    {
                        switch (section.kind)
                        {
                            case MarkdownSectionKind.text:
                            case MarkdownSectionKind.header:
                            case MarkdownSectionKind.block_quote:
                                target.text = target.text.Trim() + " " + section.text;
                                break;
                            case MarkdownSectionKind.bullet_text:
                            case MarkdownSectionKind.bullet_number:
                                if (section.items != null)
                                {
                                    foreach (var item in section.items)
                                    {
                                        target.text = target.text.Trim() + string.Format("\n {0}", item.text);
                                    }
                                    target.text = target.text.Trim();
                                }
                                break;
                            // case MarkdownSectionKind.image: // disabled
                            // case MarkdownSectionKind.video: // disabled
                            case MarkdownSectionKind.divider:
                            case MarkdownSectionKind.code_block:
                                break;
                            default:
                                break;
                        }
                    }
                }
                target.payload = string.Empty;
                target.text = target.text.Trim();

            });
        }

    }
}
