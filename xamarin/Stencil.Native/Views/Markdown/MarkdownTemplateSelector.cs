using Stencil.Native.Commanding;
using Stencil.Native.Markdown;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Native.Views.Markdown
{
    public class MarkdownTemplateSelector : DataTemplateSelector
    {
        #region Constructors

        static MarkdownTemplateSelector()
        {
            MarkdownLibrary = new MarkdownLibrary();
        }

        public MarkdownTemplateSelector(ICommandScope commandScope)
        {
            this.CommandScope = commandScope;
        }

        #endregion

        #region Properties

        public static IMarkdownLibrary MarkdownLibrary;

        public ICommandScope CommandScope { get; set; }

        #endregion


        #region Overrides

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            MarkdownSection markdownSection = item as MarkdownSection;

            if (markdownSection != null)
            {
                IMarkdownElement element = MarkdownLibrary.GetComponent(markdownSection.kind);
                if(element != null)
                {
                    markdownSection.PreparedData = element.PrepareData(this.CommandScope, this, markdownSection);
                    return element.GetDataTemplate();
                }
            }
            
            return null;//TODO:MUST: Figure out a graceful way to fail here
        }

        #endregion
    }
}
