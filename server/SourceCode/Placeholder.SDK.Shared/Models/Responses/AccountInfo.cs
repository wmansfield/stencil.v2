using System;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Models.Responses
{
    /// <summary>
    /// Auth is special, usually you want to include some extra app-specific results
    /// So this class is used as a non-envelop result
    /// </summary>
    public partial class AccountInfo : SDKModel
    {
        public AccountInfo()
        {
        }

        //<FromModel>
        public Guid account_id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string account_display { get; set; }
        public string api_key { get; set; }
        public string api_secret { get; set; }
        public AccountStatus account_status { get; set; }

        //<FromModel>

        //<OnDemand>
        public bool admin { get; set; }
        public bool super_admin { get; set; }
        public bool impersonated { get; set; }

        public ShopAccount[] shops { get; set; }

        //</OnDemand>
    }
}
