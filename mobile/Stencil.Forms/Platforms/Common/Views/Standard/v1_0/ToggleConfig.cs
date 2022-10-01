using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public class ToggleConfig : PropertyClass
    {
        public ToggleConfig()
        {
        }

        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set { SetProperty(ref _visible, value); }
        }

    }
}
