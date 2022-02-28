using System;
using System.Collections.Generic;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
     public partial interface IGlobalSettingBusiness
    {
        GlobalSetting GetByName(string name);
        List<GlobalSetting> FindWithPrefix(string prefix);

        IDictionary<string, TimeZoneInfo> TimeZonesGetAll();
        IDictionary<string, TimeZoneInfo> TimeZonesGetUsa();

        bool GetValueOrDefault(string name, bool defaultValue);
        decimal GetValueOrDefault(string name, decimal defaultValue);
        int GetValueOrDefault(string name, int defaultValue);
        string GetValueOrDefault(string name, string defaultValue);
        DateTime? GetValueOrDefault(string name, DateTime? defaultValue);

        string GetValueOrDefaultCached(string name, string defaultValue);
        int GetValueOrDefaultCached(string name, int defaultValue);
    }
}
