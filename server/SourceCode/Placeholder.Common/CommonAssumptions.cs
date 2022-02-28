using System;

namespace Placeholder.Common
{
    public static class CommonAssumptions
    {
        public static readonly string APP_KEY_BACKING_URL = "Backing-Url";
        public static readonly string APP_KEY_JWT_SIGNING_KEY = "JwtSigningKey";
        public static readonly string APP_KEY_SECTION_NAME = "AppSettings";
        public static readonly string APP_KEY_IS_LOCAL_HOST = "IsLocalHost";
        public static readonly string APP_KEY_HEALTH_DEBUG = "Health-Debug";
        public static readonly string APP_KEY_HEALTH_APIKEY = "Health-ApiKey";
        public static readonly string APP_KEY_HEALTH_HOST = "Health-Host";
        public static readonly string APP_KEY_HEALTH_SERVER_ALIAS = "Health-Alias";

        public static readonly string APP_KEY_IS_BACKING = "Placeholder-IsBacking";
        public static readonly string APP_KEY_IS_HYDRATE = "Placeholder-IsHydrate";
        public static readonly string APP_KEY_SQL_DB_FORMAT = "Placeholder-SQL-{0}";
        public static readonly string APP_KEY_CRYPTO_PASS = "Placeholder-Crypto";

        public static readonly string APP_KEY_ES_URL = "Placeholder-ES-HOST";
        public static readonly string APP_KEY_ES_SECONDARY_URL = "Placeholder-ES-HOST-SECONDARY";
        public static readonly string APP_KEY_ES_INDEX = "Placeholder-ES-INDEX";
        public static readonly string APP_KEY_ES_REPLICA = "Placeholder-ES-REPLICA";
        public static readonly string APP_KEY_ES_SHARDS = "Placeholder-ES-SHARDS";
        public static readonly string APP_KEY_DEBUG_QUERIES = "Placeholder-ES-DEBUG";
        public static readonly string APP_KEY_COSMOS_CACHE_DATABASE_FORMAT = "Placeholder-CS-Database-{0}";
        public static readonly string APP_KEY_COSMOS_CACHE_SERVER_FORMAT = "Placeholder-CS-Server-{0}";
        public static readonly string APP_KEY_COSMOS_CACHE_AUTH_KEY_FORMAT = "Placeholder-CS-AuthKey-{0}";

        public static readonly string APP_KEY_BLOB_CONNECTION_KEY_FORMAT = "Placeholder-BLOB-{0}";
        public static readonly string BLOB_ASSET_CONTAINER_NAME = "assets";

        public static readonly int INDEX_RETRY_THRESHOLD_SECONDS = 5;

        public static readonly string SQL_PRIMARY_CODE = "primary";
        public static readonly string COSMOS_PRIMARY_CODE = "primary";


        public static string SWALLOWED_EXCEPTION_HANDLER = "Swallowed";

        public static readonly string DAEMON_EMAIL_FORMAT = "EmailDaemon_{0}";

        public static readonly string EMAILTEMPLATE_GLOBAL_SUBJECT_FORMAT = "email.global.{0}.subject";
        public static readonly string EMAILTEMPLATE_GLOBAL_BODY_FORMAT = "email.global.{0}.body";
        public static readonly string EMAILTEMPLATE_GLOBAL_FROM_NAME = "email.global.from_name";
        public static readonly string EMAILTEMPLATE_GLOBAL_FROM_EMAIL = "email.global.from_email";
    }
}
