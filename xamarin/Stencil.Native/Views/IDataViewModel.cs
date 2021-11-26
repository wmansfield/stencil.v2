using Stencil.Native.Base;
using Stencil.Native.Presentation.Menus;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stencil.Native.Views
{
    public interface IDataViewModel : IBaseViewModel
    {
        IDataViewVisual DataViewVisual { get; set; }
        ICommand NavigateBackCommand { get; }
        bool IsMenuSupported { get; }
        ObservableCollection<IMenuEntry> MenuEntries { get; }
        ObservableCollection<IDataViewItem> DataViewItems { get; }
        DataTemplateSelector DataTemplateSelector { get; }
        Thickness Margin { get; }
        Color BackgroundColor { get; }

    }
}
