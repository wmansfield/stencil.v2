using Placeholder.Primary.Exceptions;
using Zero.Foundation;
using Zero.Foundation.System;

namespace Placeholder.Primary.Health.Exceptions
{
    public class HealthFriendlyExceptionHandlerProvider : FriendlyExceptionHandlerProvider
    {
        public HealthFriendlyExceptionHandlerProvider(IFoundation foundation, ILogger iLogger)
            : base(foundation, iLogger)
        {
        }

        #region IHandleExceptionProvider Members

        public override IHandleException CreateHandler()
        {
            return new HealthFriendlyExceptionHandler(this.Foundation, this.ILogger, this.PolicyName);
        }

        #endregion
    }
}
