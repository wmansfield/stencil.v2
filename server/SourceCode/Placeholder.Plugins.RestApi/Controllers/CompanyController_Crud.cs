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
    [Route("api/companys")]
    public partial class CompanyController : HealthPlaceholderApiController
    {
        public CompanyController(IFoundation foundation)
            : base(foundation, "Company")
        {
        }

        [HttpGet("{shop_id}/{company_id}")]
        public Task<IActionResult> GetById(Guid shop_id, Guid company_id)
        {
            return base.ExecuteFunctionAsync<IActionResult>("GetById", async delegate()
            {
                sdk.Company result = await this.API.Store.Companies.GetDocumentAsync(shop_id, company_id);
                
                if (result == null)
                {
                    return base.Http404("Company");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                

                return base.Http200(new ItemResult<sdk.Company>()
                {
                    success = true, 
                    item = result
                });
            });
        }
        
        [HttpGet("for_shop/{shop_id}")]
        public Task<IActionResult> FindForShop(Guid shop_id, int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "", bool? disabled = null)
        {
            return base.ExecuteFunctionAsync<IActionResult>("FindForShop", async delegate()
            {
                Guid? shop_id_security = shop_id;
                this.Security.ValidateCanSearchCompany(this.GetCurrentAccount(), shop_id_security);
                
                ListResult<sdk.Company> result = await this.API.Store.Companies.FindForShopAsync(shop_id, skip, take, order_by, descending, keyword, disabled);
                
                result.success = true;
                return base.Http200(result);
            });
        }
        
        [HttpPost]
        public Task<IActionResult> Create(sdk.Company company)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Create", async delegate()
            {
                this.ValidateNotNull(company, "Company");

                dm.Company insert = company.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.Companies.Insert(insert);
                
                sdk.Company insertResult = await this.API.Store.Companies.GetDocumentAsync(insert.shop_id, insert.company_id);

                return base.Http200(new ItemResult<sdk.Company>()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        
        [HttpPut("{company_id}")]
        public Task<IActionResult> Update(Guid company_id, sdk.Company company)
        {
            return base.ExecuteFunctionAsync<IActionResult>("Update", async delegate()
            {
                this.ValidateNotNull(company, "Company");
                this.ValidateRouteMatch(company_id, company.company_id, "Company");

                dm.Company found = this.API.Direct.Companies.GetById(company.shop_id, company_id);
                this.ValidateNotNull(found, "Company");

                company.company_id = company_id;
                dm.Company update = company.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.Companies.Update(update);
                
                sdk.Company existing = await this.API.Store.Companies.GetDocumentAsync(update.shop_id, update.company_id);
                
                return base.Http200(new ItemResult<sdk.Company>()
                {
                    success = true,
                    item = existing
                });

            });

        }
        [HttpDelete("{shop_id}/{company_id}")]
        public IActionResult Delete(Guid shop_id, Guid company_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Company delete = this.API.Direct.Companies.GetById(shop_id, company_id);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.Companies.Delete(shop_id, company_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = company_id.ToString()
                });
            });
        }
        
        
       
        

    }
}

