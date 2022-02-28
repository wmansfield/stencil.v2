using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Common.Configuration.Implementations
{
    public class ConfigurationSettingsResolver : ChokeableClass, ISettingsResolver
    {
        public ConfigurationSettingsResolver(IFoundation foundation)
            : base(foundation)
        {

        }

        protected static ConcurrentDictionary<string, string> SettingsCache = new ConcurrentDictionary<string, string>();

        public virtual string GetSetting(string name)
        {
            return base.ExecuteFunction(nameof(GetSetting), delegate ()
            {
                if (SettingsCache.ContainsKey(name))
                {
                    return SettingsCache[name];
                }
                string result = this.PerformGetSetting(name);
                if (!string.IsNullOrEmpty(result))
                {
                    SettingsCache[name] = result;
                }
                return result;
            });

        }
        protected virtual string PerformGetSetting(string name)
        {
            return base.ExecuteFunction(nameof(PerformGetSetting), delegate()
            {
                IConfiguration configuration = this.IFoundation.SafeResolve<IConfiguration>();
                string result = configuration.GetConnectionString(name);
                if(string.IsNullOrWhiteSpace(result))
                {
                    result = configuration.GetSection(CommonAssumptions.APP_KEY_SECTION_NAME)?[name];
                }
                return result;
            });
        }
    }
}
