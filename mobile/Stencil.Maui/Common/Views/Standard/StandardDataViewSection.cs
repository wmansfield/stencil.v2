
using Stencil.Common.Screens;

namespace Stencil.Maui.Views.Standard
{
    public class StandardDataViewSection : IDataViewSection
    {
        public IDataViewItem[] ViewItems { get; set; }
        public VisualConfig VisualConfig { get; set; }
    }
}
