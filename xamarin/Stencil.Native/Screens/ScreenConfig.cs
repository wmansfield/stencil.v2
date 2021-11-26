using Stencil.Native.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Stencil.Native.Screens
{
    public class ScreenConfig : IScreenConfig
    {
        public ScreenConfig()
        {
            this.ViewConfigs = new List<IViewConfig>();
        }
        public string id { get; set; }
        public bool SuppressPersist { get; set; }
        public bool IsMenuSupported { get; set; }

        public Thickness Margin { get; set; }

        public Color BackgroundColor { get; set; }

        public List<IViewConfig> ViewConfigs { get; set; }

        public List<IMenuConfig> MenuConfigs { get; set; }
    }
}
