using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Plugins.SystemMonitor.Models;
using Zero.Foundation;
using Zero.Foundation.Plugins;
using Zero.Foundation.Web;

namespace Placeholder.Plugins.SystemMonitor.Controllers
{
    public class PluginController : FoundationController
    {
        public PluginController(IFoundation foundation)
            : base(foundation)
        {
            
        }
        
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None )]
        public IActionResult Index()
        {
            return base.ExecuteFunction(nameof(Index), delegate()
            {
                IPluginManager pluginManager = this.IFoundation.GetPluginManager();
                IWebPluginLoader webPluginLoader = this.IFoundation.Resolve<IWebPluginLoader>();

                PluginInfo result = new PluginInfo();
                result.FoundationPlugins = pluginManager.FoundationPlugins.ToList();
                result.WebPlugins = webPluginLoader.GetRegisteredPlugins().ToList();

                return View(result);
            });
        }
    }
}
