using Stencil.Forms.Presentation.Menus;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stencil.Forms.Presentation.Routing
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
