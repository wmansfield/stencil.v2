using Starter.App;
using Stencil.Maui;
using Stencil.Maui.Resourcing;

namespace Starter;

public partial class MainApp : Application
{
    public MainApp()
    {
        InitializeComponent();

        this.InitializeDefaultResources();

        bool darkMode = Application.Current.RequestedTheme == AppTheme.Dark;

#if DEBUG
        darkMode = false;
#endif

        string platformName = this.GetPlatformName();

        _application = StarterApplication.Initialize(this, platformName, AppInfo.Current.VersionString, darkMode);

        if (darkMode)
        {
            this.MainPage = new LoadingPageDark();
        }
        else
        {
            this.MainPage = new LoadingPageLight();
        }
    }

    private StarterApplication _application;

    protected override async void OnStart()
    {
        try
        {
            await _application.OnStartAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            //TODO:SHOULD: gulp, fatal
        }
    }
    protected override async void OnSleep()
    {
        try
        {
            await _application.OnSleepAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            //TODO:SHOULD: gulp, fatal
        }
    }
    protected override async void OnResume()
    {
        try
        {
            await _application.OnResumeAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            //TODO:SHOULD: gulp, fatal
        }
    }

    public void ApplyColorResources()
    {
        try
        {
            this.Resources["TextColor"] = Color.FromArgb(StarterColors.Current.TextColor);
            this.Resources["BorderColor"] = Color.FromArgb(StarterColors.Current.BorderColor);
            this.Resources["TextColorFaded"] = Color.FromArgb(StarterColors.Current.TextColorFaded);
            this.Resources["ButtonBackgroundFaded"] = Color.FromArgb(StarterColors.Current.ButtonBackgroundFaded);
            this.Resources["ButtonBackground"] = Color.FromArgb(StarterColors.Current.ButtonBackground);

            this.Resources["HeaderAttention"] = Color.FromArgb(StarterColors.Current.HeaderAttention);


            // stencil color overrides
            this.Resources["PageBackground"] = Color.FromArgb(StarterColors.Current.PrimaryBackground);

            // not really used anyway
            AppColors.MenuSelectedBackground = StarterColors.Current.ButtonBackground;
            AppColors.MenuSelectedText = StarterColors.Current.TextColor;

            AppColors.MenuUnselectedBackground = Colors.Transparent.ToHex();
            AppColors.MenuUnselectedText = StarterColors.Current.TextColorFaded;

            AppColors.MenuActiveBackground = StarterColors.Current.HeaderAttention;
            AppColors.MenuActiveText = StarterColors.Current.TextColor;

            AppColors.MenuBarBackground = Colors.Transparent.ToHex();

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            //TODO:SHOULD: gulp, fatal
        }
    }

    protected void InitializeDefaultResources()
    {
        try
        {
            StencilPreferences.InitFonts("LibreFranklin-Regular", "LibreFranklin-Italic", "LibreFranklin-Bold", "LibreFranklin-BoldItalic");
            StencilPreferences.InitStencilResources(this.Resources);

            this.ApplyColorResources();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            //TODO:SHOULD: gulp, fatal
        }
    }

    protected string GetPlatformName()
    {
#if IOS || MACCATALYST
        return "ios";
#elif ANDROID
        return "droid";
#elif WINDOWS
        return "win";
#else
        return "sdk";
#endif
    }
}
