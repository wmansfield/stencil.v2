using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Starter.App.Presentation.Shells.Phone
{
    public partial class MenuPageDark : BaseContentPage, IShellView
    {
        public MenuPageDark()
            : base(nameof(MenuPageDark))
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