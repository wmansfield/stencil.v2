using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Stencil.Forms.Droid.Effects;
using Stencil.Forms.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(BorderlessEntry), BorderlessEntryEffect.NAME)]

namespace Stencil.Forms.Droid.Effects
{
    public class BorderlessEntry : PlatformEffect
    {
        private Drawable _oldBackground;
        private Thickness? _oldPadding;
        private bool _hasOriginal;

        protected override void OnAttached()
        {
            CoreUtility.ExecuteMethod($"{nameof(BorderlessEntry)}.{nameof(OnAttached)}", delegate ()
            {
                EditText field = this.Control as EditText;
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
                EditText field = this.Control as EditText;
                if (field == null)
                {
                    return;
                }

                if (enabled)
                {
                    if (!_hasOriginal)
                    {
                        _oldBackground = field.Background;
                        _oldPadding = new Thickness(field.PaddingLeft, field.PaddingTop, field.PaddingRight, field.PaddingBottom);
                        _hasOriginal = true;
                    }

                    field.Background = null;
                    field.SetPadding(0, 0, 0, 0);

                }
                else
                {
                    field.Background = _oldBackground;
                    if (_oldPadding.HasValue)
                    {
                        field.SetPadding((int)_oldPadding.Value.Left, (int)_oldPadding.Value.Top, (int)_oldPadding.Value.Right, (int)_oldPadding.Value.Bottom);
                    }
                }
            });
        }
    }
}