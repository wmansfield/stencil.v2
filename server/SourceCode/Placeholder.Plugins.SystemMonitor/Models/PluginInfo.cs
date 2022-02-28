using System;
using System.Collections.Generic;
using Zero.Foundation.Plugins;

namespace Placeholder.Plugins.SystemMonitor.Models
{
    public class PluginInfo
    {
        public List<IFoundationPlugin> FoundationPlugins { get; set; }

        public List<IWebPlugin> WebPlugins { get; set; }
    }
}
