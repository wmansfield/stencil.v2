using CoreGraphics;
using Foundation;
using Stencil.Common.Markdown;
using Stencil.Forms;
using Stencil.Forms.iOS.Markdown;
using Stencil.Forms.iOS.Renderers;
using Stencil.Forms.Views;
using Stencil.Forms.Views.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MarkdownView), typeof(MarkdownViewRenderer))]

namespace Stencil.Forms.iOS.Renderers
{
    public class MarkdownViewRenderer : ViewRenderer<MarkdownView, MarkdownStackView>
    {
        public MarkdownViewRenderer()
        {
        }

        private nfloat _boundWidth;
        private IMarkdownHost _boundHost;
        private List<MarkdownSection> _boundSections;
        private CacheModel _cacheModel; // In case we aren't in stencil? possible?

        public override CGRect Frame 
        {
            get
            {

                return base.Frame;
            }
            set
            {
                base.Frame = value;
            }
        }
        public override void LayoutSubviews()
        {
            CoreUtility.ExecuteMethod(nameof(LayoutSubviews), delegate ()
            {
                base.LayoutSubviews();
                IPreparedBindingContext preparedContext = this.Element?.BindingContext as IPreparedBindingContext;
                if (preparedContext != null)
                {
                    if (preparedContext.UICache == null)
                    {
                        preparedContext.UICache = new CacheModel();
                    }
                    this.ApplyMarkdownIfNeeded("LayoutSubviews", preparedContext.UICache, this.Element, preparedContext, this.Element.Sections, (int)this.Bounds.Width);
                }

                CoreUtility.Logger.LogDebug($" ------> LayoutSubviews. Element: {this.Element != null}: ------> ");
            });
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Sections")
            {
                IPreparedBindingContext preparedContext = this.Element?.BindingContext as IPreparedBindingContext;
                if (preparedContext != null)
                {
                    if (preparedContext.UICache == null)
                    {
                        preparedContext.UICache = new CacheModel();
                    }
                    _cacheModel = preparedContext.UICache;
                    this.ApplyMarkdownIfNeeded("OnElementPropertyChanged", preparedContext.UICache, this.Element, preparedContext, this.Element.Sections, (int)this.Bounds.Width);
                }
            }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return CoreUtility.ExecuteFunction(nameof(GetDesiredSize), delegate ()
            {
                SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);

                IPreparedBindingContext preparedContext = this.Element?.BindingContext as IPreparedBindingContext;
                if (preparedContext != null)
                {
                    if (preparedContext.UICache == null)
                    {
                        preparedContext.UICache = new CacheModel();
                    }
                    _cacheModel = preparedContext.UICache;
                    this.ApplyMarkdownIfNeeded("GetDesiredSize", preparedContext.UICache, this.Element, preparedContext, this.Element.Sections, (int)widthConstraint);
                }

                System.Diagnostics.Debug.WriteLine($"Width For sizing:  {widthConstraint}, height: {_cacheModel.ui_height}, text is: {this.Element.Sections[0].text}");

                result.Request = new Size() 
                { 
                    Width = widthConstraint, 
                    Height = _cacheModel.ui_height.GetValueOrDefault() 
                };
                return result;
            });
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MarkdownView> args)
        {
            CoreUtility.ExecuteMethod(nameof(OnElementChanged), delegate ()
            {
                base.OnElementChanged(args);

                MarkdownStackView control = this.Control;

                if (args.NewElement != null)
                {
                    IPreparedBindingContext preparedContext = args.NewElement.BindingContext as IPreparedBindingContext;
                    if (preparedContext != null)
                    {
                        if (preparedContext.UICache == null)
                        {
                            preparedContext.UICache = new CacheModel();
                        }
                        _cacheModel = preparedContext.UICache;

                    }
                    else
                    {
                        _cacheModel = new CacheModel();
                    }

                    if (control == null)
                    {
                        control = new MarkdownStackView(args.NewElement.TextColor);
                        this.SetNativeControl(control);
                    }

                    this.ApplyBackground();

                    this.ApplyMarkdownIfNeeded("OnElementChanged", _cacheModel, args.NewElement, preparedContext, args.NewElement?.Sections, (int)this.Bounds.Width);
                }
                else
                {
                    if (control != null)
                    {
                        control.ClearText();
                    }
                }

                CoreUtility.Logger.LogDebug($" --[]----> OnElementChanged. NewElement: {args.NewElement != null}: ------> ");
            });
        }
        
        private void ApplyMarkdownIfNeeded(string source, CacheModel cache, IMarkdownHost markdownHost, IPreparedBindingContext bindingContext, List<MarkdownSection> sections, int width)
        {
            CoreUtility.ExecuteMethod(nameof(ApplyMarkdownIfNeeded), delegate ()
            {
                if (sections == null || width < 1)
                {
                    CoreUtility.Logger.LogDebug($" ------||> {source}: ApplyMarkdownIfNeeded Not Ready: ------> ");
                    return;
                }

                if (_boundWidth == width && _boundSections == sections)
                {
                    CoreUtility.Logger.LogDebug($" ------||> {source}: ApplyMarkdownIfNeeded Already Bound: ------> ");
                    return;
                }

                MarkdownStackView control = this.Control;
                if (control != null)
                {
                    _boundHost = markdownHost;
                    _boundSections = sections;
                    _boundWidth = width;

                    control.SetText(markdownHost, cache, width, sections);
                    CoreUtility.Logger.LogDebug($" ------||> {source}: ApplyMarkdownIfNeeded Binding: " + string.Join(", ", sections.Select(x => x.text).ToArray()));
                }
                else
                {
                    CoreUtility.Logger.LogDebug($" ------||> {source}: ApplyMarkdownIfNeeded can't bind: ------> ");
                }
            });
        }

        private void ApplyBackground()
        {
            CoreUtility.ExecuteMethod(nameof(ApplyBackground), delegate ()
            {
                if (this.Element.BackgroundColor == Color.Default)
                {
                    this.BackgroundColor = UIColor.Clear;
                }
                else
                {
                    this.BackgroundColor = this.Element.BackgroundColor.ToUIColor();
                }
            });
        }
    }
}
