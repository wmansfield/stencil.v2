
namespace Stencil.Native.Views
{
    public interface IMenuConfig
    {
        bool is_selected { get; set; }
        string identifier { get; }
        bool is_icon { get; }
        string icon_character { get; }
        string label { get; }
        string command { get; }
        string command_parameter { get; }
    }
}
