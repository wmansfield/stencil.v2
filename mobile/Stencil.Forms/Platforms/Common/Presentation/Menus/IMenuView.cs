using Xamarin.Forms;

namespace Stencil.Forms.Presentation.Menus
{
    public interface IMenuView
    {
        View GetSelf();
        IMenuViewModel MenuViewModel { get; }

    }
}
