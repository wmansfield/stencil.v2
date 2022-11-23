using Microsoft.Maui.Handlers;
using Stencil.Maui.Controls;

namespace Stencil.Maui.Handlers
{
    public partial class StencilEditorHandler
    {
        static partial void ConnectHandler(IEditorHandler handler, StencilEditor editor)
        {
            CoreUtility.ExecuteMethod("StencilEditorHandler.ConnectHandler", delegate ()
            {
                Microsoft.Maui.Platform.MauiTextView platformView = handler.PlatformView;
                if (platformView == null)
                {
                    return;
                }
                if (editor.AutoFocus)
                {
                    editor.PropertyChanged += (sender, args) =>
                    {
                        Editor_PropertyChanged(platformView, sender, args);
                    };
                }
            });
        }

        private static void Editor_PropertyChanged(Microsoft.Maui.Platform.MauiTextView platformView, object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CoreUtility.ExecuteMethod("Editor_PropertyChanged", delegate ()
            {
                if (e.PropertyName == StencilEditor.IsVisibleProperty.PropertyName)
                {
                    if (sender is StencilEditor editor)
                    {
                        if (editor.IsVisible)
                        {
                            TryAutoFocus(platformView);
                        }
                    }
                }
            });
        }

        private static void TryAutoFocus(Microsoft.Maui.Platform.MauiTextView platformView)
        {
            CoreUtility.ExecuteMethod("TryAutoFocus", delegate ()
            {
                try
                {
                    if (platformView != null)
                    {
                        platformView.BecomeFirstResponder();
                    }
                }
                catch
                {
                    // gulp
                }
            });
        }
    }
}
