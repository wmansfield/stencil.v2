using Realms;
using System;

namespace Stencil.Native.Data.Models
{
    public class ScreenConfig : RealmObject, IDatabaseModel
    {
        public ScreenConfig()
        {
        }

        [PrimaryKey]
        public string id { get; set; }
        public string screen_name { get; set; }
        public string screen_parameter { get; set; }
        public bool suppress_persist { get; set; }
        public bool automatic_download { get; set; }
        public bool is_menu_supported { get; set; }

        /// <summary>
        /// effecitvely json_view_configs
        /// </summary>
        public string json { get; set; }

        public string json_visual_config { get; set; }
        public string json_menu { get; set; }
        public string json_show_commands { get; set; }

        public DateTimeOffset? invalidated_utc { get; set; }
        public DateTimeOffset? download_utc { get; set; }
        public DateTimeOffset? cache_until_utc { get; set; }
        public DateTimeOffset? expire_utc { get; set; }

    }
}
