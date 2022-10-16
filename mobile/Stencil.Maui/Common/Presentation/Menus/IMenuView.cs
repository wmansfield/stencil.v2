using Microsoft.Maui.Controls;

namespace Stencil.Maui.Presentation.Menus
{
    public interface IMenuView
    {
        View GetSelf();
        IMenuViewModel MenuViewModel { get; }

    }
}
