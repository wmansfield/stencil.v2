using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Zero.Foundation.Aspect;

namespace Placeholder.Common
{
    public static class CommonUtility
    {

        #region File Helpers

        private static Regex _fileNameOnlyCharacters = new Regex("[^a-zA-Z0-9\\._-]");
        public static string CleanFileName(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                return _fileNameOnlyCharacters.Replace(item, "");
            }
            return string.Empty;
        }

        #endregion


        public static string FriendlyName(this Type type)
        {
            return Path.GetExtension(type.ToString()).Trim('.').ToLower();
        }


        
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer = null)
        {
            Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);

            if (source != null)
            {
                foreach (TSource item in source)
                {
                    dictionary[keySelector(item)] = elementSelector(item);
                }
            }

            return dictionary;
        }
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(comparer);

            if (source != null)
            {
                foreach (TSource item in source)
                {
                    dictionary[keySelector(item)] = item;
                }
            }

            return dictionary;
        }


        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }
        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    resultStream.Write(buffer, 0, read);
                }

                return resultStream.ToArray();
            }
        }

        public static string Base64EncodeBytes(byte[] inputBytes)
        {
            // Each 3-byte sequence in inputBytes must be converted to a 4-byte 
            // sequence 
            long arrLength = (long)(4.0d * inputBytes.Length / 3.0d);
            if ((arrLength % 4) != 0)
            {
                // increment the array length to the next multiple of 4 
                //    if it is not already divisible by 4
                arrLength += 4 - (arrLength % 4);
            }

            char[] encodedCharArray = new char[arrLength];
            Convert.ToBase64CharArray(inputBytes, 0, inputBytes.Length, encodedCharArray, 0);

            return (new string(encodedCharArray));
        }

        #region Latest Method

        private const string KEY_LATEST_METHOD_NAME = "__LATEST_METHOD_NAME";

        public static void SetLatestMethodName(string prefix, string methodName)
        {
            SetLatestMethodName(string.Format("{0}.{1}", prefix, methodName));
        }
        public static void SetLatestMethodName(string methodName)
        {
            try
            {
                SetThreadLocalLatestMethodName(methodName);
                SetAsyncLocalLatestMethodName(methodName);
            }
            catch
            {
                // gulp
            }
        }
        public static string GetLatestMethodName()
        {
            try
            {
                string result = GetThreadLocalLatestMethodName();
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = GetAsyncLocalLatestMethodName();
                }
                return result;
            }
            catch
            {
                // gulp
            }
            return null;
        }


        public static void SetThreadLocalLatestMethodName(string methodName)
        {
            try
            {
                Dictionary<string, object> data = ChokeableClass.ThreadLocalState.Value;
                if (data == null)
                {
                    data = new Dictionary<string, object>();
                    ChokeableClass.ThreadLocalState.Value = data;
                }
                data[KEY_LATEST_METHOD_NAME] = methodName;
            }
            catch
            {
                // gulp
            }

        }
        public static string GetThreadLocalLatestMethodName()
        {
            try
            {
                Dictionary<string, object> data = ChokeableClass.ThreadLocalState.Value;
                if (data != null)
                {
                    if (data.TryGetValue(KEY_LATEST_METHOD_NAME, out object result))
                    {
                        if (result != null)
                        {
                            return (string)result;
                        }
                    }
                }
            }
            catch
            {
                // gulp
            }
            return null;
        }

        public static void SetAsyncLocalLatestMethodName(string methodName)
        {
            try
            {
                Dictionary<string, object> data = ChokeableClass.AsyncLocalState.Value;
                if (data == null)
                {
                    data = new Dictionary<string, object>();
                    ChokeableClass.AsyncLocalState.Value = data;
                }
                data[KEY_LATEST_METHOD_NAME] = methodName;
            }
            catch
            {
                // gulp
            }

        }
        public static string GetAsyncLocalLatestMethodName()
        {
            try
            {
                Dictionary<string, object> data = ChokeableClass.AsyncLocalState.Value;
                if (data != null)
                {
                    if (data.TryGetValue(KEY_LATEST_METHOD_NAME, out object result))
                    {
                        if (result != null)
                        {
                            return (string)result;
                        }
                    }
                }
            }
            catch
            {
                // gulp
            }
            return null;
        }

        #endregion
    }
}
