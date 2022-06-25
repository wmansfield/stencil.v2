using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Plugins.Azure.Models;
using Placeholder.Web.Controllers;
using Zero.Foundation;
using Zero.Foundation.Daemons;

namespace Placeholder.Plugins.Azure.Controllers
{
    [AllowAnonymous]
    [Route("api/azure")]
    public class AzureWebhookController : PlaceholderApiController
    {
        public AzureWebhookController(IFoundation foundation)
            : base(foundation)
        {
        }


        [HttpPost]
        [Route("agitate")]
        public IActionResult Agitate(AgitateInput input)
        {
            return base.ExecuteFunction("Agitate", delegate ()
            {
                string result = "OK";
                if (input != null)
                {
                    if (input.key == "codeable" && !string.IsNullOrWhiteSpace(input.kind))
                    {
                        IDaemonManager daemonManager = this.IFoundation.GetDaemonManager();
                        daemonManager.StartDaemon(input.kind);
                        result = "Agitated";
                    }
                }
                return this.Http200(result);
            });
        }

    }
}
