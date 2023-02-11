using System;
using System.Diagnostics;

namespace Stencil.Maui
{
    public class DebugLogger : ILogger
    {
        public void LogTrace(string message, int level = 0)
        {
#if DEBUG
            try
            {
                if (level <= 2)
                {
                    Debug.WriteLine($"{DateTime.UtcNow.ToString("M/dd/yyyy HH:mm:ss.fff")}:{level}: {message}");
                }
            }
            catch
            {
                // gulp
            }
#endif
        }
        public void LogDebug(string message)
        {
#if DEBUG
            try
            {
                Debug.WriteLine(message);
            }
            catch
            {
                // gulp
            }
#endif
        }

        public void LogInformation(string message)
        {
            try
            {
                Debug.WriteLine(message);
            }
            catch
            {
                // gulp
            }
        }
        public void LogError(string source, Exception ex)
        {
            try
            {
                Debug.WriteLine(ex.ToString());
                NativeApplication.Analytics?.LogError(ex, source);
            }
            catch
            {
                // gulp
            }
        }

        
    }
}
