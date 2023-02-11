
namespace Stencil.Common.Screens
{
    public class ViewConfig : IViewConfig
    {
        public string library { get; set; }
        public string component { get; set; }
        public string tag { get; set; }
        public string configuration_json { get; set; }
        public ISectionConfig[] sections { get; set; }
        public IViewConfig[] encapsulated_views { get; set; }

    }
}
