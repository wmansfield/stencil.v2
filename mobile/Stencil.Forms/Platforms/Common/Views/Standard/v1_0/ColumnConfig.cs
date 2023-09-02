using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public class ColumnConfig : PropertyClass
    {
        public ColumnConfig()
        {
            this.ColumnWidth = "*";
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.VerticalAlignment = LayoutAlignmentInfo.Center;
            this.HorizontalAlignment = LayoutAlignmentInfo.Center;

            this.Margin = new Thickness(0);
        }
        private LayoutOptions _horizontalOptions;
        public LayoutOptions HorizontalOptions
        {
            get { return _horizontalOptions; }
            set 
            {
                if (SetProperty(ref _horizontalOptions, value))
                {
                    _horizontalAlignment = value.ToAlignmentInfo();
                    this.RaisePropertyChanged(nameof(HorizontalAlignment));
                }
            }
        }

        private LayoutOptions _verticalOptions;
        public LayoutOptions VerticalOptions
        {
            get { return _verticalOptions; }
            set 
            { 
                if (SetProperty(ref _verticalOptions, value))
                {
                    _verticalAlignment = value.ToAlignmentInfo();
                    this.RaisePropertyChanged(nameof(VerticalAlignment));
                }
            }
        }


        private LayoutAlignmentInfo _verticalAlignment;
        [Obsolete("For Xamarin Forms, not Maui", false)]
        public LayoutAlignmentInfo VerticalAlignment
        {
            get
            {
                return _verticalAlignment;
            }
            set
            {
                if (SetProperty(ref _verticalAlignment, value))
                {
                    this.VerticalOptions = value.ToLayoutOptions();
                }
            }
        }

        private LayoutAlignmentInfo _horizontalAlignment;
        [Obsolete("For Xamarin Forms, not Maui", false)]
        public LayoutAlignmentInfo HorizontalAlignment
        {
            get 
            { 
                return _horizontalAlignment; 
            }
            set 
            {
                if (SetProperty(ref _verticalAlignment, value))
                {
                    this.HorizontalOptions = value.ToLayoutOptions();
                }
            }
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private string _columnWidth;
        public string ColumnWidth
        {
            get { return _columnWidth; }
            set { SetProperty(ref _columnWidth, value); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }
}
