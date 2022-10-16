using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Stencil.Maui.iOS.Markdown
{
    public partial class CacheModel
    {
        [JsonIgnore]
        public Dictionary<string, string> Tag { get; set; }

        [JsonIgnore]
        public NSMutableAttributedString ui_text { get; set; }

        [JsonIgnore]
        public float? ui_height { get; set; }

        public void UIClear()
        {
            if (this.Tag != null)
            {
                this.ui_height = null;
                this.ui_text = null;
            }
        }

        public void MarkdownClear()
        {
            this.ui_height = null;
            this.ui_text = null;
            this.TagRemove("markdownGenerated");
        }
        public void MarkdownSetGenerated(bool generated = true)
        {
            this.TagSet("markdownGenerated", generated.ToString().ToLower());
        }
        public bool MarkdownHasGenerated()
        {
            return this.TagGetAsBool("markdownGenerated", false);
        }
        public void TagClear()
        {
            if (this.Tag != null)
            {
                this.Tag.Clear();
            }
        }
        public bool TagExists(string key)
        {
            if (this.Tag == null)
            {
                return false;
            }
            return this.Tag != null && this.Tag.ContainsKey(key);
        }
        public bool TagRemove(string key)
        {
            if (this.Tag == null)
            {
                return false;
            }
            return this.Tag.Remove(key);
        }
        public string TagGet(string key, string defaultValue)
        {
            if (this.Tag != null && this.Tag.ContainsKey(key))
            {
                return this.Tag[key];
            }
            return defaultValue;
        }
        public int TagGetAsInt(string key, int defaultValue)
        {
            if (this.Tag != null && this.Tag.ContainsKey(key) && !string.IsNullOrEmpty(this.Tag[key]))
            {
                int result = 0;
                if (int.TryParse(this.Tag[key], out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }
        public bool TagGetAsBool(string key, bool defaultValue)
        {
            if (this.Tag != null && this.Tag.ContainsKey(key) && !string.IsNullOrEmpty(this.Tag[key]))
            {
                bool result = false;
                if (bool.TryParse(this.Tag[key], out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }
        public void TagSet(string key, bool value)
        {
            if (this.Tag == null)
            {
                this.Tag = new Dictionary<string, string>();
            }
            this.Tag[key] = value.ToString();
        }
        public void TagSet(string key, string value)
        {
            if (this.Tag == null)
            {
                this.Tag = new Dictionary<string, string>();
            }
            this.Tag[key] = value;
        }
        public string ui_tag { get; set; }
    }
}