
namespace Stencil.Native
{
    public static class CoreAssumptions
    {
        /// <summary>
        /// Does nothing in production mode
        /// </summary>
        public static bool LOG_METHOD_INVOCATIONS = false;

        public const string KEY_CRYPTO_DB_NAME = "stencil.db_crypto";
    }
}
