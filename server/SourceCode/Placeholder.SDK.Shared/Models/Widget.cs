using System;
using System.Collections.Generic;
using System.Text;
using Stencil.Common.Markdown;

namespace Placeholder.SDK.Models
{
    public partial class Widget : SDKModel
    {	
        public Widget()
        {
				
        }

        public virtual Guid widget_id { get; set; }
        public virtual Guid shop_id { get; set; }
        public virtual DateTime stamp_utc { get; set; }
        public virtual string text { get; set; }
        public virtual string payload { get; set; }
        
        /// <summary>
        /// Index Only
        /// </summary>
        public MarkdownSection[] sections { get; set; }
        
	}
}

