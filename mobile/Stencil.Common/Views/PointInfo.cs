using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Views
{
    public struct PointInfo
    {
        public PointInfo(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
        public double x { get; set; }
        public double y { get; set; }
    }
}
