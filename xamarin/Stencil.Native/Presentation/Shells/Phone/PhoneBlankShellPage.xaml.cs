using Stencil.Native.Base;
using Stencil.Native.Presentation.Shells;

using Xamarin.Forms;

namespace Stencil.Native.Presentation.Shells.Phone
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