using Stencil.Maui.Views.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Platforms.Common.Forms
{
    /// <summary>
    /// Special interface designed to introduce dispose/cleanup/dissever pattern for binding contexts.
    /// Requires extended control to keep state and proper initilization during data template resolution.
    /// May not always be implemented in all of stencil.
    /// </summary>
    public interface ITrackBindingContext
    {
        WeakReference<IPreparedBindingContext> RecentPreparedBindingContext { get; set; }
        object BindingContext { get; }
    }
}
