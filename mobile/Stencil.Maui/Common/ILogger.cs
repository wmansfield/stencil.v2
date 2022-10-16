using System;

namespace Stencil.Maui
{
    public interface ILogger
    {
        void LogDebug(string message);
        void LogInformation(string message);
        void LogError(string source, Exception ex);
    }
}
