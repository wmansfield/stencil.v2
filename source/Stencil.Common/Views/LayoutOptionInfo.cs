using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Views
{
    public class LayoutOptionInfo
    {
        public bool expands { get; set; }
        public LayoutAlignmentInfo alignment { get; set; }
    }

    public enum LayoutAlignmentInfo
    {
        Start = 0x0,
        //
        // Summary:
        //     The center of an alignment.
        Center = 0x1,
        //
        // Summary:
        //     The end of an alignment. Usually the Bottom or Right.
        End = 0x2,
        //
        // Summary:
        //     Fill the entire area if possible.
        Fill = 0x3
    }
}
