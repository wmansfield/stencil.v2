using Stencil.Forms.Presentation.Menus;
using Stencil.Forms.Presentation.Shells;
using Stencil.Forms.Presentation.Shells.Tablet;
using Stencil.Forms.Presentation.Routing;
using System.Threading.Tasks;
using Xamarin.Forms;
using Stencil.Forms.Platform;

namespace Stencil.Forms.Presentation.Routing.Routers
{
    public class TabletRouter<TMainMenuView, TMenuViewModel> : TrackedClass, IRouter
        where TMainMenuView : View, IMainMenuView, new()
        where TMenuViewModel : IMenuViewModel, new()
    {
        public TabletRouter()
            : base(nameof(TabletRouter<TMainMenuView, TMenuViewModel>))
        {

        }

        public ShellModel CurrentShellModel { get; set; }
        public IShellView CurrentShellView { get; set; }
        public TabletMenuShellPage MainShellPage { get; set; }

        public Task SetInitialViewAsync(IRouterView view)
        {
            return base.ExecuteMethodAsync(nameof(SetInitialViewAsync), async delegate ()
            {
                DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

                TMainMenuView menuView = new TMainMenuView();
                menuView.MenuViewModel = new TMenuViewModel();

                TabletMenuShellPage menuShellPage = new TabletMenuShellPage()
                {
                    MenuContent = menuView,
                    ViewContent = this.CurrentShellModel?.View?.GetSelf()
                };

                this.CurrentShellModel = new ShellModel()
                {
                    View = view,
                    Parent = null,
                    Menu = null
                };

                this.MainShellPage = menuShellPage;
                this.CurrentShellView = menuShellPage;

                Task navigationTask = view.OnNavigatingToAsync(false);
                
                Xamarin.Forms.Application.Current.MainPage = new NavigationPage(this.MainShellPage);

                await navigationTask;
            });
        }

        public Task PushViewAsync(IRouterView view, IMenuEntry knownMainMenuEntry = null)
        {
            return base.ExecuteFunction(nameof(PushViewAsync), delegate ()
            {
                DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

                if (this.CurrentShellModel.View.IsMenuSupported)
                {
                    ShellModel newShellModel = new ShellModel()
                    {
                        Parent = this.CurrentShellModel,
                        View = view,
                        Menu = null

                    };

                    TabletMenuShellView newMenuView = new TabletMenuShellView()
                    {
                        ViewContent = view.GetSelf(),
                        MenuContent = this.CurrentShellModel?.Menu?.GetSelf(),
                    };

                    this.CurrentShellModel = newShellModel;

                    this.CurrentShellView.ViewContent = newMenuView;

                    this.CurrentShellView = newMenuView;

                    return view.OnNavigatingToAsync(false);
                }
                else
                {
                    ShellModel newShellModel = new ShellModel()
                    {
                        Parent = this.CurrentShellModel,
                        View = view
                    };

                    TabletBlankShellView newBlankView = new TabletBlankShellView()
                    {
                        ViewContent = view.GetSelf(),
                    };

                    this.CurrentShellModel = newShellModel;

                    this.CurrentShellView.ViewContent = newBlankView;
                    this.CurrentShellView = newBlankView;

                    return view.OnNavigatingToAsync(false);
                }
            });
            
        }
        public Task PopViewAsync(bool reloadPrevious)
        {
            return base.ExecuteFunction(nameof(PopViewAsync), delegate ()
            {
                DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

                if (this.CurrentShellModel.Parent != null)
                {
                    if (this.CurrentShellModel.Parent.View.IsMenuSupported)
                    {
                        TabletMenuShellView parentMenuView = new TabletMenuShellView()
                        {
                            ViewContent = this.CurrentShellModel?.Parent?.View?.GetSelf(),
                            MenuContent = this.CurrentShellModel?.Menu?.GetSelf(),
                        };
                        this.CurrentShellView.ViewContent = parentMenuView;
                        this.CurrentShellView = parentMenuView;
                    }
                    else
                    {
                        TabletBlankShellView parentBlankView = new TabletBlankShellView()
                        {
                            ViewContent = this.CurrentShellModel?.Parent?.View?.GetSelf(),
                        };
                        this.CurrentShellView.ViewContent = parentBlankView;
                        this.CurrentShellView = parentBlankView;
                    }

                    this.CurrentShellModel = this.CurrentShellModel.Parent;
                }
                return Task.CompletedTask;
            });
            
        }
    }
}
