
using Stencil.Common.Screens;

namespace Stencil.Maui.Views
{
    public interface IDataViewSection
    {
        IDataViewItem[] ViewItems { get; set; }
        VisualConfig VisualConfig { get; set; }
    }
}
