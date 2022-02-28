using System;

namespace Stencil.Forms
{
    public interface IAppAnalytics
    {
        void TrackScreen(string screenName);
        void LogError(Exception ex, string source);
    }
}
