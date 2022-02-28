using Stencil.Forms.Base;
using Stencil.Forms.Presentation.Shells;

using Xamarin.Forms;

namespace Stencil.Forms.Presentation.Shells.Phone
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