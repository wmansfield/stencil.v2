using Microsoft.Maui.Controls;
using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Menus;


namespace Stencil.Maui.Presentation.Shells.Tablet
{
    public partial class TabletMenuBarView : BaseContentView, IMenuView, IMainMenuView
    {
        public TabletMenuBarView()
            : base(nameof(TabletMenuBarView))
        {
            InitializeComponent();
            this.BindingContext = this.MenuViewModel;
        }

        public IMenuViewModel MenuViewModel { get; set; }

        public View GetSelf()
        {
            return this;
        }
    }
}