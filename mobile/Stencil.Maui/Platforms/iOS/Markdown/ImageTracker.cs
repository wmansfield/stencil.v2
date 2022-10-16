using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Stencil.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Stencil.Maui.iOS.Markdown
{
    // Quick and hacky, should upgrade to better
    public class ImageTracker : IDisposable
    {
        public ImageTracker(UIImageView imageView)
        {
            _imageView = imageView;
        }
        private UIImageView _imageView;
        private bool _disposed;

        public void Dispose()
        {
            this.Dispose(true);
        }
        public void ClearImage()
        {
            CoreUtility.ExecuteMethod(nameof(ClearImage), delegate ()
            {
                _imageView.Image = null;
            });
        }
        public Task<UIImage> DownloadImage(string url, int cacheSeconds = 60, float scale = 1f, CancellationToken cancelationToken = default(CancellationToken))
        {
            return CoreUtility.ExecuteFunctionAsync(nameof(DownloadImage), async delegate ()
            {
                UIImage result = null;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    UriImageSource imageSource = new UriImageSource()
                    {
                        Uri = new Uri(url),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromSeconds(cacheSeconds),
                    };
                    using (var streamImage = await ((IStreamImageSource)imageSource).GetStreamAsync(cancelationToken).ConfigureAwait(false))
                    {
                        if (streamImage != null)
                        {
                            result = UIImage.LoadFromData(NSData.FromStream(streamImage), scale);
                        }
                    }
                }
                if (!_disposed)
                {
                    _imageView.Image = result;
                }
                return result;
            });
        }

        protected void Dispose(bool disposing)
        {
            _disposed = true;
            if(disposing)
            {
                _imageView.Image = null;
            }
        }
    }
}