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

            return base.GetSizeForItem(collectionView, layout, indexPath);
        }
    }
}
