using CarPlay;
using CoreGraphics;
using Foundation;
using MongoDB.Bson.IO;
using Stencil.Forms.Views.Standard;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace Stencil.Forms.Platforms.ios.Renderers
{
    public class StencilGroupableItemsViewController<TItemsView> : GroupableItemsViewController<TItemsView>
        where TItemsView : GroupableItemsView
    {
        public StencilGroupableItemsViewController(TItemsView groupableItemsView, ItemsViewLayout layout, bool enableDynamicCellReuse, bool enableDynamicCellSizeCaching)
            : base(groupableItemsView, layout)
        {
            this.EnableDynamicCellReuse = enableDynamicCellReuse;
            this.EnableDynamicCellSizeCaching = enableDynamicCellSizeCaching;
        }

        protected static object _DynamicRegistrationLock = new object();
        protected ConcurrentDictionary<string, NSString> DynamicRegistrations = new ConcurrentDictionary<string, NSString>();

        protected bool EnableDynamicCellReuse { get; set; }
        /// <summary>
        /// Note: this does not react to parent layout changes!
        /// </summary>
        protected bool EnableDynamicCellSizeCaching { get; set; }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (this.EnableDynamicCellReuse)
            {
                string templateIdentifier = this.GetDateTemplateReuseIdentifier(collectionView, indexPath);
                TemplatedCell templatedCell = collectionView.DequeueReusableCell(templateIdentifier, indexPath) as TemplatedCell;

                var prop = templatedCell.GetType().GetProperty("VisualElementRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance);

                var renderer = (prop.GetValue(templatedCell) as IVisualElementRenderer);

                base.UpdateTemplatedCell(templatedCell, indexPath);

                if (this.ItemsSource.IsIndexPathValid(indexPath))
                {
                    Xamarin.Forms.VisualElement newElement = renderer?.Element;
                    object newContext = this.ItemsSource[indexPath];
                    if (newElement != null)
                    {
                        newElement.BindingContext = newContext;
                    }
                }

                if (this.EnableDynamicCellSizeCaching)
                {
                    if (!this.ItemsViewLayout.EstimatedItemSize.IsEmpty)
                    {
                        if (this.ItemsSource.IsIndexPathValid(indexPath))
                        {
                            object item = this.ItemsSource[indexPath];

                            templatedCell.Bind(ItemsView.ItemTemplate, item, ItemsView);

                            if (item is IPreparedBindingContext preparedBindingContext)
                            {
                                DataTemplate itemTemplate = (ItemsView.ItemTemplate as DataTemplateSelector).SelectTemplate(item, ItemsView);

                                if (!preparedBindingContext.CachedSize.HasValue)
                                {
                                    // Calling Measure() here is a waste (default renderer calls it)
                                    // Xamarin Forms Renderer does not properly cache sizing (though they have the plumbing!)
                                    // We cannot re-use their measurements without reflection, this is the safest method to get the known size.
                                    preparedBindingContext.CachedSize = templatedCell.Measure();

#if DEBUG
                                    System.Diagnostics.Debug.WriteLine($"Row {indexPath.Row} {preparedBindingContext.TypeName} is now: {preparedBindingContext.CachedSize}. Payload: {preparedBindingContext.DataViewItem?.ConfigurationJson}");
#endif
                                }
                            }
                        }
                    }
                }

                return templatedCell;
            }
            else
            {
                return base.GetCell(collectionView, indexPath);
            }
        }
        
        protected override UICollectionViewDelegateFlowLayout CreateDelegator()
        {
            if (this.EnableDynamicCellSizeCaching)
            {
                return new StencilGroupableItemsViewDelegator<TItemsView, GroupableItemsViewController<TItemsView>>(ItemsViewLayout, this);
            }
            return base.CreateDelegator();
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
                            lock(_DynamicRegistrationLock)
                            {
                                if (!this.DynamicRegistrations.ContainsKey(contextType))
                                {
                                    this.DynamicRegistrations[contextType] = identifier;
                                    CollectionView.RegisterClassForCell(cell.GetType(), identifier); // only attempt once (if fail, dont try again)
                                }
                            }
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
