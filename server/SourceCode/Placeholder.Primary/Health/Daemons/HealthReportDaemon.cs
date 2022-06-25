using System;
using System.Collections.Generic;
using System.Threading;
using Placeholder.Common;
using Placeholder.Common.Configuration;
using Placeholder.Primary.Daemons;
using Zero.Foundation;
using Zero.Foundation.Daemons;

namespace Placeholder.Primary.Health.Daemons
{
    public class HealthReportDaemon : DaemonBase, IDaemonTask
    {
        public HealthReportDaemon(IFoundation iFoundation)
            : base(iFoundation)
        {
        }

        #region IDaemonTask Members

        public const string DAEMON_NAME = "HealthReportDaemon";



        /// <summary>
        /// hostName, metric, value, time
        /// </summary>
        public static string GRAPHITE_FORMAT = "{0}.{1} {2} {3}";


        protected static bool _executing;

        public string DaemonName
        {
            get
            {
                return DAEMON_NAME;
            }
            protected set
            {
            }
        }

        public void Execute(IFoundation iFoundation, CancellationToken cancellationToken)
        {
            base.ExecuteMethod("Execute", delegate()
            {
                if (_executing) { return; } // safety

                try
                {
                    _executing = true;
                    this.PersistHealth();
                }
                finally
                {
                    _executing = false;
                }
            });
        }

        public DaemonSynchronizationPolicy SynchronizationPolicy
        {
            get { return DaemonSynchronizationPolicy.SingleAppDomain; }
        }

        #endregion

        protected void PersistHealth()
        {
            base.ExecuteMethod("PersistHealth", delegate()
            {
                base.IFoundation.LogTrace("Sending Health Reports");

                string hostName = HealthReporter.Current.GetHostName();
                Dictionary<string, decimal> metrics = null;
                List<string> logs = null;
                HealthReporter.Current.ResetMetrics(out metrics, out logs);

                string suffix = DateTime.UtcNow.ToUnixSecondsUTC().ToString();
                foreach (var item in metrics)
                {
                    logs.Add(string.Format(GRAPHITE_FORMAT, hostName, item.Key, (int)item.Value, suffix));
                }

                ISettingsResolver settingsResolver = this.IFoundation.Resolve<ISettingsResolver>();

                if (settingsResolver.IsDebugHealth() || !settingsResolver.IsLocalHost())
                {
                    string apiKey = this.ApiKey;
                    string apiHost = this.ApiHost;
                    if(!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(apiHost))
                    {
                        HostedGraphiteClient client = this.AcquireGraphiteClient(apiKey, apiHost);
                        client.SendMany(logs, true);
                    }
                }
            });
        }

        private HostedGraphiteClient _graphiteClient;
        private DateTime? _graphiteExpiration;

        private HostedGraphiteClient AcquireGraphiteClient(string apiKey, string host)
        {
            if(!_graphiteExpiration.HasValue || _graphiteExpiration.Value < DateTime.UtcNow)
            {
                try
                {
                    if (_graphiteClient != null)
                    {
                        _graphiteClient.Dispose();
                    }
                }
                catch
                {
                    // gulp
                }
                _graphiteClient = null;
            }
            if(_graphiteClient == null)
            {
                _graphiteClient = new HostedGraphiteClient(apiKey, host);
                _graphiteExpiration = DateTime.UtcNow.AddMinutes(5);
            }
            return _graphiteClient;
        }

        protected virtual string ApiKey
        {
            get
            {
                ISettingsResolver settings = this.IFoundation.Resolve<ISettingsResolver>();
                return settings.GetSetting(CommonAssumptions.APP_KEY_HEALTH_APIKEY);
            }
        }
        protected virtual string ApiHost
        {
            get
            {
                ISettingsResolver settings = this.IFoundation.Resolve<ISettingsResolver>();
                return settings.GetSetting(CommonAssumptions.APP_KEY_HEALTH_HOST);
            }
        }
    }
}
