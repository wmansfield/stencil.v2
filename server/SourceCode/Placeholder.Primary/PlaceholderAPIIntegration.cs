using System;
using Placeholder.Common.Configuration;
using Placeholder.Common.Synchronization;
using Placeholder.Primary.Business.Integration;
using Placeholder.Primary.Integration;
using Placeholder.Primary.Markdown;
using Placeholder.Primary.Security;
using Zero.Foundation;

namespace Placeholder.Primary
{
    public partial class PlaceholderAPIIntegration : BaseClass
    {
        public PlaceholderAPIIntegration(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        
        public IEmailer Email
        {
            get { return this.IFoundation.Resolve<IEmailer>(); }
        }
        public ILocalizer Localizer
        {
            get { return this.IFoundation.Resolve<ILocalizer>(); }
        }
        public INotifySynchronizer Synchronization
        {
            get { return this.IFoundation.Resolve<INotifySynchronizer>(); }
        }
        public ISettingsResolver Settings
        {
            get { return this.IFoundation.Resolve<ISettingsResolver>(); }
        }
        public IDependencyCoordinator Dependencies
        {
            get { return this.IFoundation.Resolve<IDependencyCoordinator>(); }
        }
        public ISecurityEnforcer Security
        {
            get { return this.IFoundation.Resolve<ISecurityEnforcer>(); }
        }
        public IUploadFiles UploadFiles
        {
            get { return this.IFoundation.Resolve<IUploadFiles>(); }
        }
        public IMarkdownProcessor Markdown
        {
            get { return this.IFoundation.Resolve<IMarkdownProcessor>(); }
        }
    }
}
