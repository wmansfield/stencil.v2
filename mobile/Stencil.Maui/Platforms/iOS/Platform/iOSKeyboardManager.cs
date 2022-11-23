using Stencil.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Stencil.Maui.iOS
{
    public class iOSKeyboardManager : IKeyboardManager
    {
        public void TryHideKeyboard()
        {
            CoreUtility.ExecuteMethod(nameof(TryHideKeyboard), delegate ()
            {
                try
                {
                    UIApplication.SharedApplication.KeyWindow.EndEditing(true);
                }
                catch 
                {
                    //gulp
                }
            });
        }
    }
}
