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
    [Route("api/widgets")]
    public partial class WidgetController : HealthPlaceholderApiController
    {
        public WidgetController(IFoundation foundation)
            : base(foundation, "Widget")
        {
        }

        [HttpGet("{shop_id}/{widget_id}")]
        public Task<IActionResult> GetById(Guid shop_id, Guid widget_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.Widget result = await this.API.Store.Widgets.GetDocumentAsync(shop_id, widget_id);
                
                if (result == null)
                {
                    return base.Http404("Widget");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.Widget>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.Widget widget)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(widget, "Widget");

                dm.Widget insert = widget.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.Widgets.Insert(insert);
                
                sdk.Widget insertResult = await this.API.Store.Widgets.GetDocumentAsync(insert.shop_id, insert.widget_id);

                return base.Http200(new ItemResult<sdk.Widget>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{widget_id}")]
        public Task<IActionResult> Update(Guid widget_id, sdk.Widget widget)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(widget, "Widget");
                this.ValidateRouteMatch(widget_id, widget.widget_id, "Widget");

                dm.Widget found = this.API.Direct.Widgets.GetById(widget.shop_id, widget_id);
                this.ValidateNotNull(found, "Widget");

                widget.widget_id = widget_id;
                dm.Widget update = widget.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.Widgets.Update(update);
                
                sdk.Widget existing = await this.API.Store.Widgets.GetDocumentAsync(update.shop_id, update.widget_id);
                
                return base.Http200(new ItemResult<sdk.Widget>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{shop_id}/{widget_id}")]
        public IActionResult Delete(Guid shop_id, Guid widget_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Widget delete = this.API.Direct.Widgets.GetById(shop_id, widget_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.Widgets.Delete(shop_id, widget_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = widget_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

