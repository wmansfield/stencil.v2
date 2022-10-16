using Microsoft.Maui.Controls;
using Stencil.Maui.Presentation.Menus;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Stencil.Maui.Presentation.Routing
{
    public interface IRouterView
    {
        bool IsMenuSupported { get; }
        ObservableCollection<IMenuEntry> MenuEntries { get; }

        View GetSelf();
        Task OnNavigatingToAsync(bool reload);
        Task OnNavigatedToAsync();
        ICommand NavigateBackCommand { get; }
    }
}
