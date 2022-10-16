using CoreGraphics;
using Foundation;
using Stencil.Maui;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Stencil.Maui.iOS.Markdown
{
    public class MarkDownMediaView : UIView
    {
        public MarkDownMediaView()
            : base()
        {
        }

        private ImageTracker _imageTracker;
        private UIImageView _imageView;

        public CGSize? ExpectedSize { get; set; }
        public AssetData Asset { get; set; }
        public MarkdownSection Section { get; set; }
        public Action ActionAnythingTapped { get; set; }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                if (this.ExpectedSize.HasValue && this.ExpectedSize.Value.Height > 0)
                {
                    return this.ExpectedSize.Value;
                }
                return base.IntrinsicContentSize;
            }
        }
        

        public Task SetMedia(IMarkdownHost host, int width, MarkdownSection section)
        {
            return CoreUtility.ExecuteMethodAsync("SetMedia", async delegate ()
            {
                if (_imageView != null)
                {
                    NSLayoutConstraint.DeactivateConstraints(_imageView.Constraints);
                    _imageView.RemoveFromSuperview();
                    _imageView = _imageView.DisposeSafe();
                }

                _imageTracker = _imageTracker.DisposeSafe();

                this.ActionAnythingTapped = host.AnythingTapped;

                this.Section = section;
                this.Asset = null;
                this.ExpectedSize = new CGSize(width, section.ui_height.GetValueOrDefault());

                if (section.ui_height > 0) // only if ready
                {
                    this.Asset = section.asset;

                    _imageView = new UIImageView(new CGRect(0, 0, width, section.ui_height.GetValueOrDefault()));
                    _imageView.BackgroundColor = StencilPreferences.COLOR_Markdown_Image_Background.ConvertHexToColor();
                    _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    _imageView.UserInteractionEnabled = true;
                    _imageView.TranslatesAutoresizingMaskIntoConstraints = false;

                    _imageTracker = new ImageTracker(_imageView);

                    this.AddSubview(_imageView);

                    this.AddConstraint(NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, this, NSLayoutAttribute.Leading, 1, 5));
                    this.AddConstraint(NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, this, NSLayoutAttribute.Trailing, 1, 5));
                    this.AddConstraint(NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 5));
                    this.AddConstraint(NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, 5));

                    this.UpdateConstraints();

                    if (section.asset != null)
                    {
                        section.ui_data = await _imageTracker.DownloadImage(section.asset.url);
                    }
                    else if (section.ui_data != null)
                    {
                        _imageView.Image = section.ui_data as UIImage;
                    }
                }
            });
        }

    }
}