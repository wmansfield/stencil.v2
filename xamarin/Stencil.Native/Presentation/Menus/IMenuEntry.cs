
namespace Stencil.Native.Presentation.Menus
{
    public interface IMenuEntry
    {
        bool IsIcon { get; }
        string IconCharacter { get; }
        string Label { get; }
        string CommandName { get; }
        string CommandParameter { get; }
    }
}
