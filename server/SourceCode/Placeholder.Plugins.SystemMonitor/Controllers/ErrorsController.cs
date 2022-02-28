using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Zero.Foundation;
using Zero.Foundation.Web;

namespace Placeholder.Plugins.SystemMonitor.Controllers
{
    public class ErrorsController : FoundationController
    {
        public ErrorsController(IFoundation foundation)
            : base(foundation)
        {
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index(bool delete = false)
        {
            return base.ExecuteFunction<ActionResult>("Index", delegate ()
            {
                string fileName = Path.Combine(Path.GetTempPath(), @"PlaceholderErrorTracking\Error.Log");
                if (System.IO.File.Exists(fileName))
                {
                    if (delete)
                    {
                        System.IO.File.WriteAllText(fileName, "cleared");
                        return Json(new { success = true, message = "deleted" });
                    }
                    else
                    {
                        return File(System.IO.File.ReadAllBytes(fileName), "text/text");
                    }
                }

                return Json(new { success = false });
            });
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult RestartIf(string name = "")
        {
            bool restarted = false;
            if (name == Environment.MachineName)
            {
                this.IFoundation.Resolve<FoundationHost>().RequestRestart();
            }
            return Json(new { machine = Environment.MachineName, restarted = restarted });
        }

    }
}
