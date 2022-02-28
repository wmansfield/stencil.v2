using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Zero.Foundation;
using Zero.Foundation.Daemons;
using Zero.Foundation.Web;

namespace Placeholder.Plugins.SystemMonitor.Controllers
{
    public class DaemonsController : FoundationController
    {
        public DaemonsController(IFoundation foundation)
            : base(foundation)
        {
            
        }

        [HttpGet]
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None )]
        public IActionResult Index(string name = "", string op = "")
        {
            return base.ExecuteFunction("Index", delegate ()
            {
                string message = string.Empty;
                IDaemonManager daemonManager = this.IFoundation.GetDaemonManager();
                if (op == "removeall")
                {
                    daemonManager.UnRegisterAllDaemons();
                    message = "RemovedAll";
                }
                else if (op == "remove")
                {
                    daemonManager.UnRegisterDaemon(name);
                    message = "Removed: " + name;
                }
                else if (op == "stop")
                {
                    daemonManager.StopDaemon(name);
                    message = "Stopped: " + name;
                }
                else if (op == "start")
                {
                    daemonManager.StartDaemon(name);
                    message = "Started: " + name;
                }

                ViewBag.Message = message;
                return View(daemonManager.GetAllTimerDetails().OrderBy(x => x.Name).ToList());
            });
        }
        
    }
}
