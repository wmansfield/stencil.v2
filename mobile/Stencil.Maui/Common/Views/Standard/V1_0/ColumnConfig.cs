using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public class ColumnConfig : PropertyClass
    {
        public ColumnConfig()
        {
            this.ColumnWidth = "*";
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.Margin = new Thickness(0);
        }
        private LayoutOptions _horizontalOptions;
        public LayoutOptions HorizontalOptions
        {
            get { return _horizontalOptions; }
            set { SetProperty(ref _horizontalOptions, value); }
        }

        private LayoutOptions _verticalOptions;
        public LayoutOptions VerticalOptions
        {
            get { return _verticalOptions; }
            set { SetProperty(ref _verticalOptions, value); }
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
