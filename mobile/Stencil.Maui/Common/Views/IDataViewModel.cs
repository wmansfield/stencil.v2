using Stencil.Common.Screens;
using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Menus;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Stencil.Maui.Views
{
    public interface IDataViewModel : INestedDataViewModel
    {
        ICommand NavigateBackCommand { get; }
        bool IsMenuSupported { get; }
        ObservableCollection<IMenuEntry> MenuEntries { get; }
        NavigationData NavigationData { get; set; }
    }
}
