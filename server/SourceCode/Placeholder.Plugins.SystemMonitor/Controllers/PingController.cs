using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Data.Sql;
using Zero.Foundation;
using Zero.Foundation.Web;
using db = Placeholder.Data.Sql.Models;

namespace Placeholder.Plugins.SystemMonitor.Controllers
{
    public class PingController : FoundationController
    {
        public PingController(IFoundation foundation)
            : base(foundation)
        {
            
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None )]
        public ActionResult Index()
        {
            return base.ExecuteFunction("Index", delegate ()
            {
                IPlaceholderContextFactory factory = this.IFoundation.Resolve<IPlaceholderContextFactory>();
                
                string result = string.Empty;
                try
                {
                    using(var database = factory.CreateSharedContext())
                    {
                        db.Shop shop = database.Shops.FirstOrDefault();
                    }
                }
                catch (Exception)
                {
                    result += "DIRECT-FAIL ";
                }
                
                if (string.IsNullOrEmpty(result))
                {
                    result = "PONG";
                }
                ViewBag.Result = result;
                return View();
            });
        }
        
    }
}
