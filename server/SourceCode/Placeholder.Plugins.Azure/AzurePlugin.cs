using System;
using System.Collections.Generic;
using System.Reflection;
using Placeholder.Common;
using Placeholder.Common.Configuration;
using Placeholder.Common.Synchronization;
using Placeholder.Domain;
using Placeholder.Primary;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Daemons;
using Zero.Foundation.Plugins;
using Unity;
using Placeholder.Primary.Integration;
using Placeholder.Plugins.Azure.Integration;

namespace Placeholder.Plugins.Azure
{
    public class AzurePlugin : ChokeableClass, IWebPlugin
    {
        public AzurePlugin()
            : base(CoreFoundation.Current)
        {
        }
        #region IPlugin Members

        public string DisplayName
        {
            get { return "Azure"; }
        }
        public string DisplayVersion
        {
            get { return FoundationUtility.GetInformationalVersion(Assembly.GetExecutingAssembly()); }
        }

        public bool Construct(IFoundation foundation)
        {
            base.IFoundation = foundation;
            return true;
        }

        #endregion

        #region IWebPlugin Members


        public void Initialize()
        {
            base.ExecuteMethod("Initialize", delegate ()
            {
                this.IFoundation.Container.RegisterSingleton<IUploadFiles, AzureUploadFiles>();
            });
        }

        public int DesiredInitializionPriority
        {
            get { return 0; }
        }

        public void OnWebPluginInitialized(IWebPlugin plugin)
        {
        }

        public void OnAllWebPluginsInitialized(IEnumerable<IWebPlugin> allWebPlugins)
        {
        }

        public T RetrieveMetaData<T>(string token)
        {
            return default(T);
        }

        public object InvokeCommand(string name, Dictionary<string, object> caseInsensitiveParameters)
        {
            return null;
        }

        #endregion
    }
}
