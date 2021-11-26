using Stencil.Native.Presentation.Menus;
using Stencil.Native.Presentation.Shells;
using System.Threading.Tasks;

namespace Stencil.Native.Presentation.Routing
{
    public interface IRouter
    {
        ShellModel CurrentShellModel { get; set; }
        Task SetInitialViewAsync(IRouterView view);
        Task PushViewAsync(IRouterView view, IMenuEntry knownMainMenuEntry = null);
        Task PopViewAsync();
    }
}
