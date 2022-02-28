using System;
using System.Security.Cryptography;
using Placeholder.Common;

namespace Placeholder.Web.Security
{
    public static class HashedTimeSignatureVerifier
    {
        public static string CreateSignature(string apiKey, string apiSecret)
        {
            string prefix = string.Format("{0}{1}", apiKey, apiSecret);
            string unixUTCNow = ToUnixTime(DateTime.UtcNow).ToString();
            using (MD5 md5 = MD5.Create())
            {
                return md5.HashAsString(prefix + unixUTCNow);
            }
        }
        public static bool ValidateSignature(string key, string secret, string signature, int secondsRange = 300)
        {
            using (MD5 md5 = MD5.Create())
            {
                string prefix = string.Format("{0}{1}", key, secret);
                DateTime start = DateTime.UtcNow;
                DateTime end = start;
                for (int i = 0; i < secondsRange; i++)
                {
                    string startTime = ToUnixTime(start.AddSeconds(i)).ToString();
                    string endTime = ToUnixTime(end.AddSeconds(-i)).ToString();
                    if ((signature == md5.HashAsString(prefix + startTime))
                        || (signature == md5.HashAsString(prefix + endTime)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static long ToUnixTime(DateTime utcTime)
        {
            var unixTime = utcTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)unixTime.TotalSeconds;
        }

    }
}
