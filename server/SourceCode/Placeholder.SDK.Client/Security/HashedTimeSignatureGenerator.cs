using System;
using System.Security.Cryptography;

namespace Placeholder.SDK.Client.Security
{
    public partial class HashedTimeSignatureGenerator
    {
        public virtual string CreateSignature(string apiKey, string apiSecret)
        {
            string prefix = string.Format("{0}{1}", apiKey, apiSecret);
            string unixUTCNow = GetUnixTime().ToString();
            return MD5.Create().GenerateHash(prefix + unixUTCNow);

        }

        protected virtual long GetUnixTime()
        {
            var unixTime = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)unixTime.TotalSeconds;
        }
    }
}
