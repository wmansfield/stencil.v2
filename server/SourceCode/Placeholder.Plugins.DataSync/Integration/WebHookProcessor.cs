using System;
using Placeholder.Plugins.DataSync.Daemons;
using Placeholder.Primary.Daemons;
using Zero.Foundation;
using Zero.Foundation.Daemons;

namespace Placeholder.Plugins.DataSync.Integration
{
    public static class WebHookProcessor
    {
        /// <summary>
        /// Not aspect wrapped
        /// </summary>
        public static string ProcessSyncWebHook(IFoundation foundation, string secretkey, string hookType, string tenant)
        {
            string result = "";
            if (secretkey == "codeable")
            {
                IDaemonManager daemonManager = foundation.GetDaemonManager();

                switch (hookType)
                {
                    case "sync":
                    case "failed":
                        daemonManager.StartDaemon(string.Format(DataSynchronizeDaemon.DAEMON_NAME_TENANT_FORMAT, tenant, Agents.AGENT_DEFAULT));
                        daemonManager.StartDaemon(string.Format(DataSynchronizeDaemon.DAEMON_NAME_TENANT_FORMAT, tenant, Agents.AGENT_STATS));
                        result = "Queued Normal Sync";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Not aspect wrapped
        /// </summary>
        public static string ProcessAgitateWebHook(IFoundation foundation, string secretkey, string daemonName)
        {
            string result = "";
            if (secretkey == "codeable")
            {
                IDaemonManager daemonManager = foundation.GetDaemonManager();
                if (null != daemonManager.GetRegisteredDaemonTask(daemonName))
                {
                    daemonManager.StartDaemon(daemonName);
                    result = "Agitated";
                }
            }
            return result;
        }
    }
}
