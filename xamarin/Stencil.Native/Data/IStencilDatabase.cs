using Stencil.Native.Screens;
using System;
using System.Collections.Generic;

namespace Stencil.Native.Data
{
    public interface IStencilDatabase : IDisposable
    {
        ScreenConfig ScreenConfig_Get(string screenIdentifier);
        void ScreenConfig_Upsert(ScreenConfig screenConfig);
        void ScreenConfig_Invalidate(string screenIdentifier);
        List<ScreenConfig> ScreenConfig_GetForDownloading();
        List<ScreenConfig> ScreenConfig_GetWithName(string screen_name);
    }
}
