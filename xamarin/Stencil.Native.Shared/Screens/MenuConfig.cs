﻿using Stencil.Native.Views;

namespace Stencil.Native.Screens
{
    public class MenuConfig : IMenuConfig
    {
        public bool is_icon { get; set; }
        public string icon_character { get; set; }
        public string label { get; set; }
        public string command { get; set; }
        public string command_parameter { get; set; }
    }
}