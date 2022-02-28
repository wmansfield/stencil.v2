using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Plugins.SystemMonitor.Models;
using Zero.Foundation;
using Zero.Foundation.Web;

namespace Placeholder.Plugins.SystemMonitor.Controllers
{
    public class VersionController : FoundationController
    {
        public VersionController(IFoundation foundation)
            : base(foundation)
        {
            
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None )]
        public IActionResult Index()
        {
            return base.ExecuteFunction("Index", delegate ()
            {
                IWebHostEnvironment hostEnvironment = this.IFoundation.Resolve<IWebHostEnvironment>();

                VersionInfo info = new VersionInfo();
                info.BuildDate = System.IO.File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "_build.txt"));
                return View(info);
            });
        }
        
    }
}
