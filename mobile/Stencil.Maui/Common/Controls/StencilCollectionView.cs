using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Controls
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

        public static readonly BindableProperty SuppressOverScrollProperty = BindableProperty.Create(nameof(SuppressOverScroll), typeof(bool), typeof(StencilCollectionView), false);

        public bool SuppressOverScroll
        {
            get
            {
                return (bool)GetValue(SuppressOverScrollProperty);
            }
            set
            {
                SetValue(SuppressOverScrollProperty, value);
            }
        }
    }
}
