using Stencil.Forms.Controls;
using Stencil.Forms.ios.Renderers;
using Stencil.Forms.Platforms.ios.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StencilCollectionView), typeof(StencilCollectionViewRenderer))]

namespace Stencil.Forms.ios.Renderers
{
    public class StencilCollectionViewRenderer : GroupableItemsViewRenderer<GroupableItemsView, StencilGroupableItemsViewController<GroupableItemsView>>
    {
        public StencilCollectionViewRenderer()
        { 
        }
        protected override StencilGroupableItemsViewController<GroupableItemsView> CreateController(GroupableItemsView itemsView, ItemsViewLayout layout)
        {
            bool enableDynamicCellReuse = false;
            if(this.Element is StencilCollectionView stencilCollectionView)
            {
                enableDynamicCellReuse = stencilCollectionView.EnableDynamicCellReuse;
            }
            return new StencilGroupableItemsViewController<GroupableItemsView>(itemsView, layout, enableDynamicCellReuse);
        }
       
    }
}
