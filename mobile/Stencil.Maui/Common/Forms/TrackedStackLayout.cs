using Microsoft.Maui.Controls;
using Stencil.Maui.Platforms.Common.Forms;
using Stencil.Maui.Views.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui
{
    public class TrackedStackLayout : StackLayout, ITrackBindingContext
    {
        public WeakReference<IPreparedBindingContext> RecentPreparedBindingContext { get; set; }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BindingUtility.TrackOnBindingContextChanged(this);
        }
    }
}
