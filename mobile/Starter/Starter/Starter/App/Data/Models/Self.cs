using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Data.Models
{
    public class Self : RealmObject // Don't inherit, custom id mapping //, IDatabaseModel
    {
        public Self()
        {

        }
        public static string SINGLE_ID = "00000000-0000-0000-0000-000000000000";

        [PrimaryKey]
        [Obsolete("Never Change This", true)]
        public string id { get; set; } = SINGLE_ID;

        public string json { get; set; }
    }
}
