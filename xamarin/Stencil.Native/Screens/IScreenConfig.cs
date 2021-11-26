using Stencil.Native.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Stencil.Native.Screens
{
    public interface IScreenConfig
    {
        bool IsMenuSupported { get; }
        Thickness Margin { get; }
        Color BackgroundColor { get; }
        List<IViewConfig> ViewConfigs { get; }
        List<IMenuConfig> MenuConfigs { get;  }

    }
}