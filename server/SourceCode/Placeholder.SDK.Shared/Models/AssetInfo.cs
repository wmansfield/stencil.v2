using System;

namespace Placeholder.SDK.Models
{
    public class AssetInfo
    {
        public AssetInfo()
        {

        }
        public AssetInfo(string knownImage)
        {
            this.url_large = knownImage;
            this.url_small = knownImage;
        }
        public string url_small { get; set; }
        public string url_large { get; set; }
        public Guid asset_id { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.url_small))
            {
                return this.url_small;
            }
            if (!string.IsNullOrEmpty(this.url_large))
            {
                return this.url_large;
            }
            return string.Empty;
        }
    }
}
