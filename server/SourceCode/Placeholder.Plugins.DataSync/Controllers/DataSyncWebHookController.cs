using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Plugins.DataSync.Integration;
using Placeholder.Web.Controllers;
using Zero.Foundation;

namespace Placeholder.Plugins.DataSync.Controllers
{
    [AllowAnonymous]
    [Route("api/datasync")]
    public class DataSyncWebHookController : PlaceholderApiController
    {
        public DataSyncWebHookController(IFoundation foundation)
            : base(foundation)
        {
        }

        [HttpPost]
        [Route("failed")]
        public IActionResult OnSynchronousFailed([FromForm] SyncInput input)
        {
            return base.ExecuteFunction("OnSynchronousFailed", delegate ()
            {
                string message = WebHookProcessor.ProcessSyncWebHook(this.IFoundation, input.key, "failed", input.tenant);

                return this.Http200(message);
            });
        }

        [HttpPost]
        [Route("sync")]
        public IActionResult OnSyncWasRequested([FromForm] SyncInput input)
        {
            return base.ExecuteFunction("OnSyncWasRequested", delegate ()
            {
                string message = WebHookProcessor.ProcessSyncWebHook(this.IFoundation, input.key, "sync", input.tenant);

                return this.Http200(message);
            });
        }


        [HttpPost]
        [Route("agitate")]
        public IActionResult Agitate([FromForm] AgitateInput input)
        {
            return base.ExecuteFunction("Agitate", delegate ()
            {
                string message = WebHookProcessor.ProcessAgitateWebHook(this.IFoundation, input.key, input.name);

                return this.Http200(message);
            });
        }

        public class AgitateInput
        {
            public string key { get; set; }
            public string name { get; set; }
        }

        public class SyncInput
        {
            public string key { get; set; }
            public string tenant { get; set; }
        }

    }
}
