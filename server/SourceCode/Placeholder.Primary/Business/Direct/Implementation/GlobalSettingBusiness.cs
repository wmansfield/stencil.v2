using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Placeholder.Domain;
using db = Placeholder.Data.Sql.Models;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    public partial class GlobalSettingBusiness
    {
        
        partial void PreProcess(GlobalSetting globalsetting, Crud crud)
        {
            if (globalsetting.encrypted)
            {
                if (!string.IsNullOrEmpty(globalsetting.value))
                {
                    globalsetting.value_encrypted = this.API.EncryptData(string.Format("GlobalSetting:{0}", globalsetting.name), globalsetting.value);
                    globalsetting.value = string.Empty;
                }
            }
        }
        public IDictionary<string, TimeZoneInfo> TimeZonesGetAll()
        {
            return base.ExecuteFunction<IDictionary<string, TimeZoneInfo>>("TimeZonesGetAll", delegate ()
            {
                return this.SharedCacheStatic15.PerLifetime("GlobalSettingBusinessTimeZonesGetAll", delegate ()
                {
                    ConcurrentDictionary<string, TimeZoneInfo> result = new ConcurrentDictionary<string, TimeZoneInfo>(StringComparer.OrdinalIgnoreCase);
                    HashSet<string> preventedTimeZones = new HashSet<string>(); //TODO:SHOULD:Have an exclusion list for timezones
                    List<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones().ToList();
                    foreach (var item in timeZones)
                    {
                        if (item != null && !string.IsNullOrEmpty(item.StandardName) && !preventedTimeZones.Contains(item.StandardName))
                        {
                            result[item.StandardName] = item;
                        }
                    }
                    return result;
                });

            });
        }
        public IDictionary<string, TimeZoneInfo> TimeZonesGetUsa()
        {
            return base.ExecuteFunction<IDictionary<string, TimeZoneInfo>>("TimeZonesGetUsa", delegate ()
            {
                return this.SharedCacheStatic15.PerLifetime("GlobalSettingBusinessTimeZonesGetUsa", delegate ()
                {
                    string usaZones = this.GetValueOrDefaultCached("usa_time_zone_list", "Aleutian Standard Time,Hawaiian Standard Time,Alaskan Standard Time,Pacific Standard Time,US Mountain Standard Time,Mountain Standard Time,Central Standard Time,Eastern Standard Time,US Eastern Standard Time").ToLower();
                    List<string> usaTimeZones = new List<string>(usaZones.Split(','));

                    ConcurrentDictionary<string, TimeZoneInfo> result = new ConcurrentDictionary<string, TimeZoneInfo>(StringComparer.OrdinalIgnoreCase);
                    HashSet<string> preventedTimeZones = new HashSet<string>(); //TODO:SHOULD:Have an exclusion list for timezones
                    List<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones().ToList();
                    foreach (var item in timeZones)
                    {
                        if (item != null && !string.IsNullOrEmpty(item.StandardName) && !preventedTimeZones.Contains(item.StandardName))
                        {
                            if (usaTimeZones.Contains(item.StandardName.ToLower()))
                            {
                                result[item.StandardName] = item;
                            }
                        }
                    }
                    return result;
                });

            });
        }
        public GlobalSetting GetByName(string name)
        {
            return base.ExecuteFunction("GetByName", delegate ()
            {
                using (var database = this.CreateSQLSharedContext())
                {
                    db.GlobalSetting result = (from n in database.GlobalSettings
                                              where (n.name == name)
                                              select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<GlobalSetting> FindWithPrefix(string prefix)
        {
            return base.ExecuteFunction("FindWithPrefix", delegate ()
            {
                using (var database = this.CreateSQLSharedContext())
                {
                    var result = (from n in database.GlobalSettings
                                  where (n.name.StartsWith(prefix))
                                  select n);
                    return result.ToDomainModel();
                }
            });
        }


        public string GetValueOrDefault(string name, string defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                string result = defaultValue;
                GlobalSetting setting = this.GetByName(name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    result = setting.GetValue(this.API);
                }
                return result;
            });
            
        }
        public int GetValueOrDefault(string name, int defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                int result = defaultValue;
                GlobalSetting setting = this.GetByName(name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    string value = setting.GetValue(this.API);
                    int.TryParse(value, out result);
                }
                return result;
            });
            
        }
        public decimal GetValueOrDefault(string name, decimal defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                decimal result = defaultValue;
                GlobalSetting setting = this.GetByName(name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    string value = setting.GetValue(this.API);
                    decimal.TryParse(value, out result);
                }
                return result;
            });
            
        }
        public bool GetValueOrDefault(string name, bool defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                bool result = defaultValue;
                GlobalSetting setting = this.GetByName(name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    string value = setting.GetValue(this.API);
                    bool.TryParse(value, out result);
                }
                return result;
            });
           
        }
        public DateTime? GetValueOrDefault(string name, DateTime? defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                DateTime? result = null;
                string foundValue = this.GetValueOrDefault(name, string.Empty);
                if (!string.IsNullOrEmpty(foundValue))
                {
                    result = DateTime.Parse(foundValue); // break if bad date
                }
                return result;
            });
        }

        public string GetValueOrDefaultCached(string name, string defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefaultCached", delegate ()
            {
                GlobalSetting setting = this.SharedCacheStatic15.PerLifetime(string.Format("GlobalSettingCached_{0}", name), delegate ()
                {
                    return this.GetByName(name);
                });

                string result = defaultValue;
                if (setting != null)
                {
                    string foundValue = setting.GetValue(this.API);
                    if (!string.IsNullOrEmpty(foundValue))
                    {
                        result = foundValue;
                    }
                }
                return result;
            });
        }

        public int GetValueOrDefaultCached(string name, int defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefaultCached", delegate ()
            {
                int result = defaultValue;
                string foundValue = this.GetValueOrDefaultCached(name, string.Empty);
                if (!string.IsNullOrEmpty(foundValue))
                {
                    result = int.Parse(foundValue); // break if bad
                }
                return result;
            });
        }

    }
}
