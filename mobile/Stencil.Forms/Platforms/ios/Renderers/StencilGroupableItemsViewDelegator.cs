using CoreGraphics;
using Foundation;
using Stencil.Forms.Views.Standard;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Stencil.Forms.Platforms.ios.Renderers
{
    public class StencilGroupableItemsViewDelegator<TItemsView, TViewController> : GroupableItemsViewDelegator<TItemsView, TViewController>
        where TItemsView : GroupableItemsView
        where TViewController : GroupableItemsViewController<TItemsView>
    {
        public StencilGroupableItemsViewDelegator(ItemsViewLayout itemsViewLayout, TViewController itemsViewController)
            : base(itemsViewLayout, itemsViewController)
        {

        }

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            // you think that the new size isn't being respected. 
            // check to see if re-use is causing the issue first.
            if (!this.ItemsViewLayout.EstimatedItemSize.IsEmpty)
            {
                if (this.ViewController.ItemsSource.IsIndexPathValid(indexPath))
                {
                    object item = this.ViewController.ItemsSource[indexPath];

                    if (item is IPreparedBindingContext preparedBindingContext)
                    {
                        if (preparedBindingContext.CachedSize.HasValue)
                        {
                            return preparedBindingContext.CachedSize.Value;
                        }
                    }
                }
            }
            CGSize size = base.GetSizeForItem(collectionView, layout, indexPath);
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"--->> Had to get default size for item at {indexPath.Row}, size is now: {size.Width} x {size.Height}");
#endif
            return size;
        }
    }
}
