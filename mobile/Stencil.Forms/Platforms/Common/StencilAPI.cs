
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

        public virtual string Localize(string token, string defaultText, params object[] arguments)
        {
            //TODO:SHOULD: Localize by token
            return string.Format(defaultText, arguments);
        }

        public virtual IAppAnalytics Analytics
        {
            get
            {
                return NativeApplication.Analytics;
            }
        }
        public virtual IRouter Router
        {
            get
            {
                return NativeApplication.Router;
            }
        }
        public virtual ICommandProcessor CommandProcessor
        {
            get
            {
                return NativeApplication.CommandProcessor;
            }
        }
        public virtual IScreenManager StencilScreens
        {
            get
            {
                return NativeApplication.ScreenManager;
            }
        }
        public virtual ITrackedDataManager StencilTrackedData
        {
            get
            {
                return NativeApplication.TrackedDataManager;
            }
        }

        public virtual ILogger Logger
        {
            get
            {
                return NativeApplication.Logger;
            }
        }
        public virtual IAlerts Alerts
        {
            get
            {
                return NativeApplication.Alerts;
            }
        }

        public virtual IStencilDatabaseConnector StencilDatabase
        {
            get
            {
                return NativeApplication.StencilDatabaseConnector;
            }
        }


    }
}
