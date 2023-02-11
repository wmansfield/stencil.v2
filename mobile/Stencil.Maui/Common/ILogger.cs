using System;

namespace Stencil.Maui
{
    public interface ILogger
    {
        void LogTrace(string message, int level = 0);
        void LogDebug(string message);
        void LogInformation(string message);
        void LogError(string source, Exception ex);
    }
}
