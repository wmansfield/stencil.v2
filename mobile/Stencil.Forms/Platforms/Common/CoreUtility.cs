using System;
using System.Threading.Tasks;

namespace Stencil.Forms
{
    public static class CoreUtility
    {
        [Obsolete("Incorrect api call, use the Async Version of this method", true)]
        public static void ExecuteMethod(string name, Func<Task> method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {

        }
        public static void ExecuteMethod(string name, Action method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {
            try
            {
#if DEBUG
                DateTime start = DateTime.UtcNow;
#endif
                method();
#if DEBUG
                DateTime end = DateTime.UtcNow;
                if (CoreAssumptions.LOG_METHOD_INVOCATIONS)
                {
                    CoreUtility.Logger.LogDebug(string.Format("{1} - {0}", name, (int)(DateTime.UtcNow - start).TotalMilliseconds));
                }
#endif
            }
            catch (Exception ex)
            {
                CoreUtility.Logger.LogError(name, ex);
                if (onError != null)
                {
                    onError(ex);
                }
                else
                {
                    CoreUtility.HandleException(ex);
                }
            }
        }
        public static async Task ExecuteMethodAsync(string name, Func<Task> method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {
            try
            {
#if DEBUG
                DateTime start = DateTime.UtcNow;
#endif
                await method();
#if DEBUG
                DateTime end = DateTime.UtcNow;
                if (CoreAssumptions.LOG_METHOD_INVOCATIONS)
                {
                    CoreUtility.Logger.LogDebug(string.Format("{1} - {0}", name, (int)(DateTime.UtcNow - start).TotalMilliseconds));
                }
#endif
            }
            catch (Exception ex)
            {
                CoreUtility.Logger.LogError(name, ex);
                if (onError != null)
                {
                    onError(ex);
                }
                else
                {
                    CoreUtility.HandleException(ex);
                }
            }
        }
        public static T ExecuteFunction<T>(string name, Func<T> method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {
            try
            {
#if DEBUG
                DateTime start = DateTime.UtcNow;
#endif
                T result = method();
#if DEBUG
                DateTime end = DateTime.UtcNow;
                if (CoreAssumptions.LOG_METHOD_INVOCATIONS)
                {
                    CoreUtility.Logger.LogDebug(string.Format("{1} - {0}", name, (int)(DateTime.UtcNow - start).TotalMilliseconds));
                }
#endif
                return result;
            }
            catch (Exception ex)
            {
                CoreUtility.Logger.LogError(name, ex);
                if (onError != null)
                {
                    onError(ex);
                }
                else
                {
                    CoreUtility.HandleException(ex);
                }
                return default(T);
            }
        }
        public static async Task<T> ExecuteFunctionAsync<T>(string name, Func<Task<T>> method, Action<Exception> onError = null)
        {
            try
            {
#if DEBUG
                DateTime start = DateTime.UtcNow;
#endif
                T result = await method();
#if DEBUG
                DateTime end = DateTime.UtcNow;
                if (CoreAssumptions.LOG_METHOD_INVOCATIONS)
                {
                    CoreUtility.Logger.LogDebug(string.Format("{1} - {0}", name, (int)(DateTime.UtcNow - start).TotalMilliseconds));
                }
#endif
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(name, ex);
                if (onError != null)
                {
                    onError(ex);
                }
                else
                {
                    CoreUtility.HandleException(ex);
                }
                return default(T);
            }
        }

        public static void HandleException(Exception ex)
        {
            string message = CoreUtility.IsOutdated(ex);
            if (!string.IsNullOrWhiteSpace(message))
            {
                NativeApplication.Instance?.Outdated(message);
            }
        }

        public static string IsUnauthorized(Exception ex)
        {
            AggregateException aggregate = ex as AggregateException;
            if (aggregate != null)
            {
                foreach (var item in aggregate.InnerExceptions)
                {
                    string message = IsUnauthorized(item);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        return message;
                    }
                }
            }
            //TODO:MUST: Detected Unauthorized
            /*EndpointException endpointException = ex as EndpointException;
            if (endpointException != null)
            {
                if (endpointException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }
            */
            return null;
        }

        public static string IsOutdated(Exception ex)
        {
            AggregateException aggregate = ex as AggregateException;
            if (aggregate != null)
            {
                foreach (var item in aggregate.InnerExceptions)
                {
                    string message = IsOutdated(item);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        return message;
                    }
                }
            }
            //TODO:MUST: Detected IsOutdated
            /*EndpointException endpointException = ex as EndpointException;
            if (endpointException != null)
            {
                if (endpointException.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    return new Tuple<bool, string>(true, endpointException.Message);
                }
            }
            */
            return null;
        }

        public static bool IsForbidden(Exception ex)
        {
            AggregateException aggregate = ex as AggregateException;
            if (aggregate != null)
            {
                foreach (var item in aggregate.InnerExceptions)
                {
                    if (IsForbidden(item))
                    {
                        return true;
                    }
                }
            }
            //TODO:MUST: Detected IsForbidden
            /*EndpointException endpointException = ex as EndpointException;
            if (endpointException != null)
            {
                if (endpointException.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return true;
                }
            }
            */
            return false;
        }

        public static ILogger Logger
        {
            get
            {
                return NativeApplication.Logger;
            }
        }


        public static bool TryParseDimensions(string dimensions, out System.Drawing.Size size)
        {
            size = new System.Drawing.Size();
            try
            {
                if (!string.IsNullOrEmpty(dimensions) && dimensions.Contains("x"))
                {
                    string[] split = dimensions.Split('x');
                    if (split != null && split.Length == 2)
                    {
                        size = new System.Drawing.Size(int.Parse(split[0]), int.Parse(split[1]));
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
