using Acr.UserDialogs;
using Stencil.Maui;
using Stencil.Maui.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Presentation
{
    public class UserDialogAlerts : TrackedClass, IAlerts
    {
        public UserDialogAlerts()
            : base(nameof(UserDialogAlerts))
        {

        }
        public Task AlertAsync(string message, string title = null, string okText = null, CancellationToken? cancelToken = null)
        {
            return base.ExecuteFunction(nameof(AlertAsync), delegate ()
            {
#if WINDOWS
                //TODO:SHOULD: Support windows dialogs
                Microsoft.Maui.Controls.Compatibility.Platform.UWP.AlertDialog dialog = new Microsoft.Maui.Controls.Compatibility.Platform.UWP.AlertDialog()
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = okText
                };

                return dialog.ShowAsync().AsTask();
#elif IOS || MACCATALYST || ANDROID
                return UserDialogs.Instance.AlertAsync(message, title, okText, cancelToken);
#else
                return Task.CompletedTask;
#endif
            });
        }

        public IDisposable Toast(string message, TimeSpan? dismissTimer = null)
        {
            return base.ExecuteFunction(nameof(Toast), delegate ()
            {
#if WINDOWS
                //TODO:SHOULD: Support windows dialogs
                Microsoft.Maui.Controls.Compatibility.Platform.UWP.AlertDialog dialog = new Microsoft.Maui.Controls.Compatibility.Platform.UWP.AlertDialog()
                {
                    Content = message,
                };

                return dialog.ShowAsync().AsTask();
#elif IOS || MACCATALYST || ANDROID
                return UserDialogs.Instance.Toast(message, dismissTimer);
#else
                return (IDisposable)null;
#endif
            });
        }

        public Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons)
        {
            return base.ExecuteFunctionAsync(nameof(AlertAsync), async delegate ()
            {
#if WINDOWS
                //TODO:SHOULD: Support windows dialogs
                Microsoft.Maui.Controls.Compatibility.Platform.UWP.AlertDialog dialog = new Microsoft.Maui.Controls.Compatibility.Platform.UWP.AlertDialog()
                {
                    Content = title,
                };

                await dialog.ShowAsync().AsTask();
                return buttons[0];
#elif IOS || MACCATALYST || ANDROID
                return await UserDialogs.Instance.ActionSheetAsync(title, cancel, destructive, cancelToken, buttons);
#else
                return (string)null;
#endif
            });
        }
    }
}
