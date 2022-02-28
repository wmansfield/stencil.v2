using Stencil.Forms.Base;
using Xamarin.Forms;

namespace Stencil.Forms.Presentation.Shells.Tablet
{
    public partial class TabletBlankShellView : BaseContentView, IShellView
    {
        public TabletBlankShellView()
            : base(nameof(TabletBlankShellView))
        {
            InitializeComponent();
        }

        public View MenuContent { get; set; }

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