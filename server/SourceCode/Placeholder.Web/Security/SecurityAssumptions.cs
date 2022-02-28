using System;

namespace Placeholder.Web.Security
{
    public static class SecurityAssumptions
    {
        public static readonly string CURRENT_ACCOUNT_HTTP_CONTEXT_KEY = "__current_account";
        public static readonly string CURRENT_CUSTOMER_HTTP_CONTEXT_KEY = "__current_customer";
        public static readonly string CURRENT_LANGUAGE_HTTP_CONTEXT_KEY = "__current_language";
        public static readonly string API_PARAM_KEY = "x-api-key";
        public static readonly string API_PARAM_SIGNATURE = "x-api-signature";
        public static readonly string PARAM_PLATFORM = "x-device-platform";
        public static readonly string PARAM_VERSION = "x-device-version";
        public static readonly string PARAM_LANGUAGE = "x-language";
        public static readonly string PARAM_SHOP = "x-shop";
    }
}
