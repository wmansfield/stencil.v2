using Realms;

namespace Stencil.Native.Data.Models
{
    public class ScreenConfig : RealmObject, IDatabaseModel
    {
        public ScreenConfig()
        {
        }

        [PrimaryKey]
        public string id { get; set; }
        public bool suppress_persist { get; set; }
        public bool is_menu_supported { get; set; }
        public string margin { get; set; }
        public string background_color_hex { get; set; }

        public string json { get; set; }
    }
}
