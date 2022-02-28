using Zero.Foundation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActionResult = Placeholder.SDK.ActionResult;
using sdk = Placeholder.SDK.Models;
using dm = Placeholder.Domain;
using Placeholder.Primary;
using Placeholder.SDK;
using Placeholder.SDK.Client;
using Placeholder.Web.Controllers;

namespace Placeholder.Plugins.RestAPI.Controllers
{
    [Authorize]
    [Route("api/globalsettings")]
    public partial class GlobalSettingController : HealthPlaceholderApiController
    {
        public GlobalSettingController(IFoundation foundation)
            : base(foundation, "GlobalSetting")
        {
        }

        [HttpGet("{global_setting_id}")]
        public IActionResult GetById(Guid global_setting_id)
        {
            return base.ExecuteFunction<IActionResult>("GetById", delegate()
            {
                dm.GlobalSetting result = this.API.Direct.GlobalSettings.GetById(global_setting_id);
                if (result == null)
                {
                    return Http404("GlobalSetting");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result);
                

                return base.Http200(new ItemResult<sdk.GlobalSetting>()
                {
                    success = true,
                    item = result.ToSDKModel()
                });
            });
        }
        
        [HttpGet("")]
        public IActionResult Find(int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunction<IActionResult>("Find", delegate()
            {

                this.Security.ValidateCanSearchGlobalSetting(this.GetCurrentAccount());

                int takePlus = take;
                if (take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }

                List<dm.GlobalSetting> result = this.API.Direct.GlobalSettings.Find(skip, takePlus, keyword, order_by, descending);
                return base.Http200(result.ToSteppedListResult(skip, take, result.Count));

            });
        }
        
        
        [HttpPost]
        public IActionResult Create(sdk.GlobalSetting globalsetting)
        {
            return base.ExecuteFunction<IActionResult>("Create", delegate()
            {
                this.ValidateNotNull(globalsetting, "GlobalSetting");

                dm.GlobalSetting insert = globalsetting.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.GlobalSettings.Insert(insert);
                
                sdk.GlobalSetting insertResult = insert.ToSDKModel();

                return base.Http200(new ItemResult<sdk.GlobalSetting>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{global_setting_id}")]
        public IActionResult Update(Guid global_setting_id, sdk.GlobalSetting globalsetting)
        {
            return base.ExecuteFunction<IActionResult>("Update", delegate()
            {
                this.ValidateNotNull(globalsetting, "GlobalSetting");
                this.ValidateRouteMatch(global_setting_id, globalsetting.global_setting_id, "GlobalSetting");

                dm.GlobalSetting found = this.API.Direct.GlobalSettings.GetById(global_setting_id);
                this.ValidateNotNull(found, "GlobalSetting");

                globalsetting.global_setting_id = global_setting_id;
                dm.GlobalSetting update = globalsetting.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.GlobalSettings.Update(update);
                
                sdk.GlobalSetting existing = this.API.Direct.GlobalSettings.GetById(update.global_setting_id).ToSDKModel();
                
                return base.Http200(new ItemResult<sdk.GlobalSetting>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{global_setting_id}")]
        public IActionResult Delete(Guid global_setting_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.GlobalSetting delete = this.API.Direct.GlobalSettings.GetById(global_setting_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.GlobalSettings.Delete(global_setting_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = global_setting_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

