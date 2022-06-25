using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Primary.Health;
using Zero.Foundation;
using Zero.Foundation.System;

namespace Placeholder.Web.Controllers
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    public abstract class HealthPlaceholderApiController : PlaceholderApiController
    {
        public HealthPlaceholderApiController(IFoundation foundation, string trackPrefix)
            : base(foundation)
        {
            this.TrackPrefix = trackPrefix;
        }
        public HealthPlaceholderApiController(IFoundation foundation, IHandleExceptionProvider iHandleExceptionProvider, string trackPrefix)
            : base(foundation, iHandleExceptionProvider)
        {
            this.TrackPrefix = trackPrefix;
        }

        public virtual string TrackPrefix { get; set; }

        #region Health Monitoring

        protected override void ExecuteMethod(string methodName, Action action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                base.ExecuteMethod(methodName, action, parameters);
            }
        }

        protected async override Task ExecuteMethodAsync(string methodName, Func<Task> action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                await base.ExecuteMethodAsync(methodName, action, parameters);
            }
        }
        protected override void ExecuteMethodThrowing(string methodName, Action action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                base.ExecuteMethodThrowing(methodName, action, parameters);
            }
        }
        protected async override Task ExecuteMethodThrowingAsync(string methodName, Func<Task> action, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                await base.ExecuteMethodThrowingAsync(methodName, action, parameters);
            }
        }

        protected override K ExecuteFunction<K>(string methodName, Func<K> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunction<K>(methodName, function, parameters);
            }
        }

        protected async override Task<T> ExecuteFunctionAsync<T>(string methodName, Func<Task<T>> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return await base.ExecuteFunctionAsync<T>(methodName, function, parameters);
            }
        }

        protected override K ExecuteFunctionThrowing<K>(string methodName, Func<K> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return base.ExecuteFunctionThrowing<K>(methodName, function, parameters);
            }
        }
        protected async override Task<T> ExecuteFunctionThrowingAsync<T>(string methodName, Func<Task<T>> function, params object[] parameters)
        {
            using (var scope = HealthReporter.BeginTrack(HealthTrackType.CountAndDurationAverage, string.Format(HealthReporter.RESTAPI_FORMAT, this.TrackPrefix + "." + methodName)))
            {
                return await base.ExecuteFunctionThrowingAsync<T>(methodName, function, parameters);
            }
        }

        #endregion
    }
}
