using System;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.System;

namespace Placeholder.Primary.Daemons
{
    public abstract class DaemonBase : ChokeableClass, IDisposable
    {
        #region Constructor

        public DaemonBase(IFoundation iFoundation)
            : base(iFoundation)
        {
        }
        public DaemonBase(IFoundation iFoundation, IHandleExceptionProvider handleExceptionProvider)
            : base(iFoundation, handleExceptionProvider)
        {
        }

        #endregion

        #region IDisposable Support

        protected bool HasDisposed { get; set; } // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.HasDisposed)
            {
                if (disposing)
                {

                }

                this.HasDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
