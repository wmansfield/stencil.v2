
namespace Stencil.Native.Views
{
    public class ThicknessInfo
    {
        public ThicknessInfo()
        {

        }
        public ThicknessInfo(double all)
        {
            this.top = all;
            this.bottom = all;
            this.left = all;
            this.right = all;
        }
        public double top { get; set; }
        public double bottom { get; set; }
        public double left { get; set; }
        public double right { get; set; }
    }
}
