using System;
using System.Collections.Generic;
using System.Text;

namespace Starter.App
{
    public class StarterAssumptions
    {
        public static string TERMS_LINK = "";
        public static string PRIVACY_LINK = "";
        public static string APP_COPYRIGHT_NAME = "Foundation Zero";
        public static string APP_LABEL_NAME = "Starter";
        public static string APP_PUBLIC_NAME = "Starter";

        public const string URL_SCHEME = "stencilstart";


#if DEBUG

        public static string API_URL = "https://stencilstarter-api.foundationzero.com/api";

        public static string DEBUG_API_URL = "https://stencilstarter.ngrok.io/api";
        public static string PUBLIC_URL = "https://stencilstarter.foundationzero.com";
#else
        public static string API_URL = "https://stencilstarter-api.foundationzero.com/api";
        public static string PUBLIC_URL = "https://stencilstarter.foundationzero.com";
#endif

    }
}
