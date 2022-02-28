using System;
using Placeholder.Common;
using Placeholder.Primary.Health;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Primary.Business.Store
{
    /// <summary>
    /// Should only be inherited from StoreBase
    /// </summary>
    public abstract class StoreBaseHealth : ChokeableClass
    {
        public StoreBaseHealth(IFoundation foundation, string trackPrefix)
            : base(foundation)
        {
            this.TrackPrefix = trackPrefix;
        }

        public virtual string TrackPrefix { get; set; }

        #region Health Monitoring

        protected override void ExecuteMethod(string methodName, Action action, params object[] parameters)
        {
            CommonUtility.SetLatestMethodName(this.TrackPrefix, methodName);
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.COSMOS_READ_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                base.ExecuteMethod(methodName, action, parameters);
            }
        }
        protected void ExecuteMethodInternal(string methodName, Action action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.COSMOS_READ_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                base.ExecuteMethod(methodName, action, parameters);
            }
        }
        protected override K ExecuteFunction<K>(string methodName, Func<K> function, params object[] parameters)
        {
            CommonUtility.SetLatestMethodName(this.TrackPrefix, methodName);
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.COSMOS_READ_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, function, parameters);
            }
        }
        protected K ExecuteFunctionInternal<K>(string methodName, Func<K> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.COSMOS_READ_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, function, parameters);
            }
        }
        protected virtual K ExecuteFunctionWrite<K>(string methodName, Func<K> function, params object[] parameters)
        {
            CommonUtility.SetLatestMethodName(this.TrackPrefix, methodName);
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.COSMOS_WRITE_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, false, function, parameters);
            }
        }
        protected virtual K ExecuteFunctionWriteInternal<K>(string methodName, Func<K> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.COSMOS_WRITE_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, false, function, parameters);
            }
        }
        protected virtual K ExecuteFunctionByPassHealth<K>(string methodName, Func<K> function, params object[] parameters)
        {
            return base.ExecuteFunction<K>(methodName, function, parameters);
        }
        protected virtual void ExecuteMethodByPassHealth(string methodName, Action action, params object[] parameters)
        {
            base.ExecuteMethod(methodName, action, parameters);
        }

        #endregion

    }
}
