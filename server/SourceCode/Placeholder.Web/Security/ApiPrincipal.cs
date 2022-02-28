using System;
using System.Security.Principal;

namespace Placeholder.Web.Security
{
    public class ApiPrincipal : GenericPrincipal
    {
        public ApiPrincipal(IIdentity identity, Guid appId, string appName, bool isCustomer)
            : base(identity, (string[])null)
        {
            this.ApiID = appId;
            this.ApiName = appName;
            this.IsCustomer = IsCustomer;
        }
        public bool IsCustomer { get; set; }
        public Guid ApiID { get; set; }
        public string ApiName { get; set; }
    }
}
