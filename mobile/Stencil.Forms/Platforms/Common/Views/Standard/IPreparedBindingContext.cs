using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard
{
    public partial interface IPreparedBindingContext
    {
        void PrepareInteractions(bool force = false);

        /// <summary>
        /// Only enforced by stencil native controls. Ex. (TrackedStackLayout)
        /// Other controls are urged to follow the pattern, but may not.
        /// See ITrackBindingContext
        /// </summary>
        void OnViewDetachedFromContext();
    }
}
