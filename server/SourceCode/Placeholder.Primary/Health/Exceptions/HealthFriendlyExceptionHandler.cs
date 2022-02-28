using System;
using Placeholder.Primary.Exceptions;
using Zero.Foundation;
using Zero.Foundation.System;

namespace Placeholder.Primary.Health.Exceptions
{
    public class HealthFriendlyExceptionHandler : FriendlyExceptionHandler
    {
        public HealthFriendlyExceptionHandler(IFoundation iFoundation, ILogger iLogger)
            : this(iFoundation, iLogger, string.Empty)
        {

        }
        public HealthFriendlyExceptionHandler(IFoundation iFoundation, ILogger iLogger, string policyName)
            : base(iFoundation, iLogger, policyName)
        {
        }
        protected override void LogException(Exception ex)
        {
            HealthReporter.LogException(ex);
            base.LogException(ex);
        }
    }
}
