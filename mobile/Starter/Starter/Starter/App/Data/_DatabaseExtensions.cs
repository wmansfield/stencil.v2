using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ui = Starter.App.Models;

namespace Starter.App.Data
{
    public static class _DatabaseExtensions
    {
        public static bool? SettingRetrieve(this IStarterDatabase database, string key, bool? defaultValue)
        {
            if (database == null)
            {
                return defaultValue;
            }
            string value = database.SettingRetrieve(key, string.Empty);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            if (bool.TryParse(value, out bool parsed))
            {
                return parsed;
            }
            return defaultValue;
        }
        public static bool SettingRetrieve(this IStarterDatabase database, string key, bool defaultValue)
        {
            if (database == null)
            {
                return defaultValue;
            }
            string value = database.SettingRetrieve(key, string.Empty);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            if (bool.TryParse(value, out bool parsed))
            {
                return parsed;
            }
            return defaultValue;
        }
        public static DateTime SettingRetrieve(this IStarterDatabase database, string key, DateTime defaultValueUTC)
        {
            if (database == null)
            {
                return defaultValueUTC;
            }
            string value = database.SettingRetrieve(key, string.Empty);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValueUTC;
            }
            if (DateTime.TryParse(value, null, DateTimeStyles.AdjustToUniversal, out DateTime parsed))
            {
                return parsed;
            }
            return defaultValueUTC;
        }
        public static void SettingUpsert(this IStarterDatabase database, string key, bool? value)
        {
            if (database == null)
            {
                return;
            }
            if (!value.HasValue)
            {
                database.SettingUpsert(key, string.Empty);
            }
            else
            {
                database.SettingUpsert(key, value.Value.ToString());
            }
        }
        public static void SettingUpsert(this IStarterDatabase database, string key, bool value)
        {
            if (database == null)
            {
                return;
            }
            database.SettingUpsert(key, value.ToString());
        }
        public static void SettingUpsert(this IStarterDatabase database, string key, DateTime valueUTC)
        {
            if (database == null)
            {
                return;
            }
            database.SettingUpsert(key, valueUTC.ToString("u"));
        }
    }
}
