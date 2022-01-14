using Foundation;
using Stencil.Forms.Effects;
using Stencil.Forms.iOS.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(BouncelessListView), BouncelessListViewEffect.NAME)]

namespace Stencil.Forms.iOS.Effects
{
    public class BouncelessListView : PlatformEffect
    {
        protected override void OnAttached()
        {
            CoreUtility.ExecuteMethod($"{nameof(BorderlessEntry)}.{nameof(OnAttached)}", delegate ()
            {
                UITableView tableView = this.Control as UITableView;
                if (tableView != null)
                {
                    tableView.Bounces = false;
                }

                UICollectionView collectionView = this.Control as UICollectionView;
                if (collectionView != null)
                {
                    collectionView.Bounces = false;
                }
            });
        }

        protected override void OnDetached()
        {
            CoreUtility.ExecuteMethod($"{nameof(BorderlessEntry)}.{nameof(OnDetached)}", delegate ()
            {
                UITableView tableView = this.Control as UITableView;
                if (tableView != null)
                {
                    tableView.Bounces = true;
                }

                UICollectionView collectionView = this.Control as UICollectionView;
                if (collectionView != null)
                {
                    collectionView.Bounces = true;
                }
            });
        }
    }
}