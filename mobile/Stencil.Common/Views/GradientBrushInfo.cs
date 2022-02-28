using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Views
{
    public class GradientBrushInfo
    {
        public GradientBrushInfo()
        {
            this.Start = new PointInfo(0, 0);
            this.End = new PointInfo(0, 1);
        }
        public PointInfo Start { get; set; }
        public PointInfo End { get; set; }
        public GradientStopInfo[] Stops { get; set; }
    }
}
