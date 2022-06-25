using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Commands
{
    public class RemoteCommandResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string response { get; set; }
        public string command_name { get; set; }
        public string command_parameter { get; set; }
        public bool command_show_errors { get; set; }
    }
}
