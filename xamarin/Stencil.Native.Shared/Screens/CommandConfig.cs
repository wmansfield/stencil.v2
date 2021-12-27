using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public class CommandConfig : ICommandConfig
    {
        public string CommandName { get; set; }
        public string CommandParameter { get; set; }
    }
}
