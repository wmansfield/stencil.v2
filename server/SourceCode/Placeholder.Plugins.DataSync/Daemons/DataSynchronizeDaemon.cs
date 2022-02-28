using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Placeholder.Primary.Business.Synchronization;
using Placeholder.Primary.Daemons;
using Placeholder.Primary.Health;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Daemons;
using Zero.Foundation.System;
using Zero.Foundation.Unity;

namespace Placeholder.Plugins.DataSync.Daemons
{
    public class DataSynchronizeDaemon : DaemonBase, IDaemonTask
    {
        #region Constructor

        public DataSynchronizeDaemon(IFoundation iFoundation, string agentName, string tenantCode)
            : base(iFoundation)
        {
            this.TenantCode = tenantCode;
            this.AgentName = agentName;
            this.Cache = new AspectCache("DataSynchronizeDaemon", iFoundation, new ExpireStaticLifetimeManager("DataSynchronizeDaemon.Life15", System.TimeSpan.FromMinutes(15), false));
        }

        #endregion

        #region Public Properties

        public AspectCache Cache { get; set; }

        /// <summary>
        /// 0: tenant
        /// 1: agent
        /// </summary>
        public const string DAEMON_NAME_TENANT_FORMAT = "DataSynchronizeDaemon_{0}_{1}";

        #endregion

        #region Properties

        protected static bool _executing;
        public string TenantCode { get; set; }
        public string AgentName { get; set; }

        #endregion

        #region IDaemonTask Members

        public DaemonSynchronizationPolicy SynchronizationPolicy
        {
            get { return DaemonSynchronizationPolicy.SingleAppDomain; }
        }
        public string DaemonName
        {
            get { return string.Format(DAEMON_NAME_TENANT_FORMAT, this.TenantCode, this.AgentName); }
        }


        public void Execute(IFoundation iFoundation, CancellationToken cancellationToken)
        {
            if (_executing) { return; } // safety

            base.ExecuteMethod("Execute", delegate ()
            {
                try
                {
                    _executing = true;
                    this.PerformProcessSync(string.Empty);
                }
                finally
                {
                    _executing = false;
                }
            });
        }

        #endregion

        #region Protected Methods

        protected virtual void PerformProcessSync(string specificTable)
        {
            base.ExecuteMethod("PerformProcessSync", delegate ()
            {
                IFindClassTypes finder = this.IFoundation.Resolve<IFindClassTypes>();
                IEnumerable<Type> synchronizers = FindInterfacesOfType(typeof(ISynchronizer), finder.GetAssemblies(null));
                List<ISynchronizer> synchronizersToRun = new List<ISynchronizer>();
                foreach (Type item in synchronizers)
                {
                    if (string.IsNullOrEmpty(specificTable) || item.Name.Contains(specificTable)) // not perfect.. but should be good enough, its just for dev ease anyway
                    {
                        base.IFoundation.LogTrace(string.Format("DataSynchronizeDaemon.{0} Loading", item.ToString()));
                        if (!item.IsGenericTypeDefinition && item != typeof(ISynchronizer))
                        {
                            try
                            {
                                ISynchronizer synchronizer = this.IFoundation.Container.Resolve(item, null) as ISynchronizer;
                                base.IFoundation.LogTrace(string.Format("DataSynchronizeDaemon.{0} Running", item.ToString()));
                                if (synchronizer != null)
                                {
                                    synchronizersToRun.Add(synchronizer);
                                }
                            }
                            catch
                            {
                                // gulp, can't resolve
                            }
                        }
                        else
                        {
                            base.IFoundation.LogTrace("DataSynchronizeDaemon: " + item.ToString() + "is a generic or the base interface");
                        }
                    }
                }

                // order them
                synchronizersToRun = synchronizersToRun.OrderBy(x => x.Priority).ToList();

                // process them by bulk-priority
                while (synchronizersToRun.Count > 0)
                {
                    int priority = synchronizersToRun[0].Priority;
                    List<ISynchronizer> itemsWithPriority = synchronizersToRun.Where(x => x.Priority == priority).ToList();
                    List<Task> tasks = new List<Task>();

                    foreach (var synchronizer in itemsWithPriority)
                    {
                        synchronizersToRun.Remove(synchronizer);

                        tasks.Add(Task.Run(delegate ()
                        {
                            try
                            {
                                using (var scope = HealthReporter.BeginTrack(HealthTrackType.DurationAverage, string.Format(HealthReporter.INDEXER_QUEUE_TIME_FORMAT, synchronizer.EntityName)))
                                {
#pragma warning disable 612, 618
                                    int count = synchronizer.PerformSynchronization(this.AgentName, this.TenantCode);
#pragma warning restore 612, 618
                                    if (count > 0)
                                    {
                                        HealthReporter.Current.UpdateMetric(HealthTrackType.Count, string.Format(HealthReporter.INDEXER_QUEUE_SIZE_FORMAT, synchronizer.EntityName), 0, count);
                                    }
                                }

                                base.IFoundation.LogWarning(string.Format("DataSynchronizeDaemon.{0} Complete", synchronizer.ToString()));
                            }
                            catch (Exception ex)
                            {
                                base.IFoundation.LogError(ex, "PerformProcessSync" + synchronizer.GetType().ToString());
                                base.IFoundation.LogWarning(string.Format("DataSynchronizeDaemon.{0} Error", synchronizer.ToString()));
                            }
                        }));
                    }

                    base.IFoundation.LogWarning(string.Format("DataSynchronizeDaemon.Waiting"));
                    Task.WaitAll(tasks.ToArray());
                    base.IFoundation.LogWarning(string.Format("DataSynchronizeDaemon.Done"));
                }
            });
        }

        public IEnumerable<Type> FindInterfacesOfType(Type assignTypeFrom, IEnumerable<System.Reflection.Assembly> assemblies)
        {
            foreach (var a in assemblies)
            {
                Type[] types = null;
                try
                {
                    types = a.GetTypes();
                }
                catch
                {
                    // ah well, we're looking for valid items.
                }
                if (types != null)
                {
                    foreach (var t in types)
                    {
                        if (assignTypeFrom.IsAssignableFrom(t))
                        {
                            if (t.IsInterface)
                            {
                                yield return t;
                            }
                        }
                    }
                }
            }
        }


        #endregion
    }
}
