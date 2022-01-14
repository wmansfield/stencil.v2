using Realms;
using Stencil.Forms.Screens;
using System;

namespace Stencil.Forms.Data.Models
{
    public class ScreenConfig : RealmObject, IDatabaseModel
    {
        public ScreenConfig()
        {
        }

        /// <summary>
        /// ScreenStorageKey
        /// </summary>
        [PrimaryKey]
        public string id { get; set; }
        public string screen_name { get; set; }
        public string screen_parameter { get; set; }
        public string screen_navigation_data { get; set; }
        public bool suppress_persist { get; set; }
        public bool automatic_download { get; set; }
        public bool is_menu_supported { get; set; }
        public int lifetime { get; set; }


        /// <summary>
        /// effecitvely json_view_configs
        /// </summary>
        public string json { get; set; }

        public string json_visual_config { get; set; }
        public string json_menu { get; set; }
        public string json_show_commands { get; set; }
        public string json_header_configs { get; set; }
        public string json_footer_configs { get; set; }


        public DateTimeOffset? invalidated_utc { get; set; }
        public DateTimeOffset? download_utc { get; set; }
        public DateTimeOffset? cache_until_utc { get; set; }
        public DateTimeOffset? expire_utc { get; set; }

    }
}
