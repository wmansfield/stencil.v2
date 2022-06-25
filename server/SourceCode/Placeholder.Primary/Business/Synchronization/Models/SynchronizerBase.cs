using System;
using System.Threading;
using System.Threading.Tasks;
using Placeholder.Common;
using Placeholder.Common.Synchronization;
using Placeholder.Primary.Health;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.System;

namespace Placeholder.Primary.Business.Synchronization
{
    public abstract class SynchronizerBase<TPkey> : ChokeableClass, ISynchronizer
    {
        #region Constructor

        /// <summary>
        /// Creates a SynchronizerBase
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="entityName">Used to notify health system which entity this synchronizer references</param>
        public SynchronizerBase(IFoundation foundation, string entityName, int synchronousTimeoutMilliseconds = 5000)
            : base(foundation, foundation.Resolve<IHandleExceptionProvider>(CommonAssumptions.SWALLOWED_EXCEPTION_HANDLER))
        {
            this.EntityName = entityName;
            this.SynchronousTimeoutMilliseconds = synchronousTimeoutMilliseconds;
            this.SynchronousCriticalTimeoutMilliseconds = CRITICAL_SYNC_TIMEOUT_MILLISECONDS;
            this.API = foundation.Resolve<PlaceholderAPI>();
        }

        #endregion

        #region Constants

        public const int CRITICAL_SYNC_TIMEOUT_MILLISECONDS = 20000;
        public const int MILLISECONDS_FOR_REFRESH = 1000;

        #endregion

        #region Protected Properties
        public PlaceholderAPI API { get; protected set; }
        #endregion



        #region Public Properties

        public virtual int SynchronousCriticalTimeoutMilliseconds { get; set; }
        public virtual int SynchronousTimeoutMilliseconds { get; set; }
        public virtual int MillisecondsForRefresh 
        { 
            get
            {
                return MILLISECONDS_FOR_REFRESH;
            }
        }

        public virtual string EntityName { get; set; }

        public virtual string AgentName { get; set; }

        /// <summary>
        /// Defaults to 50
        /// </summary>
        public virtual int Priority
        {
            get
            {
                return 50;
            }
        }

        #endregion

        #region Public Methods

        public void WaitForRefreshManual()
        {
            Task.Delay(MILLISECONDS_FOR_REFRESH).Wait();
        }


        [Obsolete("You should only override this to disable the functionality", false)]
        public virtual void SynchronizeItem(TPkey primaryKey, Availability availability)
        {
            switch (availability)
            {
                case Availability.None:
                    this.AttemptExecuteWithinTimeout(false, 0, "SynchronizeItem", delegate ()
                    {
                        PerformSynchronizationForItem(primaryKey);
                    });
                    break;
                case Availability.Default:
                    this.AttemptExecuteWithinTimeout(false, SynchronousTimeoutMilliseconds, "SynchronizeItem", delegate ()
                    {
                        PerformSynchronizationForItem(primaryKey);
                    });
                    break;
                case Availability.Retrievable:
                    this.AttemptExecuteWithinTimeout(false, SynchronousCriticalTimeoutMilliseconds, "SynchronizeItem", delegate ()
                    {
                        PerformSynchronizationForItem(primaryKey);
                    });
                    break;
                case Availability.Searchable:
                    this.AttemptExecuteWithinTimeout(true, SynchronousCriticalTimeoutMilliseconds, "SynchronizeItem", delegate ()
                    {
                        PerformSynchronizationForItem(primaryKey);
                    });
                    break;
                default:
                    break;
            }
        }

        public abstract int PerformSynchronization(string requestedAgentName, string tenant_code);
        public abstract void PerformSynchronizationForItem(TPkey primaryKey);

        #endregion

        #region Protected Methods


        protected virtual void AttemptExecuteWithinTimeout(bool waitForRefreshInterval, int customMillisecondTimeout, string methodName, Action action)
        {
            base.ExecuteMethod(methodName, delegate ()
            {
                if (customMillisecondTimeout <= 0)
                {
                    // run in new task without care, we dont wanna wait [only used in debug really]
                    Task.Run(action);
                    return;
                }
                try
                {
                    using (CancellationTokenSource tokenSource = new CancellationTokenSource())
                    {
                        CancellationToken token = tokenSource.Token;

                        Task task = Task.Run(action, token);

                        tokenSource.CancelAfter(customMillisecondTimeout);

                        task.Wait(token);

                        token.ThrowIfCancellationRequested();

                        if (waitForRefreshInterval && this.MillisecondsForRefresh > 0)
                        {
                            Task.Delay(this.MillisecondsForRefresh).Wait();
                        }
                    }
                }
                catch (OperationCanceledException cex)
                {
                    HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_INSTANT_FAIL_TIMEOUT_FORMAT, this.EntityName), 0, 1);

                    this.IFoundation.LogError(cex, "AttemptExecuteWithinTimeout:" + methodName);
                }
                catch (Exception ex)
                {
                    HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_INSTANT_FAIL_ERROR_FORMAT, this.EntityName), 0, 1);

                    this.IFoundation.LogError(ex, "AttemptExecuteWithinTimeout:" + methodName);
                }

            });
        }


        #endregion
    }
}
