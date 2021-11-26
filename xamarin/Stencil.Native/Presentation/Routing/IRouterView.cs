using Stencil.Native.Presentation.Menus;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stencil.Native.Presentation.Routing
{
    public interface IRouterView
    {
        bool IsMenuSupported { get; }
        ObservableCollection<IMenuEntry> MenuEntries { get; }

        View GetSelf();
        Task OnNavigatingToAsync();
        ICommand NavigateBackCommand { get; }
    }
}
