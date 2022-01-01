using Stencil.Native.Base;
using Stencil.Native.Presentation.Menus;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        ObservableCollection<IDataViewItem> MainItemsUnFiltered { get; }
        ObservableCollection<IDataViewItem> MainItemsFiltered { get; }
        ObservableCollection<IDataViewItem> FooterItems { get; }
        bool ShowFooter { get; }
        DataTemplateSelector DataTemplateSelector { get; }
        Thickness Padding { get; }
        Color BackgroundColor { get; }
        string BackgroundImage { get; }

        Task InitializeData();
        Task ApplyFiltersAsync();

    }
}
