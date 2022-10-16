using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stencil.Maui.iOS.Markdown;

namespace Stencil.Maui.Handlers
{
    public partial class MarkdownViewHandler
    {
        //TODO:MUST: Finish this handler

        protected override MarkdownStackView CreatePlatformView()
        {
            return CoreUtility.ExecuteFunction("MarkdownViewHandler.CreatePlatformView", delegate ()
            {
                MarkdownStackView control = new MarkdownStackView(this.VirtualView.TextColor);
                
                return control;
            });
        }

        protected override void ConnectHandler(MarkdownStackView platformView)
        {
            CoreUtility.ExecuteMethod("MarkdownViewHandler.ConnectHandler", delegate ()
            {
                base.ConnectHandler(platformView);

                this.UpdateContent();

            });
        }
        protected override void DisconnectHandler(MarkdownStackView platformView)
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
                if(this.PlatformView != null && this.VirtualView != null)
                {
                    //this.PlatformView?.ApplyMarkdownIfNeeded(this.VirtualView, this.VirtualView.Sections);
                }
            });
        }
    }
}
