using Android.Content;
using Stencil.Forms.Controls;
using Stencil.Forms.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TrimLabel), typeof(TrimLabelRenderer))]

namespace Stencil.Forms.Droid.Renderers
{
    public class TrimLabelRenderer : LabelRenderer
    {
        public TrimLabelRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            CoreUtility.ExecuteMethod(nameof(OnElementChanged), delegate ()
            {
                base.OnElementChanged(e);

                if (this.Control != null)
                {
                    this.Control.FirstBaselineToTopHeight = 0;
                    this.Control.SetIncludeFontPadding(false);
                }

            });
        }
    }
}
