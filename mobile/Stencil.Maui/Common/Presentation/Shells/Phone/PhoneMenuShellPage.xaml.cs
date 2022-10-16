using Microsoft.Maui.Controls;
using Stencil.Maui.Base;


namespace Stencil.Maui.Presentation.Shells.Phone
{
    public partial class PhoneMenuShellPage : BaseContentPage, IShellView
    {
        public PhoneMenuShellPage()
            : base(nameof(PhoneMenuShellPage))
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