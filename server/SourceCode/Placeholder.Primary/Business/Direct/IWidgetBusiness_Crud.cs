using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IWidgetBusiness
    {
        IWidgetSynchronizer Synchronizer { get; }
        Widget GetById(Guid shop_id, Guid widget_id);
        
        List<Widget> GetByShop(Guid shop_id);
        
        Widget Insert(Widget insertWidget);
        Widget Insert(Widget insertWidget, Availability availability);
        Widget Update(Widget updateWidget);
        Widget Update(Widget updateWidget, Availability availability);
        
        void Delete(Guid shop_id, Guid widget_id);
        
        void SynchronizationUpdate(Guid shop_id, Guid widget_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid shop_id, Guid widget_id, bool success, DateTime sync_date_utc, string sync_log);
        List<IdentityInfo> SynchronizationHydrateGetInvalid(string tenant_code, int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid shop_id, Guid widget_id, string reason);
    }
}

