using System;
using Zero.Foundation;
using Zero.Foundation.System;

namespace Placeholder.Primary.Exceptions
{
    public class FriendlyExceptionHandlerProvider : IHandleExceptionProvider
    {
        public FriendlyExceptionHandlerProvider(IFoundation foundation, ILogger iLogger)
        {
            this.ILogger = iLogger;
            this.Foundation = foundation;
        }
        protected virtual ILogger ILogger { get; set; }
        protected virtual IFoundation Foundation { get; set; }

        #region IHandleExceptionProvider Members

        public virtual string PolicyName { get; set; }

        public virtual IHandleException CreateHandler()
        {
            return new FriendlyExceptionHandler(this.Foundation, this.ILogger, this.PolicyName);
        }

        #endregion
    }
}
