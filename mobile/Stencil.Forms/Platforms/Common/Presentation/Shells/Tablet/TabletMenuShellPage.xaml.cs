using Stencil.Forms.Base;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Forms.Presentation.Shells.Tablet
{
    public partial class TabletMenuShellPage : BaseContentPage, IShellView
    {
        public TabletMenuShellPage()
            : base(nameof(TabletMenuShellPage))
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