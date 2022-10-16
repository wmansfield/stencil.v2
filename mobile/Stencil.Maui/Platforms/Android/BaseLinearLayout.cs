using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using System;
using System.Threading.Tasks;

namespace Stencil.Maui.Droid
{
    public abstract class BaseLinearLayout : LinearLayout
    {
        public BaseLinearLayout(Context context)
            : base(context)
        {
        }

        public BaseLinearLayout(Context contect, IAttributeSet attrs)
            : base(contect, attrs)
        {
        }

        public BaseLinearLayout(Context contect, IAttributeSet attrs, int defStyleAttr)
            : base(contect, attrs, defStyleAttr)
        {
        }

        public BaseLinearLayout(Context contect, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(contect, attrs, defStyleAttr, defStyleRes)
        {
        }
        public BaseLinearLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }


        public string TrackPrefix { get; protected set; }


        #region Core Methods

        public virtual void ScrollToTop()
        {
            // extensibility
        }

        protected virtual void ExecuteMethodOnMainThread(string name, Action method)
        {
            ((Activity)this.Context).RunOnUiThread(delegate ()
            {
                this.ExecuteMethod(name, method);
            });
        }
        protected virtual void ExecuteMethod(string name, Action method, Action<Exception> onError = null)
        {
            CoreUtility.ExecuteMethod(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError);
        }
        protected virtual Task ExecuteMethodAsync(string name, Func<Task> method, Action<Exception> onError = null)
        {
            return CoreUtility.ExecuteMethodAsync(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError);
        }
        protected virtual T ExecuteFunction<T>(string name, Func<T> method, Action<Exception> onError = null)
        {
            return CoreUtility.ExecuteFunction<T>(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError);
        }
        protected virtual Task<T> ExecuteFunctionAsync<T>(string name, Func<Task<T>> method, Action<Exception> onError = null)
        {
            return CoreUtility.ExecuteFunctionAsync<T>(string.Format("{0}.{1}", this.TrackPrefix, name), method, onError);
        }

        #endregion
    }
}