using Stencil.Native.Commanding;
using Stencil.Native.Presentation.Menus;
using Stencil.Native.Presentation.Shells;
using Stencil.Native.Presentation.Shells.Phone;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Native.Presentation.Routing.Routers
{
    public class PhoneRouter<TMainMenuView> : TrackedClass, IRouter
        where TMainMenuView : View, IMainMenuView, new()
    {
        public PhoneRouter(ICommandProcessor commandProcessor)
            : base(nameof(PhoneRouter<TMainMenuView>))
        {
            this.CommandProcessor = commandProcessor;
        }

        public ShellModel CurrentShellModel { get; set; }
        public Page CurrentPage { get; set; }
        public ICommandProcessor CommandProcessor { get; set; }


        public Task SetInitialViewAsync(IRouterView view)
        {
            return base.ExecuteMethodAsync(nameof(SetInitialViewAsync), async delegate ()
            {
                Page page = null;
                if (view.IsMenuSupported == true)
                {
                    page = new PhoneMenuShellPage()
                    {
                        ViewContent = view.GetSelf(),
                        MenuContent = new TMainMenuView()
                        {
                            MenuViewModel = new MainMenuViewModel()
                            {
                                CommandProcessor = this.CommandProcessor,
                                MenuEntries = view.MenuEntries
                            }
                        }
                    };
                }
                else
                {
                    page = new PhoneBlankShellPage()
                    {
                        ViewContent = view.GetSelf(),
                    };
                }

                ShellModel shellModel = new ShellModel()
                {
                    IsRoot = true,
                    Parent = null,
                    View = view
                };

                this.CurrentPage = page;
                this.CurrentShellModel = shellModel;

                Task navigatingToTask = view.OnNavigatingToAsync();

                Application.Current.MainPage = new NavigationPage(page);

                await navigatingToTask;
            });

        }
   
        public Task PushViewAsync(IRouterView view, IMenuEntry knownMainMenuEntry = null)
        {
            return base.ExecuteMethodAsync(nameof(PushViewAsync), async delegate ()
            {
                PhoneMenuShellPage menuShellPage = this.CurrentPage as PhoneMenuShellPage;

                bool isMainMenuContent = false;

                if (knownMainMenuEntry != null && menuShellPage != null)
                {
                    IMainMenuView mainMenu = menuShellPage.MenuContent as IMainMenuView;
                    if (mainMenu != null && mainMenu.MenuViewModel != null && mainMenu.MenuViewModel.MenuEntries != null)
                    {
                        isMainMenuContent = mainMenu.MenuViewModel.MenuEntries.Contains(knownMainMenuEntry);
                    }
                }

                if (isMainMenuContent)
                {
                    ShellModel newShellModel = new ShellModel()
                    {
                        Parent = this.CurrentShellModel,
                        View = view,
                    };

                    this.CurrentShellModel = newShellModel;

                    Task onNavigatingTask = view.OnNavigatingToAsync();

                    menuShellPage.ViewContent = view.GetSelf();

                    await onNavigatingTask;
                }
                else
                {
                    ShellModel newShellModel = new ShellModel()
                    {
                        Parent = this.CurrentShellModel,
                        View = view,
                    };
                    PhoneBlankShellPage navShellPage = new PhoneBlankShellPage()
                    {
                        ViewContent = view.GetSelf(),
                    };
                    
                    navShellPage.BindingContext = navShellPage.ViewContent.BindingContext;
                    
                    this.CurrentShellModel = newShellModel;

                    Task onNavigatingTask = view.OnNavigatingToAsync();
                    
                    await this.CurrentPage.Navigation.PushAsync(navShellPage);
                    await onNavigatingTask;
                }

            });

        }
        public Task PopViewAsync()
        {
            return base.ExecuteMethodAsync(nameof(PopViewAsync), async delegate ()
            {
                this.CurrentShellModel = this.CurrentShellModel?.Parent;

                Task onNavigatingTask = this.CurrentShellModel?.View?.OnNavigatingToAsync();

                await this.CurrentPage.Navigation.PopAsync(true);

                if(onNavigatingTask != null)
                {
                    await onNavigatingTask;
                }
            });
        }
    }
}
