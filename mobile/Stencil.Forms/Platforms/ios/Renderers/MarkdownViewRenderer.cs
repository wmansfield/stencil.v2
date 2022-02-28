using CoreGraphics;
using Foundation;
using Stencil.Forms;
using Stencil.Forms.iOS.Markdown;
using Stencil.Forms.iOS.Renderers;
using Stencil.Forms.Views;
using System;
using System.Collections.Generic;
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
        private MarkdownView _boundView;
        private CacheModel _cacheModel;

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

                this.ApplyMarkdownIfNeeded(this.Element, (int)this.Bounds.Width);
            });
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return CoreUtility.ExecuteFunction(nameof(GetDesiredSize), delegate ()
            {
                SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);

                this.ApplyMarkdownIfNeeded(this.Element, (int)widthConstraint);

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

                if(control != null)
                {
                    control.ClearText();
                }

                if (args.NewElement != null)
                {
                    _cacheModel = new CacheModel();

                    if (control == null)
                    {
                        control = new MarkdownStackView(args.NewElement.TextColor);
                        this.SetNativeControl(control);
                    }

                    this.ApplyBackground();

                    this.ApplyMarkdownIfNeeded(args.NewElement, (int)this.Bounds.Width);
                }

                
            });
        }
        
        private void ApplyMarkdownIfNeeded(MarkdownView newMarkdownView, int width)
        {
            CoreUtility.ExecuteMethod(nameof(ApplyMarkdownIfNeeded), delegate ()
            {
                if (newMarkdownView == null || width < 1)
                {
                    CoreUtility.Logger.LogDebug($" ------> ApplyMarkdownIfNeeded Not Ready: ------> ");
                    return;
                }

                if (_boundView == newMarkdownView && _boundWidth == width)
                {
                    CoreUtility.Logger.LogDebug($" ------> ApplyMarkdownIfNeeded Already Bound: ------> ");
                    return;
                }

                MarkdownStackView control = this.Control;
                if (control != null)
                {
                    _boundView = newMarkdownView;
                    _boundWidth = width;

                    control.SetText(newMarkdownView, _cacheModel, width, newMarkdownView.Sections);
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
