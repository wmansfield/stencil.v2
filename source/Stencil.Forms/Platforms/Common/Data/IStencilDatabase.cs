using Stencil.Forms.Screens;
using System;
using System.Collections.Generic;

namespace Stencil.Forms.Data
{
    public interface IStencilDatabase : IDisposable
    {
#if !DEBUG
        [Obsolete("Do not use in production.", true)]
#endif
        List<ScreenConfig> ScreenConfig_All();
        ScreenConfig ScreenConfig_Get(string screenIdentifier);
        void ScreenConfig_Upsert(ScreenConfig screenConfig);
        void ScreenConfig_Invalidate(string screenIdentifier);
        void ScreenConfig_Remove(string screenIdentifier);

        List<ScreenConfig> ScreenConfig_GetForDownloading();
        List<ScreenConfig> ScreenConfig_GetWithName(string screen_name);
    }
}
