using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Data.Models
{
    public class Setting : RealmObject
    {
        public Setting()
        {

        }

        [PrimaryKey]
        public string id { get; set; }

        public string value { get; set; }
    }
}
