using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public interface INavigationData
    {
        string screen_name { get; set; }
        string screen_parameter { get; set; }
        DateTime? last_retrieved_utc { get; set; }
        Dictionary<string, string> data { get; set; }

    }
}
