using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Markdown
{
    public class AnnotatedTextItem
    {
        public AnnotatedTextItem()
        {

        }
        public string text { get; set; }
        public List<TextAnnotation> annotations { get; set; }

        /// <summary>
        /// Typed object used by the view component, not serialized
        /// </summary>
        [JsonIgnore]
        public object PreparedData { get; set; }
    }
}
