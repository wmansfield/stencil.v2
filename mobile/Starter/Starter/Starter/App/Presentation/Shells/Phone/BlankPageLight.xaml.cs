using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Starter.App.Presentation.Shells.Phone
{
    public partial class BlankPageLight : BaseContentPage, IShellView
    {
        public BlankPageLight()
            : base("BlankPageLight")
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