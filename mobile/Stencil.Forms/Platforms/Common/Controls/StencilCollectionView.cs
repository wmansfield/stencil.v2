using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Controls
{
    public class StencilCollectionView : CollectionView
    {
        public StencilCollectionView()
            : base()
        {

        }

        public static readonly BindableProperty EnableDynamicCellReuseProperty = BindableProperty.Create(nameof(EnableDynamicCellReuse), typeof(bool), typeof(StencilCollectionView), false);

        public bool EnableDynamicCellReuse
        {
            get
            {
                return (bool)GetValue(EnableDynamicCellReuseProperty);
            }
            set
            {
                SetValue(EnableDynamicCellReuseProperty, value);
            }
        }

        public static readonly BindableProperty EnableDynamicCellSizeCachingProperty = BindableProperty.Create(nameof(EnableDynamicCellSizeCaching), typeof(bool), typeof(StencilCollectionView), false);

        public bool EnableDynamicCellSizeCaching
        {
            get
            {
                return (bool)GetValue(EnableDynamicCellSizeCachingProperty);
            }
            set
            {
                SetValue(EnableDynamicCellSizeCachingProperty, value);
            }
        }
    }
}
