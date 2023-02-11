using Starter.App;
using Stencil.Maui.Data.Sync.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unveilr.App.Data.Sync
{
    public class StarterTrackedDataManager : TrackedDataManager<StarterAPI>
    {
        public StarterTrackedDataManager()
            : base(StarterAPI.Instance)
        {

        }
    }
}
