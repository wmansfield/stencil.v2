using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ui = Starter.App.Models;
using db = Starter.App.Data.Models;

namespace Starter.App.Data
{
    public static class _DatabaseMapping
    {
        public static JsonSerializerSettings DefaultSettings()
        {
            return new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None,
            };
        }

        #region Self

        public static db.Self ToDbModel(this ui.Self source, db.Self destination = null)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new db.Self(); }

            destination.json = JsonConvert.SerializeObject(source, _DatabaseMapping.DefaultSettings());

            return destination;
        }

        public static ui.Self ToUIModel(this db.Self source)
        {
            if (source == null) { return null; }

            return JsonConvert.DeserializeObject<ui.Self>(source.json);
        }

        #endregion

    }
}
