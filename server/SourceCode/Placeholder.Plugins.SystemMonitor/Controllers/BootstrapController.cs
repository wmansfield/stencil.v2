using System;
using Microsoft.AspNetCore.Mvc;
using Zero.Foundation;
using Zero.Foundation.Web;
using Placeholder.Primary;
using Placeholder.Domain;

namespace Placeholder.Plugins.SystemMonitor.Controllers
{
    public class BootstrapController : FoundationController
    {
        public BootstrapController(IFoundation foundation)
            : base(foundation)
        {
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None )]
        public IActionResult Index()
        {
            return base.ExecuteFunction("Index", delegate ()
            {
                PlaceholderAPI API = this.IFoundation.Resolve<PlaceholderAPI>();

                string result = string.Empty;
                try
                {
                    Account newAccount1 = new Account()
                    {
                        account_id = new Guid("{0AB9F424-DF92-4678-8AB7-68F186E1C497}"),// hard code for super safety
                        first_name = "William",
                        last_name = "Mansfield",
                        account_display = "William Mansfield",
                        email = "wmansfield@foundationzero.com",
                        password = "Magic1!",
                        entitlements = "super_admin",
                        password_changed_utc = DateTime.UtcNow,
                        account_status = AccountStatus.enabled
                    };
                    Account initialAccount = API.Direct.Accounts.CreateInitialAccounts(newAccount1);
                    if (initialAccount != null)
                    {
                        result += "Created new user.";
                    }
                    else
                    {
                        result += "Ping";
                    }

                    Tenant newTenant = new Tenant()
                    {
                        tenant_id = new Guid("2373A8F6-267C-43FA-8D2D-8EBA84EF9B23"),
                        tenant_code = "primary",
                        tenant_name = "Primary Tenant"
                    };

                    Tenant initialTenant = API.Direct.Tenants.CreateInitialTenant(newTenant);

                    if (initialAccount != null)
                    {
                        result += " Created new tenant.";
                    }
                    else
                    {
                        result += " Pong";
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
               
                ViewBag.Result = result;
                return View();
            });
        }
    }
}
