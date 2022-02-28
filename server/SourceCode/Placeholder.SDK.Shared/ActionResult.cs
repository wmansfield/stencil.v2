using System;
using System.Collections.Generic;

namespace Placeholder.SDK
{
    public class ActionResult
    {
        public ActionResult()
        {
        }
        public virtual bool success { get; set; }
        public virtual string message { get; set; }
        public virtual int? error_code { get; set; }
        public virtual Dictionary<string, object> errors { get; set; }
    }
}
