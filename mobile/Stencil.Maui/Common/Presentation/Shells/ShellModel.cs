using Stencil.Maui.Presentation.Menus;
using Stencil.Maui.Presentation.Routing;

namespace Stencil.Maui.Presentation.Shells
{
    public class ShellModel
    {
        public ShellModel Parent { get; set; }
        public IMenuView Menu { get; set; }
        public ShellModel Content { get; set; }
        public IRouterView View { get; set; }
        public bool IsRoot { get; set; }
    }
}