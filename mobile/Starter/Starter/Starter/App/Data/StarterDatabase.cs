using Realms;
using Stencil.Maui.Data;
using System;
using System.Collections.Generic;
using System.Text;
using ui = Starter.App.Models;
using db = Starter.App.Data.Models;
using System.Linq;
using Newtonsoft.Json;
using Starter.App.Models;
using System.Reflection.Metadata;

namespace Starter.App.Data
{
    public class StarterDatabase : StencilDatabase, IStarterDatabase
    {
        public StarterDatabase(Realm realm)
            : base(realm)
        {

        }


        #region Self

        public ui.Self SelfRetrieve()
        {
            return base.ExecuteFunction("SelfRetrieve", delegate ()
            {
                try
                {
                    return this.Realm.Find<db.Self>(db.Self.SINGLE_ID).ToUIModel();

                }
                catch (Exception)
                {

                    throw;
                }
            });
        }

        public ui.Self SelfUpsert(ui.Self self)
        {
            return base.ExecuteFunction("SelfUpsert", delegate ()
            {
                db.Self existing = this.Realm.Find<db.Self>(db.Self.SINGLE_ID);

                this.Realm.Write(delegate ()
                {
                    db.Self upsert = self.ToDbModel(existing);
                    if (existing == null)
                    {
                        this.Realm.Add(upsert);
                    }
                });
                return this.SelfRetrieve();
            });
        }
        public ui.Self SelfUpdate(Action<ui.Self> operation)
        {
            return base.ExecuteFunction("SelfUpdate", delegate ()
            {
                db.Self existing = this.Realm.Find<db.Self>(db.Self.SINGLE_ID);
                if (existing != null)
                {
                    this.Realm.Write(delegate ()
                    {
                        ui.Self appExisting = existing.ToUIModel();
                        operation(appExisting);
                        appExisting.ToDbModel(existing);
                    });
                }
                return existing.ToUIModel();
            });
        }

        #endregion


        #region Settings

        public string SettingRetrieve(string key, string defaultValue)
        {
            return base.ExecuteFunction("SettingRetrieve", delegate ()
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return defaultValue;
                }

                db.Setting found = this.Realm.Find<db.Setting>(key);
                if (found != null)
                {
                    return found.value;
                }
                return defaultValue;
            });
        }

        public void SettingUpsert(string key, string value)
        {
            base.ExecuteMethod("SettingUpsert", delegate ()
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return;
                }
                db.Setting found = this.Realm.Find<db.Setting>(key);

                this.Realm.Write(delegate ()
                {
                    db.Setting upsert = found;
                    if (upsert == null)
                    {
                        upsert = new db.Setting()
                        {
                            id = key
                        };
                    }
                    upsert.value = value;

                    if (found == null)
                    {
                        this.Realm.Add(upsert);
                    }
                });
            });
        }

        #endregion

    }
}
