using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Hosting;
using Stencil.Maui.Controls;
using Stencil.Maui.Handlers;
using Stencil.Maui.Platform;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui
{
    public static class _MauiExtensions
    {
        public static MauiAppBuilder ConfigureStencil(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(MarkdownView), typeof(MarkdownViewHandler));
            });
            
            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("LibreFranklin-Regular.ttf", "LibreFranklin-Regular");
                fonts.AddFont("LibreFranklin-Italic.ttf", "LibreFranklin-Italic");
                fonts.AddFont("LibreFranklin-Bold.ttf", "LibreFranklin-Bold");
                fonts.AddFont("LibreFranklin-BoldItalic.ttf", "LibreFranklin-BoldItalic");
                fonts.AddFont("fontawesome-webfont.ttf", "FontAwesome");
            });
#if !WINDOWS
            //builder.UseMauiCompatibility();
#endif
#if ANDROID
            NativeApplication.Keyboard = new Stencil.Maui.Droid.DroidKeyboardManager();
#elif IOS
            NativeApplication.Keyboard = new Stencil.Maui.iOS.iOSKeyboardManager();
#endif

            StencilEditorHandler.Register();
            StencilCollectionViewHandler.Register();



            return builder;
        }
    }
}
