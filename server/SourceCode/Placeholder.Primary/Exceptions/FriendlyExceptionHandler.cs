using System;
using System.Data.Common;
using System.Threading;
using Placeholder.Common.Exceptions;
using Zero.Foundation;
using Zero.Foundation.System;
using Zero.Foundation.System.Implementations;

namespace Placeholder.Primary.Exceptions
{
    public class FriendlyExceptionHandler : StandardThrowExceptionHandler
    {
        public FriendlyExceptionHandler(IFoundation iFoundation, ILogger iLogger)
            : this(iFoundation, iLogger, string.Empty)
        {

        }
        public FriendlyExceptionHandler(IFoundation iFoundation, ILogger iLogger, string policyName)
            : base(iLogger)
        {
            this.PolicyName = policyName;
            this.Foundation = iFoundation;
        }

        public IFoundation Foundation { get; set; }

        #region IHandleException Members

        public override bool HandleException(Exception ex, out bool rethrowCurrent, out Exception replacedException)
        {
            this.LogException(ex);

            replacedException = null;
            rethrowCurrent = true;

            if (ex is ThreadAbortException)
            {
                rethrowCurrent = false;// don't rethrow, aborting anyway
                return true;
            }
            if (ex is ThreadInterruptedException)
            {
                rethrowCurrent = false;// don't rethrow, aborting anyway
                return true;
            }
            if ((ex is HttpResponseException)
                || (ex is ServerException)
                || (ex is UIException))
            {
                return true; // pass through
            }

            if (ex is DbException)
            {
                replacedException = new ServerException("A server data error has occurred.");
                return true;
            }
            if (ex is ArgumentNullException)
            {
                replacedException = new ServerException("A server reference error was detected.");
                return true;
            }
            if (ex is NullReferenceException)
            {
                replacedException = new ServerException("A server reference error has occurred.");
                return true;
            }

            replacedException = new ServerException("A server error has occurred.");
            return true;
        }

        #endregion
    }
}
