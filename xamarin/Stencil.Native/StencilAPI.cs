
using Stencil.Native.Commanding;
using Stencil.Native.Data;
using Stencil.Native.Data.Sync;
using Stencil.Native.Presentation.Routing;
using Stencil.Native.Screens;

namespace Stencil.Native
{
    public class StencilAPI
    {
        static StencilAPI()
        {
            Instance = new StencilAPI();
        }
        /// <summary>
        /// Often replaced with an extended version
        /// </summary>
        public static StencilAPI Instance;

        protected StencilAPI()
        {

        }

        public string Localize(string token, string defaultText, params object[] arguments)
        {
            //TODO:SHOULD: Localize by token
            return string.Format(defaultText, arguments);
        }

        public IAppAnalytics Analytics
        {
            get
            {
                return NativeApplication.Analytics;
            }
        }
        public IRouter Router
        {
            get
            {
                return NativeApplication.Router;
            }
        }
        public ICommandProcessor CommandProcessor
        {
            get
            {
                return NativeApplication.CommandProcessor;
            }
        }
        public IScreenManager Screens
        {
            get
            {
                return NativeApplication.ScreenManager;
            }
        }

        public ILogger Logger
        {
            get
            {
                return NativeApplication.Logger;
            }
        }

        public IStencilDatabaseConnector StencilDatabase
        {
            get
            {
                return NativeApplication.StencilDatabaseConnector;
            }
        }
       
        
    }
}
