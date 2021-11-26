using Stencil.Native.Presentation.Menus;
using Stencil.Native.Presentation.Routing;

namespace Stencil.Native.Presentation.Shells
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