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
        public string screen { get; set; }
        public string identifier { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}
