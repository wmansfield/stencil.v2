using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IAssetBusiness
    {
        Asset GetById(Guid asset_id);
        List<Asset> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        int FindTotal(string keyword = "");
        
        Asset Insert(Asset insertAsset);
        Asset Insert(Asset insertAsset, Availability availability);
        Asset Update(Asset updateAsset);
        Asset Update(Asset updateAsset, Availability availability);
        
        void Delete(Guid asset_id);
        
        
    }
}

