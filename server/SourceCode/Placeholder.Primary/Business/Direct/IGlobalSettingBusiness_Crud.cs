using System;
using System.Collections.Generic;
using System.Text;
using Placeholder.Domain;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IGlobalSettingBusiness
    {
        GlobalSetting GetById(Guid global_setting_id);
        List<GlobalSetting> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        int FindTotal(string keyword = "");
        
        GlobalSetting Insert(GlobalSetting insertGlobalSetting);
        GlobalSetting Insert(GlobalSetting insertGlobalSetting, Availability availability);
        GlobalSetting Update(GlobalSetting updateGlobalSetting);
        GlobalSetting Update(GlobalSetting updateGlobalSetting, Availability availability);
        
        void Delete(Guid global_setting_id);
        
        
    }
}

