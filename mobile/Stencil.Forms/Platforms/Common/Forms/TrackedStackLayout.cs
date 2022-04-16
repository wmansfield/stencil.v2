using Stencil.Forms.Platforms.Common.Forms;
using Stencil.Forms.Views.Standard;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms
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
