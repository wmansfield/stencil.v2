using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Stencil.Maui.Droid.Markdown;
using Stencil.Maui.Views;

namespace Stencil.Maui.Handlers
{
    public partial class MarkdownViewHandler
    {

        protected override MarkdownLinearLayout CreatePlatformView()
        {
            return CoreUtility.ExecuteFunction("MarkdownViewHandler.CreatePlatformView", delegate ()
            {
                MarkdownLinearLayout control = new MarkdownLinearLayout(this.Context);
                control.Orientation = Orientation.Vertical;
                control.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                return control;
            });
        }

        protected override void ConnectHandler(MarkdownLinearLayout platformView)
        {
            CoreUtility.ExecuteMethod("MarkdownViewHandler.ConnectHandler", delegate ()
            {
                base.ConnectHandler(platformView);

                platformView.SetBackgroundColor(this.VirtualView.BackgroundColor.ToAndroid(Colors.Transparent));

                this.UpdateContent();

            });
        }
        protected override void DisconnectHandler(MarkdownLinearLayout platformView)
        {
            CoreUtility.ExecuteMethod("MarkdownViewHandler.DisconnectHandler", delegate ()
            {
                platformView?.ClearText();
                platformView?.Dispose();

                base.DisconnectHandler(platformView);
            });
        }

        partial void UpdateContent()
        {
            CoreUtility.ExecuteMethod("UpdateContent", delegate ()
            {
                if (this.PlatformView != null && this.VirtualView != null)
                {
                    this.PlatformView?.SetText(this.VirtualView, this.VirtualView.Sections);
                }
            });
        }
    }
}
