using System;
using am = AutoMapper;

namespace Placeholder.Primary.Mapping
{
    public partial class PrimaryMappingProfile
    {
        partial void DbAndDomainMappings_Manual()
        {

        }
        partial void DomainAndSDKMappings_Manual()
        {
            this.CreateMap<Domain.Account, SDK.Models.Responses.AccountInfo>();
            this.CreateMap<SDK.Models.Account, SDK.Models.Responses.AccountInfo>();
        }
    }
}
