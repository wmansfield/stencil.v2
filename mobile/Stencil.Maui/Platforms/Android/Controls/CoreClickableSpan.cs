using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Stencil.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Maui.Droid.Controls
{
    public class CoreClickableSpan : ClickableSpan
    {
        public CoreClickableSpan()
            : base()
        {
        }
        public CoreClickableSpan(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        public CoreClickableSpan(Action<CoreClickableSpan, View> click, string argument)
            : base()
        {
            this.Click = click;
            this.Argument = argument;
        }

        public Action<CoreClickableSpan, View> Click { get; set; }
        public bool HideUnderline { get; set; }
        public string Argument { get; set; }

        public override void UpdateDrawState(TextPaint ds)
        {
            base.UpdateDrawState(ds);
            if (HideUnderline)
            {
                ds.UnderlineText = false;
            }
        }

        public override void OnClick(View widget)
        {
            CoreUtility.ExecuteMethod("OnClick", delegate ()
            {
                this.Click?.Invoke(this, widget);
            });
        }

    }
}