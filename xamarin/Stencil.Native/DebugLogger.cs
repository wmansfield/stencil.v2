using System;
using System.Diagnostics;

namespace Stencil.Native
{
    public class DebugLogger : ILogger
    {
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
