using Stencil.Native.Commanding;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Native.Views.Markdown
{
    public interface IMarkdownViewItem
    {
        /// <summary>
        /// Typed object used by the view component
        /// </summary>
        object PreparedData { get; set; }
    }
}
