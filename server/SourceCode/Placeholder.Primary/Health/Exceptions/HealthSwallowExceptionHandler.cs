using System;
using Zero.Foundation.System;
using Zero.Foundation.System.Implementations;

namespace Placeholder.Primary.Health.Exceptions
{
    public class HealthSwallowExceptionHandler : SwallowExceptionHandler
    {
        public HealthSwallowExceptionHandler(ILogger iLogger)
            : base(iLogger)
        {

        }

        protected override void LogException(Exception ex)
        {
            HealthReporter.LogException(ex);
            base.LogException(ex);
        }
    }
}
