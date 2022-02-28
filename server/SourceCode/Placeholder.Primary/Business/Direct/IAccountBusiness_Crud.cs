using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IAccountBusiness
    {
        IAccountSynchronizer Synchronizer { get; }
        Account GetById(Guid account_id);
        List<Account> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        int FindTotal(string keyword = "");
        
        Account Insert(Account insertAccount);
        Account Insert(Account insertAccount, Availability availability);
        Account Update(Account updateAccount);
        Account Update(Account updateAccount, Availability availability);
        
        void Delete(Guid account_id);
        
        void SynchronizationUpdate(Guid account_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid account_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid account_id, string reason);
    }
}

