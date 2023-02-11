using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Commands
{
    public static class WellKnownCommands
    {
        public const string APP_NAVIGATE_PUSH = "app.navigate.push";
        public const string APP_NAVIGATE_POP = "app.navigate.pop";
        public const string APP_NAVIGATE_ROOT = "app.navigate.root";
        public const string CHAINED_COMMAND = "command.chained";
        public const string COPY_CLIPBOARD = "clipboard.copy";
        public const string STATE_CHANGE = "state.change";
    }
}
