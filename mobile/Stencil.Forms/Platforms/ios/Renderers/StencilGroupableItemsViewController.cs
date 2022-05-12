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
    public class StencilGroupableItemsViewController<TItemsView> : GroupableItemsViewController<TItemsView>
        where TItemsView : GroupableItemsView
    {
        public StencilGroupableItemsViewController(TItemsView groupableItemsView, ItemsViewLayout layout, bool enableDynamicCellReuse)
            : base(groupableItemsView, layout)
        {
            this.EnableDynamicCellReuse = enableDynamicCellReuse;
        }

        protected bool EnableDynamicCellReuse { get; set; }
        protected Dictionary<string, NSString> DynamicRegistrations = new Dictionary<string, NSString>();

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if(this.EnableDynamicCellReuse)
            {
                string templateIdentifier = this.GetDateTemplateReuseIdentifier(collectionView, indexPath);
                
                TemplatedCell templatedCell = collectionView.DequeueReusableCell(templateIdentifier, indexPath) as TemplatedCell;
                base.UpdateTemplatedCell(templatedCell, indexPath);
                
                return templatedCell;
            }
            else
            {
                return base.GetCell(collectionView, indexPath);
            }
        }

        protected string GetDateTemplateReuseIdentifier(UICollectionView collectionView, NSIndexPath indexPath)
        {
            try
            {
                IItemsViewSource source = this.ItemsSource;
                if (source != null)
                {
                    object bindingContext = source[indexPath];
                    if (bindingContext != null && bindingContext is PreparedBindingContext preparedBindingContext)
                    {
                        string contextType = bindingContext.GetType().ToString();
                        if (!this.DynamicRegistrations.ContainsKey(contextType))
                        {
                            // Hack to get internal data type (also is re-used if present, lucky us)
                            object cell = this.CreateMeasurementCell(indexPath) as object;
                            NSString identifier = new NSString(contextType);
                            this.DynamicRegistrations[contextType] = identifier;
                            CollectionView.RegisterClassForCell(cell.GetType(), identifier); // only attempt once (if fail, dont try again)
                        }
                        return contextType;
                    }
                }
            }
            catch
            {
                // We'll just have it be choppy, vs crash
            }

            return base.DetermineCellReuseId();
        }
    }
}
