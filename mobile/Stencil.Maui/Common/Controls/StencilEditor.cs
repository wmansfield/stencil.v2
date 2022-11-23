using Microsoft.Maui.Controls;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Controls
{
    public class StencilEditor : Editor
    {
        public static readonly BindableProperty SuppressBottomLineProperty = BindableProperty.Create(propertyName: nameof(SuppressBottomLine), returnType: typeof(bool), declaringType: typeof(StencilEditor), defaultValue: false);
        public bool SuppressBottomLine
        {
            get { return (bool)GetValue(SuppressBottomLineProperty); }
            set { SetValue(SuppressBottomLineProperty, value); }
        }

        public static readonly BindableProperty AutoFocusProperty = BindableProperty.Create(propertyName: nameof(AutoFocus), returnType: typeof(bool), declaringType: typeof(StencilEditor), defaultValue: false);
        public bool AutoFocus
        {
            get { return (bool)GetValue(AutoFocusProperty); }
            set { SetValue(AutoFocusProperty, value); }
        }
        
    }
}
