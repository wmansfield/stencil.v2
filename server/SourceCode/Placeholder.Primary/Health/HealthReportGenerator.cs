using System;
using System.Collections.Generic;
using System.Net;
using Placeholder.Common;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Primary.Health
{
    public class HealthReportGenerator : ChokeableClass
    {
        public HealthReportGenerator(IFoundation foundation, List<string> logs, Dictionary<string, decimal> metrics)
            : base(foundation)
        {
            this.LastReset = DateTime.UtcNow;
            this.Logs = logs;
            this.Metrics = metrics;
            this.Interim = new Dictionary<string, decimal>();

        }

        protected internal virtual List<string> Logs { get; protected set; }

        protected internal virtual Dictionary<string, decimal> Metrics { get; set; }
        protected virtual Dictionary<string, decimal> Interim { get; set; }

        protected virtual DateTime LastReset { get; set; }

        private string _hostName;

        public virtual string GetHostName()
        {
            if (string.IsNullOrEmpty(_hostName))
            {
                _hostName = Dns.GetHostName();
            }
            return _hostName;
        }

        public virtual void UpdateMetricRaw(string trackName, int count, bool includeServer = true, DateTime? utcTimeStamp = null)
        {
            try
            {
                if (!utcTimeStamp.HasValue)
                {
                    utcTimeStamp = DateTime.UtcNow;
                }
                string prefix = "";
                if (includeServer)
                {
                    prefix = string.Format("{0}.", GetHostName());
                }
                this.Logs.Add(prefix + string.Format(HealthReporter.RAW_FORMAT, trackName, count, utcTimeStamp.Value.ToUnixSecondsUTC().ToString()));
            }
            catch (Exception ex)
            {
                base.IFoundation.LogError(ex, "UpdateMetricRaw");
            }
        }
        /// <summary>
        /// Tracks metrics to graphite
        /// </summary>
        /// <param name="type">The operation transform to apply</param>
        /// <param name="trackName">The name to track</param>
        /// <param name="milliseconds">Ignored if each or none</param>
        /// <param name="count">Ignored if each or none</param>
        public virtual void UpdateMetric(HealthTrackType type, string trackName, long milliseconds, int count)
        {
            try
            {
                if (type == HealthTrackType.None) { return; }

                if (type == HealthTrackType.Count || type == HealthTrackType.CountAndDurationAverage)
                {
                    string key = trackName + ".count";
                    if (!this.Metrics.ContainsKey(key))
                    {
                        this.Metrics[key] = 0;
                    }
                    this.Metrics[key] += count;
                }
                // must run after count to include current
                if (type == HealthTrackType.DurationAverage || type == HealthTrackType.CountAndDurationAverage)
                {
                    double minutesSinceLastReset = (DateTime.UtcNow - this.LastReset).TotalMinutes;
                    if (minutesSinceLastReset > 0) // jic, no zero divide [shouldnt happen]
                    {
                        string key = trackName + ".durationavg";
                        string totalKey = trackName + ".total";
                        string countkey = trackName + ".count";
                        decimal countSinceLast = 0;
                        decimal totalSinceLast = 0;
                        this.Metrics.TryGetValue(countkey, out countSinceLast);
                        this.Interim.TryGetValue(totalKey, out totalSinceLast);

                        countSinceLast++;
                        totalSinceLast += milliseconds;
                        this.Interim[totalKey] = totalSinceLast;
                        this.Metrics[key] = (totalSinceLast / countSinceLast);
                    }
                }
                if (type == HealthTrackType.Each)
                {
                    string prefix = string.Format("{0}.", GetHostName());
                    this.Logs.Add(prefix + string.Format(HealthReporter.EACH_FORMAT, trackName, DateTime.UtcNow.ToUnixSecondsUTC().ToString("###0")));
                }
            }
            catch (Exception ex)
            {
                base.IFoundation.LogError(ex, "UpdateMetric");
            }
        }
    }

}
