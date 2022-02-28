using System;
using Placeholder.Data.Sql;
using Placeholder.Data.Sql.Models;
using Placeholder.Primary.Business.Integration;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;

namespace Placeholder.Primary.Business.Direct
{
    public abstract class BusinessBase : BusinessBaseHealth
    {
        public BusinessBase(IFoundation foundation, string trackPrefix)
            : base(foundation, trackPrefix)
        {
            this.DataContextFactory = foundation.Resolve<IPlaceholderContextFactory>();
            this.API = new PlaceholderAPI(foundation);
            this.SharedCacheStatic15 = new AspectCache("BusinessBase.15", foundation, new ExpireStaticLifetimeManager("BusinessBase.15", TimeSpan.FromMinutes(15)));
            this.SharedCacheStatic2 = new AspectCache("BusinessBase.2", foundation, new ExpireStaticLifetimeManager("BusinessBase.2", TimeSpan.FromMinutes(2)));
        }

        public PlaceholderAPI API { get; set; }
        /// <summary>
        /// Shared with all business elements, use Keyed
        /// </summary>
        public AspectCache SharedCacheStatic15 { get; set; }
        /// <summary>
        /// Shared with all business elements, use Keyed
        /// </summary>
        public AspectCache SharedCacheStatic2 { get; set; }


        public virtual string DefaultAgent
        {
            get
            {
                return Daemons.Agents.AGENT_DEFAULT;
            }
        }

        public IFoundation Foundation
        {
            get
            {
                return base.IFoundation;
            }
        }

        protected virtual IPlaceholderContextFactory DataContextFactory { get; set; }
        protected virtual IDependencyCoordinator DependencyCoordinator
        {
            get
            {
                return this.IFoundation.Resolve<IDependencyCoordinator>();
            }
        }

        public virtual PlaceholderContext CreateSQLSharedContext()
        {
            return this.DataContextFactory.CreateSharedContext();
        }
        public virtual PlaceholderContext CreateSQLIsolatedContext(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(CreateSQLIsolatedContext), delegate ()
            {
                string tenant_code = this.SharedCacheStatic2.PerLifetime(string.Format("IsolatedShop{0}", shop_id), delegate ()
                {
                    Domain.Tenant tenant = this.API.Direct.Tenants.GetByShop(shop_id);
                    if(tenant != null)
                    {
                        return tenant.tenant_code;
                    }
                    return null;
                });

                return this.DataContextFactory.CreateIsolatedContext(tenant_code);
            });
        }

        public virtual PlaceholderContext CreateSQLIsolatedContext(string tenant_code)
        {
            return base.ExecuteFunction(nameof(CreateSQLIsolatedContext), delegate ()
            {
                return this.DataContextFactory.CreateIsolatedContext(tenant_code);
            });
        }
    }
}
