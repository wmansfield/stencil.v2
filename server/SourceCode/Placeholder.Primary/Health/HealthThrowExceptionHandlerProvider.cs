using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Foundation.System;

namespace Placeholder.Primary.Health
{
    public class HealthThrowExceptionHandlerProvider : IHandleException
    {
        public HealthThrowExceptionHandlerProvider(ILogger iLogger)
        {
            this.ILogger = iLogger;
        }

        public virtual string PolicyName { get; set; }
        protected virtual ILogger ILogger { get; set; }

        public virtual bool HandleException(Exception ex, out bool rethrowCurrent, out Exception replacedException)
        {
            this.LogException(ex);

            rethrowCurrent = true;
            replacedException = null;
            return true;
        }

        protected virtual void LogException(Exception ex)
        {
            HealthReporter.Current.TrackException(ex);
        }
    }
}
