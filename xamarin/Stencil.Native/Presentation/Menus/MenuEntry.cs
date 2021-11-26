
namespace Stencil.Native.Presentation.Menus
{
    public class MenuEntry : IMenuEntry
    {
        public bool IsIcon { get; set; }
        public string IconCharacter { get; set; }
        public string Label { get; set; }
        public string CommandName { get; set; }
        public string CommandParameter { get; set; }
    }
}
