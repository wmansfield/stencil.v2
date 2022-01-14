using Stencil.Forms.Presentation.Menus;
using Stencil.Forms.Presentation.Routing;

namespace Stencil.Forms.Presentation.Shells
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