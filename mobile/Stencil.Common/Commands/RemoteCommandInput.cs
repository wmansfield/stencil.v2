using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Commands
{
    public class RemoteCommandInput
    {
        public RemoteCommandInput()
        {
            this.user_values = new List<InputPair>();
        }
        public const string INPUT_GROUP = "input";

        public string command_name { get; set; }
        public string command_parameter { get; set; }
        public List<InputPair> user_values { get; set; }
    }
}
