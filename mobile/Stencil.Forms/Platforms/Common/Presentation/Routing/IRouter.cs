using Stencil.Forms.Presentation.Menus;
using Stencil.Forms.Presentation.Shells;
using System.Threading.Tasks;

namespace Stencil.Forms.Presentation.Routing
{
    public interface IRouter
    {
        ShellModel CurrentShellModel { get; set; }
        Task SetInitialViewAsync(IRouterView view);
        Task PushViewAsync(IRouterView view, IMenuEntry knownMainMenuEntry = null);
        Task PopViewAsync(bool reloadPrevious);
    }
}
