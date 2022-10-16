using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;

namespace Stencil.Maui.Views.Standard.v1_0
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
