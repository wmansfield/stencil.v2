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
    [Route("api/tenants")]
    public partial class TenantController : HealthPlaceholderApiController
    {
        public TenantController(IFoundation foundation)
            : base(foundation, "Tenant")
        {
        }

        [HttpGet("{tenant_id}")]
        public IActionResult GetById(Guid tenant_id)
        {
            return base.ExecuteFunction<IActionResult>("GetById", delegate()
            {
                dm.Tenant result = this.API.Direct.Tenants.GetById(tenant_id);
                if (result == null)
                {
                    return Http404("Tenant");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result);
                

                return base.Http200(new ItemResult<sdk.Tenant>()
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

                this.Security.ValidateCanSearchTenant(this.GetCurrentAccount());

                int takePlus = take;
                if (take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }

                List<dm.Tenant> result = this.API.Direct.Tenants.Find(skip, takePlus, keyword, order_by, descending);
                return base.Http200(result.ToSteppedListResult(skip, take, result.Count));

            });
        }
        
        
        [HttpPost]
        public IActionResult Create(sdk.Tenant tenant)
        {
            return base.ExecuteFunction<IActionResult>("Create", delegate()
            {
                this.ValidateNotNull(tenant, "Tenant");

                dm.Tenant insert = tenant.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.Tenants.Insert(insert);
                
                sdk.Tenant insertResult = insert.ToSDKModel();

                return base.Http200(new ItemResult<sdk.Tenant>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{tenant_id}")]
        public IActionResult Update(Guid tenant_id, sdk.Tenant tenant)
        {
            return base.ExecuteFunction<IActionResult>("Update", delegate()
            {
                this.ValidateNotNull(tenant, "Tenant");
                this.ValidateRouteMatch(tenant_id, tenant.tenant_id, "Tenant");

                dm.Tenant found = this.API.Direct.Tenants.GetById(tenant_id);
                this.ValidateNotNull(found, "Tenant");

                tenant.tenant_id = tenant_id;
                dm.Tenant update = tenant.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.Tenants.Update(update);
                
                sdk.Tenant existing = this.API.Direct.Tenants.GetById(update.tenant_id).ToSDKModel();
                
                return base.Http200(new ItemResult<sdk.Tenant>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{tenant_id}")]
        public IActionResult Delete(Guid tenant_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Tenant delete = this.API.Direct.Tenants.GetById(tenant_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.Tenants.Delete(tenant_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = tenant_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

