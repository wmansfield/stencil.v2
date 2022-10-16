using Microsoft.Maui.Controls;
using Stencil.Maui.Base;


namespace Stencil.Maui.Presentation.Shells.Tablet
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