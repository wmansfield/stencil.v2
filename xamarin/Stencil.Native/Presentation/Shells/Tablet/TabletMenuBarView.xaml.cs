using Stencil.Native.Base;
using Stencil.Native.Presentation.Menus;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Native.Presentation.Shells.Tablet
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