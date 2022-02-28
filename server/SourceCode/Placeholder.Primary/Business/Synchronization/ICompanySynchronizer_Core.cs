using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sdk = Placeholder.SDK.Models;
using dm = Placeholder.Domain;

namespace Placeholder.Primary.Business.Synchronization
{
    public partial interface ICompanySynchronizer : ISynchronizer
    {
        void SynchronizeItem(IdentityInfo primaryKey, Availability availability);
        sdk.Company AdHocHydrate(dm.Company domainModel);
        
    }
}

