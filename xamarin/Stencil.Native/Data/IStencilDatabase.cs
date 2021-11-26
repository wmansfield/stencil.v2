using Stencil.Native.Screens;
using System;

namespace Stencil.Native.Data
{
    public interface IStencilDatabase : IDisposable
    {
        ScreenConfig ScreenConfig_Get(string screenName);
        void ScreenConfig_Upsert(string screenName, ScreenConfig screenConfig);
    }
}
