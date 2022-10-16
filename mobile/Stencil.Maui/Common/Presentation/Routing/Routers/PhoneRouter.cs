using Microsoft.Maui.Controls;
using Stencil.Maui.Commanding;
using Stencil.Maui.Platform;
using Stencil.Maui.Presentation.Menus;
using Stencil.Maui.Presentation.Shells;
using Stencil.Maui.Presentation.Shells.Phone;
using System;
using System.Threading.Tasks;

namespace Stencil.Maui.Presentation.Routing.Routers
{
    public class PhoneRouter<TMainMenuView> : PhoneRouter<TMainMenuView, PhoneBlankShellPage, PhoneMenuShellPage>
        where TMainMenuView : View, IMainMenuView, new()
    {
        public PhoneRouter(ICommandProcessor commandProcessor)
            : base(commandProcessor)
        {
        }
    }

    public class PhoneRouter<TMainMenuView, TPageBlank, TPageMenu> : TrackedClass, IRouter
        where TMainMenuView : View, IMainMenuView, new()
        where TPageBlank : Page, IShellView, new()
        where TPageMenu : Page, IShellView, new()
    {
        public PhoneRouter(ICommandProcessor commandProcessor)
            : base(nameof(PhoneRouter<TMainMenuView, TPageBlank, TPageMenu>))
        {
            this.CommandProcessor = commandProcessor;
        }

        public virtual ShellModel CurrentShellModel { get; set; }
        public virtual Page CurrentPage { get; set; }
        public virtual ICommandProcessor CommandProcessor { get; set; }

        public void PrepareFromOther(IRouter routerOfSameType)
        {
            PhoneRouter<TMainMenuView, TPageBlank, TPageMenu> other = routerOfSameType as PhoneRouter<TMainMenuView, TPageBlank, TPageMenu>;
            if (other != null)
            {
                this.CurrentShellModel = other.CurrentShellModel;
                this.CurrentPage = other.CurrentPage;
                this.CommandProcessor = other.CommandProcessor;
            }
        }


        public virtual Task SetInitialViewAsync(IRouterView view)
        {
            return base.ExecuteMethodAsync(nameof(SetInitialViewAsync), async delegate ()
            {
                Page page = null;

                ShellModel shellModel = new ShellModel()
                {
                    IsRoot = true,
                    Parent = null,
                    View = view
                };

                if (view.IsMenuSupported == true)
                {
                    Page shellPage = this.GenerateMenuPage<TPageMenu>(shellModel);
                    IShellView shellView = (shellPage as IShellView);
                    shellView.ViewContent = view.GetSelf();
                    shellView.MenuContent = new TMainMenuView()
                    {
                        MenuViewModel = new MainMenuViewModel()
                        {
                            CommandProcessor = this.CommandProcessor,
                            MenuEntries = view.MenuEntries
                        }
                    };
                    page = shellPage;
                    
                }
                else
                {
                    Page shellPage = this.GenerateBlankPage<TPageBlank>(shellModel);
                    IShellView shellView = (shellPage as IShellView);
                    shellView.ViewContent = view.GetSelf();
                    page = shellPage;
                }
                
                this.CurrentPage = page;
                this.CurrentShellModel = shellModel;
                

                Task navigatingToTask = view.OnNavigatingToAsync(false);

                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.Navigation.PushAsync(page);
                    int count = navigationPage.Navigation.NavigationStack.Count;
                    for (int i = 0; i < count - 1; i++) // leave final
                    {
                        navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[i]);
                    }
                    await navigationPage.PopToRootAsync(false);
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
                await navigatingToTask;
                await view.OnNavigatedToAsync();
                DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();
            });

        }
   
        public virtual Task PushViewAsync(IRouterView view, IMenuEntry knownMainMenuEntry = null)
        {
            return base.ExecuteMethodAsync(nameof(PushViewAsync), async delegate ()
            {

                try
                {
                    IShellView menuShellPage = this.CurrentPage as IShellView;

                    bool isMainMenuContent = false;

                    IMainMenuView mainMenu = null;
                    if (knownMainMenuEntry != null && menuShellPage != null && !string.IsNullOrWhiteSpace(knownMainMenuEntry.Identifier))
                    {
                        mainMenu = menuShellPage.MenuContent as IMainMenuView;
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

                        Task onNavigatingTask = view.OnNavigatingToAsync(false);

                        menuShellPage.ViewContent = view.GetSelf();

                        mainMenu.MenuViewModel.SelectedIdentifier = knownMainMenuEntry?.Identifier;

                        await onNavigatingTask;
                        await view.OnNavigatedToAsync();

                    }
                    else
                    {
                        ShellModel newShellModel = new ShellModel()
                        {
                            Parent = this.CurrentShellModel,
                            View = view,
                        };

                        Page nextPage = null;
                        if (view.IsMenuSupported)
                        {
                            nextPage = this.GenerateMenuPage<TPageBlank>(newShellModel);
                        }
                        else
                        {
                            nextPage = this.GenerateBlankPage<TPageBlank>(newShellModel);
                        }

                        IShellView navShellView = (nextPage as IShellView);
                        navShellView.ViewContent = view.GetSelf();

                        if (view.IsMenuSupported)
                        {
                            navShellView.MenuContent = new TMainMenuView()
                            {
                                MenuViewModel = new MainMenuViewModel()
                                {
                                    CommandProcessor = this.CommandProcessor,
                                    MenuEntries = view.MenuEntries
                                }
                            };
                        }

                        nextPage.BindingContext = navShellView.ViewContent.BindingContext;

                        this.CurrentShellModel = newShellModel;

                        Task onNavigatingTask = view.OnNavigatingToAsync(false);

                        await this.CurrentPage.Navigation.PushAsync(nextPage);
                        await onNavigatingTask;

                        await view.OnNavigatedToAsync();
                    }
                }
                catch (Exception ex)
                {
                    //TODO:MUST: Localize
                    this.API.Alerts.Toast("Error loading data. Reason: " + ex.FirstNonAggregateException().Message, TimeSpan.FromSeconds(3));
                }

                DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

            });

        }
        public virtual Task PopViewAsync(bool reloadPrevious, int iterations = 1)
        {
            return base.ExecuteMethodAsync(nameof(PopViewAsync), async delegate ()
            {
                int popCount = 0;
                if(iterations < 1)
                {
                    iterations = 1;
                }
                ShellModel newModel = this.CurrentShellModel;
                for (int i = 0; i < iterations; i++)
                {
                    ShellModel model = newModel?.Parent;
                    if(model == null)
                    {
                        break;
                    }
                    else
                    {
                        popCount++;
                        newModel = model;
                    }
                }
                this.CurrentShellModel = newModel;

                Task onNavigatingTask = this.CurrentShellModel?.View?.OnNavigatingToAsync(reloadPrevious);

                for (int i = 0; i < popCount; i++)
                {
                    await this.CurrentPage.Navigation.PopAsync(i == popCount - 1); // only animate last
                }

                if (onNavigatingTask != null)
                {
                    await onNavigatingTask;
                }

                Task onNavigatedTask = this.CurrentShellModel?.View?.OnNavigatedToAsync();

                if (onNavigatedTask != null)
                {
                    await onNavigatedTask;
                }
                DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

            });
        }

        /// <summary>
        /// TPage is for reference enforcement only, not actually generic.
        /// </summary>
        protected virtual Page GenerateBlankPage<TPage>(ShellModel shellModel)
            where TPage : Page, IShellView
        {
            return base.ExecuteFunction(nameof(GenerateBlankPage), delegate ()
            {
                return new TPageBlank();
            });
        }

        /// <summary>
        /// TPage is for reference enforcement only, not actually generic.
        /// </summary>
        protected virtual Page GenerateMenuPage<TPage>(ShellModel shellModel)
            where TPage : Page, IShellView
        {
            return base.ExecuteFunction(nameof(GenerateMenuPage), delegate ()
            {
                return new TPageMenu();
            });
        }
    }
}
