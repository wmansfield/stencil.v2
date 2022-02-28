using Stencil.Forms.Base;

using Xamarin.Forms;

namespace Stencil.Forms.Presentation.Shells.Tablet
{
    public partial class TabletMenuShellView : BaseContentView, IShellView
    {
        public TabletMenuShellView()
            : base(nameof(TabletMenuShellView))
        {
            InitializeComponent();
        }

        public View MenuContent
        {
            get
            {
                return vwMenu.Content;
            }
            set
            {
                vwMenu.Content = value;
            }
        }

        public View ViewContent
        {
            get
            {
                return vwContent.Content;
            }
            set
            {
                vwContent.Content = value;
            }
        }
    }
}