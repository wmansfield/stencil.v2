
namespace Stencil.Native.Views
{
    public struct ThicknessInfo
    {
        public ThicknessInfo(double all)
        {
            this.top = all;
            this.bottom = all;
            this.left = all;
            this.right = all;
        }
        public ThicknessInfo(double horizontal, double vertical)
        {
            this.top = vertical;
            this.bottom = vertical;
            this.left = horizontal;
            this.right = horizontal;
        }
        public ThicknessInfo(double left, double top, double right, double bottom)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
        public double top { get; set; }
        public double bottom { get; set; }
        public double left { get; set; }
        public double right { get; set; }
    }
}
