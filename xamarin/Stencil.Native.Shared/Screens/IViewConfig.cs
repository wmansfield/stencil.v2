
namespace Stencil.Native.Screens
{
    public interface IViewConfig
    {
        string library { get; }
        string component { get; }
        string configuration_json { get; }
        ISectionConfig[] sections { get; set; }
    }
}
