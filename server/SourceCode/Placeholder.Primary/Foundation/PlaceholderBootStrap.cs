using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Placeholder.Common.Configuration;
using Placeholder.Common.Configuration.Implementations;
using Placeholder.Data.Sql;
using Placeholder.Primary.Business.Index;
using Placeholder.Primary.Business.Integration;
using Placeholder.Primary.Business.Store;
using Placeholder.Primary.Business.Store.Factory;
using Placeholder.Primary.Business.Store.Implementation;
using Placeholder.Primary.Emailing;
using Placeholder.Primary.Exceptions;
using Placeholder.Primary.Health;
using Placeholder.Primary.Health.Daemons;
using Placeholder.Primary.Health.Exceptions;
using Placeholder.Primary.I18n;
using Placeholder.Primary.Integration;
using Placeholder.Primary.Markdown;
using Placeholder.Primary.Security;
using Unity;
using Unity.Lifetime;
using Zero.Foundation;
using Zero.Foundation.Daemons;
using Zero.Foundation.System;
using Zero.Foundation.System.Implementations;

namespace Placeholder.Primary.Foundation
{
    public partial class PlaceholderBootStrap : AspNetCoreBootStrap
    {
        public PlaceholderBootStrap(IUnityContainer container, IWebHostEnvironment webHostEnvironment, IServiceCollection services, IConfiguration configuration)
            : base(container, webHostEnvironment, services, configuration)
        {
            
        }
        
        public override void OnFoundationCreated(IFoundation foundation)
        {
            base.OnFoundationCreated(foundation);

            foundation.Container.RegisterType<ISettingsResolver, ConfigurationSettingsResolver>(new ContainerControlledLifetimeManager());
        }
        protected override void OnBeforeLoadWebPlugins(IFoundation foundation)
        {
            base.OnBeforeLoadWebPlugins(foundation);

            this.RegisterDataMapping(foundation);
        }
        public override void OnAfterSelfRegisters(IFoundation foundation)
        {
            base.OnAfterSelfRegisters(foundation);

            foundation.Container.RegisterType<IPlaceholderContextFactory, PlaceholderContextFactory>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IPlaceholderCosmosClientFactory, PlaceholderCosmosClientFactory>(new ContainerControlledLifetimeManager());

            foundation.Container.RegisterType<IEmailer, QueuedEmailer>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IPlaceholderElasticClientFactory, PlaceholderElasticClientFactory>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IDependencyCoordinator, DependencyCoordinator>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<ISecurityEnforcer, SecurityEnforcer>(new ContainerControlledLifetimeManager());

            foundation.Container.RegisterType<ILocalizer, TransientLocalizer>(new ContainerControlledLifetimeManager());
            
            foundation.Container.RegisterType<IMarkdownProcessor, MarkdownProcessor>(new ContainerControlledLifetimeManager());
            

            this.RegisterDataElements(foundation);

            this.RegisterErrorHandlers(foundation);         
        }

        protected virtual void RegisterDataMapping(IFoundation foundation)
        {
            IServiceCollection services = foundation.Resolve<IServiceCollection>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        protected virtual void RegisterErrorHandlers(IFoundation foundation)
        {
            // Replace Exception Handlers
            foundation.Container.RegisterType<IHandleException, FriendlyExceptionHandler>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IHandleExceptionProvider, FriendlyExceptionHandlerProvider>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterInstance(new FriendlyExceptionHandlerProvider(foundation, foundation.GetLogger()), new ContainerControlledLifetimeManager());

            foundation.Container.RegisterInstance(new HealthThrowExceptionHandlerProvider(foundation.GetLogger()), new ContainerControlledLifetimeManager());
        }

        public override void OnAfterBootStrapComplete(IFoundation foundation)
        {
            base.OnAfterBootStrapComplete(foundation);

            foundation.Container.RegisterType<IHandleException, HealthFriendlyExceptionHandler>(new ContainerControlledLifetimeManager());

            // Replace Exception Handlers
            foundation.Container.RegisterType<IHandleException, HealthFriendlyExceptionHandler>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IHandleExceptionProvider, HealthFriendlyExceptionHandlerProvider>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterInstance<HealthFriendlyExceptionHandlerProvider>(new HealthFriendlyExceptionHandlerProvider(foundation, foundation.GetLogger()), new ContainerControlledLifetimeManager());

            foundation.Container.RegisterType<IHandleException, HealthSwallowExceptionHandler>(FoundationAssumptions.SWALLOWED_EXCEPTION_HANDLER, new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IHandleExceptionProvider, HealthSwallowExceptionHandlerProvider>(FoundationAssumptions.SWALLOWED_EXCEPTION_HANDLER, new ContainerControlledLifetimeManager());
            foundation.Container.RegisterInstance<HealthSwallowExceptionHandlerProvider>(FoundationAssumptions.SWALLOWED_EXCEPTION_HANDLER, new HealthSwallowExceptionHandlerProvider(foundation, foundation.GetLogger()), new ContainerControlledLifetimeManager());

            //TODO: Signals: foundation.Container.RegisterType<ISignalBroker, PlaceholderSignalBroker>(new ContainerControlledLifetimeManager());

            DaemonConfig healthConfig = new DaemonConfig()
            {
                InstanceName = HealthReportDaemon.DAEMON_NAME,
                ContinueOnError = true,
                IntervalMilliSeconds = 15 * 1000, // every 15 seconds
                StartDelayMilliSeconds = 60 * 1000,
                TaskConfiguration = string.Empty
            };
            foundation.GetDaemonManager().RegisterDaemon(healthConfig, new HealthReportDaemon(foundation), true);
            
        }
    }
}
