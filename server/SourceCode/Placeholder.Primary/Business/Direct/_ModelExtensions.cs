using System;
using dm = Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
    public static class _ModelExtensions
    {
        public static string GetValue(this dm.GlobalSetting setting, PlaceholderAPI api)
        {
            if (setting != null)
            {
                if (string.IsNullOrEmpty(setting.value))
                {
                    if (setting.encrypted && !string.IsNullOrEmpty(setting.value_encrypted))
                    {
                        setting.value = api.DecryptData(string.Format("GlobalSetting:{0}", setting.name), setting.value_encrypted);
                    }
                }

                return setting.value;
            }
            return string.Empty;
        }
        public static string GetValue(this dm.ShopSetting setting, PlaceholderAPI api)
        {
            if (setting != null)
            {
                if (string.IsNullOrEmpty(setting.value))
                {
                    if (setting.encrypted && !string.IsNullOrEmpty(setting.value_encrypted))
                    {
                        setting.value = api.DecryptData(string.Format("ShopSetting:{0}", setting.name), setting.value_encrypted);
                    }
                }

                return setting.value;
            }
            return string.Empty;
        }

    }
}
