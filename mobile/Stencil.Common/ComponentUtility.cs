using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common
{
    public static class ComponentUtility
    {
        public static string VERSION_FORMAT = "{0}.v{1}";

        public static string GenerateVersionedName(string component, string version)
        {
            return string.Format(VERSION_FORMAT, component, version);
        }
    }
}
