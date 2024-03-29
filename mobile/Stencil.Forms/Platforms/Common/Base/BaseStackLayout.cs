﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Base
{
    public abstract class BaseStackLayout : StackLayout
    {
        public BaseStackLayout(string trackPrefix)
        {
            this.API = StencilAPI.Instance;
            this.TrackPrefix = trackPrefix;
        }


        #region Properties


        public StencilAPI API { get; protected set; }

        public string TrackPrefix { get; protected set; }

        #endregion

        #region Protected Methods

        protected virtual bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Logging Methods

        protected void LogError(Exception ex, string location = "")
        {
            this.API?.Logger?.LogError(location, ex);
        }
        protected void LogDebug(string message)
        {
            this.API?.Logger?.LogDebug(message);
        }

        #endregion

        #region UI Methods

        /// <summary>
        /// Warning, this does not guarantee synchronous execution.
        /// </summary>
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

        /// <summary>
        /// Executes the command unless the command is already running, then its skipped
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void ExecuteMethodOrSkip(string name, Action method, Action<Exception> onError = null)
        {
            this.ExecuteMethod(name, delegate ()
            {
                bool added = _executingCommands.TryAdd(name, 1);
                if (!added)
                {
                    return;
                }
                try
                {
                    method();
                }
                finally
                {
                    _executingCommands.TryRemove(name, out byte ignored);
                }
            });
        }

        /// <summary>
        /// Executes the command unless the command is already running, then its skipped
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual Task ExecuteMethodOrSkipAsync(string name, Func<Task> method, Action<Exception> onError = null)
        {
            return this.ExecuteMethodAsync(name, async delegate ()
            {
                bool added = _executingCommands.TryAdd(name, 1);
                if (!added)
                {
                    return;
                }
                try
                {
                    await method();
                }
                finally
                {
                    _executingCommands.TryRemove(name, out byte ignored);
                }
            });
        }

        protected virtual bool IsExecutingCommand(string name)
        {
            return this.ExecuteFunction(name, delegate ()
            {
                return _executingCommands.ContainsKey(name);
            });
        }
        #endregion
    }
}
