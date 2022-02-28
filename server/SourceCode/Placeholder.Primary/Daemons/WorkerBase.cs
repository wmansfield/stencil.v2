using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Zero.Foundation;
using Zero.Foundation.Daemons;
using Zero.Foundation.Daemons.Implementations;

namespace Placeholder.Primary.Daemons
{
    public abstract class WorkerBase<TRequest> : DaemonBase, IDaemonTask
    {
        #region Constructor

        public WorkerBase(IFoundation iFoundation, string daemonName)
            : base(iFoundation)
        {
            this.DaemonName = daemonName;
            this.RequestQueue = new ConcurrentQueue<TRequest>();
        }

        #endregion

        #region Static Methods

        protected static readonly object _RegistrationLock = new object();
        protected static readonly object _EnqueueLock = new object();

        /// <summary>
        /// Not Aspect Wrapped
        /// </summary>
        protected static TWorker EnqueueRequest<TWorker>(IFoundation foundation, string workerName, TRequest request, bool keepAlive, int millisecondInterval = 5000)
            where TWorker : WorkerBase<TRequest>
        {
            TWorker worker = EnsureWorker<TWorker>(foundation, workerName, keepAlive, millisecondInterval);
            worker.EnqueueRequest(request);
            return worker;
        }
        /// <summary>
        /// Not Aspect Wrapped
        /// </summary>
        protected static TWorker EnsureWorker<TWorker>(IFoundation foundation, string workerName, bool keepAlive, int millisecondInterval = 5000)
            where TWorker : WorkerBase<TRequest>
        {
            IDaemonManager daemonManager = foundation.GetDaemonManager();
            IDaemonTask daemonTask = daemonManager.GetRegisteredDaemonTask(workerName);
            if (daemonTask == null)
            {
                lock (_RegistrationLock)
                {
                    daemonTask = daemonManager.GetRegisteredDaemonTask(workerName);
                    if (daemonTask == null)
                    {
                        if (millisecondInterval <= 1000)
                        {
                            millisecondInterval = 5000; // if you give bad data, we force to 5 seconds.
                        }
                        DaemonConfig config = new DaemonConfig()
                        {
                            InstanceName = workerName,
                            ContinueOnError = true,
                            IntervalMilliSeconds = millisecondInterval,
                            StartDelayMilliSeconds = 0,
                            TaskConfiguration = string.Empty
                        };
                        TWorker worker = (TWorker)foundation.Container.Resolve(typeof(TWorker), null);
                        worker.DesiredIntervalMilliseconds = millisecondInterval;
                        worker.KeepAlive = keepAlive;
                        daemonManager.RegisterDaemon(config, worker, true);
                    }
                }
                daemonTask = daemonManager.GetRegisteredDaemonTask(workerName);
            }
            return daemonTask as TWorker;
        }

        #endregion

        #region Protected Properties

        protected virtual bool Executing { get; set; }

        protected virtual ConcurrentQueue<TRequest> RequestQueue { get; set; }

        protected virtual int DesiredIntervalMilliseconds { get; set; }
        protected virtual bool KeepAlive { get; set; }

        #endregion

        #region Public Methods

        public virtual void EnqueueRequest(TRequest request)
        {
            base.ExecuteMethod("EnqueueRequest", delegate ()
            {
                lock (_EnqueueLock)
                {
                    this.RequestQueue.Enqueue(request);
                }
                this.IFoundation.GetDaemonManager().StartDaemon(this.DaemonName); // agitate
            });
        }

        #endregion

        #region IDaemonTask Members

        public virtual DaemonSynchronizationPolicy SynchronizationPolicy
        {
            get { return DaemonSynchronizationPolicy.None; }
        }
        public virtual string DaemonName { get; private set; }
        public virtual void Execute(IFoundation iFoundation, CancellationToken cancellationToken)
        {
            if (this.Executing) { return; } // safety

            base.ExecuteMethod("Execute", delegate ()
            {
                try
                {
                    this.Executing = true;

                    this.ProcessRequests();

                    if (!this.KeepAlive)
                    {
                        IDaemon daemon = this.IFoundation.GetDaemonManager().LoadedDaemons.FirstOrDefault(x => x.InstanceName == this.DaemonName);
                        CoreDaemon coreDaemon = daemon as CoreDaemon; // insiders knowledge. :)
                        if (coreDaemon != null)
                        {
                            lock (_EnqueueLock)
                            {
                                if (this.RequestQueue.Count == 0)
                                {
                                    // let it die
                                    coreDaemon.IntervalMilliSeconds = -1; // return to ondemand
                                }
                                else
                                {
                                    // bring it back
                                    coreDaemon.IntervalMilliSeconds = this.DesiredIntervalMilliseconds;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    this.Executing = false;
                }
            });
        }

        #endregion

        #region Protected Methods

        protected abstract void ProcessRequest(TRequest request);

        protected virtual void ProcessRequests()
        {
            base.ExecuteMethod("ProcessRequests", delegate ()
            {
                TRequest request = default(TRequest);
                while (this.RequestQueue.TryDequeue(out request))
                {
                    try
                    {
                        this.ProcessRequest(request);
                    }
                    catch (Exception ex)
                    {
                        base.IFoundation.LogError(ex, "ProcessRequest");
                    }
                }
            });
        }



        #endregion
    }
}
