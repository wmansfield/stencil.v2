using Newtonsoft.Json;
using Stencil.Maui;
using Stencil.Maui.Commanding;
using Stencil.Maui.Commanding.Commands;
using Stencil.Maui.Presentation.Menus;
using Stencil.Maui.Presentation.Routing;
using Stencil.Maui.Presentation.Routing.Routers;
using Stencil.Maui.Presentation.Shells.Phone;
using Stencil.Maui.Presentation.Shells.Tablet;
using System.Net;
using Starter.App.Cloud;
using Starter.App.Commands;
using Starter.App.Data;
using Starter.App.Data.Sync;
using Starter.App.Models;
using Starter.App.Presentation;
using Starter.App.Presentation.Shells.Phone;
using Starter.App.Screens;
using Unveilr.App.Data.Sync;

namespace Starter.App
{
    public class StarterApplication : NativeApplication<Self>
    {
        #region Constructor

        private StarterApplication()
            : base(nameof(StarterApplication))
        {
        }

        #endregion

        #region Statics

        private static object _syncRoot = new object();

        public static IStarterDatabaseConnector DatabaseConnector { get; protected set; }
        public static StarterCloud StarterCloud { get; protected set; }



        public static StarterApplication Initialize(Application mauiApplication, string platformName, string platformVersion, bool darkMode, ILogger logger = null)
        {

#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;
#endif
            // Stencil Mobile removes Dependency Injection in favor of API based service locators.
            // To allow for simple external assembly references, registration and misdirection during API layer
            //   requires some work arounds for cart-horse problems.
            // ______Application and ____API are coupled in nature, changes to one expects changes to the other
            StarterApplication.Analytics = null;
            StarterApplication.DatabaseConnector = new StarterDatabaseConnector();
            StarterApplication.StencilDatabaseConnector = StarterApplication.DatabaseConnector;
            StarterApplication.CommandProcessor = new StarterCommandProcessor();
            StarterApplication.ScreenManager = new StarterScreenManager();
            StarterApplication.TrackedDataManager = new StarterTrackedDataManager();
            StarterApplication.Logger = logger;
            StarterApplication.MauiApplication = mauiApplication;
            StarterApplication.StarterCloud = new StarterCloud();
            StarterApplication.DataSync = new StarterDataSync();
            StarterApplication.Alerts = new UserDialogAlerts();

            if (StarterApplication.Logger == null)
            {
                StarterApplication.Logger = new DebugLogger();
            }

            if (Microsoft.Maui.Devices.DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                if (darkMode)
                {
                    StarterApplication.Router = new StarterPhoneRouterDark(StarterApplication.CommandProcessor);
                }
                else
                {
                    StarterApplication.Router = new StarterPhoneRouterLight(StarterApplication.CommandProcessor);
                }
            }
            else
            {
                StarterApplication.Router = new TabletRouter<TabletMenuBarView, MainMenuViewModel>();//TODO:MUST: Finish upgrading this router
            }

            // Initialize
            StarterApplication application = StarterApplication.Instance as StarterApplication;

            lock (_syncRoot)
            {
                application = StarterApplication.Instance as StarterApplication;
                if (application == null)
                {
                    application = new StarterApplication()
                    {
                        PlatformName = platformName,
                        PlatformVersion = platformVersion,
                        DarkMode = null// force apply
                    };
                    StarterApplication.Instance = application;
                    application.ApplyDarkMode(darkMode);//TODO: Clean this architecture up
                }
            }

#if DEBUG
            //NativeApplication.Instance.GenerateRealm(true).Dispose();
#endif
            return application;
        }


        #endregion

        #region Properties

        public string PlatformName { get; set; }
        public string PlatformVersion { get; set; }
        public bool? DarkMode { get; protected set; }

        public IStarterDatabaseConnector StarterDatabase
        {
            get
            {
                return StarterApplication.DatabaseConnector;
            }
        }
        public StarterAPI StarterAPI
        {
            get
            {
                return StarterAPI.Instance;
            }
        }


        protected override string InternalAppName
        {
            get
            {
                return "unveilr";
            }
        }

        #endregion

        public void ApplyDarkMode(bool darkMode)
        {
            base.ExecuteMethod("ApplyDarkMode", delegate ()
            {
                if (!this.DarkMode.HasValue || this.DarkMode != darkMode)
                {
                    this.DarkMode = darkMode;

                    // color
                    if (darkMode)
                    {
                        StarterColors.Current = StarterColors.DARK;
                    }
                    else
                    {
                        StarterColors.Current = StarterColors.LIGHT;
                    }

                    (StarterApplication.MauiApplication as MainApp)?.ApplyColorResources();

                    // router
                    IRouter newRouter = null;
                    if (Microsoft.Maui.Devices.DeviceInfo.Idiom == DeviceIdiom.Phone)
                    {
                        if (darkMode)
                        {
                            if (!(StarterApplication.Router is StarterPhoneRouterDark))
                            {
                                newRouter = new StarterPhoneRouterDark(StarterApplication.CommandProcessor);
                            }
                        }
                        else
                        {
                            if (!(StarterApplication.Router is StarterPhoneRouterLight))
                            {
                                newRouter = new StarterPhoneRouterLight(StarterApplication.CommandProcessor);
                            }
                        }
                    }
                    else
                    {
                        // not yet
                    }
                    if (newRouter != null)
                    {
                        newRouter.PrepareFromOther(StarterApplication.Router);
                        StarterApplication.Router = newRouter;
                    }
                }
            });
        }


        #region Overrides

        public override Task OnStartAsync()
        {
            return base.ExecuteFunction(nameof(OnStartAsync), delegate ()
            {

                return base.OnStartAsync();
            });
        }
        public override Task OnResumeAsync()
        {
            return base.ExecuteFunction(nameof(OnResumeAsync), delegate ()
            {

                return base.OnResumeAsync();
            });
        }
        public override Task OnSleepAsync()
        {
            return base.ExecuteFunction(nameof(OnSleepAsync), delegate ()
            {

                return base.OnSleepAsync();
            });
        }

        public override void Outdated(string reason)
        {
        }

        public override Task SessionStartAsync(Self account)
        {
            return base.ExecuteMethodAsync(nameof(SessionStartAsync), async delegate ()
            {
                // privatekeycreate command does the hard work

                // get from database for super safety
                using (IStarterDatabase database = this.StarterDatabase.OpenStarterDatabase())
                {
                    Self self = database.SelfRetrieve();
                    if (self != null)
                    {
                        this.CurrentAccount = self;
                    }
                }

                await NativeApplication.DataSync?.OnSessionStartAsync();
            });
        }
        public override Task SessionEndAsync(bool redirect)
        {
            return base.ExecuteMethodAsync(nameof(SessionEndAsync), async delegate ()
            {
                //TODO:MUST: this.UnRegisterForPushNotifications();
                NativeApplication.Instance.GenerateRealm(recreate: true).Dispose();

                //TODO:SHOULD: erase data access
                this.CurrentAccount = null;

                await NativeApplication.DataSync?.OnSessionEndAsync();


                if (redirect)
                {
                    await this.NavigateToInitialPageAsync();
                }
            });
        }
        public Task SessionRefreshAsync()
        {
            return base.ExecuteFunction(nameof(SessionRefreshAsync), delegate ()
            {
                using (IStarterDatabase database = this.StarterDatabase.OpenStarterDatabase())
                {
                    Self self = database.SelfRetrieve();
                    if (self != null)
                    {
                        this.CurrentAccount = self;
                    }
                }
                return Task.CompletedTask;
            });
        }
        protected override Task OnStartup_BeforeStencilAsync()
        {
            return base.ExecuteFunction(nameof(OnStartup_BeforeStencilAsync), delegate ()
            {
                return Task.CompletedTask;
            });
        }

        protected override Task OnStartup_AfterStencilAsync()
        {
            return base.ExecuteFunction(nameof(OnStartup_AfterStencilAsync), delegate ()
            {
                // making sure db works before we use it
                try
                {
                    this.GenerateRealm().Dispose();
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                    this.GenerateRealm(recreate: true).Dispose();// Delete for safety, sure hope they saved their keys.
                }
                this.SessionRefreshAsync();

                using (IStarterDatabase database = this.StarterAPI.Database.OpenStarterDatabase())
                {
                    bool? darkMode = database.SettingRetrieve(WellKnownSettings.DARK_MODE, (bool?)null);
                    if (darkMode.HasValue)
                    {
                        this.ApplyDarkMode(darkMode.Value);
                    }
                }

                return Task.CompletedTask;
            });
        }


        protected override Task NavigateToInitialPageAsync()
        {
            return base.ExecuteFunction(nameof(NavigateToInitialPageAsync), delegate ()
            {
                ICommandScope scope = new CommandScope(this.API.CommandProcessor);
#if DEBUG
                //return this.API.CommandProcessor.ExecuteCommandAsync(scope, WellKnownCommands.APP_NAVIGATE_ROOT, WellKnownScreens.DEBUG);
#endif
                if (this.CurrentAccount == null || this.CurrentAccount.id == Guid.Empty)
                {
                    return this.API.CommandProcessor.ExecuteCommandAsync(scope, WellKnownCommands.APP_NAVIGATE_ROOT, WellKnownScreens.ONBOARDING, null);
                }
                else
                {
                    return this.API.CommandProcessor.ExecuteCommandAsync(scope, WellKnownCommands.APP_NAVIGATE_ROOT, WellKnownScreens.SAMPLE, null);
                }
            });

        }

        #endregion
    }
}
