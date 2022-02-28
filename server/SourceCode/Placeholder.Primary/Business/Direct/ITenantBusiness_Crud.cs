using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface ITenantBusiness
    {
        Tenant GetById(Guid tenant_id);
        List<Tenant> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        int FindTotal(string keyword = "");
        
        Tenant Insert(Tenant insertTenant);
        Tenant Insert(Tenant insertTenant, Availability availability);
        Tenant Update(Tenant updateTenant);
        Tenant Update(Tenant updateTenant, Availability availability);
        
        void Delete(Guid tenant_id);
        
        
    }
}

