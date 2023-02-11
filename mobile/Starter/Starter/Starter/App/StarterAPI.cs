using Stencil.Maui;
using Starter.App.Cloud;
using Starter.App.Data;
using Starter.App.Data.Sync;
using Starter.App.Screens;
using Unveilr.App.Data.Sync;

namespace Starter.App
{
    // Stencil Mobile removes Dependency Injection in favor of API based service locators.
    // To allow for simple external assembly references, registration and misdirection during API layer
    //   requires some work arounds for cart-horse problems.
    // ______Application and ____API are coupled in nature, changes to one expects changes to the other
    public class StarterAPI : StencilAPI
    {
        static StarterAPI()
        {
            StarterAPI.Instance = new StarterAPI();
            StencilAPI.Instance = StarterAPI.Instance;
        }
        public static new StarterAPI Instance;


        public StarterApplication Application
        {
            get
            {
                return StarterApplication.Instance as StarterApplication;
            }
        }

        public StarterCloud Cloud
        {
            get
            {
                return StarterApplication.StarterCloud;
            }
        }
        public IStarterDataSync DataSync
        {
            get
            {
                return NativeApplication.DataSync as IStarterDataSync;
            }
        }
        public IStarterDatabaseConnector Database
        {
            get
            {
                return StarterApplication.DatabaseConnector;
            }
        }
        public StarterScreenManager Screens
        {
            get
            {
                return NativeApplication.ScreenManager as StarterScreenManager;
            }
        }
        public StarterTrackedDataManager TrackedData
        {
            get
            {
                return NativeApplication.TrackedDataManager as StarterTrackedDataManager;
            }
        }
        
    }
}
