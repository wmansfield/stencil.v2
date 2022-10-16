
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Stencil.Maui.Commanding;
using Stencil.Maui.Data;
using Stencil.Maui.Data.Sync;
using Stencil.Maui.Data.Sync.Manager;
using Stencil.Maui.Presentation;
using Stencil.Maui.Presentation.Routing;
using Stencil.Maui.Screens;
using System;

namespace Stencil.Maui
{
    public partial class StencilAPI
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
        public virtual IServiceProvider MauiServiceProvider
        {
            get
            {
#if IOS || MACCATALYST
                return MauiUIApplicationDelegate.Current.Services;
#elif ANDROID
                return MauiApplication.Current?.Services;
#elif WINDOWS
                return MauiWinUIApplication.Current.Services;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID)
                return null;
#endif
            }
        }

    }
}
