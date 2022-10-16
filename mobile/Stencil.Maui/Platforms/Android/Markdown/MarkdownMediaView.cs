using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;

namespace Stencil.Maui.Droid.Markdown
{
    public class MarkdownMediaView : BaseLinearLayout
    {
        #region Constructor

        public MarkdownMediaView(Context context)
            : base(context)
        {
        }

        public MarkdownMediaView(Context contect, IAttributeSet attrs)
            : base(contect, attrs)
        {
        }

        public MarkdownMediaView(Context contect, IAttributeSet attrs, int defStyleAttr)
            : base(contect, attrs, defStyleAttr)
        {
        }

        public MarkdownMediaView(Context contect, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(contect, attrs, defStyleAttr, defStyleRes)
        {
        }
        public MarkdownMediaView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        #endregion

        #region Properties

        public const int MAX_IMAGE_HEIGHT = 400;
        public const int MIN_IMAGE_HEIGHT = 20;

        public AssetData Asset { get; set; }
        public MarkdownSection Section { get; set; }

        public Action ActionAnythingTapped { get; set; }


        #endregion

        #region Public Methods

        public Task SetMedia(IMarkdownHost host, MarkdownSection section, int totalHorizontalMargin)
        {
            return base.ExecuteMethodAsync("SetMedia", async delegate ()
            {
                this.Section = section;
                this.Asset = section.asset;

                Activity activity = this.Context as Activity;

                int imageSize = 0;


                int screenWidth = (int)activity.GetScreenWidth();
                int maxHeight = (int)activity.GetScreenHeight() - 150;
                if (maxHeight > MAX_IMAGE_HEIGHT)
                {
                    maxHeight = MAX_IMAGE_HEIGHT;
                }
                if (section.asset != null)
                {
                    System.Drawing.Size size = System.Drawing.Size.Empty;
                    string dimensions = section.asset.dimensions;

                    if (CoreUtility.TryParseDimensions(dimensions, out size))
                    {
                        int height = (int)((screenWidth - totalHorizontalMargin) * (float)size.Height / (float)size.Width);// W/H = w/?
                        if (height < MIN_IMAGE_HEIGHT)
                        {
                            height = MIN_IMAGE_HEIGHT;
                        }
                        if (height > MAX_IMAGE_HEIGHT)
                        {
                            height = MAX_IMAGE_HEIGHT;
                        }
                        imageSize = height;
                    }
                }
                else if (section.ui_data != null)
                {
                    Bitmap bitmap = section.ui_data as Bitmap;
                    if (bitmap != null)
                    {
                        int height = (int)((screenWidth - totalHorizontalMargin) * (float)bitmap.Height / (float)bitmap.Width);// W/H = w/?
                        if (height < MIN_IMAGE_HEIGHT)
                        {
                            height = MIN_IMAGE_HEIGHT;
                        }
                        if (height > MAX_IMAGE_HEIGHT)
                        {
                            height = MAX_IMAGE_HEIGHT;
                        }
                        imageSize = height;
                    }
                }

                this.RemoveAllViews();


                RelativeLayout vwImageContainer = new RelativeLayout(this.Context);
                vwImageContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, (imageSize + 10).ToDip(this.Context));

                if (imageSize > 0)
                {
                    ImageView imageView = new ImageView(this.Context);
                    imageView.Background = StencilPreferences.COLOR_Markdown_Image_Background.ConvertHexToDrawable();
                    imageView.SetScaleType(ImageView.ScaleType.FitCenter);
                    RelativeLayout.LayoutParams relativeParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    relativeParams.TopMargin = 5;
                    relativeParams.BottomMargin = 5;
                    imageView.LayoutParameters = relativeParams;

                    if (section.asset != null)
                    {
                        imageView.SetImageResource(global::Android.Resource.Color.Transparent);
                        UriImageSource imageSource = new UriImageSource()
                        { 
                            Uri = new Uri(section.asset.url),
                            CachingEnabled = true,
                            CacheValidity = TimeSpan.FromMinutes(5),
                        };
                        IImageViewHandler imageViewHandler = Registrar.Registered.GetHandlerForObject<IImageViewHandler>(imageSource);
                        if (imageViewHandler != null)
                        {
                            await imageViewHandler.LoadImageAsync(imageSource, imageView);
                        }
                    }
                    else if (section.ui_data != null)
                    {
                        imageView.SetImageBitmap(section.ui_data as Bitmap);
                    }

                    vwImageContainer.AddView(imageView);
                }
                
                this.AddView(vwImageContainer);
            });
        }


        #endregion
    }
}