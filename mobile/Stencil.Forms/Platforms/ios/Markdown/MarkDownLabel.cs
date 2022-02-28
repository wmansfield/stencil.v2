using CoreGraphics;
using Foundation;
using Stencil.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Stencil.Forms.iOS.Markdown
{
    public class MarkDownLabel : UITextView
    {
        public MarkDownLabel(Action<string> onTapped)
        {
            this.Tapped = onTapped;
            this.Editable = false;
            this.ScrollEnabled = false;
            this.DataDetectorTypes = UIDataDetectorType.All;
        }

        private bool _initializedListener;

        public Action<string> Tapped { get; set; }


        public Action<NSUrl> DidSelectLinkWithUrlAction { get; set; }
        public List<Tuple<NSRange, NSUrl>> Links { get; set; }

        public CGSize? ExpectedSize { get; set; }
        public override NSAttributedString AttributedText
        {
            get
            {
                return base.AttributedText;
            }
            set
            {
                base.AttributedText = value;
                this.ClearLayoutManger();
            }
        }
        public override CGRect Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
                this.ClearLayoutManger();
            }
        }
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
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.TextContainerInset = UIEdgeInsets.Zero;
            this.TextContainer.LineFragmentPadding = 0;
        }
        public void AddLinkToURL(NSUrl target, NSRange range)
        {
            CoreUtility.ExecuteMethod("AddLinkToURL", delegate ()
            {
                this.EnsureRecognizer();
                if (this.Links == null)
                {
                    this.Links = new List<Tuple<NSRange, NSUrl>>();
                }
                this.Links.Add(new Tuple<NSRange, NSUrl>(range, target));
            });
        }

        protected void EnsureRecognizer()
        {
            CoreUtility.ExecuteMethod("OnTapped", delegate ()
            {
                if (!_initializedListener)
                {
                    _initializedListener = true;
                    this.AddGestureRecognizer(new UITapGestureRecognizer(OnTapped) { NumberOfTapsRequired = 1 });
                }
            });
        }
        protected void ClearLayoutManger()
        {
            CoreUtility.ExecuteMethod("ClearLayoutManger", delegate ()
            {
                // nothing yet
            });
        }

        protected void OnTapped(UITapGestureRecognizer recognizer)
        {
            CoreUtility.ExecuteMethod("OnTapped", delegate ()
            {
                if (this.Links == null || this.Links.Count == 0)
                {
                    return;
                }

                CGPoint locationOfTouchInLabel = recognizer.LocationInView(this);

                int indexOfCharacter = (int)this.LayoutManager.GetCharacterIndex(locationOfTouchInLabel, this.TextContainer);

                List<Tuple<NSRange, NSUrl>> links = this.Links.Where(x => indexOfCharacter >= x.Item1.Location && indexOfCharacter < (x.Item1.Location + x.Item1.Length)).ToList();
                foreach (Tuple<NSRange, NSUrl> link in links)
                {
                    this.Tapped?.Invoke(link.Item2.ToString());
                }
            });
        }
    }
}