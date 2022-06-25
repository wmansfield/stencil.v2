using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.UI
{
    public class ChainInfo
    {
        public string command_name { get; set; }
        public string command_parameter { get; set; }

        public string success_command_name { get; set; }
        public string success_command_parameter { get; set; }

        public string fail_command_name { get; set; }
        public string fail_command_parameter { get; set; }
    }
}
