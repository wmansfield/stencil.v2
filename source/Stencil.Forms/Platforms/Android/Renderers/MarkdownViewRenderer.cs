using Android.Content;
using Android.Graphics.Drawables.Shapes;
using Android.Widget;
using Stencil.Forms.Droid.Markdown;
using Stencil.Forms.Droid.Renderers;
using Stencil.Forms.Views;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(MarkdownView), typeof(MarkdownViewRenderer))]

namespace Stencil.Forms.Droid.Renderers
{
    public class MarkdownViewRenderer : ViewRenderer
    {
        public MarkdownViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> args)
        {
            CoreUtility.ExecuteMethod(nameof(OnElementChanged), delegate ()
            {
                MarkdownLinearLayout control = this.Control as MarkdownLinearLayout;
                if (control == null)
                {
                    control = new MarkdownLinearLayout(this.Context);
                    control.Orientation = Orientation.Vertical;
                    control.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                    this.SetNativeControl(control);
                }

                MarkdownView newMarkdownView = args.NewElement as MarkdownView;

                if (newMarkdownView == null)
                {
                    control.ClearText();
                }
                else
                {
                    this.ApplyBackground();
                    control.SetText(newMarkdownView, newMarkdownView.Sections);
                }

            });
        }

        private void ApplyBackground()
        {
            CoreUtility.ExecuteMethod(nameof(ApplyBackground), delegate ()
            {
                this.SetBackgroundColor(this.Element.BackgroundColor.ToAndroid(Color.Transparent));
            });
        }
    }
}
