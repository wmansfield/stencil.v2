using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Markdown
{
    public class TextAnnotation
    {
        public int start { get; set; }
        public int end { get; set; }
        public string type { get; set; }
        public string target { get; set; }
        /// <summary>
        /// Not always respected
        /// </summary>
        public string color { get; set; }
    }
}
