using System;
using System.Linq;
using Placeholder.Data.Sql;
using Placeholder.Domain;
using db = Placeholder.Data.Sql.Models;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    public partial class ShopSettingBusiness
    {
        partial void PreProcess(ShopSetting shopsetting, Crud crud)
        {
            if (shopsetting.encrypted)
            {
                if (!string.IsNullOrEmpty(shopsetting.value))
                {
                    shopsetting.value_encrypted = this.API.EncryptData(string.Format("ShopSetting:{0}", shopsetting.name), shopsetting.value);
                    shopsetting.value = string.Empty;
                }
            }
        }
        protected void PreProcess(db.ShopSetting shopsetting, Crud crud)
        {
            if (shopsetting.encrypted)
            {
                if (!string.IsNullOrEmpty(shopsetting.value))
                {
                    shopsetting.value_encrypted = this.API.EncryptData(string.Format("ShopSetting:{0}", shopsetting.name), shopsetting.value);
                    shopsetting.value = string.Empty;
                }
            }
        }

        public ShopSetting Upsert(Guid shop_id, string name, string value, bool encrypt = false)
        {
            return base.ExecuteFunction("Upsert", delegate ()
            {
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }
                using (var database = base.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopSetting found = (from n in database.ShopSettings
                                           where n.shop_id == shop_id
                                           && n.name == name
                                           select n).FirstOrDefault();

                    ShopSetting match = found.ToDomainModel();

                    if (found == null)
                    {
                        found = new db.ShopSetting()
                        {
                            shop_setting_id = Guid.NewGuid(),
                            shop_id = shop_id,
                            name = name,
                            value = value,
                        };
                        database.ShopSettings.Add(found);
                    }
                    found.deleted_utc = null;
                    found.encrypted = encrypt;
                    found.value = value;
                    found.updated_utc = DateTimeOffset.UtcNow;
                    found.InvalidateSync(this.DefaultAgent, "changed");

                    this.PreProcess(found, Crud.Update);
                    database.SaveChanges();


                    this.Synchronizer.SynchronizeItem(new IdentityInfo(found.shop_setting_id, found.shop_id), Synchronization.Availability.Searchable);

                    this.DependencyCoordinator.ShopSettingInvalidated(Dependency.none, found.shop_setting_id, found.shop_id);

                    return this.GetById(shop_id, found.shop_setting_id);
                }
            });
        }

        public ShopSetting GetByName(Guid shop_id, string name)
        {
            return base.ExecuteFunction("GetByName", delegate ()
            {
                if(string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }
                using (var database = this.CreateSQLIsolatedContext(shop_id))
                {
                    db.ShopSetting result = (from n in database.ShopSettings
                                              where n.name == name
                                              && n.shop_id == shop_id
                                              select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }

        public string GetValueOrDefault(Guid shop_id, string name, string defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                string result = defaultValue;
                ShopSetting setting = this.GetByName(shop_id, name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    result = setting.GetValue(this.API);
                }
                return result;
            });

        }
        public int GetValueOrDefault(Guid shop_id, string name, int defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                int result = defaultValue;
                ShopSetting setting = this.GetByName(shop_id, name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    string value = setting.GetValue(this.API);
                    int.TryParse(value, out result);
                }
                return result;
            });

        }
        public decimal GetValueOrDefault(Guid shop_id, string name, decimal defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                decimal result = defaultValue;
                ShopSetting setting = this.GetByName(shop_id, name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    string value = setting.GetValue(this.API);
                    decimal.TryParse(value, out result);
                }
                return result;
            });

        }
        public bool GetValueOrDefault(Guid shop_id, string name, bool defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                bool result = defaultValue;
                ShopSetting setting = this.GetByName(shop_id, name);
                if (setting != null && (!string.IsNullOrEmpty(setting.value) || !string.IsNullOrEmpty(setting.value_encrypted)))
                {
                    string value = setting.GetValue(this.API);
                    bool.TryParse(value, out result);
                }
                return result;
            });

        }
        public DateTime? GetValueOrDefault(Guid shop_id, string name, DateTime? defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                DateTime? result = defaultValue;
                string foundValue = this.GetValueOrDefault(shop_id, name, string.Empty);
                if (!string.IsNullOrEmpty(foundValue))
                {
                    result = DateTime.Parse(foundValue);
                }
                return result;
            });
        }
        public long? GetValueOrDefault(Guid shop_id, string name, long? defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefault", delegate ()
            {
                long? result = defaultValue;
                string foundValue = this.GetValueOrDefault(shop_id, name, string.Empty);
                if (!string.IsNullOrEmpty(foundValue))
                {
                    result = long.Parse(foundValue);
                }
                return result;
            });
        }
        public string GetValueOrDefaultCached(Guid shop_id, string name, string defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefaultCached", delegate ()
            {
                ShopSetting setting = this.SharedCacheStatic15.PerLifetime(string.Format("ShopSettingCached_{0}:{1}", shop_id, name), delegate ()
                {
                    return this.GetByName(shop_id, name);
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

        public int GetValueOrDefaultCached(Guid shop_id, string name, int defaultValue)
        {
            return base.ExecuteFunction("GetValueOrDefaultCached", delegate ()
            {
                int result = defaultValue;
                string foundValue = this.GetValueOrDefaultCached(shop_id, name, string.Empty);
                if (!string.IsNullOrEmpty(foundValue))
                {
                    result = int.Parse(foundValue); // break if bad
                }
                return result;
            });
        }
    }
}
