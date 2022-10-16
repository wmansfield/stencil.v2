using System;

namespace Stencil.Maui
{
    public interface IAppAnalytics
    {
        void TrackScreen(string screenName);
        void LogError(Exception ex, string source);
    }
}
