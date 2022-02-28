using Zero.Foundation;
using Zero.Foundation.System;
using Zero.Foundation.System.Implementations;

namespace Placeholder.Primary.Health.Exceptions
{
    public class HealthSwallowExceptionHandlerProvider : SwallowExceptionHandlerProvider
    {
        public HealthSwallowExceptionHandlerProvider(IFoundation foundation, ILogger iLogger)
            : base(iLogger)
        {
        }

        public override IHandleException CreateHandler()
        {
            return new HealthSwallowExceptionHandler(this.ILogger);
        }
    }
}
