using Microsoft.Maui.Controls;
using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Shells;


namespace Stencil.Maui.Presentation.Shells.Phone
{
    public partial class PhoneBlankShellPage : BaseContentPage, IShellView
    {
        public PhoneBlankShellPage()
            : base(nameof(PhoneBlankShellPage))
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