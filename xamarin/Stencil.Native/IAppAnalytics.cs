using System;

namespace Stencil.Native
{
    public interface IAppAnalytics
    {
        void TrackScreen(string screenName);
        void LogError(Exception ex, string source);
    }
}
