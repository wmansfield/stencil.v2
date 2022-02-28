using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Plugins;

namespace Placeholder.Plugins.RestAPI
{
    public class RestApiPlugin : ChokeableClass, IWebPlugin
    {
        public RestApiPlugin()
            : base(CoreFoundation.Current)
        {
        }
        #region IPlugin Members

        public string DisplayName
        {
            get { return "RestApi"; }
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
