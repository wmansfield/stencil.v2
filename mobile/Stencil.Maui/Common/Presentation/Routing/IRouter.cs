using Microsoft.Maui.Controls;
using Stencil.Maui.Commanding;
using Stencil.Maui.Presentation.Menus;
using Stencil.Maui.Presentation.Shells;
using System.Threading.Tasks;

namespace Stencil.Maui.Presentation.Routing
{
    public interface IRouter
    {
        ShellModel CurrentShellModel { get; set; }

        void PrepareFromOther(IRouter routerOfSameType);
        
        Task SetInitialViewAsync(IRouterView view);
        Task PushViewAsync(IRouterView view, IMenuEntry knownMainMenuEntry = null);
        Task PopViewAsync(bool reloadPrevious, int iterations = 1);
    }
}
