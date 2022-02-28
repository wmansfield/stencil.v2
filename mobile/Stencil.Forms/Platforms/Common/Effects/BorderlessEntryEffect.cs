using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Effects
{
    public class BorderlessEntryEffect : RoutingEffect
    {
        public BorderlessEntryEffect() 
            : base(Effects.GROUP + "." + NAME)
        {
        }

        #region Attached Properties

        public static readonly BindableProperty IsBorderlessProperty = BindableProperty.CreateAttached("IsBorderless", typeof(bool), typeof(BorderlessEntryEffect), false, propertyChanged: OnIsBorderlessChanged);
        public static bool GetIsBorderless(BindableObject view)
        {
            return (bool)view.GetValue(IsBorderlessProperty);
        }
        public static void SetIsBorderless(BindableObject view, bool value)
        {
            view.SetValue(IsBorderlessProperty, value);
        }
        private static void OnIsBorderlessChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CoreUtility.ExecuteMethod(nameof(OnIsBorderlessChanged), delegate ()
            {
                View view = bindable as View;
                if (view == null)
                {
                    return;
                }

                bool isBorderless = (bool)newValue;
                if (isBorderless)
                {
                    Effect match = view.Effects.FirstOrDefault(e => e is BorderlessEntryEffect);
                    if (match == null)
                    {
                        view.Effects.Add(new BorderlessEntryEffect());
                    }
                }
                else
                {
                    Effect match = view.Effects.FirstOrDefault(e => e is BorderlessEntryEffect);
                    if (match != null)
                    {
                        view.Effects.Remove(match);
                    }
                }
            });
        }

        #endregion

        public const string NAME = "BorderlessEntry";

    }
}
