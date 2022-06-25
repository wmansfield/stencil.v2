using System;
using System.Linq;
using dm = Placeholder.Domain;
using db = Placeholder.Data.Sql.Models;
using Placeholder.Common;
using Placeholder.Common.Data;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Synchronization;
using Zero.Foundation;
using Placeholder.Primary.Emailing;
using System.Security.Cryptography;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    public partial class AccountBusiness
    {
        public dm.Account CreateInitialAccounts(dm.Account insertIfEmpty)
        {
            return base.ExecuteFunction("CreateInitialAccounts", delegate ()
            {
                dm.Account result = null;
                db.Account found = null;
                using (var database = base.CreateSQLSharedContext())
                {
                    found = (from a in database.Accounts
                             select a).FirstOrDefault();
                }
                if (found == null)
                {
                    result = Insert(insertIfEmpty);
                }
                return result;
            });
        }

        public dm.Account GetWithAvatar(Guid account_id)
        {
            return base.ExecuteFunction("GetWithAvatar", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    var found = (from a in database.Accounts
                                 where a.account_id == account_id
                                 select new Interim<db.Account, db.Asset>() { Item1 = a, Item2 = a.Asset }).FirstOrDefault();

                    return found.ToDomainModel();
                }
            });
        }

        public dm.Account GetFirstWithEntitlement(string entitlement)
        {
            return base.ExecuteFunction("GetFirstWithEntitlement", delegate ()
            {
                if(string.IsNullOrWhiteSpace(entitlement))
                {
                    return null;
                }
                using (var database = base.CreateSQLSharedContext())
                {
                    var found = (from a in database.Accounts
                                 where a.entitlements.Contains(entitlement)
                                 && a.deleted_utc == null
                                 && a.account_status == (int)dm.AccountStatus.enabled
                                 select a).FirstOrDefault();

                    return found.ToDomainModel();
                }
            });
        }



        public dm.Account GetForValidPassword(string email, string password)
        {
            return base.ExecuteFunction("GetForValidPassword", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        email = email.Trim();
                        db.Account found = (from a in database.Accounts
                                           where a.email == email
                                           select a).FirstOrDefault();

                        if (found != null)
                        {
                            string computedPassword = GeneratePasswordHash(found.password_salt, password);
                            if (computedPassword == found.password)
                            {
                                return found.ToDomainModel();
                            }
                        }
                    }
                }
                return null;
            });
        }
        public dm.Account GetByApiKey(string api_key)
        {
            return base.ExecuteFunction("GetByApiKey", delegate ()
            {
                using (var database = this.CreateSQLSharedContext())
                {
                    db.Account result = (from n in database.Accounts
                                        where (n.api_key == api_key)
                                        select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public dm.Account GetByEmail(string email)
        {
            return base.ExecuteFunction("GetByEmail", delegate ()
            {
                using (var database = this.CreateSQLSharedContext())
                {
                    db.Account result = (from n in database.Accounts
                                        where (n.email == email)
                                        select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public void UpdateSingleLoginToken(Guid account_id, string single_login_token)
        {
            base.ExecuteMethod("UpdateSingleLoginToken", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Account found = (from n in database.Accounts
                                       where n.account_id == account_id
                                       select n).FirstOrDefault();

                    if (found != null)
                    {
                        found.single_login_token = single_login_token;
                        database.SaveChanges();
                    }
                }
            });
        }

        public void UpdateLastLogin(Guid account_id, DateTime last_login_utc, string platform)
        {
            base.ExecuteMethod("UpdateLastLogin", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    if (!string.IsNullOrEmpty(platform) && platform.Length > 250)
                    {
                        platform = platform.Substring(0, 250);
                    }
                    db.Account found = (from n in database.Accounts
                                       where n.account_id == account_id
                                       select n).FirstOrDefault();

                    if (found != null)
                    {
                        found.last_login_utc = last_login_utc;
                        found.last_login_platform = platform;
                        found.InvalidateSync(this.DefaultAgent, "login");
                        database.SaveChanges();

                        this.API.Direct.Accounts.Synchronizer.SynchronizeItem(new IdentityInfo(found.account_id), Availability.Retrievable);
                    }
                }
            });
        }
       

        public dm.Account PasswordResetStart(Guid account_id)
        {
            return base.ExecuteFunction("PasswordResetStart", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Account found = (from a in database.Accounts
                                       where a.account_id == account_id
                                       select a).FirstOrDefault();

                    if (found != null && !found.deleted_utc.HasValue)
                    {
                        found.password_reset_token = ShortGuid.NewGuid().ToString();
                        found.password_reset_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "passwordreset");
                        database.SaveChanges();

                        this.API.Integration.Email.SendAccountPasswordResetInitiated(Guid.Empty, found.account_id, found.email, string.Format("{0} {1}", found.first_name, found.last_name), found.password_reset_token);
                    }
                    return found.ToDomainModel();
                }
            });
        }
        public bool PasswordResetComplete(Guid account_id, string token, string password)
        {
            return base.ExecuteFunction("PasswordResetComplete", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    db.Account found = (from a in database.Accounts
                                       where a.account_id == account_id
                                       && a.password_reset_token == token
                                       && !string.IsNullOrEmpty(token)
                                       select a).FirstOrDefault();

                    if (found != null && !found.deleted_utc.HasValue)
                    {
                        if (found.password_reset_utc.HasValue)
                        {
                            TimeSpan difference = DateTime.UtcNow - found.password_reset_utc.Value;
                            if (difference.TotalHours > 24)
                            {
                                return false;
                            }
                        }
                        found.password_salt = Guid.NewGuid().ToString("N");
                        found.password = this.GeneratePasswordHash(found.password_salt, password); ;
                        found.password_reset_token = string.Empty;
                        found.password_reset_utc = null;
                        database.SaveChanges();

                        this.API.Integration.Email.SendAccountRequestPasswordCompleted(Guid.Empty, found.account_id, found.email, string.Format("{0} {1}", found.first_name, found.last_name));
                        return true;

                    }
                    return false;
                }
            });
        }


        public dm.Account UpdateNoCascade(dm.Account updateAccount, Availability availability)
        {
            return base.ExecuteFunction("UpdateNoCascade", delegate ()
            {
                using (var database = base.CreateSQLSharedContext())
                {
                    this.PreProcess(updateAccount, Crud.Update);

                    var interception = this.Intercept(updateAccount, Crud.Update);
                    if (interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }

                    updateAccount.updated_utc = DateTime.UtcNow;

                    db.Account found = (from a in database.Accounts
                                       where a.account_id == updateAccount.account_id
                                       select a).FirstOrDefault();

                    if (found != null)
                    {
                        dm.Account previous = found.ToDomainModel();

                        found = updateAccount.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");

                        database.SaveChanges();

                        this.AfterUpdatePersisted(database, found, updateAccount, previous);

                        this.Synchronizer.SynchronizeItem(new IdentityInfo(found.account_id), availability);
                        this.AfterUpdateIndexed(database, found);
                    }

                    return this.GetById(updateAccount.account_id);
                }
            });
        }

        public virtual string GeneratePasswordHash(string salt, string password)
        {
            return base.ExecuteFunction("GeneratePasswordHash", delegate ()
            {
                return SHA256.Create().HashAsString(salt + password);
            });
        }

        public dm.Account GetByIdCached(Guid account_id)
        {
            return base.ExecuteFunction("GetByIdCached", delegate ()
            {
                return this.SharedCacheStatic2.PerLifetime(string.Format("", account_id), delegate ()
                {
                    return this.GetById(account_id);
                });
            });
        }
        partial void PreProcess(dm.Account account, Crud crud)
        {
            base.ExecuteMethod("PreProcess", delegate ()
            {
                if (crud == Crud.Insert)
                {
                    if (string.IsNullOrEmpty(account.password_salt))
                    {
                        account.password_salt = Guid.NewGuid().ToString("N");
                    }

                    account.password = GeneratePasswordHash(account.password_salt, account.password); ;
                }

                if(string.IsNullOrWhiteSpace(account.account_display))
                {
                    account.account_display = string.Format("{0} {1}", account.first_name, (account.last_name ?? string.Empty).FirstOrDefault());
                }

                // ensure keys, controller should, but this is critical
                if (string.IsNullOrEmpty(account.api_key))
                {
                    account.api_key = Guid.NewGuid().ToString("N").ToLower();
                }
                if (string.IsNullOrEmpty(account.api_secret))
                {
                    account.api_secret = Guid.NewGuid().ToString("N").ToLower();
                }

                if (string.IsNullOrEmpty(account.email_verify_token))
                {
                    account.email_verify_token = Guid.NewGuid().ToString("N").ToLower();
                }
                if (string.IsNullOrEmpty(account.password_reset_token))
                {
                    account.password_reset_token = Guid.NewGuid().ToString("N").ToLower();
                }
                if (string.IsNullOrEmpty(account.single_login_token))
                {
                    account.single_login_token = Guid.NewGuid().ToString("N").ToLower();
                }
            });
        }
    }
}
