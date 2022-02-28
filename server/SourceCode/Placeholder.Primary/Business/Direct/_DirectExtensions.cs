using System;
using System.Collections.Generic;
using Placeholder.Common;
using Placeholder.Primary.Integration;
using Placeholder.Primary.Security;

namespace Placeholder.Primary.Business.Direct
{
    public static class _DirectExtensions
    {
        
        public static string EncryptData(this PlaceholderAPI api, string purpose, string nonProtectedData)
        {
            try
            {
                string password = api.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_CRYPTO_PASS);
                return EncryptionUtility.SimpleEncryptWithPassword(nonProtectedData, password);
            }
            catch (Exception ex)
            {
                api.Integration.Email.SendAdminEmail("Error Encrypting Data", string.Format("Error while encrypting data for caller: {0}. Error: {1}", purpose, ex.Message));
                return nonProtectedData;
            }
        }
        public static string DecryptData(this PlaceholderAPI api, string purpose, string protectedData)
        {
            try
            {
                string password = api.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_CRYPTO_PASS);
                return EncryptionUtility.SimpleDecryptWithPassword(protectedData, password);
            }
            catch (Exception ex)
            {
                api.Integration.Email.SendAdminEmail("Error Decrypting Data", string.Format("Error while decrypting data for caller: {0}. Error: {1}", purpose, ex.Message));
                return protectedData;
            }
        }

    }
}
