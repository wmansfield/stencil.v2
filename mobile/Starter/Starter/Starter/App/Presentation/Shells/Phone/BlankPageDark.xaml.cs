using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Starter.App.Presentation.Shells.Phone
{
    public partial class BlankPageDark : BaseContentPage, IShellView
    {
        public BlankPageDark()
            : base("BlankPageDark")
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