using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Quantum.Native.Droid.Stencil.Platform;
using Stencil.Forms;
using Stencil.Forms.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidKeyboardHelper))]

namespace Quantum.Native.Droid.Stencil.Platform
{
    public class DroidKeyboardHelper : IKeyboardManager
    {
        public void TryHideKeyboard()
        {
            CoreUtility.ExecuteMethod(nameof(TryHideKeyboard), delegate ()
            {
                Context context = Android.App.Application.Context;
                Activity activity = context as Activity;
                if(context != null && activity != null)
                {
                    InputMethodManager inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                    if (inputMethodManager != null)
                    {
                        IBinder token = activity.CurrentFocus?.WindowToken;
                        if (token != null)
                        {
                            inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                            activity.Window.DecorView.ClearFocus();
                        }
                    }
                }
                
            });
            
        }
    }
}