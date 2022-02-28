using Foundation;
using Quantum.Native.iOS.Stencil.Dependencies;
using Stencil.Forms;
using Stencil.Forms.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSKeyboardManager))]

namespace Quantum.Native.iOS.Stencil.Dependencies
{
    public class iOSKeyboardManager : IKeyboardManager
    {
        public void TryHideKeyboard()
        {
            CoreUtility.ExecuteMethod(nameof(TryHideKeyboard), delegate ()
            {
                UIApplication.SharedApplication.KeyWindow.EndEditing(true);
            });
        }
    }
}