using System;
using System.Collections.Generic;

namespace Placeholder.SDK.Client
{
    public static class _ResultExtensions
    {
        public static string GetMessage(this ActionResult result, string messageIfNull = "Error Processing Request")
        {
            if (result != null && !string.IsNullOrEmpty(result.message))
            {
                return result.message;
            }
            return messageIfNull;
        }
        public static string GetMessage<ActionResult>(this ItemResult<ActionResult> result, string messageIfNull = "Error Processing Request")
        {
            if (result != null)
            {
                if (result.item != null)
                {
                    return result.GetMessage();
                }
                return result.message;
            }
            return messageIfNull;
        }
        public static bool IsSuccess(this ActionResult result)
        {
            if (result != null && result.success)
            {
                return true;
            }
            return false;
        }
        public static bool IsSuccess<T>(this ItemResult<T> result)
        {
            if (result != null && (result.success || !EqualityComparer<T>.Default.Equals(result.item, default(T))))
            {
                return true;
            }
            return false;
        }
        [Obsolete("List success can be ambiguous, define your own mechanism or rely solely on .success", true)]
        public static bool IsSuccess<T>(this ListResult<T> result)
        {
            if (result != null && (result.success || result.items != null))
            {
                return true;
            }
            return false;
        }
        [Obsolete("List success can be ambiguous, define your own mechanism or rely solely on .success", true)]
        public static bool IsSuccess<T, K>(this ListResult<T, K> result)
            where K : new()
        {
            if (result != null && result.success)
            {
                return true;
            }
            return false;
        }
    }
}
