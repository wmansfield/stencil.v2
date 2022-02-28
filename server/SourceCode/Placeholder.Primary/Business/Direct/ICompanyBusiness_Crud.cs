using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface ICompanyBusiness
    {
        ICompanySynchronizer Synchronizer { get; }
        Company GetById(Guid shop_id, Guid company_id);
        List<Company> Find(Guid shop_id, int skip, int take, string keyword = "", string order_by = "", bool descending = false,  bool? disabled = null);
        int FindTotal(Guid shop_id, string keyword = "",  bool? disabled = null);
        
        List<Company> GetByShop(Guid shop_id);
        
        Company Insert(Company insertCompany);
        Company Insert(Company insertCompany, Availability availability);
        Company Update(Company updateCompany);
        Company Update(Company updateCompany, Availability availability);
        
        void Delete(Guid shop_id, Guid company_id);
        
        void SynchronizationUpdate(Guid shop_id, Guid company_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid shop_id, Guid company_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid shop_id, Guid company_id, string reason);
    }
}

