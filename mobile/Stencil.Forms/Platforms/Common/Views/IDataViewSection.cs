
using Stencil.Common.Screens;

namespace Stencil.Forms.Views
{
    public interface IDataViewSection
    {
        IDataViewItem[] ViewItems { get; set; }
        VisualConfig VisualConfig { get; set; }
    }
}
