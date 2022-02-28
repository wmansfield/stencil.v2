using System;
using Placeholder.Common.Configuration;

namespace Placeholder.Common
{
    public static class _ConfigurationExtensions
    {
        public static bool IsLocalHost(this ISettingsResolver settingsResolver)
        {
            if (settingsResolver != null)
            {
                try
                {
                    return bool.Parse(settingsResolver.GetSetting(CommonAssumptions.APP_KEY_IS_LOCAL_HOST));
                }
                catch { }
            }
            return false;
        }
        public static bool IsDebugHealth(this ISettingsResolver settingsResolver)
        {
            if (settingsResolver != null)
            {
                try
                {
                    return bool.Parse(settingsResolver.GetSetting(CommonAssumptions.APP_KEY_HEALTH_DEBUG));
                }
                catch { }
            }
            return false;
        }

        public static bool IsBackPane(this ISettingsResolver settingsResolver)
        {
            return settingsResolver.GetSetting(CommonAssumptions.APP_KEY_IS_BACKING) == "true";
        }

        public static bool IsHydrate(this ISettingsResolver settingsResolver)
        {
            return settingsResolver.GetSetting(CommonAssumptions.APP_KEY_IS_HYDRATE) == "true";
        }
    }
}
