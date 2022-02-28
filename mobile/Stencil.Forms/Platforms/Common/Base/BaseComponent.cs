using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Base
{
    public class BaseDataViewComponent : ResourceDictionary
    { 
        #region Constructor

        public BaseDataViewComponent(string trackPrefix)
        {
            this.API = StencilAPI.Instance;
            this.TrackPrefix = trackPrefix;
        }

        #endregion

        #region Properties

        public StencilAPI API { get; protected set; }

        protected string TrackPrefix { get; set; }

        #endregion

        #region Logging Methods

        protected void LogError(Exception ex, string location = "")
        {
            this.API.Logger.LogError(location, ex);
        }

        protected void LogTrace(string message)
        {
            this.API.Logger.LogDebug(message);
        }
        protected void LogTrace(string format, params object[] args)
        {
            this.API.Logger.LogDebug(string.Format(format, args));
        }

        #endregion

        #region UI Methods

        /// <summary>
        /// Warning, this does not guarantee synchronous execution.
        /// </summary>
        [DebuggerNonUserCode]
        protected void ExecuteMethodOnMainThreadBegin(string name, Action action, Action<Exception> onError = null)
        {
            if (Device.IsInvokeRequired)
            {
                Device.BeginInvokeOnMainThread(delegate ()
                {
                    this.ExecuteMethod(name, action, onError);
                });
            }
            else
            {
                this.ExecuteMethod(name, action, onError);
            }
        }

        #endregion

        #region Aspect Methods

        [Obsolete("Incorrect api call, use the Async Version of this method", true)]
        protected virtual void ExecuteMethod(string name, Func<Task> method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {

        }
        [DebuggerNonUserCode]
        protected virtual void ExecuteMethod(string name, Action method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {
            CoreUtility.ExecuteMethod(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError, supressMethodLogging);
        }
        [DebuggerNonUserCode]
        protected virtual Task ExecuteMethodAsync(string name, Func<Task> method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {
            return CoreUtility.ExecuteMethodAsync(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError, supressMethodLogging);
        }
        [DebuggerNonUserCode]
        protected virtual T ExecuteFunction<T>(string name, Func<T> method, Action<Exception> onError = null, bool supressMethodLogging = false)
        {
            return CoreUtility.ExecuteFunction<T>(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError, supressMethodLogging);
        }
        [DebuggerNonUserCode]
        protected virtual T ExecuteThrowingFunction<T>(string name, Func<T> method, bool supressMethodLogging = false)
        {
            return CoreUtility.ExecuteFunction<T>(string.Format("{0}.{1}", this.TrackPrefix, name), method, delegate (Exception ex) { throw ex; }, supressMethodLogging);
        }
        [DebuggerNonUserCode]
        protected virtual Task<T> ExecuteFunctionAsync<T>(string name, Func<Task<T>> method, Action<Exception> onError = null)
        {
            return CoreUtility.ExecuteFunctionAsync<T>(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError);
        }
        [DebuggerNonUserCode]
        protected virtual Task<T> ExecuteThrowingFunctionAsync<T>(string name, Func<Task<T>> method)
        {
            return CoreUtility.ExecuteFunctionAsync<T>(string.Format("{0}.{1}", this.TrackPrefix, name), method, delegate (Exception ex) { throw ex; });
        }


        private ConcurrentDictionary<string, byte> _executingCommands = new ConcurrentDictionary<string, byte>();
        protected virtual ConcurrentDictionary<string, byte> executingCommands
        {
            get
            {
                return _executingCommands;
            }
        }

        [DebuggerNonUserCode]
        protected virtual void ExecuteMethodOrSkip(string name, Action method, Action<Exception> onError = null)
        {
            this.ExecuteMethod(name, delegate ()
            {
                bool added = _executingCommands.TryAdd(name, 1);
                if (!added) { return; }
                try
                {
                    method();
                }
                finally
                {
                    byte ignored = 0;
                    _executingCommands.TryRemove(name, out ignored);
                }
            });
        }

        [DebuggerNonUserCode]
        protected virtual Task ExecuteMethodOrSkipAsync(string name, Func<Task> method, Action<Exception> onError = null)
        {
            return this.ExecuteMethodAsync(name, async delegate ()
            {
                bool added = _executingCommands.TryAdd(name, 1);
                if (!added) { return; }
                try
                {
                    await method();
                }
                finally
                {
                    byte ignored = 0;
                    _executingCommands.TryRemove(name, out ignored);
                }
            });
        }

        protected virtual bool IsExecutingCommand(string name)
        {
            return this.ExecuteFunction(nameof(IsExecutingCommand), delegate ()
            {
                return _executingCommands.ContainsKey(name);
            });
        }

        #endregion
    }
}
