using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Placeholder.Common;
using Zero.Foundation;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary
{
    public static class PrimaryUtility
    {
        public static TEntity JsonClone<TEntity>(TEntity entity)
            where TEntity : class
        {
            if(entity == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(entity));
        }
        public static TResult JsonCloneAs<TResult>(object entity)
        {
            if (entity == null)
            {
                return default(TResult);
            }
            return JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject(entity));
        }

        public static Regex RegexCleanHTMLHelper = new Regex(@"\s+");

        private static string[] _badValues = null;
        private static string[] GetBadValues(PlaceholderAPI api)
        {
            if (_badValues == null)
            {
                _badValues = api.Direct.GlobalSettings.GetValueOrDefault("xss_values", "").ToLower().Split(',');
            }
            return _badValues;
        }

        public static string HashSHA256(byte[] bytes)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(bytes);

                string hashString = string.Empty;
                foreach (byte x in data)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                return hashString;
            }
        }

        public static string SanitizeHtml(PlaceholderAPI api, string entity, Guid identifier, string fieldName, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string result = text;
            if (fieldName.Contains("url"))
            {
                result = HttpUtility.UrlEncode(text);
            }
            else
            {
                string test = RegexCleanHTMLHelper.Replace(text, "");
                string[] badValues = GetBadValues(api);
                foreach (var item in badValues)
                {
                    if (test.Contains(item))
                    {
                        result = HttpUtility.HtmlEncode(text);
                        break;
                    }
                }
            }

            if (result != text)
            {
                //TODO:COULD: api.Integration.Email.SendAdminEmail("Possible XSS Injection", string.Format("Please review the entity {1}:{0}.{2} for possible injection attacks.", entity, identifier, fieldName));
            }
            result = result.Replace("&#39;", "'"); // we allow apostrophe, we dont general manual sql
            return result;
        }


        public static string ConstructAssetPath_ForShop(Guid shop_id, Guid asset_id, string reason, string filename)
        {
            string shop = ShortGuid.Encode(shop_id);
            string asset = ShortGuid.Encode(asset_id);
            string intent = CommonUtility.CleanFileName(reason);
            string name = CommonUtility.CleanFileName(filename);

            return $"placeholder/shops/{shop}/{intent}/{asset}/{name}";
        }

        private static Random RANDOM;

        public static void ShuffleFisherYates<TData>(TData[] array)
        {
            if (RANDOM == null)
            {
                RANDOM = new Random();
            }
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + RANDOM.Next(n - i);
                TData t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
        public static TData[] ShuffleFisherYates<TData>(List<TData> items)
        {
            TData[] result = items.ToArray();
            ShuffleFisherYates(result);
            return result;
        }
        public static TData GetRandomItem<TData>(this List<TData> items)
        {
            if(items == null || items.Count == 0)
            {
                return default(TData);
            }
            else if(items.Count == 1)
            {
                return items[0];
            }
            else
            {
                int ix = RANDOM.Next(items.Count);
                return items[ix];
            }
        }
    }
}
