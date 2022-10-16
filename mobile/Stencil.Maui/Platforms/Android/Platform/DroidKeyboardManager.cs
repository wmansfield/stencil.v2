using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Microsoft.Maui.Controls;
using Quantum.Native.Droid.Stencil.Platform;
using Stencil.Maui;
using Stencil.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Dependency(typeof(DroidKeyboardHelper))]

namespace Quantum.Native.Droid.Stencil.Platform
{
    public class DroidKeyboardHelper : IKeyboardManager
    {
        public void TryHideKeyboard()
        {
            CoreUtility.ExecuteMethod(nameof(TryHideKeyboard), delegate ()
            {
                try
                {
                    Context context = Android.App.Application.Context;
                    Activity activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
                    if (context != null && activity != null)
                    {
                        InputMethodManager inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                        if (inputMethodManager != null)
                        {
                            Android.Views.View focusView = activity.CurrentFocus;
                            if (focusView == null)
                            {
                                focusView = new Android.Views.View(activity);
                            }
                            focusView?.ClearFocus();
                            IBinder token = focusView.WindowToken;
                            inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                            activity.Window.DecorView.ClearFocus();
                        }
                    }
                }
                catch 
                {
                    //gup
                }
                
                
            });
            
        }
    }
}