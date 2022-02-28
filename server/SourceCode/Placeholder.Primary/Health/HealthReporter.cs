using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Placeholder.Common;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;

namespace Placeholder.Primary.Health
{
    /// <summary>
    /// DO NOT CHOKE! Could cause endless loop
    /// </summary>
    public class HealthReporter : ChokeableClass
    {
        #region Statics

        protected static readonly object SyncLock = new object();

        private static HealthReporter _current;
        public static HealthReporter Current
        {
            get
            {
                if (_current == null)
                {
                    lock (SyncLock)
                    {
                        if (_current == null)
                        {
                            _current = new HealthReporter(CoreFoundation.Current);
                        }
                    }
                }
                return _current;
            }
            protected set
            {
                _current = value;
            }
        }

        public static IDisposable BeginTrack(HealthTrackType type, string trackName, int count = 1)
        {
            return new HealthTrackScope(type, trackName, count);
        }
        public static void LogException(Exception ex)
        {
            HealthReporter.Current.TrackException(ex);
        }
        public static string CleanNode(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return name.Replace(" ", "").Replace(".", "_");
            }
            return name;
        }


        public static bool TRACK_HOST_NAMES = false;
        public static string DEFAULT_HOST_NAME = "server";

        private static string _hostName;
        public static string GetHostName()
        {
            if (!TRACK_HOST_NAMES)
            {
                return DEFAULT_HOST_NAME;
            }
            if (string.IsNullOrEmpty(_hostName))
            {
                _hostName = Dns.GetHostName();
            }
            return _hostName;
        }
        #endregion

        #region Constants

        public const string COSMOS_RU_FORMAT = "data.cosmos.ru.{0}";
        public const string RESTAPI_FORMAT = "rest.controller.{0}";
        public const string MVCAPI_FORMAT = "mvc.controller.{0}";
        public const string BUSINESS_FORMAT = "data.business.{0}";
        public const string CACHE_READ_FORMAT = "data.cache.read.{0}";
        public const string CACHE_WRITE_FORMAT = "data.cache.write.{0}";
        public const string ERROR_FORMAT = "error.{0} 1 {1:N0}";
        public const string EACH_FORMAT = "{0} 1 {1:N0}";
        public const string RAW_FORMAT = "{0} {1} {2:N0}";
        /// <summary>
        /// 0: email type
        /// 1: queued|sent|failed
        /// </summary>
        public const string EMAIL_FORMAT = "email.{0}.count";



        public const string VIDEO_TRANSCODE_QUEUE_SUCCESS = "video.queued.success";
        public const string VIDEO_TRANSCODE_QUEUE_FAILED = "video.queued.fail";
        public const string VIDEO_TRANSCODE_COMPLETE_SUCCESS = "video.complete.success";
        public const string VIDEO_TRANSCODE_COMPLETE_FAILED = "video.complete.fail";

        public const string PHOTO_RESIZE_SUCCESS = "photo.resize.success";
        public const string PHOTO_RESIZE_FAILED = "photo.resize.fail";

        public const string LEADERBOARD_CHECK_FAILED = "leaderboard.check.fail";

        public const string INDEXER_INVALID_DEPENDENCY_DATA = "data.indexer.{0}.sync.missingdata";
        public const string INDEXER_ERROR_SYNC = "data.indexer.{0}.sync.error";
        public const string INDEXER_QUEUE_TIME_FORMAT = "data.indexer.{0}.queue.duration";
        public const string INDEXER_QUEUE_SIZE_FORMAT = "data.indexer.{0}.queue.size";
        public const string INDEXER_INSTANT_FAIL_TIMEOUT_FORMAT = "data.indexer.{0}.instant.fail.timeout";
        public const string INDEXER_INSTANT_FAIL_ERROR_FORMAT = "data.indexer.{0}.instant.fail.error";
        public const string INDEXER_INSTANT_FAIL_SOFT_FORMAT = "data.indexer.{0}.instant.fail.soft.nosuccess";
        public const string INDEXER_INSTANT_FAIL_SOFT_CONFLICT_FORMAT = "data.indexer.{0}.instant.fail.soft.conflict";

        public const string COSMOS_READ_FORMAT = "data.cosmos.read.{0}";
        public const string COSMOS_WRITE_FORMAT = "data.cosmos.write.{0}";

        public const string SOCIAL_OAUTH_EXCHANGE_SUCCESS_FORMAT = "social.{0}.exchange.success";
        public const string SOCIAL_OAUTH_EXCHANGE_FAIL_FORMAT = "social.{0}.exchange.success";
        public const string SOCIAL_REQUEST_FORMAT = "social.{0}.request";
        public const string SOCIAL_POST_FAIL = "social.{0}.post.fail";
        public const string SOCIAL_POST_SUCCESS = "social.{0}.post.success";


        /// <summary>
        /// 0: sent|failed
        /// </summary>
        public const string PUSH_FORMAT = "push.{0}.count";

        /// <summary>
        /// 0: mobile|desktop, 1: username
        /// </summary>
        public const string AUTH_PASSWORD_SUCCESS_FORMAT = "accounts.auth.{0}.password.success.{1}";
        /// <summary>
        /// 0: mobile|desktop, 1: username
        /// </summary>
        public const string AUTH_PASSWORD_FAIL_FORMAT = "accounts.auth.{0}.password.failed.{1}";
        /// <summary>
        /// 0: mobile|desktop, 1: username
        /// </summary>
        public const string AUTH_TOKEN_FAIL_FORMAT = "accounts.auth.{0}.token.failed.{1}";
        /// <summary>
        /// 0: mobile|desktop, 1: username
        /// </summary>
        public const string AUTH_DAILY_USER_FORMAT = "accounts.auth.{0}.daily.{1}";

        /// <summary>
        /// 0: mobile|desktop|any
        /// </summary>
        public const string AUTH_CURRENT_USER_FORMAT = "accounts.auth.{0}.current";


        /// <summary>
        /// 0: memory type
        /// </summary>
        public const string SERVER_MEMORY_SIZE = "server.memory.{0}.size";

        #endregion

        #region Constructors

        public HealthReporter(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.Extractors = new HashSet<IHealthExtractor>();
            this.Generator = new HealthReportGenerator(iFoundation, new List<string>(), new Dictionary<string, decimal>());
            this.ExclusionCache = new AspectCache("HealthReportStaticCache", iFoundation, new ExpireStaticLifetimeManager("HealthReportStaticCacheLifeTime", TimeSpan.FromMinutes(15), false));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 15 minute static lifetime
        /// </summary>
        public virtual AspectCache ExclusionCache { get; set; }
        public virtual HealthReportGenerator Generator { get; set; }

        public virtual HashSet<IHealthExtractor> Extractors { get; set; }
        
        #endregion

        #region Public Methods


        [Obsolete("Architect: This should use weak referencing or have another way to verify existence", false)]
        public virtual void AddExtractor(IHealthExtractor extractor)
        {
            base.ExecuteMethod("AddExtractor", delegate()
            {
                lock (SyncLock)
                {
                    this.Extractors.Add(extractor);
                }
            });
        }
        public virtual void RemoveExtractor(IHealthExtractor extractor)
        {
            base.ExecuteMethod("RemoveExtractor", delegate()
            {
                lock (SyncLock)
                {
                    this.Extractors.Remove(extractor);
                }
            });
        }
        public virtual void UpdateMetricEach(string trackName)
        {
            UpdateMetric(HealthTrackType.Each, trackName, 0, 1);
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

                lock (SyncLock)
                {
                    this.Generator.UpdateMetric(type, trackName, milliseconds, count);
                }
            }
            catch (Exception ex)
            {
                base.IFoundation.LogError(ex, "UpdateMetric");
            }
        }
        public virtual void UpdateMetricRaw(string trackName, int count, bool includeServer = true, DateTime? utcTimeStamp = null)
        {
            try
            {
                lock (SyncLock)
                {
                    this.Generator.UpdateMetricRaw(trackName, count, includeServer, utcTimeStamp);
                }
            }
            catch (Exception ex)
            {
                base.IFoundation.LogError(ex, "UpdateMetricRaw");
            }
        }


        public virtual void TrackException(Exception ex)
        {
            // Don't choke, circular
            try
            {
                if (ex is ThreadAbortException)
                {
                    return;
                }
                 if (ex is ThreadInterruptedException)
                {
                    return;
                }
                if (ex is AggregateException)
                {
                    AggregateException aex = ex as AggregateException;
                    foreach (var item in aex.InnerExceptions)
                    {
                        TrackException(item);
                    }
                    return;
                }
                string exceptionType = ex.GetType().FriendlyName().ToLower();

                List<string> excludedExceptionTypes = GetExcludedExceptionTypes();
                if (excludedExceptionTypes != null)
                {
                    if (excludedExceptionTypes.Contains(exceptionType))
                    {
                        return;
                    }
                }

                List<string> excludedExceptionMessages = GetExcludedMessageTypes();
                if (excludedExceptionMessages != null)
                {
                    foreach (var message in excludedExceptionMessages)
                    {
                        if (ex.Message.ToLower().Contains(message))
                        {
                            return;
                        }
                    }
                }
                DateTime stamp = DateTime.UtcNow;

                lock (SyncLock)
                {
                    try
                    {
                        string prefix = string.Format("{0}.", this.Generator.GetHostName());
                        this.Generator.Logs.Add(prefix + string.Format(ERROR_FORMAT, exceptionType.ToLower(), stamp.ToUnixSecondsUTC().ToString("###0")));
                    }
                    catch
                    {
                        // gulp
                    }

                    string fileName = Path.Combine(Path.GetTempPath(), @"PlaceholderErrorTracking\Error.Log");
                    if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    }
                    System.IO.File.AppendAllText(fileName, FoundationUtility.FormatException(ex));
                }
            }
            catch (Exception)
            {
                // gulp
            }
        }

        public virtual void ResetMetrics(out Dictionary<string, decimal> metrics, out List<string> logs)
        {
            metrics = null;
            logs = null;
            try
            {
                lock (SyncLock)
                {
                    metrics = this.Generator.Metrics;
                    logs = this.Generator.Logs;

                    HealthReportGenerator extractGenerator = new HealthReportGenerator(this.IFoundation, logs, metrics);
                    foreach (var item in this.Extractors)
                    {
                        try
                        {
                            item.ExtractHealthMetrics(extractGenerator);
                        }
                        catch
                        {
                            //gulp
                        }
                    }

                    this.Generator = new HealthReportGenerator(this.IFoundation, new List<string>(), new Dictionary<string, decimal>());
                }
            }
            catch (Exception ex)
            {
                base.IFoundation.LogError(ex, "ResetMetrics");
            }
        }
        #endregion

        #region Protected Methods
        protected virtual List<string> GetExcludedExceptionTypes()
        {
            return this.ExclusionCache.PerLifetime("GetExcludedExceptionTypes", delegate()
            {
                //TODO:SHOULD:Health: Excluded Exception
                return new List<string>();
            });
        }
        protected virtual List<string> GetExcludedMessageTypes()
        {
            return this.ExclusionCache.PerLifetime("GetExcludedMessageTypes", delegate()
            {
                //TODO:SHOULD:Health: Excluded Exception
                return new List<string>();
            });
        }
        #endregion

    }
}
