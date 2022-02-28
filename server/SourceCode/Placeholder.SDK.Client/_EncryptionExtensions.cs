using System;
using System.Security.Cryptography;
using System.Text;

namespace Placeholder.SDK.Client
{
    public static class _EncryptionExtensions
    {
        public static string GenerateHash(this MD5 md5, string content)
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
