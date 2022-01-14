using Foundation;
using Stencil.Forms.Effects;
using Stencil.Forms.iOS.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(BorderlessEntry), BorderlessEntryEffect.NAME)]

namespace Stencil.Forms.iOS.Effects
{
    public class BorderlessEntry : PlatformEffect
    {
        #region Private Properties

        private UITextBorderStyle _oldBorderStyle;
        private nfloat _oldBorderWidth;
        private bool _hasOriginal;

        #endregion

        protected override void OnAttached()
        {
            CoreUtility.ExecuteMethod($"{nameof(BorderlessEntry)}.{nameof(OnAttached)}", delegate ()
            {
                UITextField field = this.Control as UITextField;
                if (field == null)
                {
                    return;
                }

                BorderlessEntryEffect effect = (BorderlessEntryEffect)Element.Effects.FirstOrDefault(e => e is BorderlessEntryEffect);
                if (effect == null)
                {
                    return;
                }

                this.ApplyBorderless(true);
                
            });
        }

        protected override void OnDetached()
        {
            CoreUtility.ExecuteMethod($"{nameof(BorderlessEntry)}.{nameof(OnDetached)}", delegate ()
            {
                this.ApplyBorderless(false);
            });
        }

        protected void ApplyBorderless(bool enabled)
        {
            CoreUtility.ExecuteMethod($"{nameof(BorderlessEntry)}.{nameof(ApplyBorderless)}", delegate ()
            {
                UITextField field = this.Control as UITextField;
                if (field == null)
                {
                    return;
                }

                if (enabled)
                {
                    if (!_hasOriginal)
                    {
                        _oldBorderStyle = field.BorderStyle;
                        _oldBorderWidth = field.Layer.BorderWidth;
                        _hasOriginal = true;
                    }
                    field.Layer.BorderWidth = 0;
                    field.BorderStyle = UITextBorderStyle.None;
                }
                else
                {
                    field.BorderStyle = _oldBorderStyle;
                    if (field.Layer != null)
                    {
                        field.Layer.BorderWidth = _oldBorderWidth;
                    }
                }
            });
        }
    }
}