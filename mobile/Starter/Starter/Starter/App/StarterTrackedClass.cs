using Stencil.Maui;
using System;
using System.Collections.Generic;
using System.Text;

namespace Starter.App
{
    public class StarterTrackedClass : TrackedClass<StarterAPI>
    {
        #region Constructor

        public StarterTrackedClass(string trackPrefix)
            : base(StarterAPI.Instance, trackPrefix)
        {
            this.TrackPrefix = trackPrefix;
        }

        #endregion
    }
}
