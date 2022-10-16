using Microsoft.Maui.Controls;
using Stencil.Maui.Base;

namespace Stencil.Maui.Presentation.Shells.Tablet
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