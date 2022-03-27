using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Markdown
{
    public class AssetData
    {
        public AssetData()
        {

        }
        public string identifier { get; set; }
        public string dimensions { get; set; }
        public string url { get; set; }
        public string cover_url { get; set; }
    }
}
