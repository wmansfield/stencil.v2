using System;
using Placeholder.Domain;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    public partial interface IAccountBusiness
    {
        Account PasswordResetStart(Guid account_id);
        bool PasswordResetComplete(Guid account_id, string token, string password);
        Account UpdateNoCascade(Account updateAccount, Availability availability);
        Account CreateInitialAccounts(Account insertIfEmpty);
        Account GetWithAvatar(Guid account_id);
        Account GetFirstWithEntitlement(string entitlement);
        Account GetForValidPassword(string email, string password);
        Account GetByApiKey(string api_key);
        Account GetByEmail(string email);
        void UpdateSingleLoginToken(Guid account_id, string single_login_token);
        void UpdateLastLogin(Guid account_id, DateTime last_login_utc, string platform);
        Account GetByIdCached(Guid account_id);

        string GeneratePasswordHash(string salt, string password);


    }
}
