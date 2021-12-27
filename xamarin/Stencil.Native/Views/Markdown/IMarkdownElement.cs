using Stencil.Native.Commanding;
using Stencil.Native.Markdown;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Native.Views.Markdown
{
    public interface IMarkdownElement
    {
        DataTemplate GetDataTemplate();
        object PrepareData(ICommandScope commandScope, DataTemplateSelector selector, AnnotatedTextItem annotatedTextItem);
    }
}
