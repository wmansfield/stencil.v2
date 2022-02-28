using System;

namespace Stencil.Forms
{
    public interface ILogger
    {
        void LogDebug(string message);
        void LogInformation(string message);
        void LogError(string source, Exception ex);
    }
}
