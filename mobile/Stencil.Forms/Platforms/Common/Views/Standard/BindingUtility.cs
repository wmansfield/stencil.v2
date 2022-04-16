using Stencil.Forms.Platforms.Common.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views.Standard
{
    public static class BindingUtility
    {
        /// <summary>
        /// Special interface designed to introduce dispose/cleanup/dissever pattern for binding contexts.
        /// Requires extended control to keep state and proper initilization during data template resolution.
        /// May not always be implemented in all of stencil.
        /// </summary>
        public static void TrackOnBindingContextChanged(ITrackBindingContext element)
        {
            CoreUtility.ExecuteMethod(nameof(TrackOnBindingContextChanged), delegate ()
            {
                object context = element.BindingContext;

                if (context == null)
                {
                    if (element.RecentPreparedBindingContext.TryGetTarget(out IPreparedBindingContext previousBindingContext))
                    {
                        previousBindingContext?.OnViewDetachedFromContext();
                    };
                    element.RecentPreparedBindingContext = null;
                }
                else
                {
                    if (element.RecentPreparedBindingContext != null && element.RecentPreparedBindingContext.TryGetTarget(out IPreparedBindingContext previousBindingContext))
                    {
                        if (previousBindingContext != context)
                        {
                            previousBindingContext?.OnViewDetachedFromContext();
                        }
                    };

                    IPreparedBindingContext preparedBindingContext = context as IPreparedBindingContext;
                    if (preparedBindingContext != null)
                    {
                        element.RecentPreparedBindingContext = new WeakReference<IPreparedBindingContext>(preparedBindingContext);
                    }
                }
            });
        }
    }
}
