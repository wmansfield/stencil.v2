using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if IOS
using PlatformView = Stencil.Maui.iOS.Markdown.MarkdownStackView;
#elif MACCATALYST
using PlatformView = UIKit.UIView;
#elif ANDROID
using PlatformView = Stencil.Maui.Droid.Markdown.MarkdownLinearLayout;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

namespace Stencil.Maui.Handlers
{
    public partial class MarkdownViewHandler : ViewHandler<MarkdownView, PlatformView>
    {
        public static IPropertyMapper<MarkdownView, MarkdownViewHandler> PropertyMapper = new PropertyMapper<MarkdownView, MarkdownViewHandler>()
        {
            [nameof(MarkdownView.Sections)] = MapAnyProperty,
            [nameof(MarkdownView.TextColor)] = MapAnyProperty,
            [nameof(MarkdownView.SuppressDivider)] = MapAnyProperty,
            [nameof(MarkdownView.FontSize)] = MapAnyProperty,
            [nameof(MarkdownView.LinkTappedCommand)] = MapAnyProperty,
            [nameof(MarkdownView.AnythingTappedCommand)] = MapAnyProperty,
        };

        /// <summary>
        /// This class invalidates all content on any property changed, no need for specific mappers
        /// </summary>
        public static void MapAnyProperty(MarkdownViewHandler handler, MarkdownView markdownView)
        {
            handler?.UpdateContent();
        }

        public MarkdownViewHandler() 
            : base(PropertyMapper)
        {
        }


        partial void UpdateContent();

#if IOS || ANDROID
        // In respective folders
#elif WINDOWS
        protected override Microsoft.UI.Xaml.FrameworkElement CreatePlatformView()
        {
            //TODO:Support Windows
            return null;
        }
#elif MACCATALYST
        protected override UIKit.UIView CreatePlatformView()
        {
            //TODO:Support MACCATALYST
            return null;
        }
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID)
        protected override System.Object CreatePlatformView()
        {
            //TODO:Support Standard
            return null;
        }
#endif
    }
}
