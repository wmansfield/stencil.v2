using System;
using Placeholder.Primary.Health;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Primary.Business.Direct
{
    /// <summary>
    /// Should only be inherited from BusinessBase
    /// </summary>
    public abstract class BusinessBaseHealth : ChokeableClass
    {
        public BusinessBaseHealth(IFoundation foundation, string trackPrefix)
            : base(foundation)
        {
            this.TrackPrefix = trackPrefix;
        }

        public virtual string TrackPrefix { get; set; }

        #region Health Monitoring

      
        protected override void ExecuteMethod(string methodName, Action action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.BUSINESS_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                base.ExecuteMethod(methodName, action, parameters);
            }
        }
        protected override K ExecuteFunction<K>(string methodName, Func<K> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.BUSINESS_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, function, parameters);
            }
        }
        protected virtual void ExecuteMethod(HealthTrackType type, string methodName, Action action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(type, string.Format(HealthReporter.BUSINESS_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                base.ExecuteMethod(methodName, action, parameters);
            }
        }
        protected virtual K ExecuteFunction<K>(HealthTrackType type, string methodName, Func<K> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(type, string.Format(HealthReporter.BUSINESS_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, function, parameters);
            }
        }

        #endregion
    }
}
