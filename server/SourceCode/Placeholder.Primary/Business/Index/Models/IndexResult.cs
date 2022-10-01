using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Index
{
    public struct IndexResult
    {
        public bool success { get; set; }
        public string version { get; set; }
        public string error { get; set; }
        public int attempts { get; set; }
        public override string ToString()
        {
            if (this.success)
            {
                return string.Format("Success: version({0}), attempts({1})", this.version, this.attempts);
            }
            else
            {
                if (string.IsNullOrEmpty(this.error))
                {
                    return "Error: Unknown.";
                }
                else
                {
                    return "Error: " + this.error;
                }
            }
        }
    }
}
