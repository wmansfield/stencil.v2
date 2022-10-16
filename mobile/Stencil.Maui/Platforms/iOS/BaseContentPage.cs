using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Base
{
    public abstract partial class BaseContentPage
    {
        partial void Platform_ApplyBasePageUI()
        {
            Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);
        }
    }
}
