using Android.Graphics.Drawables;
using Android.Views.InputMethods;
using Android.Widget;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Stencil.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Handlers
{
    public partial class StencilEditorHandler
    {
        static partial void ConnectHandler(IEditorHandler handler, StencilEditor editor)
        {
            CoreUtility.ExecuteMethod("StencilEditorHandler.ConnectHandler", delegate ()
            {
                AndroidX.AppCompat.Widget.AppCompatEditText platformView = handler.PlatformView;
                if(platformView == null)
                {
                    return;
                }

                if(editor.SuppressBottomLine)
                {
                    GradientDrawable gradient = new GradientDrawable();
                    gradient.SetColor(Android.Graphics.Color.Transparent);
                    platformView.SetBackground(gradient);
                }
                if(editor.AutoFocus)
                {
                    editor.PropertyChanged += (sender, args) => 
                    {
                        Editor_PropertyChanged(platformView, sender, args);
                    };
                }
            });
        }

        private static void Editor_PropertyChanged(AndroidX.AppCompat.Widget.AppCompatEditText platformView, object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CoreUtility.ExecuteMethod("Editor_PropertyChanged", delegate ()
            {
                if(e.PropertyName == StencilEditor.IsVisibleProperty.PropertyName)
                {
                    if(sender is StencilEditor editor)
                    {
                        if(editor.IsVisible)
                        {
                            TryAutoFocus(platformView, editor, 1, 4);
                        }
                    }
                }
            });
        }

        private static void TryAutoFocus(AndroidX.AppCompat.Widget.AppCompatEditText editText, StencilEditor editor, int currentAttempt, int attemptLimit = 4)
        {
            CoreUtility.ExecuteMethod("TryAutoFocus", delegate ()
            {
                try
                {
                    if (editText != null)
                    {
                        if (editor.IsVisible)
                        {
                            bool gotFocus = editText.RequestFocus();
                            if (gotFocus)
                            {
                                InputMethodManager inputMethodManager = editText.Context.GetSystemService(Android.Content.Context.InputMethodService) as InputMethodManager;
                                inputMethodManager.ShowSoftInput(editText, ShowFlags.Forced);
                            }
                            else
                            {
                                currentAttempt++;
                                if (currentAttempt < attemptLimit)
                                {
                                    // A little ugly, but change may happen before creation of native
                                    Task.Delay(100).ContinueWith((t) =>
                                    {
                                        MainThread.BeginInvokeOnMainThread(() =>
                                        {
                                            TryAutoFocus(editText, editor, currentAttempt, attemptLimit);
                                        });
                                    });
                                }
                            }
                        }
                    }
                }
                catch
                {
                    //gulp
                }
            });
            
        }
    }
}
