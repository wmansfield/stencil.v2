using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public interface INavigationData
    {
        string screen { get; set; }
        string identifier { get; set; }
        Dictionary<string, string> data { get; set; }

    }
}
