using Placeholder.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;
using Placeholder.SDK;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Security
{
    public partial class SecurityEnforcerBase : BaseClass
    {
        public SecurityEnforcerBase(IFoundation ifoundation)
            : base(ifoundation)
        {
            this.API = ifoundation.Resolve<PlaceholderAPI>();
            this.Cache1 = new AspectCache("SecurityEnforcerCache", ifoundation, new ExpireStaticLifetimeManager("SecurityEnforcerLifeTime", TimeSpan.FromMinutes(1), false));
        }

        public const string CURRENT_ACCOUNT_HTTP_CONTEXT_KEY = "__current_account"; // DO NOT CHANGE, VALUE USED ELSEWHERE

        protected virtual PlaceholderAPI API { get; set; }
        protected virtual AspectCache Cache1 { get; set; }


        public virtual T CachedExecute<T>(string entity, Guid identifier, Func<Guid, T> method)
        {
            string key = string.Format("{0}CachedExecute{1}", identifier, entity);
            return this.Cache1.PerLifetime(key, delegate ()
            {
                return method(identifier);
            });
        }

        public virtual T CachedExecute<T>(string entity, Guid? route, Guid identifier, Func<Guid, Guid, T> method)
        {
            string key = string.Format("{0}CachedExecuteOverload{1}{2}", identifier, entity, route);
            return this.Cache1.PerLifetime(key, delegate ()
            {
                if(route.HasValue)
                {
                    return method(route.GetValueOrDefault(), identifier);
                }
                return default(T);
            });
        }



        public virtual bool IsAnyShopRole(Account account, Guid shop_id)
        {
            return IsShopRole(account, shop_id, null);
        }
        public virtual bool IsShopRole(Account account, Guid shop_id, sdk.ShopRole? role)
        {
            if(account.IsSuperAdmin())
            {
                return true;
            }
            string key = string.Format("IsShopRole:{0}:{1}:{2}", account.account_id, account.updated_utc, shop_id);

            List<sdk.ShopAccount> shopAccounts = this.Cache1.PerLifetime(key, delegate ()
            {
                return AsyncHelper.RunSync(async delegate ()
                {
                    ListResult<sdk.ShopAccount> data = await this.API.Store.ShopAccounts.FindForShopAsync(shop_id, 0, int.MaxValue, enabled: true);
                    return data.items;
                });
            });

            if (role == null)
            {
                return shopAccounts.Count > 0;
            }
            else
            {
                return shopAccounts.Any(x => x.shop_role == role);
            }
        }
    }
}
