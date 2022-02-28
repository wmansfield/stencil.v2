using System;
using System.Collections.Generic;
using System.Reflection;
using Placeholder.Common;
using Placeholder.Common.Configuration;
using Placeholder.Common.Synchronization;
using Placeholder.Domain;
using Placeholder.Plugins.DataSync.Integration;
using Placeholder.Primary;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Daemons;
using Zero.Foundation.Plugins;
using Unity;
using Placeholder.Plugins.DataSync.Daemons;
using Placeholder.Primary.Daemons;

namespace Placeholder.Plugins.DataSync
{
    public class DataSyncPlugin : ChokeableClass, IWebPlugin
    {
        public DataSyncPlugin()
            : base(CoreFoundation.Current)
        {
        }
        #region IPlugin Members

        public string DisplayName
        {
            get { return "DataSync"; }
        }
        public string DisplayVersion
        {
            get { return FoundationUtility.GetInformationalVersion(Assembly.GetExecutingAssembly()); }
        }

        public bool Construct(IFoundation foundation)
        {
            base.IFoundation = foundation;
            return true;
        }

        #endregion

        #region IWebPlugin Members


        public void Initialize()
        {
            base.ExecuteMethod("Initialize", delegate ()
            {
                this.IFoundation.Container.RegisterInstance<INotifySynchronizer>(new DataSyncNotifySynchronizer(this.IFoundation));

                IDaemonManager daemonManager = this.IFoundation.GetDaemonManager();
                ISettingsResolver settingsResolver = this.IFoundation.Resolve<ISettingsResolver>();
                bool isBackPane = settingsResolver.IsBackPane();
                bool isHydrate = settingsResolver.IsHydrate();
                bool isLocalHost = settingsResolver.IsLocalHost();

                PlaceholderAPI API = this.IFoundation.Resolve<PlaceholderAPI>();

                List<Tenant> tenants = API.Direct.Tenants.Find(0, int.MaxValue);

                if (isBackPane)
                {
                    foreach (Tenant item in tenants)
                    {
                        DaemonConfig config = new DaemonConfig()
                        {
                            InstanceName = string.Format(DataSynchronizeDaemon.DAEMON_NAME_TENANT_FORMAT, item.tenant_code, Agents.AGENT_DEFAULT),
                            ContinueOnError = true,
                            IntervalMilliSeconds = (int)TimeSpan.FromSeconds(30).TotalMilliseconds,
                            StartDelayMilliSeconds = 15,
                            TaskConfiguration = string.Empty
                        };
                        daemonManager.RegisterDaemon(config, new DataSynchronizeDaemon(this.IFoundation, Agents.AGENT_DEFAULT, item.tenant_code), true);

                        config = new DaemonConfig()
                        {
                            InstanceName = string.Format(DataSynchronizeDaemon.DAEMON_NAME_TENANT_FORMAT, item.tenant_code, Agents.AGENT_STATS),
                            ContinueOnError = true,
                            IntervalMilliSeconds = (int)TimeSpan.FromSeconds(30).TotalMilliseconds,
                            StartDelayMilliSeconds = 25,
                            TaskConfiguration = string.Empty
                        };
                        daemonManager.RegisterDaemon(config, new DataSynchronizeDaemon(this.IFoundation, Agents.AGENT_STATS, item.tenant_code), true);

                    }

                }
                else if(isLocalHost)
                {
                    foreach (Tenant item in tenants)
                    {
                        DaemonConfig config = new DaemonConfig()
                        {
                            InstanceName = string.Format(DataSynchronizeDaemon.DAEMON_NAME_TENANT_FORMAT, item.tenant_code, Agents.AGENT_DEFAULT),
                            ContinueOnError = false,
                            IntervalMilliSeconds = 0,
                            StartDelayMilliSeconds = 0,
                            TaskConfiguration = string.Empty
                        };
                        daemonManager.RegisterDaemon(config, new DataSynchronizeDaemon(this.IFoundation, Agents.AGENT_DEFAULT, item.tenant_code), false);

                    }
                }
            });
        }

        public int DesiredInitializionPriority
        {
            get { return 0; }
        }

        public void OnWebPluginInitialized(IWebPlugin plugin)
        {
        }

        public void OnAllWebPluginsInitialized(IEnumerable<IWebPlugin> allWebPlugins)
        {
        }

        public T RetrieveMetaData<T>(string token)
        {
            return default(T);
        }

        public object InvokeCommand(string name, Dictionary<string, object> caseInsensitiveParameters)
        {
            return null;
        }

        #endregion
    }
}
