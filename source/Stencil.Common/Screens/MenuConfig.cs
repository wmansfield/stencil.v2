using Stencil.Common.Views;

namespace Stencil.Common.Screens
{
    public class MenuConfig : IMenuConfig
    {
        public bool is_selected { get; set; }
        public string identifier { get; set; }
        public bool is_icon { get; set; }
        public string icon_character { get; set; }
        public string label { get; set; }
        public string command { get; set; }
        public string command_parameter { get; set; }
    }
}
