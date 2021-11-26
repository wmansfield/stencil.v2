using Xamarin.Forms;

namespace Stencil.Native.Presentation.Menus
{
    public interface IMenuView
    {
        View GetSelf();
        IMenuViewModel MenuViewModel { get; }

    }
}
