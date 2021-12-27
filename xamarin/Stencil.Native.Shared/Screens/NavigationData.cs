using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Screens
{
    public class NavigationData : INavigationData
    {
        public NavigationData()
        {

        }
        public string screen_name { get; set; }
        public string screen_parameter { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}
