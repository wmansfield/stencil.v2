using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Markdown
{
    public class AnnotatedTextItem
    {
        public AnnotatedTextItem()
        {

        }
        public AnnotatedTextItem(string source)
        {
            _source = source;
        }
        public string text { get; set; }
        public string extra { get; set; }
        public List<TextAnnotation> annotations { get; set; }

        /// <summary>
        /// Typed object used by the view component, not serialized
        /// </summary>
        [JsonIgnore]
        public object PreparedData { get; set; }


        [JsonIgnore]
        public object ui_data { get; set; }

        [JsonIgnore]
        public object ui_text { get; set; }

        [JsonIgnore]
        public List<TextAnnotation> ui_links { get; set; }

        [JsonIgnore]
        public float? ui_height { get; set; }


        private string _source = string.Empty;
        public string GetSource()
        {
            return _source;
        }
        public void SetSource(string value)
        {
            _source = value;
        }
    }
}
