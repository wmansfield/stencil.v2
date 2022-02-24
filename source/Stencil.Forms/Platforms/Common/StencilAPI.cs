
using Stencil.Forms.Commanding;
using Stencil.Forms.Data;
using Stencil.Forms.Data.Sync;
using Stencil.Forms.Data.Sync.Manager;
using Stencil.Forms.Presentation;
using Stencil.Forms.Presentation.Routing;
using Stencil.Forms.Screens;

namespace Stencil.Forms
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
        public IScreenManager StencilScreens
        {
            get
            {
                return NativeApplication.ScreenManager;
            }
        }
        public ITrackedDataManager StencilTrackedData
        {
            get
            {
                return NativeApplication.TrackedDataManager;
            }
        }

        public ILogger Logger
        {
            get
            {
                return NativeApplication.Logger;
            }
        }
        public IAlerts Alerts
        {
            get
            {
                return NativeApplication.Alerts;
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
